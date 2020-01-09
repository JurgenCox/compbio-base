namespace BaseLibS.Graph.Image.Formats.Jpg{
	internal class IDCT{
		private const int w1 = 2841; // 2048*sqrt(2)*cos(1*pi/16)
		private const int w2 = 2676; // 2048*sqrt(2)*cos(2*pi/16)
		private const int w3 = 2408; // 2048*sqrt(2)*cos(3*pi/16)
		private const int w5 = 1609; // 2048*sqrt(2)*cos(5*pi/16)
		private const int w6 = 1108; // 2048*sqrt(2)*cos(6*pi/16)
		private const int w7 = 565; // 2048*sqrt(2)*cos(7*pi/16)
		private const int w1Pw7 = w1 + w7;
		private const int w1Mw7 = w1 - w7;
		private const int w2Pw6 = w2 + w6;
		private const int w2Mw6 = w2 - w6;
		private const int w3Pw5 = w3 + w5;
		private const int w3Mw5 = w3 - w5;
		private const int r2 = 181; // 256/sqrt(2)
		public static void Transform(Block src){
			for (int y = 0; y < 8; y++){
				int y8 = y*8;
				if (src[y8 + 1] == 0 && src[y8 + 2] == 0 && src[y8 + 3] == 0 && src[y8 + 4] == 0 && src[y8 + 5] == 0 &&
					src[y8 + 6] == 0 && src[y8 + 7] == 0){
					int dc = src[y8 + 0] << 3;
					src[y8 + 0] = dc;
					src[y8 + 1] = dc;
					src[y8 + 2] = dc;
					src[y8 + 3] = dc;
					src[y8 + 4] = dc;
					src[y8 + 5] = dc;
					src[y8 + 6] = dc;
					src[y8 + 7] = dc;
					continue;
				}
				int x0 = (src[y8 + 0] << 11) + 128;
				int x1 = src[y8 + 4] << 11;
				int x2 = src[y8 + 6];
				int x3 = src[y8 + 2];
				int x4 = src[y8 + 1];
				int x5 = src[y8 + 7];
				int x6 = src[y8 + 5];
				int x7 = src[y8 + 3];
				int x8 = w7*(x4 + x5);
				x4 = x8 + w1Mw7*x4;
				x5 = x8 - w1Pw7*x5;
				x8 = w3*(x6 + x7);
				x6 = x8 - w3Mw5*x6;
				x7 = x8 - w3Pw5*x7;
				x8 = x0 + x1;
				x0 -= x1;
				x1 = w6*(x3 + x2);
				x2 = x1 - w2Pw6*x2;
				x3 = x1 + w2Mw6*x3;
				x1 = x4 + x6;
				x4 -= x6;
				x6 = x5 + x7;
				x5 -= x7;
				x7 = x8 + x3;
				x8 -= x3;
				x3 = x0 + x2;
				x0 -= x2;
				x2 = (r2*(x4 + x5) + 128) >> 8;
				x4 = (r2*(x4 - x5) + 128) >> 8;
				src[y8 + 0] = (x7 + x1) >> 8;
				src[y8 + 1] = (x3 + x2) >> 8;
				src[y8 + 2] = (x0 + x4) >> 8;
				src[y8 + 3] = (x8 + x6) >> 8;
				src[y8 + 4] = (x8 - x6) >> 8;
				src[y8 + 5] = (x0 - x4) >> 8;
				src[y8 + 6] = (x3 - x2) >> 8;
				src[y8 + 7] = (x7 - x1) >> 8;
			}
			for (int x = 0; x < 8; x++){
				int y0 = (src[x] << 8) + 8192;
				int y1 = src[32 + x] << 8;
				int y2 = src[48 + x];
				int y3 = src[16 + x];
				int y4 = src[8 + x];
				int y5 = src[56 + x];
				int y6 = src[40 + x];
				int y7 = src[24 + x];
				int y8 = w7*(y4 + y5) + 4;
				y4 = (y8 + w1Mw7*y4) >> 3;
				y5 = (y8 - w1Pw7*y5) >> 3;
				y8 = w3*(y6 + y7) + 4;
				y6 = (y8 - w3Mw5*y6) >> 3;
				y7 = (y8 - w3Pw5*y7) >> 3;
				y8 = y0 + y1;
				y0 -= y1;
				y1 = w6*(y3 + y2) + 4;
				y2 = (y1 - w2Pw6*y2) >> 3;
				y3 = (y1 + w2Mw6*y3) >> 3;
				y1 = y4 + y6;
				y4 -= y6;
				y6 = y5 + y7;
				y5 -= y7;
				y7 = y8 + y3;
				y8 -= y3;
				y3 = y0 + y2;
				y0 -= y2;
				y2 = (r2*(y4 + y5) + 128) >> 8;
				y4 = (r2*(y4 - y5) + 128) >> 8;
				src[x] = (y7 + y1) >> 14;
				src[8 + x] = (y3 + y2) >> 14;
				src[16 + x] = (y0 + y4) >> 14;
				src[24 + x] = (y8 + y6) >> 14;
				src[32 + x] = (y8 - y6) >> 14;
				src[40 + x] = (y0 - y4) >> 14;
				src[48 + x] = (y3 - y2) >> 14;
				src[56 + x] = (y7 - y1) >> 14;
			}
		}
	}
}