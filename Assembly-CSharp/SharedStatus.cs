using System;
using UnityEngine;

[Serializable]
public class SharedStatus
{
	public const int defaultSharedAp = 9;

	public const int maxSharedAp = 16;

	private static SharedStatus _currentSharedStatus;

	[SerializeField]
	private bool _isShared;

	[SerializeField]
	private int _sharedAp;

	private SharedAPMulti _sharedAPMulti;

	public SharedStatus()
	{
		this._isShared = false;
		this._sharedAp = 0;
		SharedStatus.currentSharedStatus = this;
	}

	public SharedStatus(int sharedAp, SharedAPMulti sharedAPMulti)
	{
		this._sharedAp = sharedAp;
		this._isShared = true;
		this._sharedAPMulti = sharedAPMulti;
	}

	public static SharedStatus currentSharedStatus
	{
		get
		{
			return SharedStatus._currentSharedStatus;
		}
		set
		{
			SharedStatus._currentSharedStatus = value;
		}
	}

	public static SharedStatus GetEmptyStatus()
	{
		return new SharedStatus();
	}

	public static void SetAllStaticStatus(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			characterStateControl.sharedStatus = SharedStatus._currentSharedStatus;
		}
	}

	public bool isShared
	{
		get
		{
			return this._isShared;
		}
	}

	public int sharedAp
	{
		get
		{
			return this._sharedAp;
		}
		set
		{
			this._sharedAp = Mathf.Clamp(value, 0, 16);
			this._sharedAPMulti.APNum = value;
		}
	}

	public void SetStatic()
	{
		SharedStatus.currentSharedStatus = this;
	}
}
