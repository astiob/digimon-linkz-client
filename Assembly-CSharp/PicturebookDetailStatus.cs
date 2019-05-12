using System;
using System.Collections.Generic;
using UnityEngine;

public class PicturebookDetailStatus : MonoBehaviour
{
	[SerializeField]
	private UILabel monsterNameLabel;

	[SerializeField]
	private UILabel specificTypeName;

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

	public void Initialize(string charaName, string grade, string tribe, string specificTypeName, string description, List<string> skillNameList, List<string> skillDescriptionList)
	{
		this.monsterNameLabel.text = charaName;
		this.specificTypeName.text = specificTypeName;
		this.gradeLabel.text = grade;
		this.tribeLabel.text = tribe;
		this.monsterDescriptionLabel.text = description;
		for (int i = 0; i < this.skillNameLabelList.Count; i++)
		{
			if (i < skillNameList.Count)
			{
				this.skillNameLabelList[i].text = skillNameList[i];
			}
			else
			{
				this.skillNameLabelList[i].text = skillNameList[0];
			}
		}
		for (int j = 0; j < this.skillDescriptionLabelList.Count; j++)
		{
			if (j < skillNameList.Count)
			{
				this.skillDescriptionLabelList[j].text = skillDescriptionList[j];
			}
			else
			{
				this.skillDescriptionLabelList[j].text = skillDescriptionList[0];
			}
		}
		base.transform.gameObject.SetActive(true);
	}
}
