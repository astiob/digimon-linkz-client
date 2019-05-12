using System;

public sealed class RecruitRoomOut : TCPData<RecruitRoomOut>
{
	public string roomId;

	public string userId;

	public int roomOutType;

	public int positionNumber;

	public string hashValue;
}
