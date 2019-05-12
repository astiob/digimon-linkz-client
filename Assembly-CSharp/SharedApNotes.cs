using System;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class SharedApNotes : MonoBehaviour
{
	[FormerlySerializedAs("iconActive")]
	[SerializeField]
	[Header("APアイコン (有効)")]
	private UIBasicSprite _iconActive;

	[SerializeField]
	[Header("APアイコン (無効)")]
	[FormerlySerializedAs("iconDeactive")]
	private UIBasicSprite _iconDeactive;

	[FormerlySerializedAs("tweenerActivePlays")]
	[Header("AP UITweenerActivePlay")]
	[SerializeField]
	private UITweenerActivePlay _tweenerActivePlays;

	[FormerlySerializedAs("multiAPUpEffects")]
	[Header("AP UPエフェクト")]
	[SerializeField]
	private GameObject _multiAPUpEffect;

	[Header("AP Activeエフェクト")]
	[FormerlySerializedAs("multiAPActiveEffects")]
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
