using System;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
	[SerializeField]
	private float decay;

	[SerializeField]
	private float intensity;

	private Vector3 shakePosition;

	private Vector3 originalPosition;

	private Action actionShakeFinished;

	public void StartShake(float intensity, float decay, Action completed)
	{
		base.enabled = true;
		this.intensity = intensity;
		this.decay = decay;
		this.originalPosition = base.transform.localPosition;
		this.actionShakeFinished = completed;
	}

	public void StopShake(float decay, Action completed)
	{
		if (base.enabled)
		{
			this.decay = decay;
			this.actionShakeFinished = completed;
		}
	}

	public void SuspendShake()
	{
		if (base.enabled)
		{
			this.intensity = 0f;
		}
	}

	public void ResetPosition()
	{
		base.transform.localPosition = this.originalPosition;
	}

	private void Update()
	{
		if (0f < this.intensity)
		{
			this.shakePosition.x = (UnityEngine.Random.value - 0.5f) * this.intensity;
			this.shakePosition.y = (UnityEngine.Random.value - 0.5f) * this.intensity;
			this.intensity -= this.decay;
			base.transform.localPosition = this.originalPosition + this.shakePosition;
		}
		else
		{
			base.transform.localPosition = this.originalPosition;
			if (this.actionShakeFinished != null)
			{
				this.actionShakeFinished();
			}
			base.enabled = false;
		}
	}
}
