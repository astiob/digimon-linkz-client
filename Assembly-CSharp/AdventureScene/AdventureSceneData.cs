using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureScene
{
	public sealed class AdventureSceneData : ClassSingleton<AdventureSceneData>
	{
		public AdventureSceneData.DataType dataType;

		public AdventureCamera adventureCamera;

		public Light adventureLight;

		public GameObject scriptObjectRoot;

		public GameObject stage;

		public List<AdventureDigimonInfo> digimonInfoList;

		public List<AdventureEffectInfo> effectInfoList;

		public AdventureScriptEngine adventureScriptEngine;

		public string scriptFileName;

		public Action sceneBeginAction;

		public Action sceneEndAction;

		public Action sceneDeleteAction;

		public AdventureSceneData()
		{
			this.adventureCamera = new AdventureCamera();
			this.digimonInfoList = new List<AdventureDigimonInfo>();
			this.effectInfoList = new List<AdventureEffectInfo>();
		}

		public AdventureDigimonInfo GetDigimonInfo(int charaId)
		{
			for (int i = 0; i < this.digimonInfoList.Count; i++)
			{
				if (this.digimonInfoList[i].id == charaId)
				{
					return this.digimonInfoList[i];
				}
			}
			return null;
		}

		public AdventureEffectInfo GetEffectInfo(int effectId)
		{
			for (int i = 0; i < this.effectInfoList.Count; i++)
			{
				if (this.effectInfoList[i].id == effectId)
				{
					return this.effectInfoList[i];
				}
			}
			return null;
		}

		public enum DataType
		{
			USE_SCRIPT_FILE,
			NO_SCRIPT_FILE
		}
	}
}
