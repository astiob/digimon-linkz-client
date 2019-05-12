using System;
using UniRx.Toolkit;
using UnityEngine;

public class EffectPool : ObjectPool<EffectParamsGeneric>
{
	private readonly EffectParamsGeneric _basePrefab;

	private readonly Transform _paren;

	private readonly string _key;

	public EffectPool()
	{
	}

	public EffectPool(string key, Transform paren, EffectParamsGeneric prefab)
	{
		this._key = key;
		this._paren = paren;
		this._basePrefab = prefab;
	}

	protected override void OnBeforeRent(EffectParamsGeneric instance)
	{
	}

	protected override EffectParamsGeneric CreateInstance()
	{
		EffectParamsGeneric effectParamsGeneric = UnityEngine.Object.Instantiate<EffectParamsGeneric>(this._basePrefab);
		effectParamsGeneric.name = this._basePrefab.name;
		effectParamsGeneric.PoolKey = this._key;
		effectParamsGeneric.transform.SetParent(this._paren);
		return effectParamsGeneric;
	}
}
