using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GUIBannerPanel : GUISelectPanelBSPartsUD
{
	[SerializeField]
	private float timeOutSeconds;

	public static GameWebAPI.RespDataMA_BannerM Data { get; set; }

	public IEnumerator AllBuild(GameWebAPI.RespDataMA_BannerM dts)
	{
		base.InitBuild();
		this.partsCount = 0;
		if (base.selectCollider != null)
		{
			GameWebAPI.RespDataMA_BannerM.BannerM[] menuBannerList = dts.bannerM.Where((GameWebAPI.RespDataMA_BannerM.BannerM x) => x.actionType == "menu" && ServerDateTime.Now >= DateTime.Parse(x.startTime) && GUIBannerParts.GetRestTimeSeconds(DateTime.Parse(x.endTime)) > 0).ToArray<GameWebAPI.RespDataMA_BannerM.BannerM>();
			this.partsCount = menuBannerList.Length;
			List<GameWebAPI.RespDataMA_BannerM.BannerM> dtList = new List<GameWebAPI.RespDataMA_BannerM.BannerM>();
			for (int mm = 0; mm < menuBannerList.Length; mm++)
			{
				dtList.Add(menuBannerList[mm]);
			}
			dtList.Sort(delegate(GameWebAPI.RespDataMA_BannerM.BannerM a, GameWebAPI.RespDataMA_BannerM.BannerM b)
			{
				int num = int.Parse(a.dispNum);
				int num2 = int.Parse(b.dispNum);
				return num - num2;
			});
			for (int mm = 0; mm < dtList.Count; mm++)
			{
				menuBannerList[mm] = dtList[mm];
			}
			GUISelectPanelBSPartsUD.PanelBuildData pbd = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float ypos = pbd.startY;
			float xpos = pbd.startX;
			for (int i = 0; i < menuBannerList.Length; i++)
			{
				GameWebAPI.RespDataMA_BannerM.BannerM bannerInfo = menuBannerList[i];
				GameObject go = base.AddBuildPart();
				if (!(null == go))
				{
					GUIBannerParts parts = go.GetComponent<GUIBannerParts>();
					if (parts != null)
					{
						parts.name += menuBannerList[i].dispNum.ToString();
						parts.SetOriginalPos(new Vector3(xpos, ypos, -5f));
						parts.Data = bannerInfo;
						parts.SetBGColor();
						string path = ConstValue.APP_ASSET_DOMAIN + "/asset/img" + bannerInfo.img;
						yield return TextureManager.instance.Load(path, new Action<Texture2D>(parts.OnBannerReceived), this.timeOutSeconds, true);
					}
					ypos -= pbd.pitchH;
				}
			}
			base.height = pbd.lenH;
			base.initLocation = true;
			base.InitMinMaxLocation(-1, 0f);
		}
		base.selectParts.SetActive(false);
		yield break;
	}

	public void RefreshNewAlert()
	{
		List<GUIListPartBS> partObjs = this.partObjs;
		foreach (GUIListPartBS guilistPartBS in partObjs)
		{
			GUIBannerParts guibannerParts = (GUIBannerParts)guilistPartBS;
			guibannerParts.SetNew();
		}
	}
}
