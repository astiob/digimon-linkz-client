using Master;
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
		string resourcePath = MonsterDataMng.Instance().InternalGetMonsterIconPathByIconId(iconId);
		string monsterIconPathByIconId = MonsterDataMng.Instance().GetMonsterIconPathByIconId(iconId);
		GUIMonsterIcon.SetTextureMonsterParts(this.monsterImage, resourcePath, monsterIconPathByIconId, true);
		UISprite component = base.gameObject.GetComponent<UISprite>();
		GrowStep growStep2 = MonsterData.ToGrowStepId(growStep);
		GUIMonsterIcon.SetThumbnailFrame(component, this.frameImage, growStep2);
	}
}
