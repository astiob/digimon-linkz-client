using System;

public class BattleInvariant
{
	private int threshold;

	public void Add(BattleInvariant.Type type)
	{
		this.threshold += this.GetValue(type);
	}

	public void Remove(BattleInvariant.Type type)
	{
		this.threshold -= this.GetValue(type);
	}

	public BattleInvariant.Type type
	{
		get
		{
			if (this.threshold > 0)
			{
				return BattleInvariant.Type.Up;
			}
			if (this.threshold < 0)
			{
				return BattleInvariant.Type.Down;
			}
			return BattleInvariant.Type.Non;
		}
	}

	private int GetValue(BattleInvariant.Type type)
	{
		switch (type)
		{
		default:
			return 0;
		case BattleInvariant.Type.Up:
			return 1;
		case BattleInvariant.Type.Down:
			return -1;
		}
	}

	public enum Type
	{
		Non,
		Up,
		Down
	}
}
