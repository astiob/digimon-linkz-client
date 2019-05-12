using Master;
using System;
using UnityEngine;

public class BattleSkillSelect : MonoBehaviour
{
	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[SerializeField]
	[Header("エモーション送信機能（マルチ）")]
	public EmotionSenderMulti emotionSenderMulti;

	[SerializeField]
	[Header("エモーション送信機能（PvP）")]
	public EmotionSenderMulti emotionSenderPvP;

	[SerializeField]
	[Header("ボタンオブジェクト")]
	public GameObject monsterButtonRoot;

	[SerializeField]
	[Header("残りターン/マルチバトルのみ")]
	public RemainingTurn remainingTurnMiddle;

	[Header("スキルボタンの親オブジェクト/マルチバトルのみ")]
	[SerializeField]
	public GameObject skillButtonRoot;

	[Header("コライダー")]
	[SerializeField]
	private Collider[] colliderValues;

	[Header("タッチ判定")]
	[SerializeField]
	public UITouchChecker[] touchChecker;

	[Header("スキル説明1の命中率のローカライズ")]
	[SerializeField]
	private UILabel skillDesc1HitRateLocalize;

	[Header("スキル説明2の命中率のローカライズ")]
	[SerializeField]
	private UILabel skillDesc2HitRateLocalize;

	[Header("スキル説明1の威力のローカライズ")]
	[SerializeField]
	private UILabel skillDesc1PowerLocalize;

	[Header("スキル説明2の威力のローカライズ")]
	[SerializeField]
	private UILabel skillDesc2PowerLocalize;

	[Header("Leftの親")]
	[SerializeField]
	private Transform leftParent;

	[Header("スキルボタン")]
	[SerializeField]
	public BattleSkillBtn[] skillButton;

	[Header("デジモンボタン")]
	[SerializeField]
	public BattleMonsterButton[] monsterButton;

	[Header("アタックタイマー")]
	[SerializeField]
	public AttackTime attackTime;

	private void Awake()
	{
		this.SetupLocalize();
	}

	public BattleDigimonStatus CreateStatusAlly()
	{
		GameObject uibattlePrefab = ResourcesPath.GetUIBattlePrefab("Status_Ally_L");
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(uibattlePrefab);
		Transform transform = gameObject.transform;
		transform.SetParent(this.leftParent);
		transform.localPosition = new Vector3(570f, 400f, 0f);
		transform.localScale = Vector3.one;
		return gameObject.GetComponent<BattleDigimonStatus>();
	}

	public BattleDigimonEnemyStatus CreateStatusEnemy()
	{
		GameObject uibattlePrefab = ResourcesPath.GetUIBattlePrefab("Status_Enemy_M");
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(uibattlePrefab);
		Transform transform = gameObject.transform;
		transform.SetParent(this.leftParent);
		transform.localPosition = new Vector3(308f, 243f, 0f);
		transform.localScale = Vector3.one;
		return gameObject.GetComponent<BattleDigimonEnemyStatus>();
	}

	public void CreateMonsterButtons()
	{
		GameObject uibattlePrefab = ResourcesPath.GetUIBattlePrefab("MonsterButtons");
		this.monsterButtonRoot = UnityEngine.Object.Instantiate<GameObject>(uibattlePrefab);
		Transform transform = this.monsterButtonRoot.transform;
		transform.SetParent(this.leftParent);
		transform.localPosition = new Vector3(0f, 92f, 0f);
		transform.localScale = Vector3.one;
	}

	private void SetupLocalize()
	{
		string text = string.Format("{0} :", StringMaster.GetString("BattleSkillUI-04"));
		this.skillDesc1HitRateLocalize.text = text;
		this.skillDesc2HitRateLocalize.text = text;
		string text2 = string.Format("{0} :", StringMaster.GetString("BattleSkillUI-03"));
		this.skillDesc1PowerLocalize.text = text2;
		this.skillDesc2PowerLocalize.text = text2;
	}

	public void SetColliderActive(bool active)
	{
		for (int i = 0; i < this.colliderValues.Length; i++)
		{
			this.colliderValues[i].enabled = active;
		}
	}

	public void RefleshSkillButton()
	{
		foreach (BattleSkillBtn battleSkillBtn in this.skillButton)
		{
			battleSkillBtn.Reflesh();
		}
	}

	public void ApplySkillButtonRotation(int oldIndex = -1, int newIndex = -1)
	{
		if (oldIndex > -1)
		{
			this.skillButton[oldIndex].PlayCloseRotationEffect();
			this.skillButton[oldIndex].SetButtonType(BattleSkillBtn.Type.Off);
		}
		if (newIndex > -1)
		{
			this.skillButton[newIndex].PlayOpenRotationEffect();
			this.skillButton[newIndex].SetButtonType(BattleSkillBtn.Type.On);
		}
	}

	public void ApplyTwoSkillButtonPosition()
	{
		foreach (BattleSkillBtn battleSkillBtn in this.skillButton)
		{
			battleSkillBtn.ApplyTwoButtonPosition();
		}
		this.skillButton[3].gameObject.SetActive(false);
	}

	public void ApplyThreeSkillButtonPosition()
	{
		foreach (BattleSkillBtn battleSkillBtn in this.skillButton)
		{
			battleSkillBtn.ApplyThreeButtonPosition();
		}
		this.skillButton[3].gameObject.SetActive(true);
	}
}
