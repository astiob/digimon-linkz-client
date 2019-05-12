using System;
using UnityEngine;

[Serializable]
public class ToleranceShifter
{
	private const int clampMin = 0;

	private const int clampMax = 3;

	[Range(0f, 3f)]
	[SerializeField]
	private int _none;

	[Range(0f, 3f)]
	[SerializeField]
	private int _red;

	[SerializeField]
	[Range(0f, 3f)]
	private int _blue;

	[Range(0f, 3f)]
	[SerializeField]
	private int _yellow;

	[Range(0f, 3f)]
	[SerializeField]
	private int _green;

	[SerializeField]
	[Range(0f, 3f)]
	private int _white;

	[SerializeField]
	[Range(0f, 3f)]
	private int _black;

	[Range(0f, 3f)]
	[SerializeField]
	private int _poison;

	[Range(0f, 3f)]
	[SerializeField]
	private int _confusion;

	[Range(0f, 3f)]
	[SerializeField]
	private int _paralysis;

	[SerializeField]
	[Range(0f, 3f)]
	private int _sleep;

	[Range(0f, 3f)]
	[SerializeField]
	private int _stun;

	[SerializeField]
	[Range(0f, 3f)]
	private int _skillLock;

	[Range(0f, 3f)]
	[SerializeField]
	private int _instantDeath;

	public ToleranceShifter()
	{
		this._none = 0;
		this._red = 0;
		this._blue = 0;
		this._yellow = 0;
		this._green = 0;
		this._white = 0;
		this._black = 0;
		this._poison = 0;
		this._confusion = 0;
		this._paralysis = 0;
		this._sleep = 0;
		this._stun = 0;
		this._skillLock = 0;
		this._instantDeath = 0;
	}

	public ToleranceShifter(int none, int red, int blue, int yellow, int green, int white, int black, int poison, int confusion, int paralysis, int sleep, int stun, int skillLock, int instantDeath)
	{
		this._none = Mathf.Clamp(none, 0, 3);
		this._red = Mathf.Clamp(red, 0, 3);
		this._blue = Mathf.Clamp(blue, 0, 3);
		this._yellow = Mathf.Clamp(yellow, 0, 3);
		this._green = Mathf.Clamp(green, 0, 3);
		this._white = Mathf.Clamp(white, 0, 3);
		this._black = Mathf.Clamp(black, 0, 3);
		this._poison = Mathf.Clamp(poison, 0, 3);
		this._confusion = Mathf.Clamp(confusion, 0, 3);
		this._paralysis = Mathf.Clamp(paralysis, 0, 3);
		this._sleep = Mathf.Clamp(sleep, 0, 3);
		this._stun = Mathf.Clamp(stun, 0, 3);
		this._skillLock = Mathf.Clamp(skillLock, 0, 3);
		this._instantDeath = Mathf.Clamp(instantDeath, 0, 3);
	}

	public int none
	{
		get
		{
			return this._none;
		}
		set
		{
			this._none = Mathf.Clamp(value, 0, 0);
		}
	}

	public int red
	{
		get
		{
			return this._red;
		}
		set
		{
			this._red = Mathf.Clamp(value, 0, 0);
		}
	}

	public int blue
	{
		get
		{
			return this._blue;
		}
		set
		{
			this._blue = Mathf.Clamp(value, 0, 0);
		}
	}

	public int yellow
	{
		get
		{
			return this._yellow;
		}
		set
		{
			this._yellow = Mathf.Clamp(value, 0, 0);
		}
	}

	public int green
	{
		get
		{
			return this._green;
		}
		set
		{
			this._green = Mathf.Clamp(value, 0, 0);
		}
	}

	public int white
	{
		get
		{
			return this._white;
		}
		set
		{
			this._white = Mathf.Clamp(value, 0, 0);
		}
	}

	public int black
	{
		get
		{
			return this._black;
		}
		set
		{
			this._black = Mathf.Clamp(value, 0, 0);
		}
	}

	public int poison
	{
		get
		{
			return this._poison;
		}
		set
		{
			this._poison = Mathf.Clamp(value, 0, 0);
		}
	}

	public int confusion
	{
		get
		{
			return this._confusion;
		}
		set
		{
			this._confusion = Mathf.Clamp(value, 0, 0);
		}
	}

	public int paralysis
	{
		get
		{
			return this._paralysis;
		}
		set
		{
			this._paralysis = Mathf.Clamp(value, 0, 0);
		}
	}

	public int sleep
	{
		get
		{
			return this._sleep;
		}
		set
		{
			this._sleep = Mathf.Clamp(value, 0, 0);
		}
	}

	public int stun
	{
		get
		{
			return this._stun;
		}
		set
		{
			this._stun = Mathf.Clamp(value, 0, 0);
		}
	}

	public int skillLock
	{
		get
		{
			return this._skillLock;
		}
		set
		{
			this._skillLock = Mathf.Clamp(value, 0, 0);
		}
	}

	public int instantDeath
	{
		get
		{
			return this._instantDeath;
		}
		set
		{
			this._instantDeath = Mathf.Clamp(value, 0, 0);
		}
	}

	private static Strength ShiftingTolerance(Strength strength, int values)
	{
		switch (strength)
		{
		case Strength.None:
			if (values == 1)
			{
				return Strength.Strong;
			}
			if (values >= 3)
			{
				return Strength.Invalid;
			}
			break;
		case Strength.Strong:
			if (values >= 3)
			{
				return Strength.Invalid;
			}
			break;
		case Strength.Weak:
			if (values == 1)
			{
				return Strength.None;
			}
			if (values == 2)
			{
				return Strength.Strong;
			}
			if (values >= 3)
			{
				return Strength.Invalid;
			}
			break;
		}
		return strength;
	}

	public Tolerance GetShiftedTolerance(Tolerance tolerance)
	{
		return new Tolerance(ToleranceShifter.ShiftingTolerance(tolerance.none, this.none), ToleranceShifter.ShiftingTolerance(tolerance.red, this.red), ToleranceShifter.ShiftingTolerance(tolerance.blue, this.blue), ToleranceShifter.ShiftingTolerance(tolerance.yellow, this.yellow), ToleranceShifter.ShiftingTolerance(tolerance.green, this.green), ToleranceShifter.ShiftingTolerance(tolerance.white, this.white), ToleranceShifter.ShiftingTolerance(tolerance.black, this.black), ToleranceShifter.ShiftingTolerance(tolerance.poison, this.poison), ToleranceShifter.ShiftingTolerance(tolerance.confusion, this.confusion), ToleranceShifter.ShiftingTolerance(tolerance.paralysis, this.paralysis), ToleranceShifter.ShiftingTolerance(tolerance.sleep, this.sleep), ToleranceShifter.ShiftingTolerance(tolerance.stun, this.stun), ToleranceShifter.ShiftingTolerance(tolerance.skillLock, this.skillLock), ToleranceShifter.ShiftingTolerance(tolerance.instantDeath, this.instantDeath));
	}
}
