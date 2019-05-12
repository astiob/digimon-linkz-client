using System;
using UnityEngine;

public class SkillSelectPvP : MonoBehaviour
{
	[Header("モンスターボタン")]
	[SerializeField]
	public GameObject monsterButtonRoot;

	[Header("Time")]
	[SerializeField]
	public GameObject timeObject;

	[Header("攻撃ボタンのコライダー")]
	[SerializeField]
	public Collider AttackButtonCollider;

	[Header("スキルボタン1のコライダー")]
	[SerializeField]
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
