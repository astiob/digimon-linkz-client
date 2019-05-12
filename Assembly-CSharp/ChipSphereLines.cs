using System;
using UnityEngine;

public sealed class ChipSphereLines : MonoBehaviour
{
	private const string blueLineSpriteName = "Chip_Sphere_LINE_OK";

	private const string yellowLineSpriteName = "Chip_Sphere_LINE_ON";

	[Header("中から右上の光るライン")]
	[SerializeField]
	private UISprite middleToRightUp;

	[SerializeField]
	[Header("中から右下の光るライン")]
	private UISprite middleToRightDown;

	[Header("中から左下の光るライン")]
	[SerializeField]
	private UISprite middleToLeftDown;

	[Header("中から左上の光るライン")]
	[SerializeField]
	private UISprite middleToLeftUp;

	[SerializeField]
	[Header("中から左の光るライン")]
	private UISprite middleToLeft;

	[SerializeField]
	[Header("中から右の光るライン")]
	private UISprite middleToRight;

	[SerializeField]
	[Header("左の下から上の光るライン")]
	private UISprite leftDownToUp;

	[SerializeField]
	[Header("右の下から上の光るライン")]
	private UISprite rightDownToUp;

	[SerializeField]
	[Header("上の中から左の光るライン")]
	private UISprite upMiddleToLeft;

	[SerializeField]
	[Header("上の中から右の光るライン")]
	private UISprite upMiddleToRight;

	[SerializeField]
	[Header("レアキャプチャ限定ライン")]
	private GameObject[] rareOnlyLines;

	public void OpenMiddleToRightUp(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.middleToRightUp.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.middleToRightUp.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.middleToRightUp.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenMiddleToRightDown(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.middleToRightDown.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.middleToRightDown.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.middleToRightDown.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenMiddleToLeftDown(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.middleToLeftDown.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.middleToLeftDown.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.middleToLeftDown.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenMiddleToLeftUp(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.middleToLeftUp.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.middleToLeftUp.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.middleToLeftUp.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenMiddleToLeft(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.middleToLeft.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.middleToLeft.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.middleToLeft.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenMiddleToRight(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.middleToRight.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.middleToRight.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.middleToRight.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenLeftDownToUp(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.leftDownToUp.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.leftDownToUp.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.leftDownToUp.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenRightDownToUp(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.rightDownToUp.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.rightDownToUp.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.rightDownToUp.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenUpMiddleToLeft(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.upMiddleToLeft.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.upMiddleToLeft.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.upMiddleToLeft.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void OpenUpMiddleToRight(ChipSphereLines.LineType lineType)
	{
		bool state = lineType != ChipSphereLines.LineType.None;
		NGUITools.SetActiveSelf(this.upMiddleToRight.gameObject, state);
		if (lineType == ChipSphereLines.LineType.Yellow)
		{
			this.upMiddleToRight.spriteName = "Chip_Sphere_LINE_ON";
		}
		else if (lineType == ChipSphereLines.LineType.Blue)
		{
			this.upMiddleToRight.spriteName = "Chip_Sphere_LINE_OK";
		}
	}

	public void SetDegign(bool isRareCapture)
	{
		if (!isRareCapture)
		{
			NGUITools.SetActiveSelf(this.middleToLeft.gameObject, false);
			NGUITools.SetActiveSelf(this.middleToRight.gameObject, false);
			NGUITools.SetActiveSelf(this.leftDownToUp.gameObject, false);
			NGUITools.SetActiveSelf(this.rightDownToUp.gameObject, false);
			NGUITools.SetActiveSelf(this.upMiddleToLeft.gameObject, false);
			NGUITools.SetActiveSelf(this.upMiddleToRight.gameObject, false);
		}
		foreach (GameObject go in this.rareOnlyLines)
		{
			NGUITools.SetActiveSelf(go, isRareCapture);
		}
	}

	public enum LineType
	{
		None,
		Yellow,
		Blue
	}
}
