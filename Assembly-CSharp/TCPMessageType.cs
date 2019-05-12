using System;

public enum TCPMessageType
{
	None,
	EnemyTurnSync,
	RandomSeedSync,
	Emotion,
	X2,
	Attack,
	Revival1,
	Revival2,
	Revival3,
	Retire,
	Continue,
	Confirmation,
	LastConfirmation,
	Ping,
	Pong,
	BattleResult,
	RecruitShareUserInfo,
	RecruitRoomOut,
	RecruitReady,
	BattleStartConfirm,
	RecruitMemberList,
	RecruitRoomResume,
	PvPMatching,
	PvPBattleStart,
	Targat,
	RevivalCancel,
	ConnectionRecover
}
