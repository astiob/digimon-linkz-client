using System;

public sealed class RandomSeedSyncData : TCPData<RandomSeedSyncData>
{
	public string playerUserId;

	public string hashValue;

	public int randomSeed;
}
