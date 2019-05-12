using System;

namespace Mono.Math.Prime.Generator
{
	internal class SequentialSearchPrimeGeneratorBase : PrimeGeneratorBase
	{
		protected virtual BigInteger GenerateSearchBase(int bits, object context)
		{
			BigInteger bigInteger = BigInteger.GenerateRandom(bits);
			bigInteger.SetBit(0u);
			return bigInteger;
		}

		public override BigInteger GenerateNewPrime(int bits)
		{
			return this.GenerateNewPrime(bits, null);
		}

		public virtual BigInteger GenerateNewPrime(int bits, object context)
		{
			BigInteger bigInteger = this.GenerateSearchBase(bits, context);
			uint num = bigInteger % 3234846615u;
			int trialDivisionBounds = this.TrialDivisionBounds;
			uint[] smallPrimes = BigInteger.smallPrimes;
			for (;;)
			{
				if (num % 3u != 0u)
				{
					if (num % 5u != 0u)
					{
						if (num % 7u != 0u)
						{
							if (num % 11u != 0u)
							{
								if (num % 13u != 0u)
								{
									if (num % 17u != 0u)
									{
										if (num % 19u != 0u)
										{
											if (num % 23u != 0u)
											{
												if (num % 29u != 0u)
												{
													int num2 = 10;
													while (num2 < smallPrimes.Length && (ulong)smallPrimes[num2] <= (ulong)((long)trialDivisionBounds))
													{
														if (bigInteger % smallPrimes[num2] == 0u)
														{
															goto IL_105;
														}
														num2++;
													}
													if (this.IsPrimeAcceptable(bigInteger, context))
													{
														if (this.PrimalityTest(bigInteger, this.Confidence))
														{
															break;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				IL_105:
				num += 2u;
				if (num >= 3234846615u)
				{
					num -= 3234846615u;
				}
				bigInteger.Incr2();
			}
			return bigInteger;
		}

		protected virtual bool IsPrimeAcceptable(BigInteger bi, object context)
		{
			return true;
		}
	}
}
