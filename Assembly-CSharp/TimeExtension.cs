using System;
using UnityEngine;

public class TimeExtension
{
	public static float GetTimeScaleDivided(float time)
	{
		return (Time.timeScale <= 0f) ? 0f : (time / Time.timeScale);
	}
}
