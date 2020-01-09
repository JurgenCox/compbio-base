using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaseLibS.Graph.Image.Formats.Gif.Sections;
using BaseLibS.Graph.Image.Quantizers;
using BaseLibS.Num;
using BaseLibS.Parse.Endian;

namespace BaseLibS.Graph.Image.Formats.Gif{
	internal sealed class GifEncoderCore{
		private int bitDepth;
		public int Quality { get; set; }
		public byte Threshold { get; set; } = 128;
		public IQuantizer Quantizer { get; set; }
		public void Encode(ImageBase imageBase, Stream stream){
			if (imageBase == null || stream == null){
				throw new ArgumentNullException();
			}
			Image2 image = (Image2) imageBase;
			if (Quantizer == null){
				Quantizer = new OctreeQuantizer{Threshold = Threshold};
			}

			// Do not use IDisposable pattern here as we want to preserve the stream. 
			EndianBinaryWriter writer = new EndianBinaryWriter(EndianBitConverter.Little, stream);

			// Ensure that quality can be set but has a fallback.
			int quality = Quality > 0 ? Quality : imageBase.Quality;
			Quality = quality > 0 ? NumUtils.Clamp(quality,1, 256) : 256;

			// Get the number of bits.
			bitDepth = GetBitsNeededForColorDepth(Quality);

			// Quantize the image returning a palette.
			QuantizedImage quantized = Quantizer.Quantize(image, Quality);

			// Write the header.
			WriteHeader(writer);

			// Write the LSD. We'll use local color tables for now.
			WriteLogicalScreenDescriptor(image, writer, quantized.TransparentIndex);

			// Write the first frame.
			WriteGraphicalControlExtension(imageBase, writer, quantized.TransparentIndex);
			WriteImageDescriptor(image, writer);
			WriteColorTable(quantized, writer);
			WriteImageData(quantized, writer);

			// Write additional frames.
			if (image.Frames.Any()){
				WriteApplicationExtension(writer, image.RepeatCount, image.Frames.Count);
				foreach (ImageFrame frame in image.Frames){
					QuantizedImage quantizedFrame = Quantizer.Quantize(frame, Quality);
					WriteGraphicalControlExtension(frame, writer, quantizedFrame.TransparentIndex);
					WriteImageDescriptor(frame, writer);
					WriteColorTable(quantizedFrame, writer);
					WriteImageData(quantizedFrame, writer);
				}
			}

			// TODO: Write Comments extension etc
			writer.Write(GifConstants.endIntroducer);
		}
		public static int GetBitsNeededForColorDepth(int colors)
		{
			return (int)Math.Ceiling(Math.Log(colors, 2));
		}

		private void WriteHeader(EndianBinaryWriter writer){
			writer.Write((GifConstants.fileType + GifConstants.fileVersion).ToCharArray());
		}
		private void WriteLogicalScreenDescriptor(Image2 image, EndianBinaryWriter writer, int tranparencyIndex)
			{
			GifLogicalScreenDescriptor descriptor = new GifLogicalScreenDescriptor{
				Width = (short) image.Width,
				Height = (short) image.Height,
				GlobalColorTableFlag = false, // Always false for now.
				GlobalColorTableSize = bitDepth - 1,
				BackgroundColorIndex = (byte) (tranparencyIndex > -1 ? tranparencyIndex : 255)
			};
			writer.Write((ushort) descriptor.Width);
			writer.Write((ushort) descriptor.Height);
			PackedField field = new PackedField();
			field.SetBit(0, descriptor.GlobalColorTableFlag); // 1   : Global color table flag = 1 || 0 (GCT used/ not used)
			field.SetBits(1, 3, descriptor.GlobalColorTableSize); // 2-4 : color resolution
			field.SetBit(4, false); // 5   : GCT sort flag = 0
			field.SetBits(5, 3, descriptor.GlobalColorTableSize); // 6-8 : GCT size. 2^(N+1)

			// Reduce the number of writes
			byte[] arr ={
				field.Byte, descriptor.BackgroundColorIndex, // Background Color Index
				descriptor.PixelAspectRatio // Pixel aspect ratio. Assume 1:1
			};
			writer.Write(arr);
		}
		private void WriteApplicationExtension(EndianBinaryWriter writer, ushort repeatCount, int frames){
			// Application Extension Header
			if (repeatCount != 1 && frames > 0){
				byte[] ext ={
					GifConstants.extensionIntroducer, GifConstants.applicationExtensionLabel,
					GifConstants.applicationBlockSize
				};
				writer.Write(ext);
				writer.Write(GifConstants.applicationIdentification.ToCharArray()); // NETSCAPE2.0
				writer.Write((byte) 3); // Application block length
				writer.Write((byte) 1); // Data sub-block index (always 1)

				// 0 means loop indefinitely. Count is set as play n + 1 times.
				repeatCount = (ushort) (Math.Max((ushort) 0, repeatCount) - 1);
				writer.Write(repeatCount); // Repeat count for images.
				writer.Write(GifConstants.terminator); // terminator
			}
		}
		private void WriteGraphicalControlExtension(ImageBase image, EndianBinaryWriter writer,
			int transparencyIndex) {
			// TODO: Check transparency logic.
			bool hasTransparent = transparencyIndex > -1;
			DisposalMethod disposalMethod = hasTransparent ? DisposalMethod.RestoreToBackground : DisposalMethod.Unspecified;
			GifGraphicsControlExtension extension = new GifGraphicsControlExtension(){
				DisposalMethod = disposalMethod,
				TransparencyFlag = hasTransparent,
				TransparencyIndex = transparencyIndex,
				DelayTime = image.FrameDelay
			};

			// Reduce the number of writes.
			byte[] intro ={
				GifConstants.extensionIntroducer, GifConstants.graphicControlLabel, 4 // size
			};
			writer.Write(intro);
			PackedField field = new PackedField();
			field.SetBits(3, 3, (int) extension.DisposalMethod); // 1-3 : Reserved, 4-6 : Disposal

			// TODO: Allow this as an option.
			field.SetBit(6, false); // 7 : User input - 0 = none
			field.SetBit(7, extension.TransparencyFlag); // 8: Has transparent.
			writer.Write(field.Byte);
			writer.Write((ushort) extension.DelayTime);
			writer.Write((byte) (extension.TransparencyIndex == -1 ? 255 : extension.TransparencyIndex));
			writer.Write(GifConstants.terminator);
		}
		private void WriteImageDescriptor(ImageBase image, EndianBinaryWriter writer)
			{
			writer.Write(GifConstants.imageDescriptorLabel); // 2c
			// TODO: Can we capture this?
			writer.Write((ushort) 0); // Left position
			writer.Write((ushort) 0); // Top position
			writer.Write((ushort) image.Width);
			writer.Write((ushort) image.Height);
			PackedField field = new PackedField();
			field.SetBit(0, true); // 1: Local color table flag = 1 (LCT used)
			field.SetBit(1, false); // 2: Interlace flag 0
			field.SetBit(2, false); // 3: Sort flag 0
			field.SetBits(5, 3, bitDepth - 1); // 4-5: Reserved, 6-8 : LCT size. 2^(N+1)
			writer.Write(field.Byte);
		}
		private void WriteColorTable(QuantizedImage image, EndianBinaryWriter writer)
			{
			// Grab the palette and write it to the stream.
			Color2[] palette = image.Palette;
			int pixelCount = palette.Length;

			// Get max colors for bit depth.
			int colorTableLength = (int) Math.Pow(2, bitDepth)*3;
			byte[] colorTable = new byte[colorTableLength];
			Parallel.For(0, pixelCount, i =>{
				int offset = i*3;
				byte[] color = palette[i].ToBytes();
				colorTable[offset] = color[0];
				colorTable[offset + 1] = color[1];
				colorTable[offset + 2] = color[2];
			});
			writer.Write(colorTable, 0, colorTableLength);
		}
		private void WriteImageData(QuantizedImage image, EndianBinaryWriter writer) {
			byte[] indexedPixels = image.Pixels;
			LzwEncoder encoder = new LzwEncoder(indexedPixels, (byte) bitDepth);
			encoder.Encode(writer.BaseStream);
		}
	}
}