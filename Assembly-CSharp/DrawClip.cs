using System;
using UnityEngine;

public class DrawClip : MonoBehaviour
{
	private float CLIP_UD_W = 800f;

	private float CLIP_UD_H = 800f;

	private float CLIP_RL_W = 1200f;

	private float CLIP_RL_H = 1200f;

	private GameObject goClipParts;

	private Vector3 ppos = new Vector3(0f, 0f, 0f);

	private Vector3 cpos = new Vector3(0f, 0f, 0f);

	private Vector3 scale = new Vector3(0f, 0f, 0f);

	private void Awake()
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "CLIP_PARTS")
			{
				this.goClipParts = transform.gameObject;
			}
		}
	}

	public void RemoveBoxCollider()
	{
		BoxCollider component = this.goClipParts.GetComponent<BoxCollider>();
		component.center = new Vector3(0f, 0f, 10000f);
	}

	public void SetUpUDRLParam(DrawClip.STATE state, float stPos, float zpos, float zsiz)
	{
		float num = 0f;
		float num2 = 0f;
		switch (state)
		{
		case DrawClip.STATE.CLIP_UP:
			num = this.CLIP_UD_W;
			num2 = this.CLIP_UD_H;
			this.ppos.x = 0f;
			this.ppos.y = stPos;
			this.ppos.z = zpos;
			this.cpos.x = 0f;
			this.cpos.y = num2 / 2f;
			this.cpos.z = 0f;
			break;
		case DrawClip.STATE.CLIP_DW:
			num = this.CLIP_UD_W;
			num2 = this.CLIP_UD_H;
			this.ppos.x = 0f;
			this.ppos.y = stPos;
			this.ppos.z = zpos;
			this.cpos.x = 0f;
			this.cpos.y = -num2 / 2f;
			this.cpos.z = 0f;
			break;
		case DrawClip.STATE.CLIP_RI:
			num = this.CLIP_RL_W;
			num2 = this.CLIP_RL_H;
			this.ppos.x = stPos;
			this.ppos.y = 0f;
			this.ppos.z = zpos;
			this.cpos.x = num / 2f;
			this.cpos.y = 0f;
			this.cpos.z = 0f;
			break;
		case DrawClip.STATE.CLIP_LE:
			num = this.CLIP_RL_W;
			num2 = this.CLIP_RL_H;
			this.ppos.x = stPos;
			this.ppos.y = 0f;
			this.ppos.z = zpos;
			this.cpos.x = -num / 2f;
			this.cpos.y = 0f;
			this.cpos.z = 0f;
			break;
		}
		this.scale.x = num;
		this.scale.y = num2;
		this.scale.z = zsiz;
		this.goClipParts.transform.localScale = this.scale;
		base.gameObject.transform.SetX(this.ppos.x);
		base.gameObject.transform.SetY(this.ppos.y);
		base.gameObject.transform.SetZ(this.ppos.z);
		this.goClipParts.transform.localPosition = this.cpos;
	}

	public GameObject GetClipParts()
	{
		return this.goClipParts;
	}

	public void SetUpRotateMask(Vector3 wpos, float zsiz, float wide)
	{
		base.gameObject.transform.SetX(wpos.x);
		base.gameObject.transform.SetY(wpos.y);
		base.gameObject.transform.SetZ(wpos.z + 0.1f);
		this.scale.x = wide;
		this.scale.y = wide;
		this.scale.z = zsiz;
		this.goClipParts.transform.localScale = this.scale;
		this.cpos.x = 0f;
		this.cpos.y = -wide / 2f;
		this.cpos.z = 0f;
		this.goClipParts.transform.localPosition = this.cpos;
		Quaternion localRotation = Quaternion.Euler(0f, 0f, 0f);
		base.gameObject.transform.localRotation = localRotation;
	}

	public void SetRotate(float rotZ)
	{
		Quaternion localRotation = Quaternion.Euler(0f, 0f, rotZ);
		base.gameObject.transform.localRotation = localRotation;
	}

	public enum STATE
	{
		CLIP_UP,
		CLIP_DW,
		CLIP_RI,
		CLIP_LE
	}
}
