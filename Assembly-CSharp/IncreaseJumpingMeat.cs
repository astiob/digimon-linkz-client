using System;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseJumpingMeat : MonoBehaviour
{
	[SerializeField]
	private List<Animation> meatsList = new List<Animation>();

	[SerializeField]
	private List<AnimationClip> animeClipsList = new List<AnimationClip>();

	[SerializeField]
	public UITexture txMeat;

	public void Act()
	{
		if (this.meatsList.Count == 0 || this.animeClipsList.Count == 0)
		{
			global::Debug.LogWarning("<color=red>meatsList or animeClipList is zero count.</color>");
			return;
		}
		foreach (Animation animation in this.meatsList)
		{
			if (!animation.isPlaying)
			{
				this.PlayAnimation(animation);
				return;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.meatsList[0].gameObject);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localScale = Vector3.one;
		Animation component = gameObject.GetComponent<Animation>();
		this.PlayAnimation(component);
		this.meatsList.Add(component);
	}

	private void PlayAnimation(Animation Anime)
	{
		Anime.clip = this.animeClipsList[UnityEngine.Random.Range(0, this.animeClipsList.Count)];
		Anime.enabled = true;
		Anime.Play();
	}
}
