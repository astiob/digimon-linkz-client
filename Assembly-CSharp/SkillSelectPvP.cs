using System;
using UnityEngine;

public class SkillSelectPvP : MonoBehaviour
{
	[Header("モンスターボタン")]
	[SerializeField]
	public GameObject monsterButtonRoot;

	[SerializeField]
	[Header("Time")]
	public GameObject timeObject;

	[Header("攻撃ボタンのコライダー")]
	[SerializeField]
	public Collider AttackButtonCollider;

	[SerializeField]
	[Header("スキルボタン1のコライダー")]
	public Collider SkillButton1Collider;

	[SerializeField]
	[Header("スキルボタン2のコライダー")]
	public Collider SkillButton2Collider;

	[SerializeField]
	[Header("攻撃ボタンのチェッカー")]
	public UITouchChecker attackButtonChecker;

	[Header("スキルボタン1のチェッカー")]
	[SerializeField]
	public UITouchChecker skillButton1Checker;

	[Header("スキルボタン2のチェッカー")]
	[SerializeField]
	public UITouchChecker skillButton2Checker;
}
