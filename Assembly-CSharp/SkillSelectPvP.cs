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

	[SerializeField]
	[Header("攻撃ボタンのコライダー")]
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

	[Header("スキルボタン1のチェッカー")]
	[SerializeField]
	public UITouchChecker skillButton1Checker;

	[Header("スキルボタン2のチェッカー")]
	[SerializeField]
	public UITouchChecker skillButton2Checker;
}
