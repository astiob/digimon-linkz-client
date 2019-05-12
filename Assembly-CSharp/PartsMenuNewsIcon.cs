using System;
using UnityEngine;

public class PartsMenuNewsIcon : MonoBehaviour
{
	private void Start()
	{
		ClassSingleton<PartsMenuNewsIconAccessor>.Instance.artsMenuNewsIcon = this;
	}

	public void NewsCheck()
	{
		GameWebAPI.RespDataIN_InfoList respDataIN_InfoList = DataMng.Instance().RespDataIN_InfoList;
		if (respDataIN_InfoList != null)
		{
			base.gameObject.SetActive(false);
			foreach (GameWebAPI.RespDataIN_InfoList.InfoList infoList2 in respDataIN_InfoList.infoList)
			{
				if (infoList2.confirmationFlg == 0)
				{
					base.gameObject.SetActive(true);
					break;
				}
			}
		}
	}
}
