using BattleStateMachineInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

public class BattleCameraControl : IMono
{
	private BattleStateManager m_battleStateManager;

	private CameraParams m_cameraMotion;

	private Dictionary<string, CameraParams> cameraParamsObject = new Dictionary<string, CameraParams>();

	public BattleCameraControl(BattleStateManager battleStateManager)
	{
		this.m_battleStateManager = battleStateManager;
	}

	private BattleStateData battleStateData
	{
		get
		{
			return this.m_battleStateManager.battleStateData;
		}
	}

	protected BattleStateHierarchyData hierarchyData
	{
		get
		{
			return this.m_battleStateManager.hierarchyData;
		}
	}

	public List<IEnumerator> LoadCameraMotions()
	{
		List<IEnumerator> list = new List<IEnumerator>();
		bool flag = false;
		foreach (BattleWave battleWave in this.hierarchyData.batteWaves)
		{
			if (battleWave.cameraType == 1)
			{
				flag = true;
				break;
			}
		}
		string cameraMotionId = "0002_roundStart";
		IEnumerator item = this.LoadCameraMotion(cameraMotionId, null);
		list.Add(item);
		if (this.m_battleStateManager.battleMode == BattleMode.PvP)
		{
			string cameraMotionId2 = "pvpBattleStart";
			IEnumerator item2 = this.LoadCameraMotion(cameraMotionId2, null);
			list.Add(item2);
			string cameraMotionId3 = "pvpBattleStart";
			IEnumerator item3 = this.LoadCameraMotion(cameraMotionId3, null);
			list.Add(item3);
		}
		string cameraMotionId4 = "0001_bossStart";
		IEnumerator item4 = this.LoadCameraMotion(cameraMotionId4, null);
		list.Add(item4);
		string cameraMotionId5 = "0002_command";
		IEnumerator item5 = this.LoadCameraMotion(cameraMotionId5, null);
		list.Add(item5);
		string cameraMotionId6 = "0002_command_ally";
		IEnumerator item6 = this.LoadCameraMotion(cameraMotionId6, null);
		list.Add(item6);
		string cameraMotionId7 = "0006_behIncap";
		IEnumerator item7 = this.LoadCameraMotion(cameraMotionId7, null);
		list.Add(item7);
		string cameraMotionId8 = "0007_commandCharaView";
		IEnumerator item8 = this.LoadCameraMotion(cameraMotionId8, null);
		list.Add(item8);
		string cameraMotionId9 = "skillF";
		IEnumerator item9 = this.LoadCameraMotion(cameraMotionId9, null);
		list.Add(item9);
		string cameraMotionId10 = "skillA";
		IEnumerator item10 = this.LoadCameraMotion(cameraMotionId10, null);
		list.Add(item10);
		if (flag)
		{
			string cameraMotionId11 = "BigBoss/0001_Start";
			IEnumerator item11 = this.LoadCameraMotion(cameraMotionId11, null);
			list.Add(item11);
			string cameraMotionId12 = "BigBoss/0002_command";
			IEnumerator item12 = this.LoadCameraMotion(cameraMotionId12, null);
			list.Add(item12);
			string cameraMotionId13 = "BigBoss/0002_enemy";
			IEnumerator item13 = this.LoadCameraMotion(cameraMotionId13, null);
			list.Add(item13);
			string cameraMotionId14 = "BigBoss/0007_commandCharaView";
			IEnumerator item14 = this.LoadCameraMotion(cameraMotionId14, null);
			list.Add(item14);
			string cameraMotionId15 = "BigBoss/skillF";
			IEnumerator item15 = this.LoadCameraMotion(cameraMotionId15, null);
			list.Add(item15);
			string cameraMotionId16 = "BigBoss/skillA";
			IEnumerator item16 = this.LoadCameraMotion(cameraMotionId16, null);
			list.Add(item16);
			string cameraMotionId17 = "BigBoss/0008_withdrawal";
			IEnumerator item17 = this.LoadCameraMotion(cameraMotionId17, null);
			list.Add(item17);
		}
		return list;
	}

	public void PlayCameraMotionAction(string cameraKey, Transform position, bool isClamp = true)
	{
		if (this.cameraParamsObject.ContainsKey(cameraKey))
		{
			if (this.m_cameraMotion != null)
			{
				this.m_cameraMotion.StopCameraAnimation();
			}
			this.m_cameraMotion = this.cameraParamsObject[cameraKey];
			if (this.battleStateData.cameraMotionChangeFunction != null)
			{
				this.battleStateData.cameraMotionChangeFunction();
			}
			this.battleStateData.cameraMotionChangeFunction = null;
			this.m_cameraMotion.PlayCameraAnimation(position, false, isClamp);
		}
	}

	public void PlayCameraMotionActionCharacter(string cameraKey, CharacterStateControl characters)
	{
		if (this.cameraParamsObject.ContainsKey(cameraKey))
		{
			if (this.m_cameraMotion != null)
			{
				this.m_cameraMotion.StopCameraAnimation();
			}
			this.m_cameraMotion = this.cameraParamsObject[cameraKey];
			if (this.battleStateData.cameraMotionChangeFunction != null)
			{
				this.battleStateData.cameraMotionChangeFunction();
			}
			this.battleStateData.cameraMotionChangeFunction = null;
			this.m_cameraMotion.PlayCameraAnimation(characters.CharacterParams, BoolExtension.Inverse(characters.isEnemy, this.hierarchyData.onInverseCamera), true);
		}
	}

	public void StopCameraMotionAction(string cameraKey)
	{
		if (this.m_cameraMotion != null)
		{
			this.m_cameraMotion.StopCameraAnimation();
		}
		if (this.cameraParamsObject.ContainsKey(cameraKey) && this.cameraParamsObject[cameraKey].isPlaying)
		{
			this.cameraParamsObject[cameraKey].StopCameraAnimation();
		}
	}

	public void PlayTweenCameraMotion(TweenCameraTargetFunction tweenCamera, CharacterStateControl target = null)
	{
		bool flag = this.hierarchyData.batteWaves[this.battleStateData.currentWaveNumber].cameraType == 1;
		if (target == null)
		{
			tweenCamera.SetCamera(false, flag);
			return;
		}
		if (tweenCamera.isMoving && tweenCamera.currentIndex == target.myIndex)
		{
			return;
		}
		if (flag)
		{
			tweenCamera.SetCamera(!target.isEnemy, true);
		}
		else
		{
			int num = target.myIndex;
			if (target.isEnemy)
			{
				num += this.battleStateData.playerCharacters.Length;
			}
			tweenCamera.SetCamera(num, !target.isEnemy, false);
		}
	}

	public void SetCameraLengthAction(TweenCameraTargetFunction tweenCamera)
	{
		tweenCamera.SetLastTime();
	}

	public void StopTweenCameraMotionAction(TweenCameraTargetFunction tweenCamera)
	{
		tweenCamera.Stop();
	}

	public void PlayCameraShake()
	{
		if (this.m_cameraMotion != null)
		{
			this.m_cameraMotion.PlayCameraShake();
		}
	}

	public void StopCameraShake()
	{
		if (this.m_cameraMotion != null)
		{
			this.m_cameraMotion.StopCameraShake();
		}
	}

	public bool IsPlaying()
	{
		bool result = false;
		if (this.m_cameraMotion != null)
		{
			return this.m_cameraMotion.isPlaying;
		}
		return result;
	}

	public bool IsPlaying(string cameraKey)
	{
		return this.cameraParamsObject.ContainsKey(cameraKey) && this.cameraParamsObject[cameraKey].isPlaying;
	}

	public void DestroyNotContainCameraParams()
	{
		foreach (string key in this.cameraParamsObject.Keys)
		{
			if (this.cameraParamsObject[key] != null)
			{
				UnityEngine.Object.Destroy(this.cameraParamsObject[key].gameObject);
			}
		}
	}

	public IEnumerator LoadCameraMotion(string cameraMotionId, Action<CameraParams, string> result = null)
	{
		BattleDebug.Log("--- カメラモーション単体ロード cameraMotionId[" + cameraMotionId + "] : 開始");
		CameraParams cameraParams = null;
		if (this.cameraParamsObject.TryGetValue(cameraMotionId, out cameraParams))
		{
			cameraParams = this.cameraParamsObject[cameraMotionId];
		}
		else
		{
			GameObject cameraMotionPrefab = this.m_battleStateManager.serverControl.GetCameraMotionPrefab(cameraMotionId);
			GameObject cameraMotion = base.Instantiate<GameObject>(cameraMotionPrefab);
			this.cameraParamsObject.Add(cameraMotionId, cameraMotion.GetComponent<CameraParams>());
			cameraParams = this.cameraParamsObject[cameraMotionId];
			cameraParams.gameObject.name = cameraMotionId;
			yield return null;
			cameraParams.transform.SetParent(this.battleStateData.cameraMotionRoot);
			cameraParams.currentTargetCamera = this.hierarchyData.cameraObject.camera3D;
			cameraParams.GetPostEffectController(this.hierarchyData.cameraObject.postEffect);
			cameraParams.gameObject.SetActive(false);
		}
		if (result != null)
		{
			result(cameraParams, cameraMotionId);
		}
		BattleDebug.Log("--- カメラモーション単体ロード cameraMotionId[" + cameraMotionId + "] : 完了");
		yield break;
	}

	public void SetTime(string cameraKey, float time)
	{
		this.cameraParamsObject[cameraKey].time = time;
	}
}
