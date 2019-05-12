using Master;
using System;
using UnityEngine;

public class VersionUpDetail : MonoBehaviour
{
	[SerializeField]
	[Header("デジモンのサムネイル用のアイコン")]
	private UISprite charaIcon;

	[SerializeField]
	[Header("バージョンアップ 文言")]
	private UILabel lbTXT_VerUp;

	[Header("バージョンアップ LEV BEFORE 表示")]
	[SerializeField]
	private UILabel lbTXT_LevBefore;

	[Header("バージョンアップ LEV ⇒ 表示")]
	[SerializeField]
	private UILabel lbTXT_LevArrow;

	[SerializeField]
	[Header("バージョンアップ LEV AFTER 表示")]
	private UILabel lbTXT_LevAfter;

	[Header("バージョンアップ スキル追加文言")]
	[SerializeField]
	private UILabel lbTXT_SkillUp;

	private GUIMonsterIcon csMonsterIcon;

	public void ClearStatus()
	{
		this.ShowIcon(null, false);
		this.charaIcon.spriteName = "Common02_Thumbnail_none";
		this.lbTXT_VerUp.text = string.Empty;
		this.lbTXT_LevBefore.text = string.Empty;
		this.lbTXT_LevArrow.text = string.Empty;
		this.lbTXT_LevAfter.text = string.Empty;
		this.lbTXT_SkillUp.text = string.Empty;
	}

	public void ShowIcon(MonsterData md, bool active)
	{
		if (!active)
		{
			if (this.csMonsterIcon != null)
			{
				UnityEngine.Object.Destroy(this.csMonsterIcon.gameObject);
				this.csMonsterIcon = null;
			}
		}
		else
		{
			if (this.csMonsterIcon != null)
			{
				UnityEngine.Object.Destroy(this.csMonsterIcon.gameObject);
			}
			GameObject gameObject = this.charaIcon.gameObject;
			this.csMonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(md, gameObject.transform.localScale, gameObject.transform.localPosition, gameObject.transform.parent, true, false);
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component != null)
			{
				DepthController.SetWidgetDepth_2(this.csMonsterIcon.gameObject.transform, component.depth + 2);
			}
		}
	}

	public void ShowDetail(int oldLev, int newLev, bool isSkillAdd)
	{
		this.lbTXT_VerUp.text = StringMaster.GetString("VersionUpInfo");
		if (newLev > oldLev)
		{
			this.lbTXT_LevBefore.text = oldLev.ToString();
			this.lbTXT_LevArrow.text = "→";
			this.lbTXT_LevAfter.text = newLev.ToString();
		}
		else
		{
			this.lbTXT_LevBefore.text = string.Empty;
			this.lbTXT_LevArrow.text = string.Empty;
			this.lbTXT_LevAfter.text = string.Empty;
		}
		if (isSkillAdd)
		{
			this.lbTXT_SkillUp.text = StringMaster.GetString("VersionUpSkillAddInfo");
		}
		else
		{
			this.lbTXT_SkillUp.text = string.Empty;
		}
	}
}
