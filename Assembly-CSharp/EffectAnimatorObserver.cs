using System;
using UnityEngine;

public sealed class EffectAnimatorObserver : MonoBehaviour
{
	private GameObject root;

	private void Awake()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if ("root" == child.name)
			{
				this.root = child.gameObject;
				break;
			}
		}
	}

	public bool IsPlaying()
	{
		return this.root.activeSelf;
	}

	public bool IsStoped()
	{
		return !this.root.activeSelf;
	}

	public void Play()
	{
		Animator component = base.GetComponent<Animator>();
		component.Play(0, 0, 0f);
	}
}
