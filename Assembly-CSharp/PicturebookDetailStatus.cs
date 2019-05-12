using System;
using System.Collections.Generic;
using UnityEngine;

public class PicturebookDetailStatus : MonoBehaviour
{
	[SerializeField]
	private UILabel monsterNameLabel;

	[SerializeField]
	private UILabel gradeLabel;

	[SerializeField]
	private UILabel tribeLabel;

	[SerializeField]
	private UILabel monsterDescriptionLabel;

	[SerializeField]
	private List<UILabel> skillNameLabelList;

	[SerializeField]
	private List<UILabel> skillDescriptionLabelList;

	public void Initialize(string CharaName, string Grade, string Tribe, string Description, List<string> SkillNameList, List<string> SkillDescriptionList)
	{
		this.monsterNameLabel.text = CharaName;
		this.gradeLabel.text = Grade;
		this.tribeLabel.text = Tribe;
		this.monsterDescriptionLabel.text = Description;
		for (int i = 0; i < this.skillNameLabelList.Count; i++)
		{
			if (i < SkillNameList.Count)
			{
				this.skillNameLabelList[i].text = SkillNameList[i];
			}
			else
			{
				this.skillNameLabelList[i].text = SkillNameList[0];
			}
		}
		for (int j = 0; j < this.skillDescriptionLabelList.Count; j++)
		{
			if (j < SkillNameList.Count)
			{
				this.skillDescriptionLabelList[j].text = SkillDescriptionList[j];
			}
			else
			{
				this.skillDescriptionLabelList[j].text = SkillDescriptionList[0];
			}
		}
		base.transform.gameObject.SetActive(true);
	}
}
