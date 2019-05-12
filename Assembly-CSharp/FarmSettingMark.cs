using System;
using UnityEngine;

public class FarmSettingMark : MonoBehaviour
{
	[SerializeField]
	private GameObject okMark;

	[SerializeField]
	private GameObject ngMark;

	public void SetSize(int sizeX, int sizeY, Vector2 gridSize)
	{
		float scaleX = (float)sizeX * gridSize.x;
		float scaleY = (float)sizeY * gridSize.y;
		this.SetScale(this.okMark, scaleX, scaleY);
		this.SetScale(this.ngMark, scaleX, scaleY);
		this.SetMaterialUV(this.okMark, sizeX, sizeY);
		this.SetMaterialUV(this.ngMark, sizeX, sizeY);
	}

	private void SetScale(GameObject mark, float scaleX, float scaleY)
	{
		Vector3 localScale = mark.transform.localScale;
		localScale.x = scaleX;
		localScale.y = scaleY;
		mark.transform.localScale = localScale;
	}

	private void SetMaterialUV(GameObject mark, int width, int height)
	{
		MeshFilter component = mark.GetComponent<MeshFilter>();
		component.mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f + (float)width, 0f + (float)height),
			new Vector2(0f + (float)width, 0f),
			new Vector2(0f, 0f + (float)height)
		};
	}

	public void SetColor(bool placeable)
	{
		this.okMark.SetActive(placeable);
		this.ngMark.SetActive(!placeable);
	}

	public void InactiveColor()
	{
		this.okMark.SetActive(false);
		this.ngMark.SetActive(false);
	}

	public void SetParent(GameObject parent)
	{
		base.transform.parent = parent.transform;
		base.transform.localScale = Vector3.one;
		base.transform.localPosition = Vector3.zero;
		base.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
	}
}
