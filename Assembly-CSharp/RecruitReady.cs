using System;

public sealed class RecruitReady : TCPData<RecruitReady>
{
	public string roomId;

	public string userId;

	public bool isReady;

	public string hashValue;
}
