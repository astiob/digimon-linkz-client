using Master;
using System;
using UnityEngine;

public sealed class MonsterThumbnail : MonoBehaviour
{
	[SerializeField]
	private UITexture monsterImage;

	[SerializeField]
	private UISprite frameImage;

	[SerializeField]
	private UILabel bottomLabel;

	public void Initialize()
	{
		UISprite component = base.gameObject.GetComponent<UISprite>();
		if (!component.enabled)
		{
			component.enabled = true;
		}
		if (!this.monsterImage.enabled)
		{
			this.monsterImage.enabled = true;
		}
		if (!this.frameImage.enabled)
		{
			this.frameImage.enabled = true;
		}
	}

	public void SetImage(string iconId, string growStep)
	{
		string resourcePath = MonsterDataMng.Instance().InternalGetMonsterIconPathByIconId(iconId);
		string monsterIconPathByIconId = MonsterDataMng.Instance().GetMonsterIconPathByIconId(iconId);
		GUIMonsterIcon.SetTextureMonsterParts(this.monsterImage, resourcePath, monsterIconPathByIconId, true);
		UISprite component = base.gameObject.GetComponent<UISprite>();
		GrowStep growStep2 = MonsterData.ToGrowStepId(growStep);
		GUIMonsterIcon.SetThumbnailFrame(component, this.frameImage, growStep2);
	}

	public void SetSize(int width, int height)
	{
		UISprite component = base.gameObject.GetComponent<UISprite>();
		component.width = width;
		component.height = height;
		this.monsterImage.width = width;
		this.monsterImage.height = height;
		this.frameImage.width = width;
		this.frameImage.height = height;
	}

	public void SetBottomText(string text)
	{
	}

	public void SetBottomText(string text, Color color)
	{
		this.SetBottomText(text);
	}

	public void ClearBottomText()
	{
	}
}
