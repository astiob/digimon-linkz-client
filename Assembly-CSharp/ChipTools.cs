using System;
using UnityEngine;

public class ChipTools
{
	public static GameWebAPI.RespDataMA_ChipM.Chip GetChipData(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip)
	{
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipDataByUserChipId = ChipDataMng.GetUserChipDataByUserChipId(equip.userChipId);
		return ChipDataMng.GetChipMainData(userChipDataByUserChipId);
	}

	public static int ConvertToButtonNo(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip)
	{
		int type = equip.type;
		return equip.dispNum + ((type != 0) ? 5 : 0);
	}

	public static string GetRankPath(string rank)
	{
		return string.Format("Chip_Lv{0}", rank);
	}

	public static void CheckResultCode(int resultCode)
	{
		string text = string.Empty;
		switch (resultCode)
		{
		case 0:
			text = "異常終了";
			break;
		case 1:
			text = "正常終了";
			break;
		case 2:
			text = "所持チップが存在しない";
			break;
		case 3:
			text = "未開放スロットが指定された場合";
			break;
		case 4:
			text = "既に装備済みのチップが指定された場合";
			break;
		case 5:
			text = "既に同一効果のチップが装備されていた場合(CHIP_IDが同じ)";
			break;
		case 6:
			text = "スロット開放用アイテムを必要数所持していない場合";
			break;
		case 7:
			text = "チップ除去アイテムを必要数所持していない場合";
			break;
		case 8:
			text = "合成に必要な素材チップを必要数所持していない場合";
			break;
		case 9:
			text = "最大レベルのチップを強化しようとした場合";
			break;
		case 10:
			text = "既に解放済みのスロットを指定した場合";
			break;
		case 11:
			text = "1つ前のスロットが未開放だった場合";
			break;
		case 12:
			text = "チップ装備済みのスロットが指定された場合";
			break;
		case 13:
			text = "無効なチップが合成素材に指定された場合";
			break;
		case 14:
			text = "未定義スロットが指定された場合";
			break;
		}
		global::Debug.LogWarningFormat("resultCode:{0}, message:{1}", new object[]
		{
			resultCode,
			text
		});
	}

	public static void CreateChipIcon(GameWebAPI.RespDataMA_ChipM.Chip data, UITexture baseIcon, Action<ChipIcon> callback = null)
	{
		Action<UnityEngine.Object> actEnd = delegate(UnityEngine.Object obj)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
			gameObject.transform.SetParent(baseIcon.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			DepthController component = gameObject.GetComponent<DepthController>();
			component.AddWidgetDepth(baseIcon.depth);
			ChipIcon component2 = gameObject.GetComponent<ChipIcon>();
			component2.SetData(data, -1, -1);
			baseIcon.mainTexture = null;
			if (callback != null)
			{
				callback(component2);
			}
		};
		AssetDataMng.Instance().LoadObjectASync("UICommon/Parts/Parts_ChipThumbnail", actEnd);
	}
}
