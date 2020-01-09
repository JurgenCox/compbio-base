using System;
using System.Collections.Generic;

namespace BaseLibS.Num{
	public static class TukeyBiweightCalc{
		private const double c = 5.0;
		private const double epsilon = 0.0001;

		public static double TukeyBiweight(IList<double> x) {
			int length = x.Count;
			if (length == 0)
			{
				return double.NaN;
			}
			double median;
			double[] buffer = new double[length];
			double s;
			double sum = 0.0;
			double sumw = 0.0;
			for (int i = 0; i < length; i++){
				buffer[i] = x[i];
			}
			Array.Sort(buffer);
			if (length%2 == 0){
				median = (buffer[length/2 - 1] + buffer[length/2])/2.0;
			} else{
				median = buffer[length/2];
			}
			for (int i = 0; i < length; i++){
				buffer[i] = Math.Abs(x[i] - median);
			}
			Array.Sort(buffer);
			if (length%2 == 0){
				s = (buffer[length/2 - 1] + buffer[length/2])/2.0;
			} else{
				s = buffer[length/2];
			}
			for (int i = 0; i < length; i++){
				buffer[i] = (x[i] - median)/(c*s + epsilon);
			}
			for (int i = 0; i < length; i++){
				sum += WeightBisquare(buffer[i])*x[i];
				sumw += WeightBisquare(buffer[i]);
			}
			return (sum/sumw);
		}

		public static double TukeyBiweightSe(IList<double> x, double bw) {
			int length = x.Count;
			double median;
			double[] buffer = new double[length];
			double s;
			double sum = 0.0;
			double sumw = 0.0;
			for (int i = 0; i < length; i++){
				buffer[i] = x[i];
			}
			Array.Sort(buffer);
			if (length%2 == 0){
				median = (buffer[length/2 - 1] + buffer[length/2])/2.0;
			} else{
				median = buffer[length/2];
			}
			for (int i = 0; i < length; i++){
				buffer[i] = Math.Abs(x[i] - median);
			}
			Array.Sort(buffer);
			if (length%2 == 0){
				s = (buffer[length/2 - 1] + buffer[length/2])/2.0;
			} else{
				s = buffer[length/2];
			}
			for (int i = 0; i < length; i++){
				buffer[i] = (x[i] - median)/(c*s + epsilon);
			}
			for (int i = 0; i < length; i++){
				sum += WeightBisquare(buffer[i])*WeightBisquare(buffer[i])*(x[i] - bw)*(x[i] - bw);
				if (buffer[i] < 1.0){
					sumw += (1.0 - buffer[i]*buffer[i])*(1.0 - 5.0*buffer[i]*buffer[i]);
				}
			}
			return (Math.Sqrt(sum)/Math.Abs(sumw));
		}

		private static double WeightBisquare(double x){
			return Math.Abs(x) <= 1.0 ? (1 - x*x)*(1 - x*x) : 0;
		}
	}
}