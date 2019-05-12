using System;
using UnityEngine;

public class FarmCameraDrawArea : MonoBehaviour
{
	public BoxCollider drawAreaClip;

	private float topLeftX;

	private float topLeftZ;

	private float topRightX;

	private float bottomLeftZ;

	private void Start()
	{
		FarmRoot instance = FarmRoot.Instance;
		Camera component = base.GetComponent<Camera>();
		GUICameraControll component2 = base.GetComponent<GUICameraControll>();
		component.orthographicSize = component2.CameraOrthographicSizeMax;
		float z = 0.1f;
		float num = Mathf.Abs(component.ViewportToWorldPoint(new Vector3(0f, 1f, z)).y - instance.transform.position.y);
		float num2 = Mathf.Abs(component.ViewportToWorldPoint(new Vector3(0f, 0f, z)).y - instance.transform.position.y);
		float f = component.transform.localEulerAngles.x * 0.0174532924f;
		float z2 = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 b = new Quaternion(component.transform.localRotation.x, 0f, 0f, 1f) * new Vector3(0f, 0f, z2);
		Vector3 vector = new Vector3(this.drawAreaClip.size.x / 2f, 0f, this.drawAreaClip.size.z / 2f) - b;
		Vector3 vector2 = new Vector3(-this.drawAreaClip.size.x / 2f, 0f, this.drawAreaClip.size.z / 2f) - b;
		z2 = Mathf.Abs(num2 / Mathf.Cos(f));
		b = new Quaternion(component.transform.localRotation.x, 0f, 0f, 1f) * new Vector3(0f, 0f, z2);
		Vector3 vector3 = new Vector3(-this.drawAreaClip.size.x / 2f, 0f, -this.drawAreaClip.size.z / 2f) - b;
		this.topLeftX = vector2.x;
		this.topLeftZ = vector2.z;
		this.topRightX = vector.x;
		this.bottomLeftZ = vector3.z;
	}

	public Vector3 AdjustCameraAdd(Camera camera, Vector3 add)
	{
		Vector3[] cameraArea = this.GetCameraArea(camera);
		float num = cameraArea[0].x + add.x;
		float num2 = cameraArea[2].x + add.x;
		float num3 = cameraArea[0].z + add.z;
		float num4 = cameraArea[1].z + add.z;
		Vector3 result = add;
		if (num <= this.topLeftX)
		{
			result.x += this.topLeftX - num;
		}
		if (num2 >= this.topRightX)
		{
			result.x -= num2 - this.topRightX;
		}
		if (num3 >= this.topLeftZ)
		{
			result.z -= num3 - this.topLeftZ;
		}
		if (num4 <= this.bottomLeftZ)
		{
			result.z += this.bottomLeftZ - num4;
		}
		return result;
	}

	public Vector3[] GetCameraArea(Camera camera)
	{
		float z = 0.1f;
		Vector3 vector = camera.ViewportToWorldPoint(new Vector3(0f, 1f, z));
		Vector3 vector2 = camera.ViewportToWorldPoint(new Vector3(1f, 0f, z));
		Vector3 vector3 = camera.ViewportToWorldPoint(new Vector3(1f, 1f, z));
		Vector3 vector4 = camera.ViewportToWorldPoint(new Vector3(0f, 0f, z));
		float angle = -base.transform.eulerAngles.y;
		vector = Quaternion.AngleAxis(angle, Vector3.up) * vector;
		vector4 = Quaternion.AngleAxis(angle, Vector3.up) * vector4;
		vector3 = Quaternion.AngleAxis(angle, Vector3.up) * vector3;
		vector2 = Quaternion.AngleAxis(angle, Vector3.up) * vector2;
		return new Vector3[]
		{
			vector,
			vector4,
			vector3,
			vector2
		};
	}
}
