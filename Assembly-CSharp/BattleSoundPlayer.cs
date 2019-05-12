using System;
using System.Collections.Generic;
using UnityExtension;

public class BattleSoundPlayer : BattleFunctionBase
{
	private List<string> effectSe = new List<string>();

	public void AddEffectSe(string seName)
	{
		if (seName == null || seName.Length == 0)
		{
			return;
		}
		if (!this.effectSe.Contains(seName) && !BattleFunctionUtility.IsEmptyPath(seName))
		{
			this.effectSe.Add(seName);
		}
	}

	public void LoadSound()
	{
		this.LoadBGM();
		this.LoadSE();
	}

	private void LoadBGM()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		for (int i = 0; i < base.hierarchyData.batteWaves.Length; i++)
		{
			if (!list2.Contains(base.hierarchyData.batteWaves[i].bgmId) && !BattleFunctionUtility.IsEmptyPath(base.hierarchyData.batteWaves[i].bgmId))
			{
				list2.Add(base.hierarchyData.batteWaves[i].bgmId);
			}
			if (!list2.Contains(base.hierarchyData.batteWaves[i].changedBgmId) && !BattleFunctionUtility.IsEmptyPath(base.hierarchyData.batteWaves[i].changedBgmId) && !BoolExtension.AllMachValue(false, base.hierarchyData.batteWaves[i].enemiesBossFlag))
			{
				list2.Add(base.hierarchyData.batteWaves[i].changedBgmId);
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			list.Add(ResourcesPath.CreatePath(new string[]
			{
				"BGM",
				list2[j],
				"sound"
			}));
		}
		base.stateManager.soundManager.PreLoadAudio(list.ToArray(), list2.ToArray());
	}

	private void LoadSE()
	{
		List<string> list = new List<string>();
		this.AddEffectSe("bt_207");
		this.AddEffectSe("bt_010");
		this.AddEffectSe(string.Empty);
		this.AddEffectSe(string.Empty);
		for (int i = 0; i < this.effectSe.Count; i++)
		{
			list.Add(ResourcesPath.CreatePath(new string[]
			{
				"SE",
				this.effectSe[i],
				"sound"
			}));
		}
		this.effectSe.Add("bt_541Test");
		list.Add(ResourcesPath.CreatePath(new string[]
		{
			"SEInternal/Battle",
			"bt_541",
			"sound"
		}));
		base.stateManager.soundManager.PreLoadAudio(list.ToArray(), this.effectSe.ToArray());
		this.effectSe.Clear();
	}

	public override void BattleTriggerInitialize()
	{
		base.battleStateData.seValue = new float?(this.soundManager.VolumeSE);
		base.battleStateData.pauseBgmVolume = new float?(this.soundManager.VolumeBGM);
	}

	public override void BattleEndBefore()
	{
		base.BattleEndBefore();
		this.SetVolumeSE(false);
	}

	private SoundMng soundManager
	{
		get
		{
			return base.stateManager.soundManager;
		}
	}

	public void TryPlaySE(HitEffectParams hitEffect)
	{
		if (hitEffect != null)
		{
			this.TryPlaySE(hitEffect.seId, 0f, false);
		}
	}

	public void TryPlaySE(HitEffectParams[] hitEffect)
	{
		if (hitEffect != null && hitEffect.Length > 0)
		{
			this.TryPlaySE(hitEffect[0]);
		}
	}

	public void TryStopSE(HitEffectParams hitEffect)
	{
		if (hitEffect != null)
		{
			this.TryStopSE(hitEffect.seId, 0f);
		}
	}

	public void TryStopSE(HitEffectParams[] hitEffect)
	{
		if (hitEffect != null && hitEffect.Length > 0)
		{
			this.TryStopSE(hitEffect[0]);
		}
	}

	public void TryPlaySE(AlwaysEffectParams alwaysEffect, AlwaysEffectState state)
	{
		if (alwaysEffect == null)
		{
			return;
		}
		switch (state)
		{
		case AlwaysEffectState.In:
			this.TryPlaySE(alwaysEffect.inSeId, 0f, false);
			this.TryPlaySE(alwaysEffect.alwaysSeId, 0.1f, true);
			break;
		case AlwaysEffectState.Always:
			this.TryPlaySE(alwaysEffect.alwaysSeId, 0f, true);
			break;
		case AlwaysEffectState.Out:
			this.TryPlaySE(alwaysEffect.outSeId, 0f, false);
			this.TryStopSE(alwaysEffect.inSeId, 0f);
			this.TryStopSE(alwaysEffect.alwaysSeId, 0f);
			break;
		}
	}

	public void TryPlaySE(AlwaysEffectParams[] alwaysEffect, AlwaysEffectState state)
	{
		if (alwaysEffect == null)
		{
			return;
		}
		if (alwaysEffect.Length == 0)
		{
			return;
		}
		this.TryPlaySE(alwaysEffect[0], state);
	}

	public void TryStopSE(AlwaysEffectParams alwaysEffect)
	{
		this.TryStopSE(alwaysEffect.inSeId, 0f);
		this.TryStopSE(alwaysEffect.alwaysSeId, 0f);
		this.TryStopSE(alwaysEffect.outSeId, 0f);
	}

	public void TryStopSE(AlwaysEffectParams[] alwaysEffect)
	{
		if (alwaysEffect == null || alwaysEffect.Length == 0)
		{
			return;
		}
		this.TryStopSE(alwaysEffect[0]);
	}

	public void TryPlaySE(string id, float time = 0f, bool loop = false)
	{
		if (BattleFunctionUtility.IsEmptyPath(id))
		{
			return;
		}
		this.soundManager.PlayTimeScaleSE(id, time, loop, true, null, -1);
	}

	public void TryStopSE(string id, float time = 0f)
	{
		if (BattleFunctionUtility.IsEmptyPath(id))
		{
			return;
		}
		if (this.soundManager.IsPlayingSE(id))
		{
			this.soundManager.StopSE(id, time, null);
		}
	}

	public void PlayDeathSE()
	{
		this.TryPlaySE(base.battleStateData.enemiesDeathEffect[0]);
	}

	public void StopHitEffectSE()
	{
		this.TryStopSE(base.battleStateData.enemiesDeathEffect[0]);
	}

	public void SetVolumeSE(bool isX2Play)
	{
		if (base.battleStateData.seValue == null)
		{
			return;
		}
		if (isX2Play)
		{
			this.soundManager.VolumeSE = 0f;
		}
		else
		{
			this.soundManager.VolumeSE = base.battleStateData.seValue.Value;
		}
	}

	public void TryPlayBGM(string path, float time = 0f)
	{
		if (BattleFunctionUtility.IsEmptyPath(path))
		{
			return;
		}
		if (this.soundManager.IsPlayingBGM && this.soundManager.IsMatchPreviousPlayedBGM(path))
		{
			return;
		}
		this.soundManager.PlayBGM(path, time, null);
	}

	public void TryStopBGM()
	{
		this.soundManager.CertStopBGM();
	}

	public void TryStopBGM(string path)
	{
		if (!this.soundManager.IsMatchPreviousPlayedBGM(path))
		{
			this.soundManager.StopBGM(TimeExtension.GetTimeScaleDivided(base.stateManager.stateProperty.bgmFadeoutSecond), null);
		}
	}

	public void SetPauseVolume(bool enabled)
	{
		if (base.battleStateData.pauseBgmVolume == null)
		{
			return;
		}
		if (!enabled)
		{
			this.soundManager.VolumeBGM = base.battleStateData.pauseBgmVolume.Value;
		}
		else
		{
			base.stateManager.soundManager.VolumeBGM = base.battleStateData.pauseBgmVolume.Value * base.stateManager.stateProperty.pauseBgmVolumeLevel;
		}
	}
}
