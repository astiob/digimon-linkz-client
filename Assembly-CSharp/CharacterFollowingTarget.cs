using System;
using UnityEngine;

[AddComponentMenu("Digimon Effects/Tools/Character Following Target")]
public class CharacterFollowingTarget : MonoBehaviour
{
	[SerializeField]
	private CharacterTarget _characterTarget;

	[NonSerialized]
	private bool _onAutoUpdate;

	[NonSerialized]
	private Transform _following;

	public CharacterTarget characterTarget
	{
		get
		{
			return this._characterTarget;
		}
		set
		{
			this._characterTarget = value;
		}
	}

	public bool onAutoUpdate
	{
		get
		{
			return this._onAutoUpdate;
		}
		set
		{
			this._onAutoUpdate = value;
		}
	}

	public Transform following
	{
		set
		{
			this._following = value;
		}
	}

	public void ManualUpdate()
	{
		if (this._following != null)
		{
			base.transform.position = this._following.transform.position;
			base.transform.rotation = this._following.rotation;
		}
	}

	private void Update()
	{
		if (this._onAutoUpdate)
		{
			this.ManualUpdate();
		}
	}
}
