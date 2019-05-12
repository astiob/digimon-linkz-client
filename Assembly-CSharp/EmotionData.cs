using System;

public sealed class EmotionData : TCPData<EmotionData>
{
	public string playerUserId;

	public string hashValue;

	public int emotionType;

	public string spriteName;

	public int iconSpritesIndex;
}
