using System;
using System.Collections;
using UnityEngine;

public sealed class DigimonModelPlayer : MonoBehaviour
{
	private bool isAnimation;

	public CharacterParams MonsterParams { private get; set; }

	public void OnDisplayClick()
	{
		if (!this.isAnimation)
		{
			base.StartCoroutine(this.RandomAnimateModel());
		}
	}

	private IEnumerator RandomAnimateModel()
	{
		if (this.MonsterParams == null || this.isAnimation)
		{
			yield break;
		}
		this.isAnimation = true;
		int rand = UnityEngine.Random.Range(0, 3);
		float animeClipLength = 0f;
		float playTime = 0f;
		switch (rand)
		{
		case 0:
			this.MonsterParams.PlayAnimationSmooth(CharacterAnimationType.win, SkillType.Attack, 0, null, null);
			animeClipLength = this.MonsterParams.AnimationClipLength;
			break;
		case 1:
			this.MonsterParams.PlayAnimationSmooth(CharacterAnimationType.eat, SkillType.Attack, 0, null, null);
			animeClipLength = this.MonsterParams.AnimationClipLength;
			break;
		case 2:
			this.MonsterParams.PlayAnimationSmooth(CharacterAnimationType.attacks, SkillType.Attack, 0, null, null);
			animeClipLength = this.MonsterParams.AnimationClipLength;
			break;
		}
		while (playTime < animeClipLength)
		{
			playTime += Time.deltaTime;
			yield return null;
		}
		this.isAnimation = false;
		yield break;
	}
}
