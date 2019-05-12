using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnPointParams : MonoBehaviour
{
	[FormerlySerializedAs("playersSpawnPoint")]
	[SerializeField]
	private Transform[] _playersSpawnPoint;

	[SerializeField]
	[FormerlySerializedAs("enemiesSpawnPoint")]
	private Transform[] _enemiesSpawnPoint;

	[SerializeField]
	private int[] _enemiesTargetSelectOrder;

	public Transform[] playersSpawnPoint
	{
		get
		{
			return this._playersSpawnPoint;
		}
	}

	public Transform[] enemiesSpawnPoint
	{
		get
		{
			return this._enemiesSpawnPoint;
		}
	}

	public Transform stageSpawnPoint
	{
		get
		{
			return base.transform;
		}
	}

	public int[] enemiesTargetSelectOrder
	{
		get
		{
			return this._enemiesTargetSelectOrder;
		}
	}

	public Vector3[] GetTotalSpanwPointPosition()
	{
		List<Vector3> list = new List<Vector3>();
		foreach (Transform transform in this._playersSpawnPoint)
		{
			list.Add(transform.position);
		}
		foreach (Transform transform2 in this._enemiesSpawnPoint)
		{
			list.Add(transform2.position);
		}
		return list.ToArray();
	}

	public Vector3[] GetHalfSpanwPointPosition()
	{
		Vector3 localScale = base.transform.localScale;
		base.transform.localScale = new Vector3(base.transform.localScale.x * 0.5f, base.transform.localScale.y, base.transform.localScale.z);
		List<Vector3> list = new List<Vector3>();
		foreach (Transform transform in this._playersSpawnPoint)
		{
			list.Add(transform.position);
		}
		foreach (Transform transform2 in this._enemiesSpawnPoint)
		{
			list.Add(transform2.position);
		}
		base.transform.localScale = localScale;
		return list.ToArray();
	}
}
