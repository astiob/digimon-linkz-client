using System;
using UnityEngine;

namespace Master
{
	public sealed class StringMasterResource : ScriptableObject
	{
		[SerializeField]
		private GameWebAPI.RespDataMA_MessageStringM.Message[] messageList;

		public GameWebAPI.RespDataMA_MessageStringM.Message GetMessage(string code)
		{
			GameWebAPI.RespDataMA_MessageStringM.Message result = null;
			if (this.messageList != null)
			{
				for (int i = 0; i < this.messageList.Length; i++)
				{
					if (this.messageList[i].messageCode == code)
					{
						result = this.messageList[i];
						break;
					}
				}
			}
			return result;
		}
	}
}
