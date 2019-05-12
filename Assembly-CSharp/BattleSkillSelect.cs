using Master;
using System;
using UnityEngine;

public class BattleSkillSelect : MonoBehaviour
{
	[Header("UIWidget")]
	[SerializeField]
	public UIWidget widget;

	[Header("エモーション送信機能（マルチ）")]
	[SerializeField]
	public EmotionSenderMulti emotionSenderMulti;

	[Header("エモーション送信機能（PvP）")]
	[SerializeField]
	public EmotionSenderMulti emotionSenderPvP;

	[SerializeField]
	[Header("ボタンオブジェクト")]
	public GameObject monsterButtonRoot;

	[Header("残りターン/マルチバトルのみ")]
	[SerializeField]
	public RemainingTurn remainingTurnMiddle;

	[SerializeField]
	[Header("スキルボタンの親オブジェクト/マルチバトルのみ")]
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

	[SerializeField]
	[Header("スキル説明2の命中率のローカライズ")]
	private UILabel skillDesc2HitRateLocalize;

	[Header("スキル説明3の命中率のローカライズ")]
	[SerializeField]
	private UILabel skillDesc3HitRateLocalize;

	[Header("スキル説明1の威力のローカライズ")]
	[SerializeField]
	private UILabel skillDesc1PowerLocalize;

	[Header("スキル説明2の威力のローカライズ")]
	[SerializeField]
	private UILabel skillDesc2PowerLocalize;

	[Header("スキル説明3の威力のローカライズ")]
	[SerializeField]
	private UILabel skillDesc3PowerLocalize;

	[SerializeField]
	[Header("Leftの親")]
	private Transform leftParent;

	[Header("スキルボタン")]
	[SerializeField]
	public BattleSkillBtn[] skillButton;

	[SerializeField]
	[Header("デジモンボタン")]
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
		string @string = StringMaster.GetString("BattleSkillUI-04");
		this.skillDesc1HitRateLocalize.text = @string;
		this.skillDesc2HitRateLocalize.text = @string;
		this.skillDesc3HitRateLocalize.text = @string;
		string string2 = StringMaster.GetString("BattleSkillUI-03");
		this.skillDesc1PowerLocalize.text = string2;
		this.skillDesc2PowerLocalize.text = string2;
		this.skillDesc3PowerLocalize.text = string2;
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
