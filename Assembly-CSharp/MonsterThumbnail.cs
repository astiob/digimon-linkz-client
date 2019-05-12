using Monster;
using System;
using UnityEngine;

public sealed class MonsterThumbnail : MonoBehaviour
{
	[SerializeField]
	private UITexture monsterImage;

	[SerializeField]
	private UISprite frameImage;

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
		string resourcePath = GUIMonsterIcon.InternalGetMonsterIconPathByIconId(iconId);
		string monsterIconPathByIconId = GUIMonsterIcon.GetMonsterIconPathByIconId(iconId);
		this.monsterImage.enabled = false;
		GUIMonsterIcon.SetTextureMonsterParts(this.monsterImage, resourcePath, monsterIconPathByIconId, true);
		this.monsterImage.enabled = true;
		UISprite component = base.gameObject.GetComponent<UISprite>();
		int growStep2 = (int)MonsterGrowStepData.ToGrowStep(growStep);
		GUIMonsterIcon.SetThumbnailFrame(component, this.frameImage, growStep2);
	}

	public void SetEmptyIcon()
	{
		if (this.monsterImage.enabled)
		{
			this.monsterImage.enabled = false;
		}
		UISprite component = base.gameObject.GetComponent<UISprite>();
		component.spriteName = "Common02_Thumbnail_Empty";
		this.frameImage.spriteName = "Common02_Thumbnail_wakuE";
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
}
