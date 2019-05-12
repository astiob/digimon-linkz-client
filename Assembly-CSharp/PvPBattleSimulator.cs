using BattleStateMachineInternal;
using MultiBattle.Tools;
using System;
using System.Reflection;
using UnityEngine;

public class PvPBattleSimulator : BattleSimulator
{
	private const int numberCount = 3;

	[SerializeField]
	public string myPlayerUserId;

	[SerializeField]
	public MultiBattleData.PvPUserData[] pvpUsers;

	[SerializeField]
	public int maxAttackTime;

	[SerializeField]
	public int hurryUpAttackTime;

	[SerializeField]
	public int maxRoundNum;

	public bool Validate()
	{
		if (this._usePlayerCharactersId.Length < 3)
		{
			global::Debug.LogErrorFormat("味方プレイヤーが{0}人以下です. 味方数:{1}", new object[]
			{
				3,
				this._usePlayerCharactersId.Length
			});
			return true;
		}
		if (this._battleWaves.Length != 1)
		{
			global::Debug.LogErrorFormat("Wave数が1以外です. Wave数:{0}", new object[]
			{
				this._battleWaves.Length
			});
			return true;
		}
		string[] useEnemiesId = this._battleWaves[0].useEnemiesId;
		if (useEnemiesId.Length < 3)
		{
			global::Debug.LogErrorFormat("敵が{0}人以下です. 敵数:{1}", new object[]
			{
				3,
				useEnemiesId.Length
			});
			return true;
		}
		return false;
	}

	protected override void ApplyBattleHierarchy(BattleStateHierarchyData hierarchyData, bool onAutoPlay = false, int continueWaitSecond = 10)
	{
		if (BattleStateManager.onAutoServerConnect)
		{
			return;
		}
		new GameObject("TCPUtil").AddComponent<TCPUtil>();
		TCPUtil instance = Singleton<TCPUtil>.Instance;
		FieldInfo field = instance.GetType().GetField("hostAddress", BindingFlags.Instance | BindingFlags.NonPublic);
		field.SetValue(instance, string.Format("{0}:{1}", "210.129.116.190", "18021"));
		this.SetData();
		base.ApplyBattleHierarchy(hierarchyData, onAutoPlay, continueWaitSecond);
	}

	private void SetData()
	{
		MultiBattleData instance = ClassSingleton<MultiBattleData>.Instance;
		instance.IsSimulator = true;
		instance.MyPlayerUserId = this.myPlayerUserId;
		instance.PvPUserDatas = this.pvpUsers;
		if (this.maxAttackTime <= 2)
		{
			global::Debug.LogError("Attack待ちMAX時間(maxAttackTime)が2以下ですが宜しいですか?");
		}
		instance.MaxAttackTime = this.maxAttackTime;
		if (this.hurryUpAttackTime <= 1)
		{
			global::Debug.LogError("Attack急かす時間(hurryUpAttackTime)が1以下ですが宜しいですか?");
		}
		instance.HurryUpAttackTime = this.hurryUpAttackTime;
		if (this.maxRoundNum <= 0)
		{
			global::Debug.LogError("シミュレータのラウンド数(maxRoundNum)が0以下ですが宜しいですか?");
		}
		ClassSingleton<MultiBattleData>.Instance.MaxRoundNum = this.maxRoundNum;
	}
}
