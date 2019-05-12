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

	[Header("攻撃ボタンのコライダー")]
	[SerializeField]
	public Collider AttackButtonCollider;

	[SerializeField]
	[Header("スキルボタン1のコライダー")]
	public Collider SkillButton1Collider;

	[Header("スキルボタン2のコライダー")]
	[SerializeField]
	public Collider SkillButton2Collider;

	[Header("攻撃ボタンのチェッカー")]
	[SerializeField]
	public UITouchChecker attackButtonChecker;

	[SerializeField]
	[Header("スキルボタン1のチェッカー")]
	public UITouchChecker skillButton1Checker;

	[SerializeField]
	[Header("スキルボタン2のチェッカー")]
	public UITouchChecker skillButton2Checker;
}
