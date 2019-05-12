using System;

namespace Mono.Math.Prime.Generator
{
	internal abstract class PrimeGeneratorBase
	{
		public virtual ConfidenceFactor Confidence
		{
			get
			{
				return ConfidenceFactor.Medium;
			}
		}

		public virtual PrimalityTest PrimalityTest
		{
			get
			{
				return new PrimalityTest(PrimalityTests.RabinMillerTest);
			}
		}

		public virtual int TrialDivisionBounds
		{
			get
			{
				return 4000;
			}
		}

		protected bool PostTrialDivisionTests(BigInteger bi)
		{
			return this.PrimalityTest(bi, this.Confidence);
		}

		public abstract BigInteger GenerateNewPrime(int bits);
	}
}
