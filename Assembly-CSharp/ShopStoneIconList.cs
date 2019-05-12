using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopStoneIconList : MonoBehaviour
{
	[SerializeField]
	private List<ShopStoneIconList.StoneSpriteInfo> stoneSpriteInfo;

	public string GetSpriteName(StoreUtil.StoneSpriteType type)
	{
		if (this.stoneSpriteInfo != null)
		{
			ShopStoneIconList.StoneSpriteInfo stoneSpriteInfo = this.stoneSpriteInfo.SingleOrDefault((ShopStoneIconList.StoneSpriteInfo x) => x.type == type);
			if (stoneSpriteInfo != null)
			{
				return stoneSpriteInfo.spriteName;
			}
		}
		return string.Empty;
	}

	[Serializable]
	private class StoneSpriteInfo
	{
		public StoreUtil.StoneSpriteType type;

		public string spriteName;
	}
}
