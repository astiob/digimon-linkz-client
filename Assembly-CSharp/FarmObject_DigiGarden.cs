using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FarmObject_DigiGarden : FarmObject
{
	private static FarmObject_DigiGarden instance;

	private GameObject canEvolveParticle;

	[SerializeField]
	private GameObject growthPlate;

	[SerializeField]
	private Vector3 canEvolveParticlePosition = new Vector3(-1f, 0f, -5f);

	private void OnDestroy()
	{
		if (FarmObject_DigiGarden.instance == this)
		{
			FarmObject_DigiGarden.instance = null;
		}
	}

	public static FarmObject_DigiGarden Instance
	{
		get
		{
			return FarmObject_DigiGarden.instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.CreateCanEvolveParticle();
	}

	protected override void Start()
	{
		base.Start();
		if (FarmObject_DigiGarden.instance == null || FarmObject_DigiGarden.instance != this)
		{
			FarmObject_DigiGarden.instance = this;
			this.SetAutoActiveCanEvolveParticle();
		}
	}

	private void CreateCanEvolveParticle()
	{
		if (null == this.canEvolveParticle)
		{
			this.canEvolveParticle = (UnityEngine.Object.Instantiate(Resources.Load("Cutscenes/NewFX14")) as GameObject);
			this.canEvolveParticle.transform.SetParent(base.transform);
			this.canEvolveParticle.transform.localPosition = this.canEvolveParticlePosition;
		}
	}

	public void SetAutoActiveCanEvolveParticle()
	{
		bool flag = false;
		bool flag2 = false;
		base.CancelInvoke("SetActiveCanEvolveParticle");
		List<MonsterData> list = MonsterDataMng.Instance().GetMonsterDataList();
		list = MonsterDataMng.Instance().SelectMonsterDataList(list, MonsterFilterType.GROWING_IN_GARDEN);
		if (0 < list.Count)
		{
			int num = -1;
			flag2 = true;
			for (int i = 0; i < list.Count; i++)
			{
				MonsterData monsterData = list[i];
				if (monsterData.userMonster.IsEgg())
				{
					flag = true;
					break;
				}
				TimeSpan timeSpan = DateTime.Parse(monsterData.userMonster.growEndDate) - ServerDateTime.Now;
				if ((int)timeSpan.TotalSeconds <= 0)
				{
					flag = true;
					break;
				}
				if (num == -1 || num > (int)timeSpan.TotalSeconds)
				{
					num = (int)timeSpan.TotalSeconds;
				}
			}
			if (!flag)
			{
				base.Invoke("SetActiveCanEvolveParticle", (float)num);
			}
		}
		this.SetEvolveParticle(flag);
		this.SetGrowthPlate(flag2);
	}

	private void SetActiveCanEvolveParticle()
	{
		this.canEvolveParticle.SetActive(true);
	}

	public void SetEvolveParticle(bool isDisplay)
	{
		this.canEvolveParticle.SetActive(isDisplay);
	}

	public void DisbledEvolveParticle()
	{
		this.canEvolveParticle.SetActive(false);
		base.CancelInvoke("SetActiveCanEvolveParticle");
	}

	public void SetGrowthPlate(bool isDisplay)
	{
		if (null != this.growthPlate)
		{
			this.growthPlate.SetActive(isDisplay);
		}
	}

	public override void OnResumeFromCache()
	{
		this.SetAutoActiveCanEvolveParticle();
	}
}
