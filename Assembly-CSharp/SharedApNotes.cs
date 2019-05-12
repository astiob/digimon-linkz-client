using System;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class SharedApNotes : MonoBehaviour
{
	[SerializeField]
	[Header("APアイコン (有効)")]
	[FormerlySerializedAs("iconActive")]
	private UIBasicSprite _iconActive;

	[FormerlySerializedAs("iconDeactive")]
	[SerializeField]
	[Header("APアイコン (無効)")]
	private UIBasicSprite _iconDeactive;

	[SerializeField]
	[Header("AP UITweenerActivePlay")]
	[FormerlySerializedAs("tweenerActivePlays")]
	private UITweenerActivePlay _tweenerActivePlays;

	[SerializeField]
	[FormerlySerializedAs("multiAPUpEffects")]
	[Header("AP UPエフェクト")]
	private GameObject _multiAPUpEffect;

	[FormerlySerializedAs("multiAPActiveEffects")]
	[SerializeField]
	[Header("AP Activeエフェクト")]
	private GameObject _multiAPActiveEffect;

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
}
