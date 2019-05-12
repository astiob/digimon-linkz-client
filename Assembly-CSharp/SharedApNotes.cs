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
	[Header("APアイコン (無効)")]
	[SerializeField]
	private UIBasicSprite _iconDeactive;

	[SerializeField]
	[FormerlySerializedAs("tweenerActivePlays")]
	[Header("AP UITweenerActivePlay")]
	private UITweenerActivePlay _tweenerActivePlays;

	[Header("AP UPエフェクト")]
	[SerializeField]
	[FormerlySerializedAs("multiAPUpEffects")]
	private GameObject _multiAPUpEffect;

	[FormerlySerializedAs("multiAPActiveEffects")]
	[Header("AP Activeエフェクト")]
	[SerializeField]
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
