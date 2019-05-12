using System;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class RandomRotater : MonoBehaviour
{
	public Vector3 _value = Vector3.zero;

	public Vector3 _min = Vector3.zero;

	public Vector3 _max = new Vector3(360f, 360f, 360f);

	public bool onEnableAutoRotate;

	public void RandomRotation()
	{
		this._value.x = UnityEngine.Random.Range(this._min.x, this._max.x);
		this._value.y = UnityEngine.Random.Range(this._min.y, this._max.y);
		this._value.z = UnityEngine.Random.Range(this._min.z, this._max.z);
		base.transform.rotation = Quaternion.Euler(this._value);
	}

	private void OnEnable()
	{
		if (this.onEnableAutoRotate)
		{
			this.RandomRotation();
		}
	}
}
