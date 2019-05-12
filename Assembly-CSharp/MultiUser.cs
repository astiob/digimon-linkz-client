using System;

[Serializable]
public class MultiUser : ICloneable
{
	public bool isOwner;

	public string userName;

	public string userId;

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public override string ToString()
	{
		return string.Format("isOwner:{0}, userName:{1}, userId:{2}", this.isOwner, this.userName, this.userId);
	}
}
