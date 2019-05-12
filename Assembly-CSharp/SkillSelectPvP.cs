using System;
using UnityEngine;

public class SkillSelectPvP : MonoBehaviour
{
	[SerializeField]
	[Header("モンスターボタン")]
	public GameObject monsterButtonRoot;

	[Header("Time")]
	[SerializeField]
	public GameObject timeObject;

	[SerializeField]
	[Header("攻撃ボタンのコライダー")]
	public Collider AttackButtonCollider;

	[Header("スキルボタン1のコライダー")]
	[SerializeField]
	public Collider SkillButton1Collider;

	[Header("スキルボタン2のコライダー")]
	[SerializeField]
	public Collider SkillButton2Collider;

	[SerializeField]
	[Header("攻撃ボタンのチェッカー")]
	public UITouchChecker attackButtonChecker;

	[SerializeField]
	[Header("スキルボタン1のチェッカー")]
	public UITouchChecker skillButton1Checker;

	[Header("スキルボタン2のチェッカー")]
	[SerializeField]
	public UITouchChecker skillButton2Checker;
}
