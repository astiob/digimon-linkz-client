using System;
using UnityEngine;

public class SkillSelectPvP : MonoBehaviour
{
	[SerializeField]
	[Header("モンスターボタン")]
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

	[SerializeField]
	[Header("スキルボタン1のチェッカー")]
	public UITouchChecker skillButton1Checker;

	[SerializeField]
	[Header("スキルボタン2のチェッカー")]
	public UITouchChecker skillButton2Checker;
}
