using System;
using UnityEngine;

namespace Master
{
	public class AlertMasterResource : ScriptableObject
	{
		[SerializeField]
		private GameWebAPI.RespDataMA_MessageM.MessageM[] alertList;

		public GameWebAPI.RespDataMA_MessageM.MessageM GetAlert(string code)
		{
			GameWebAPI.RespDataMA_MessageM.MessageM result = null;
			if (this.alertList != null)
			{
				for (int i = 0; i < this.alertList.Length; i++)
				{
					if (this.alertList[i].messageCode == code)
					{
						result = this.alertList[i];
						break;
					}
				}
			}
			return result;
		}
	}
}
