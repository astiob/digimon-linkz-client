using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CharacterAnimationClip
{
	[FormerlySerializedAs("idle")]
	[SerializeField]
	private AnimationClip _idle;

	[FormerlySerializedAs("hit")]
	[SerializeField]
	private AnimationClip _hit;

	[SerializeField]
	[FormerlySerializedAs("guard")]
	private AnimationClip _guard;

	[SerializeField]
	[FormerlySerializedAs("revival")]
	private AnimationClip _revival;

	[FormerlySerializedAs("win")]
	[SerializeField]
	private AnimationClip _win;

	[SerializeField]
	private AnimationClip _eat;

	[SerializeField]
	private AnimationClip _move;

	[SerializeField]
	private AnimationClip _down;

	[SerializeField]
	private AnimationClip _getup;

	[SerializeField]
	private AnimationClip _normalAttack;

	[SerializeField]
	private AnimationClip[] _deathblowAttack = new AnimationClip[1];

	[SerializeField]
	private AnimationClip _inheritanceTechnique;

	private List<AnimationClip> animationList;

	public CharacterAnimationClip(CharacterAnimationClip clipBase, Animation animation)
	{
		Func<string, AnimationClip, AnimationClip> func = delegate(string key, AnimationClip animationClip)
		{
			AnimationClip clip = animation.GetClip(key);
			if (clip != null)
			{
				return clip;
			}
			return animationClip;
		};
		this.animationList = new List<AnimationClip>();
		this.animationList.Add(this._idle = func(clipBase._idle.name, this._idle));
		this.animationList.Add(this._hit = func(clipBase._hit.name, this._hit));
		this.animationList.Add(this._guard = func(clipBase._guard.name, this._guard));
		this.animationList.Add(this._revival = func(clipBase._revival.name, this._revival));
		this.animationList.Add(this._win = func(clipBase._win.name, this._win));
		this.animationList.Add(this._eat = func(clipBase._eat.name, this._eat));
		this.animationList.Add(this._move = func(clipBase._move.name, this._move));
		this.animationList.Add(this._getup = func(clipBase._getup.name, this._getup));
		this.animationList.Add(this._normalAttack = func(clipBase._normalAttack.name, this._normalAttack));
		this.animationList.Add(this._inheritanceTechnique = func(clipBase._inheritanceTechnique.name, this._inheritanceTechnique));
		this.animationList.Add(this._down = func(clipBase._down.name, this._down));
		this._deathblowAttack = new AnimationClip[clipBase._deathblowAttack.Length];
		for (int i = 0; i < this._deathblowAttack.Length; i++)
		{
			this.animationList.Add(this._deathblowAttack[i] = func(clipBase._deathblowAttack[i].name, this._deathblowAttack[i]));
		}
	}

	public AnimationClip idle
	{
		get
		{
			return this._idle;
		}
	}

	public AnimationClip hit
	{
		get
		{
			return this._hit;
		}
	}

	public AnimationClip guard
	{
		get
		{
			return this._guard;
		}
	}

	public AnimationClip revival
	{
		get
		{
			return this._revival;
		}
	}

	public AnimationClip win
	{
		get
		{
			return this._win;
		}
	}

	public AnimationClip eat
	{
		get
		{
			return this._eat;
		}
	}

	public AnimationClip move
	{
		get
		{
			return this._move;
		}
	}

	public AnimationClip down
	{
		get
		{
			return this._down;
		}
	}

	public AnimationClip getup
	{
		get
		{
			return this._getup;
		}
	}

	public AnimationClip[] GetUsedAnimationClips()
	{
		return this.animationList.Distinct<AnimationClip>().ToArray<AnimationClip>();
	}

	public AnimationClip GetAttackClip(SkillType attackType, int motionIndex = 0)
	{
		switch (attackType)
		{
		case SkillType.Attack:
			return this._normalAttack;
		case SkillType.Deathblow:
			return this._deathblowAttack[Mathf.Clamp(motionIndex, 0, this._deathblowAttack.Length - 1)];
		case SkillType.InheritanceTechnique:
			return this._inheritanceTechnique;
		default:
			return this._normalAttack;
		}
	}

	public Exception GetFindAttackClip(int motionIndex = 0)
	{
		if (this._deathblowAttack.Length <= motionIndex || 0 > motionIndex)
		{
			return new IndexOutOfRangeException();
		}
		if (!this._deathblowAttack[motionIndex])
		{
			return new NullReferenceException();
		}
		return new Exception();
	}
}
