using BattleStateMachineInternal;
using MultiBattle.Tools;
using System;
using System.Reflection;
using UnityEngine;

public class MultiBattleSimulator : BattleSimulator
{
	public const string hostAddress = "210.129.116.190";

	public const string port = "18021";

	public const string memberName = "hostAddress";

	[SerializeField]
	[Header("MyユーザID")]
	public string myPlayerUserId;

	[Header("マルチユーザ達")]
	[SerializeField]
	public MultiUser[] multiUsers;

	[SerializeField]
	public int maxAttackTime;

	[SerializeField]
	public int hurryUpAttackTime;

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
	}
}
