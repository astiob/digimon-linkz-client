using System;
using UnityEngine;

[AddComponentMenu("Digimon Effects/Tools/Manual Start Animation")]
[RequireComponent(typeof(Animation))]
public class ManualStartAnimation : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	private float _startAnimationTime;

	[SerializeField]
	private ManualStartAnimation.AnimateSign _animateSign;

	[SerializeField]
	[Range(0f, 10f)]
	private float _speed = 1f;

	private void Start()
	{
		Animation component = base.GetComponent<Animation>();
		component[component.clip.name].normalizedTime = this._startAnimationTime;
		component[component.clip.name].speed = this.GetSpeed(this._animateSign) * this._speed;
	}

	private float GetSpeed(ManualStartAnimation.AnimateSign sign)
	{
		if (this._animateSign == ManualStartAnimation.AnimateSign.Plus)
		{
			return 1f;
		}
		return -1f;
	}

	public enum AnimateSign
	{
		Plus,
		Minus
	}
}
