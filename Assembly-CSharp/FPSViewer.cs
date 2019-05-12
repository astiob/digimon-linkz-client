using System;
using UnityEngine;

public class FPSViewer : MonoBehaviour
{
	private float timeA;

	public int fps;

	public int lastFPS;

	public GUIStyle textStyle;

	private void Start()
	{
		this.timeA = Time.timeSinceLevelLoad;
	}

	private void Update()
	{
		if (Time.timeSinceLevelLoad - this.timeA <= 1f)
		{
			this.fps++;
		}
		else
		{
			this.lastFPS = this.fps + 1;
			this.timeA = Time.timeSinceLevelLoad;
			this.fps = 0;
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(450f, 5f, 30f, 30f), string.Empty + this.lastFPS, this.textStyle);
	}
}
