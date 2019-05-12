using System;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
	public Texture2D fadeOutTexture;

	public float fadeSpeed = 0.3f;

	private int drawDepth = -1000;

	private float alpha = 1f;

	private float fadeDir = -1f;

	private void OnGUI()
	{
		this.alpha += this.fadeDir * this.fadeSpeed * Time.deltaTime;
		this.alpha = Mathf.Clamp01(this.alpha);
		GUI.color = new Color(1f, 1f, 1f, this.alpha);
		GUI.depth = this.drawDepth;
		GUI.DrawTexture(new Rect(-1f, -1f, (float)(Screen.width + 2), (float)(Screen.height + 2)), this.fadeOutTexture);
	}

	public void fadeIn()
	{
		this.fadeDir = -0.8f;
	}

	public void fadeOut()
	{
		this.fadeDir = 1.5f;
	}

	private void Start()
	{
		this.fadeOutTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		this.fadeOutTexture.SetPixel(0, 0, Color.black);
		this.fadeOutTexture.Apply();
		this.alpha = 1f;
		this.fadeIn();
	}
}
