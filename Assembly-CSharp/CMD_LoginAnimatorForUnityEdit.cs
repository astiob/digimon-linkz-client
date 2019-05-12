using System;
using UnityEngine;

public class CMD_LoginAnimatorForUnityEdit : MonoBehaviour
{
	public GameObject loginBounsParamGO;

	private LoginBonusParam loginBounsParam;

	private Animator animator;

	public string completeFlagName = "IsComplete";

	public string effectName = "UISPR_GET1";

	public string rewardName = "UISPR_GET_";

	public int day = 1;

	public bool isCompleteAnimation;

	public bool isMessageState;

	public string messageStateName = "Base Layer.GetReward";

	public UILabel message;

	private void Awake()
	{
		this.loginBounsParam = this.loginBounsParamGO.GetComponent<LoginBonusParam>();
		this.animator = base.gameObject.GetComponent<Animator>();
		if (this.animator == null)
		{
			this.animator = base.gameObject.AddComponent<Animator>();
		}
		this.animator.runtimeAnimatorController = this.loginBounsParam.animatorController;
	}

	private void Update()
	{
		Transform transform = base.transform.Find(this.effectName);
		Transform transform2 = base.transform.Find(this.rewardName + this.day);
		transform.position = transform2.position;
		if (this.animator != null)
		{
			if (this.isCompleteAnimation && this.day == this.loginBounsParam.maxLoginCount)
			{
				this.animator.SetBool(this.completeFlagName, true);
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
			if (this.isMessageState)
			{
				if (currentAnimatorStateInfo.IsName(this.messageStateName) && currentAnimatorStateInfo.normalizedTime >= 1f && this.message != null)
				{
					this.message.text = this.day + "日目の報酬";
				}
			}
			else if (currentAnimatorStateInfo.normalizedTime >= 1f && this.message != null)
			{
				this.message.text = this.day + "日目の報酬";
			}
		}
	}
}
