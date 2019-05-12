using System;
using UnityEngine;

public class StageTextureScroll : MonoBehaviour
{
	[SerializeField]
	public int materialIndex;

	[SerializeField]
	public Vector2 uvOffset = new Vector2(1f, 0f);

	[SerializeField]
	public float animationRate = 1f;

	[SerializeField]
	public string textureName = "_MainTex";

	[SerializeField]
	public StageTextureScroll.ScrollType scrollType;

	[SerializeField]
	public AnimationCurve scrollCurve = AnimationCurve.Linear(0f, 0f, 5f, 1f);

	[SerializeField]
	public AnimationCurve pingPongCurve = AnimationCurve.EaseInOut(0f, -1f, 5f, 1f);

	[SerializeField]
	public AnimationCurve stretchCurve = AnimationCurve.EaseInOut(0f, 0f, 5f, 40f);

	[SerializeField]
	public AnimationCurve rotateCurve = AnimationCurve.Linear(0f, 0f, 5f, 359f);

	[SerializeField]
	public bool useTextureMatrix;

	[SerializeField]
	public bool useScrollCurve = true;

	private Material[] materialArray;

	private Vector2 textureOffset = Vector2.zero;

	private Vector2 scroll = Vector2.zero;

	private Quaternion rotation = Quaternion.identity;

	private Vector3 translation = Vector3.zero;

	private Vector3 scale = Vector3.one;

	private Matrix4x4 textureMatrix = Matrix4x4.identity;

	private float timeValue;

	private void Start()
	{
		base.GetComponent<Renderer>().enabled = true;
		this.materialArray = base.GetComponent<Renderer>().materials;
		this.scrollCurve.postWrapMode = WrapMode.Loop;
		this.pingPongCurve.postWrapMode = WrapMode.PingPong;
		this.stretchCurve.postWrapMode = WrapMode.PingPong;
		this.rotateCurve.postWrapMode = WrapMode.Loop;
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		this.timeValue += Time.smoothDeltaTime;
		if (this.scrollType == StageTextureScroll.ScrollType.Normal)
		{
			if (this.useScrollCurve)
			{
				float num = this.scrollCurve.Evaluate(this.timeValue * this.animationRate);
				float x = num * this.uvOffset.x;
				float y = num * this.uvOffset.y;
				this.scroll.x = x;
				this.scroll.y = y;
			}
			else
			{
				float num2 = 0.2f;
				float num = this.timeValue * this.animationRate;
				float x = num * this.uvOffset.x * num2;
				float y = num * this.uvOffset.y * num2;
				this.scroll.x = x;
				this.scroll.y = y;
			}
			this.rotation = Quaternion.identity;
			this.translation.Set(this.scroll.x, this.scroll.y, 0f);
			this.scale = Vector3.one;
			this.textureMatrix.SetTRS(this.translation, this.rotation, this.scale);
			this.textureOffset.x = this.textureMatrix.GetColumn(3).x;
			this.textureOffset.y = this.textureMatrix.GetColumn(3).y;
		}
		else if (this.scrollType == StageTextureScroll.ScrollType.PingPong)
		{
			float d = this.pingPongCurve.Evaluate(this.timeValue);
			Vector2 vector = this.uvOffset * d;
			this.rotation = Quaternion.identity;
			this.translation.Set(vector.x, vector.y, 0f);
			this.scale = Vector3.one;
			this.textureMatrix.SetTRS(this.translation, this.rotation, this.scale);
			this.textureOffset.x = this.textureMatrix.GetColumn(3).x;
			this.textureOffset.y = this.textureMatrix.GetColumn(3).y;
		}
		else if (this.scrollType == StageTextureScroll.ScrollType.Rotate)
		{
			Vector2 vector2 = this.uvOffset;
			float z = this.rotateCurve.Evaluate(this.timeValue);
			this.rotation = Quaternion.Euler(0f, 0f, z);
			this.scale = Vector3.one;
			Matrix4x4 rhs = Matrix4x4.TRS(Vector3.zero, this.rotation, this.scale);
			Matrix4x4 rhs2 = Matrix4x4.TRS(-vector2, Quaternion.identity, this.scale);
			Matrix4x4 lhs = Matrix4x4.TRS(vector2, Quaternion.identity, this.scale);
			this.textureMatrix = lhs * rhs * rhs2;
			this.textureOffset.x = this.textureMatrix.GetColumn(0).x;
			this.textureOffset.y = this.textureMatrix.GetColumn(0).y;
		}
		else if (this.scrollType == StageTextureScroll.ScrollType.Stretch)
		{
			Vector2 vector3 = this.uvOffset;
			float num3 = this.stretchCurve.Evaluate(this.timeValue);
			float y2 = num3 * this.uvOffset.x;
			float x2 = num3 * this.uvOffset.y;
			this.rotation = Quaternion.Euler(x2, y2, 0f);
			this.scale = Vector3.one;
			Matrix4x4 rhs3 = Matrix4x4.TRS(Vector3.zero, this.rotation, this.scale);
			Matrix4x4 rhs4 = Matrix4x4.TRS(-vector3, Quaternion.identity, this.scale);
			Matrix4x4 lhs2 = Matrix4x4.TRS(vector3, Quaternion.identity, this.scale);
			this.textureMatrix = lhs2 * rhs3 * rhs4;
			this.textureOffset.x = this.textureMatrix.GetColumn(3).x;
			this.textureOffset.y = this.textureMatrix.GetColumn(3).y;
		}
		else
		{
			this.textureMatrix = Matrix4x4.identity;
		}
		if (base.GetComponent<Renderer>().enabled)
		{
			if (this.useTextureMatrix)
			{
				this.materialArray[this.materialIndex].SetMatrix("_Rotation", this.textureMatrix);
			}
			else
			{
				this.materialArray[this.materialIndex].SetTextureOffset(this.textureName, this.textureOffset);
			}
		}
	}

	public enum ScrollType
	{
		Normal,
		PingPong,
		Stretch,
		Rotate
	}
}
