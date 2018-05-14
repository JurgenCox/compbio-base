using System;
using System.Collections.Generic;

namespace BaseLibS.Num {
	/// <summary>
	/// Class containing utility methods for generating pseudo random numbers.
	/// </summary>
	public class Random2 {
		private readonly RandomType type;

		/// <summary>
		/// Intrinsic random number generator used for samping from uniform distribion.
		/// </summary>
		private readonly Random random;

		/// <summary>
		/// Temporary store needed for generating Gaussian random numbers.
		/// </summary>
		private bool iset;

		/// <summary>
		/// Temporary store needed for generating Gaussian random numbers.
		/// </summary>
		private double gset;

		/// <summary>
		/// Initializes a new instance of the Random class, using the specified seed value.
		/// </summary>
		/// <param name="seed">A number used to calculate a starting value for the pseudo-random number 
		/// sequence. If a negative number is specified, the absolute value of the number is used.</param>
		/// <param name="type"></param>
		public Random2(int seed, RandomType type) {
			this.type = type;
			if (type == RandomType.Csharp) {
				random = new Random(seed);
			} else {
				InitKnuth(seed);
			}
		}

		public Random2(int seed) : this(seed, RandomType.Knuth) { }

		/// <summary>
		/// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
		/// </summary>
		/// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
		public double NextDouble() {
			return type == RandomType.Csharp ? random.NextDouble() : Sample();
		}

		/// <summary>
		/// Returns a random integer that is within a specified range.
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number returned. 
		/// maxValue must be greater than or equal to minValue.</param>
		/// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; 
		/// that is, the range of return values includes minValue but not maxValue. If minValue equals 
		/// maxValue, minValue is returned.</returns>
		public int Next(int minValue, int maxValue) {
			return minValue + Next(maxValue - minValue);
		}

		/// <summary>
		/// Returns a non-negative random integer that is less than the specified maximum.
		/// </summary>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. 
		/// maxValue must be greater than or equal to 0.</param>
		/// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than maxValue; 
		/// that is, the range of return values ordinarily includes 0 but not maxValue. However, if 
		/// maxValue equals 0, maxValue is returned.</returns>
		public int Next(int maxValue) {
			if (maxValue < 0) {
				throw new ArgumentException("maxValue cannot be negative.");
			}
			return maxValue < 2 ? 0 : (int) (NextDouble() * maxValue);
		}

		/// <summary>
		/// This method generates a pseudo random number drawn from a normal distribution
		/// with zero mean and unit variance.
		/// </summary>
		/// <returns> The Gaussian random number.</returns>
		public double NextGaussian() {
			return NextGaussian(0, 1);
		}

		public bool NextBoolean() {
			return NextBoolean(0.5);
		}

		public bool NextBoolean(double pTrue) {
			return NextDouble() < pTrue;
		}

		public bool[] NextBooleanArray(int n) {
			return NextBooleanArray(n, 0.5);
		}

		public bool[] NextBooleanArray(int n, double pTrue) {
			bool[] result = new bool[n];
			for (int i = 0; i < n; i++) {
				result[i] = NextBoolean(pTrue);
			}
			return result;
		}

		/// <summary>
		/// Returns a random number uniformly distributed between min and max;
		/// </summary>
		/// <returns></returns>
		public double NextRange(double min, double max) {
			double range = max - min;
			double x = NextDouble();
			return min + x * range;
		}

		/// <summary>
		/// This method generates a pseudo random number drawn from a normal distribution
		/// with the given mean and standard deviation.
		/// </summary>
		/// <returns> The Gaussian random number.</returns>
		public double NextGaussian(double mean, double stddev) {
			double x = Gasdev();
			return x * stddev + mean;
		}

		/// <summary>
		/// This routine generates a random number between 0 and n inclusive,
		/// following the binomial distribution with probability p and n trials. The
		/// routine is based on the BTPE algorithm, described in:
		/// 
		/// Voratas Kachitvichyanukul and Bruce W. Schmeiser: Binomial Random Variate
		/// Generation Communications of the ACM, Volume 31, Number 2, February 1988,
		/// pages 216-222.
		/// 
		/// </summary>
		/// <param name="n">The number of trials.</param>
		/// <param name="p">The probability of a single event. This probability should be less than or equal to 0.5.</param>
		/// <returns>An integer drawn from a binomial distribution with parameters (p, n).</returns>
		public int NextBinomial(int n, double p) {
			return Binomial(n, p);
		}

		public int[] NextMultinomial(double[] source, int n) {
			return Multinomial(source, n, source.Length);
		}

		/// <summary>
		/// Produces a random permutation of the integers from 0 to n-1.
		/// </summary>
		/// <param name="n">The length of the vector of permuted integers.</param>
		/// <returns></returns>
		public int[] NextPermutation(int n) {
			int[] perm = ArrayUtils.ConsecutiveInts(n);
			for (int i = 0; i < n; i++) {
				int pos = Next(i, n);
				int tmp = perm[i];
				perm[i] = perm[pos];
				perm[pos] = tmp;
			}
			return perm;
		}

		public short[] NextPermutationShort(int n) {
			short[] perm = ArrayUtils.ConsecutiveShorts(n);
			for (int i = 0; i < n; i++) {
				int pos = Next(i, n);
				short tmp = perm[i];
				perm[i] = perm[pos];
				perm[pos] = tmp;
			}
			return perm;
		}

		/// <summary>
		/// Returns randomized training and test set indices for n-fold cross
		/// validation.
		/// </summary>
		/// <param name="n">The number of items the cross validation will be performed on.</param>
		/// <param name="nfold">The number of cross validation folds.</param>
		/// <param name="train">Contains on output <c>nfold</c> vectors of integers filled with the 
		/// indices of the training set for the particular fold.</param>
		/// <param name="test">Contains on output <c>nfold</c> vectors of integers filled with the 
		/// indices of the test set for the particular fold.</param>
		public void NextCrossValidationIndices(int n, int nfold, out int[][] train, out int[][] test) {
			if (nfold > n) {
				nfold = n;
			}
			int[] perm = NextPermutation(n);
			int[][] subsets = new int[nfold][];
			for (int i = 0; i < nfold; i++) {
				int start = (int) Math.Round(i * n / (double) nfold);
				int end = (int) Math.Round((i + 1) * n / (double) nfold);
				subsets[i] = ArrayUtils.SubArray(perm, start, end);
			}
			train = new int[nfold][];
			test = new int[nfold][];
			for (int i = 0; i < nfold; i++) {
				test[i] = subsets[i];
				train[i] = ArrayUtils.Concat(ArrayUtils.SubArray(subsets,
					ArrayUtils.RemoveAtIndex(ArrayUtils.ConsecutiveInts(nfold), i)));
			}
		}

		public void RandomSubsamplingIndices(int n, int nfold, int nsampling, out int[][] train, out int[][] test) {
			if (nfold > n) {
				nfold = n;
			}
			train = new int[nsampling][];
			test = new int[nsampling][];
			for (int i = 0; i < nsampling; i++) {
				int[] perm = NextPermutation(n);
				const int start = 0;
				int end = (int) Math.Round(n / (double) nfold);
				test[i] = ArrayUtils.SubArray(perm, start, end);
				train[i] = ArrayUtils.SubArray(perm, end, perm.Length);
			}
		}

		private double Gasdev() {
			if (!iset) {
				double rsq;
				double v1;
				double v2;
				do {
					v1 = 2.0 * NextDouble() - 1.0;
					v2 = 2.0 * NextDouble() - 1.0;
					rsq = v1 * v1 + v2 * v2;
				} while (rsq >= 1.0 || rsq == 0.0);
				double fac1 = Math.Sqrt(-2.0 * Math.Log(rsq) / rsq);
				gset = v1 * fac1;
				iset = true;
				return v2 * fac1;
			}
			iset = false;
			return gset;
		}

		/// <summary>
		/// Method doing the actual work of drawing from a binomial distribution.
		/// </summary>
		/// <param name="n">The number of trials.</param>
		/// <param name="p">The probability of a single event. This probability should be less than or equal to 0.5.</param>
		/// <returns>An integer drawn from a binomial distribution with parameters (p, n).</returns>
		private int Binomial(int n, double p) {
			double q = 1 - p;
			if (n * p < 30.0) {
				// Algorithm BINV
				double s = p / q;
				double a = (n + 1) * s;
				double r = Math.Exp(n * Math.Log(q));
				int x = 0;
				double u = NextDouble();
				while (true) {
					if (u < r) {
						return x;
					}
					u -= r;
					x++;
					r *= a / x - s;
				}
			} else {
				// Algorithm BTPE 
				// Step 0 
				double fm = n * p + p;
				int m = (int) Math.Floor(fm);
				double p1 = Math.Floor(2.195 * Math.Sqrt(n * p * q) - 4.6 * q) + 0.5;
				double xm = m + 0.5;
				double xl = xm - p1;
				double xr = xm + p1;
				double c = 0.134 + 20.5 / (15.3 + m);
				double a = (fm - xl) / (fm - xl * p);
				double b = (xr - fm) / (xr * q);
				double lambdal = a * (1.0 + 0.5 * a);
				double lambdar = b * (1.0 + 0.5 * b);
				double p2 = p1 * (1 + 2 * c);
				double p3 = p2 + c / lambdal;
				double p4 = p3 + c / lambdar;
				while (true) {
					// Step 1
					int y;
					double u = NextDouble();
					double v = NextDouble();
					u *= p4;
					if (u <= p1) {
						return (int) Math.Floor(xm - p1 * v + u);
					}
					// Step 2
					if (u > p2) {
						// Step 3
						if (u > p3) {
							// Step 4
							y = (int) (xr - Math.Log(v) / lambdar);
							if (y > n) {
								continue;
							}
							// Go to step 5
							v = v * (u - p3) * lambdar;
						} else {
							y = (int) (xl + Math.Log(v) / lambdal);
							if (y < 0) {
								continue;
							}
							// Go to step 5
							v = v * (u - p2) * lambdal;
						}
					} else {
						double x = xl + (u - p1) / c;
						v = v * c + 1.0 - Math.Abs(m - x + 0.5) / p1;
						if (v > 1) {
							continue;
						}
						// Go to step 5
						y = (int) x;
					}
					// Step 5
					// Step 5.0
					int k = Math.Abs(y - m);
					if (k > 20 && k < 0.5 * n * p * q - 1.0) {
						// Step 5.2
						double rho = k / (n * p * q) * ((k * (k / 3.0 + 0.625) + 0.1666666666666) / (n * p * q) + 0.5);
						double t = -k * k / (2 * n * p * q);
						double a2 = Math.Log(v);
						if (a2 < t - rho) {
							return y;
						}
						if (a2 > t + rho) {
							continue;
						}
						// Step 5.3
						double x1 = y + 1;
						double f1 = m + 1;
						double z = n + 1 - m;
						double w = n - y + 1;
						double x2 = x1 * x1;
						double f2 = f1 * f1;
						double z2 = z * z;
						double w2 = w * w;
						if (a2 > xm * Math.Log(f1 / x1) + (n - m + 0.5) * Math.Log(z / w) + (y - m) * Math.Log(w * p / (x1 * q)) +
						    (13860.0 - (462.0 - (132.0 - (99.0 - 140.0 / f2) / f2) / f2) / f2) / f1 / 166320.0 +
						    (13860.0 - (462.0 - (132.0 - (99.0 - 140.0 / z2) / z2) / z2) / z2) / z / 166320.0 +
						    (13860.0 - (462.0 - (132.0 - (99.0 - 140.0 / x2) / x2) / x2) / x2) / x1 / 166320.0 +
						    (13860.0 - (462.0 - (132.0 - (99.0 - 140.0 / w2) / w2) / w2) / w2) / w / 166320.0) {
							continue;
						}
						return y;
					}
					// Step 5.1 
					int i;
					double s = p / q;
					double aa = s * (n + 1);
					double f = 1.0;
					for (i = m; i < y; f *= aa / ++i - s) { }
					for (i = y; i < m; f /= aa / ++i - s) { }
					if (v > f) {
						continue;
					}
					return y;
				}
			}
		}

		/// <summary>
		/// This function generates a vector of random variates from a multinomial distribution.
		/// </summary>
		/// <param name="source">An input array containing the probability or fraction of each color in the urn.</param>
		/// <param name="n">The number of balls drawn from the urn.</param>
		/// <param name="colors">The number of possible colors.</param>
		/// <returns>The number of balls of each color.</returns>
		private int[] Multinomial(IList<double> source, int n, int colors) {
			int[] destination = new int[source.Count];
			double s;
			double sum;
			int i;
			if (n < 0 || colors < 0) {
				throw new Exception("Parameter negative in multinomial function");
			}
			if (colors == 0) {
				return new int[0];
			}
			// compute sum of probabilities
			for (i = 0, sum = 0; i < colors; i++) {
				s = source[i];
				if (s < 0) {
					throw new Exception("Parameter negative in multinomial function");
				}
				sum += s;
			}
			if (sum == 0 && n > 0) {
				throw new Exception("Zero sum in multinomial function");
			}
			for (i = 0; i < colors - 1; i++) {
				// generate output by calling binomial (colors-1) times
				s = source[i];
				int x = sum <= s ? n : Binomial(n, s / sum);
				n -= x;
				sum -= s;
				destination[i] = x;
			}
			// get the last one
			destination[i] = n;
			return destination;
		}

		private const int mbig = int.MaxValue;
		private const int mseed = 161803398;
		private int inext;
		private int inextp;
		private readonly int[] seedArray = new int[56];

		public void InitKnuth(int seed) {
			//Initialize our seed array.
			//This algorithm comes from Numerical Recipes in C (2nd Ed.)
			int subtraction = seed == int.MinValue ? int.MaxValue : Math.Abs(seed);
			int mj = mseed - subtraction;
			seedArray[55] = mj;
			int mk = 1;
			for (int i = 1; i < 55; i++) {
				//Apparently the range [1..55] is special (Knuth) and so we're wasting the 0'th position.
				int ii = (21 * i) % 55;
				seedArray[ii] = mk;
				mk = mj - mk;
				if (mk < 0) mk += mbig;
				mj = seedArray[ii];
			}
			for (int k = 1; k < 5; k++) {
				for (int i = 1; i < 56; i++) {
					seedArray[i] -= seedArray[1 + (i + 30) % 55];
					if (seedArray[i] < 0) seedArray[i] += mbig;
				}
			}
			inext = 0;
			inextp = 21;
		}

		private double Sample() {
			//Including this division at the end gives us significantly improved
			//random number distribution.
			return InternalSample() * (1.0 / mbig);
		}

		private int InternalSample() {
			int locINext = inext;
			int locINextp = inextp;
			if (++locINext >= 56) locINext = 1;
			if (++locINextp >= 56) locINextp = 1;
			int retVal = seedArray[locINext] - seedArray[locINextp];
			if (retVal == mbig) retVal--;
			if (retVal < 0) retVal += mbig;
			seedArray[locINext] = retVal;
			inext = locINext;
			inextp = locINextp;
			return retVal;
		}
	}
}