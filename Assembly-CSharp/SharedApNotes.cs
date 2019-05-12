using System;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class SharedApNotes : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("iconActive")]
	[Header("APアイコン (有効)")]
	private UIBasicSprite _iconActive;

	[Header("APアイコン (無効)")]
	[SerializeField]
	[FormerlySerializedAs("iconDeactive")]
	private UIBasicSprite _iconDeactive;

	[Header("AP UITweenerActivePlay")]
	[SerializeField]
	[FormerlySerializedAs("tweenerActivePlays")]
	private UITweenerActivePlay _tweenerActivePlays;

	[Header("AP UPエフェクト")]
	[SerializeField]
	[FormerlySerializedAs("multiAPUpEffects")]
	private GameObject _multiAPUpEffect;

	[FormerlySerializedAs("multiAPActiveEffects")]
	[Header("AP Activeエフェクト")]
	[SerializeField]
	private GameObject _multiAPActiveEffect;

	private SharedApNotes.State state;

	public UIBasicSprite icon
	{
		get
		{
			return this._iconActive;
		}
	}

	public UIBasicSprite iconDeactive
	{
		get
		{
			return this._iconDeactive;
		}
	}

	public UITweenerActivePlay tweenerActivePlays
	{
		get
		{
			return this._tweenerActivePlays;
		}
	}

	public GameObject multiAPUpEffect
	{
		get
		{
			return this._multiAPUpEffect;
		}
	}

	public GameObject multiAPActiveEffect
	{
		get
		{
			return this._multiAPActiveEffect;
		}
	}

	public bool isAnimation
	{
		get
		{
			return this.state != SharedApNotes.State.None;
		}
	}

	private void Update()
	{
		SharedApNotes.State state = this.state;
		if (state != SharedApNotes.State.Up)
		{
			if (state == SharedApNotes.State.Down)
			{
				if (this.tweenerActivePlays.allDisabled)
				{
					this.StopDownAnimation();
				}
			}
		}
		else
		{
			Animator component = this.multiAPUpEffect.GetComponent<Animator>();
			if (component.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			{
				this.StopUpAnimation();
			}
		}
	}

	public void PlayUpAnimation()
	{
		this.state = SharedApNotes.State.Up;
		this.multiAPUpEffect.SetActive(true);
	}

	public void StopUpAnimation()
	{
		this.state = SharedApNotes.State.None;
		this.multiAPUpEffect.SetActive(false);
	}

	public void PlayDownAnimation()
	{
		this.state = SharedApNotes.State.Down;
		this.tweenerActivePlays.enabled = true;
	}

	public void StopDownAnimation()
	{
		this.state = SharedApNotes.State.None;
		this.tweenerActivePlays.enabled = false;
	}

	private enum State
	{
		None,
		Up,
		Down
	}
}
