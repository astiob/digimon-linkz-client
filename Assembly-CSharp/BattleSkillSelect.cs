using Master;
using System;
using UnityEngine;

public class BattleSkillSelect : MonoBehaviour
{
	[SerializeField]
	[Header("UIWidget")]
	public UIWidget widget;

	[SerializeField]
	[Header("EmotionSender")]
	public EmotionSenderMulti emotionSenderMulti;

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

	[SerializeField]
	[Header("スキル説明1の威力のローカライズ")]
	private UILabel skillDesc1PowerLocalize;

	[SerializeField]
	[Header("スキル説明2の威力のローカライズ")]
	private UILabel skillDesc2PowerLocalize;

	[Header("Leftの親")]
	[SerializeField]
	private Transform leftParent;

	private MonsterButtons monsterButtons;

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
		this.monsterButtons = this.monsterButtonRoot.GetComponent<MonsterButtons>();
		global::Debug.Log(this.monsterButtons);
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
}
