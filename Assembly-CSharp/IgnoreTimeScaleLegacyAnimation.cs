using System;
using UnityEngine;

public class IgnoreTimeScaleLegacyAnimation : MonoBehaviour
{
	private Animation anim;

	private void OnEnable()
	{
		this.anim = (this.anim ?? base.GetComponent<Animation>());
		string name = this.anim.clip.name;
		base.StartCoroutine(this.anim.Play(name, true, null));
	}
}
