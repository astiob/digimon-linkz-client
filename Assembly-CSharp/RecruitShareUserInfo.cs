using System;

public sealed class RecruitShareUserInfo : TCPData<RecruitShareUserInfo>
{
	public string roomId;

	public string userId;

	public string nickname;

	public string titleId;

	public int positionNumber;

	public bool isRequestMemberData;

	public bool isReady;

	public string hashValue;

	public GameWebAPI.Common_MonsterData monsInfo;

	public GameWebAPI.Common_MonsterData subMonsInfo;
}
