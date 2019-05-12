using System;

public static class StrengthExtension
{
	public static int ToInt(this Strength str)
	{
		switch (str)
		{
		case Strength.Strong:
			return 1;
		case Strength.Weak:
			return -1;
		case Strength.Drain:
			return 2;
		case Strength.Invalid:
			return 99;
		default:
			return 0;
		}
	}

	public static int ToInverseInt(this Strength str)
	{
		return -str.ToInt();
	}
}
