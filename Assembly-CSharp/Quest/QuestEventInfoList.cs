using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quest
{
	public sealed class QuestEventInfoList : ScriptableObject
	{
		[SerializeField]
		private List<QuestEventInfoList.EventInfo> eventInfo;

		public bool ExistEvent(string encryptoDungeonId)
		{
			bool result = false;
			if (this.eventInfo != null)
			{
				for (int i = 0; i < this.eventInfo.Count; i++)
				{
					if (this.eventInfo[i].dungeonId == encryptoDungeonId)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public QuestEventInfoList.EventInfo GetEventInfo(string encryptoDungeonId)
		{
			QuestEventInfoList.EventInfo result = null;
			if (this.eventInfo != null)
			{
				for (int i = 0; i < this.eventInfo.Count; i++)
				{
					if (this.eventInfo[i].dungeonId == encryptoDungeonId)
					{
						result = this.eventInfo[i];
						break;
					}
				}
			}
			return result;
		}

		[Serializable]
		public sealed class EventInfo
		{
			public string dungeonId;

			public string scriptFileName;
		}
	}
}
