using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillConverter
{
	public static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM[] Convert(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetailsMaster)
	{
		List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM> list = new List<GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM>();
		GameWebAPI.RespDataMA_GetSkillM.SkillM[] skillM = MasterDataMng.Instance().RespDataMA_SkillM.skillM;
		foreach (GameWebAPI.RespDataMA_GetSkillM.SkillM skillM2 in skillM)
		{
			List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM> list2 = new List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM>();
			List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM> list3 = new List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM>();
			foreach (GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM receiveSkillDetailM in skillDetailsMaster)
			{
				if (receiveSkillDetailM.skillId == skillM2.skillId)
				{
					list2.Add(receiveSkillDetailM);
				}
				else
				{
					list3.Add(receiveSkillDetailM);
				}
			}
			if (!list2.Any<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM>())
			{
				UnityEngine.Debug.LogWarning("skillId " + skillM2.skillId + " " + skillM2.name);
			}
			while (list2.Any<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM>())
			{
				List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM> list4 = new List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM>();
				List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM> list5 = new List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM>();
				foreach (GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM receiveSkillDetailM2 in list2)
				{
					if (receiveSkillDetailM2.subId == list2[0].subId)
					{
						list4.Add(receiveSkillDetailM2);
					}
					else
					{
						list5.Add(receiveSkillDetailM2);
					}
				}
				try
				{
					GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM item;
					if (skillM2.type == "1")
					{
						item = LeaderSkillConverter.Convert(list4);
					}
					else
					{
						item = StandardSkillConverter.Convert(list4);
					}
					list.Add(item);
					list2 = list5;
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(string.Concat(new string[]
					{
						"skillId ",
						skillM2.skillId,
						" ",
						skillM2.name,
						"\n",
						ex.Message
					}));
					break;
				}
			}
			skillDetailsMaster = list3.ToArray();
		}
		return list.ToArray();
	}
}
