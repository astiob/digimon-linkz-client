using System;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class ManualAnimationSpeed : MonoBehaviour
{
	[SerializeField]
	private float animationSpeed = 1f;

	private Animation animation;

	private void Awake()
	{
		this.animation = base.GetComponent<Animation>();
	}

	private void Update()
	{
		this.animation[this.animation.clip.name].speed = this.animationSpeed;
	}
}
