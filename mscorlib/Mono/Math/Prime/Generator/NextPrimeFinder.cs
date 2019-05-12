using System;

namespace Mono.Math.Prime.Generator
{
	internal class NextPrimeFinder : SequentialSearchPrimeGeneratorBase
	{
		protected override BigInteger GenerateSearchBase(int bits, object Context)
		{
			if (Context == null)
			{
				throw new ArgumentNullException("Context");
			}
			BigInteger bigInteger = new BigInteger((BigInteger)Context);
			bigInteger.SetBit(0u);
			return bigInteger;
		}
	}
}
