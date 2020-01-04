using System;
using System.IO;
using BaseLibS.Graph.Image.Formats.Gif.Sections;

namespace BaseLibS.Graph.Image.Formats.Gif{
	internal class GifDecoderCore{
		private Image2 decodedImage;
		private Stream currentStream;
		private byte[] globalColorTable;
		private Color2[] currentFrame;
		private GifLogicalScreenDescriptor logicalScreenDescriptor;
		private GifGraphicsControlExtension graphicsControlExtension;
		public void Decode(Image2 image, Stream stream){
			decodedImage = image;
			currentStream = stream;

			// Skip the identifier
			currentStream.Seek(6, SeekOrigin.Current);
			ReadLogicalScreenDescriptor();
			if (logicalScreenDescriptor.GlobalColorTableFlag){
				globalColorTable = new byte[logicalScreenDescriptor.GlobalColorTableSize*3];

				// Read the global color table from the stream
				stream.Read(globalColorTable, 0, globalColorTable.Length);
			}

			// Loop though the respective gif parts and read the data.
			int nextFlag = stream.ReadByte();
			while (nextFlag != GifConstants.terminator){
				if (nextFlag == GifConstants.imageLabel){
					ReadFrame();
				} else if (nextFlag == GifConstants.extensionIntroducer){
					int label = stream.ReadByte();
					switch (label){
						case GifConstants.graphicControlLabel:
							ReadGraphicalControlExtension();
							break;
						case GifConstants.commentLabel:
							ReadComments();
							break;
						case GifConstants.applicationExtensionLabel:
							Skip(12); // No need to read.
							break;
						case GifConstants.plainTextLabel:
							Skip(13); // Not supported by any known decoder.
							break;
					}
				} else if (nextFlag == GifConstants.endIntroducer){
					break;
				}
				nextFlag = stream.ReadByte();
			}
		}
		private void ReadGraphicalControlExtension(){
			byte[] buffer = new byte[6];
			currentStream.Read(buffer, 0, buffer.Length);
			byte packed = buffer[1];
			graphicsControlExtension = new GifGraphicsControlExtension{
				DelayTime = BitConverter.ToInt16(buffer, 2),
				TransparencyIndex = buffer[4],
				TransparencyFlag = (packed & 0x01) == 1,
				DisposalMethod = (DisposalMethod) ((packed & 0x1C) >> 2)
			};
		}
		private GifImageDescriptor ReadImageDescriptor(){
			byte[] buffer = new byte[9];
			currentStream.Read(buffer, 0, buffer.Length);
			byte packed = buffer[8];
			GifImageDescriptor imageDescriptor = new GifImageDescriptor{
				Left = BitConverter.ToInt16(buffer, 0),
				Top = BitConverter.ToInt16(buffer, 2),
				Width = BitConverter.ToInt16(buffer, 4),
				Height = BitConverter.ToInt16(buffer, 6),
				LocalColorTableFlag = ((packed & 0x80) >> 7) == 1,
				LocalColorTableSize = 2 << (packed & 0x07),
				InterlaceFlag = ((packed & 0x40) >> 6) == 1
			};
			return imageDescriptor;
		}
		private void ReadLogicalScreenDescriptor(){
			byte[] buffer = new byte[7];
			currentStream.Read(buffer, 0, buffer.Length);
			byte packed = buffer[4];
			logicalScreenDescriptor = new GifLogicalScreenDescriptor{
				Width = BitConverter.ToInt16(buffer, 0),
				Height = BitConverter.ToInt16(buffer, 2),
				BackgroundColorIndex = buffer[5],
				PixelAspectRatio = buffer[6],
				GlobalColorTableFlag = ((packed & 0x80) >> 7) == 1,
				GlobalColorTableSize = 2 << (packed & 0x07)
			};
			if (logicalScreenDescriptor.GlobalColorTableSize > 255*4){
				throw new Exception($"Invalid gif colormap size '{logicalScreenDescriptor.GlobalColorTableSize}'");
			}
			if (logicalScreenDescriptor.Width > decodedImage.MaxWidth || logicalScreenDescriptor.Height > decodedImage.MaxHeight){
				throw new ArgumentOutOfRangeException(
					$"The input gif '{logicalScreenDescriptor.Width}x{logicalScreenDescriptor.Height}' is bigger then the max allowed size '{decodedImage.MaxWidth}x{decodedImage.MaxHeight}'");
			}
		}
		private void Skip(int length){
			currentStream.Seek(length, SeekOrigin.Current);
			int flag;
			while ((flag = currentStream.ReadByte()) != 0){
				currentStream.Seek(flag, SeekOrigin.Current);
			}
		}
		private void ReadComments(){
			int flag;
			while ((flag = currentStream.ReadByte()) != 0){
				if (flag > GifConstants.maxCommentLength){
					throw new Exception($"Gif comment length '{flag}' exceeds max '{GifConstants.maxCommentLength}'");
				}
				byte[] buffer = new byte[flag];
				currentStream.Read(buffer, 0, flag);
				decodedImage.Properties.Add(new ImageProperty("Comments", BitConverter.ToString(buffer)));
			}
		}
		private void ReadFrame(){
			GifImageDescriptor imageDescriptor = ReadImageDescriptor();
			byte[] localColorTable = ReadFrameLocalColorTable(imageDescriptor);
			byte[] indices = ReadFrameIndices(imageDescriptor);

			// Determine the color table for this frame. If there is a local one, use it
			// otherwise use the global color table.
			byte[] colorTable = localColorTable ?? globalColorTable;
			ReadFrameColors(indices, colorTable, imageDescriptor);

			// Skip any remaining blocks
			Skip(0);
		}
		private byte[] ReadFrameIndices(GifImageDescriptor imageDescriptor){
			int dataSize = currentStream.ReadByte();
			LzwDecoder lzwDecoder = new LzwDecoder(currentStream);
			byte[] indices = lzwDecoder.DecodePixels(imageDescriptor.Width, imageDescriptor.Height, dataSize);
			return indices;
		}
		private byte[] ReadFrameLocalColorTable(GifImageDescriptor imageDescriptor){
			byte[] localColorTable = null;
			if (imageDescriptor.LocalColorTableFlag){
				localColorTable = new byte[imageDescriptor.LocalColorTableSize*3];
				currentStream.Read(localColorTable, 0, localColorTable.Length);
			}
			return localColorTable;
		}
		private void ReadFrameColors(byte[] indices, byte[] colorTable, GifImageDescriptor descriptor){
			int imageWidth = logicalScreenDescriptor.Width;
			int imageHeight = logicalScreenDescriptor.Height;
			if (currentFrame == null){
				currentFrame = new Color2[imageWidth*imageHeight];
			}
			Color2[] lastFrame = null;
			if (graphicsControlExtension != null && graphicsControlExtension.DisposalMethod == DisposalMethod.RestoreToPrevious){
				lastFrame = new Color2[imageWidth*imageHeight];
				Array.Copy(currentFrame, lastFrame, lastFrame.Length);
			}
			int offset, i = 0;
			int interlacePass = 0; // The interlace pass
			int interlaceIncrement = 8; // The interlacing line increment
			int interlaceY = 0; // The current interlaced line
			for (int y = descriptor.Top; y < descriptor.Top + descriptor.Height; y++){
				// Check if this image is interlaced.
				int writeY; // the target y offset to write to
				if (descriptor.InterlaceFlag){
					// If so then we read lines at predetermined offsets.
					// When an entire image height worth of offset lines has been read we consider this a pass.
					// With each pass the number of offset lines changes and the starting line changes.
					if (interlaceY >= descriptor.Height){
						interlacePass++;
						switch (interlacePass){
							case 1:
								interlaceY = 4;
								break;
							case 2:
								interlaceY = 2;
								interlaceIncrement = 4;
								break;
							case 3:
								interlaceY = 1;
								interlaceIncrement = 2;
								break;
						}
					}
					writeY = interlaceY + descriptor.Top;
					interlaceY += interlaceIncrement;
				} else{
					writeY = y;
				}
				for (int x = descriptor.Left; x < descriptor.Left + descriptor.Width; x++){
					offset = (writeY*imageWidth) + x;
					int index = indices[i];
					if (graphicsControlExtension == null || graphicsControlExtension.TransparencyFlag == false ||
						graphicsControlExtension.TransparencyIndex != index){
						// Stored in r-> g-> b-> a order.
						int indexOffset = index*3;
						Color2 pixel = Color2.FromArgb(colorTable[indexOffset], colorTable[indexOffset + 1], colorTable[indexOffset + 2]);
						currentFrame[offset] = pixel;
					}
					i++;
				}
			}
			Color2[] pixels = new Color2[imageWidth*imageHeight];
			Array.Copy(currentFrame, pixels, pixels.Length);
			ImageBase currentImage;
			if (decodedImage.Pixels == null){
				currentImage = decodedImage;
				currentImage.SetPixels(imageWidth, imageHeight, pixels);
				currentImage.Quality = colorTable.Length/3;
				if (graphicsControlExtension != null && graphicsControlExtension.DelayTime > 0){
					decodedImage.FrameDelay = graphicsControlExtension.DelayTime;
				}
			} else{
				ImageFrame frame = new ImageFrame();
				currentImage = frame;
				currentImage.SetPixels(imageWidth, imageHeight, pixels);
				currentImage.Quality = colorTable.Length/3;
				if (graphicsControlExtension != null && graphicsControlExtension.DelayTime > 0){
					currentImage.FrameDelay = graphicsControlExtension.DelayTime;
				}
				decodedImage.Frames.Add(frame);
			}
			if (graphicsControlExtension != null){
				if (graphicsControlExtension.DisposalMethod == DisposalMethod.RestoreToBackground){
					for (int y = descriptor.Top; y < descriptor.Top + descriptor.Height; y++){
						for (int x = descriptor.Left; x < descriptor.Left + descriptor.Width; x++){
							offset = (y*imageWidth) + x;

							// Stored in r-> g-> b-> a order.
							currentFrame[offset] = default(Color2);
						}
					}
				} else if (graphicsControlExtension.DisposalMethod == DisposalMethod.RestoreToPrevious){
					currentFrame = lastFrame;
				}
			}
		}
	}
}