using System;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
	public class ScriptCommandParams
	{
		public ScriptCommandParams.WindowInfo windowInfo;

		public ScriptCommandParams.TextInfo textInfo;

		public ScriptCommandParams.SelectInfo selectInfo;

		public ScriptCommandParams.CharaInfo charaInfo;

		public ScriptCommandParams.WindowPictureInfo windowPictureInfo;

		public float waitTime;

		public bool enableMask;

		public ScriptCommandParams.FadeInfo fadeInfo;

		public string tutorialStateID;

		public ScriptCommandParams.SceneInfo sceneInfo;

		public ScriptCommandParams.ShakeInfo shakeInfo;

		public ScriptCommandParams.UIInfo uiInfo;

		public ScriptCommandParams.FarmCameraMoveInfo farmCameraMoveInfo;

		public ScriptCommandParams.SelectFacilityInfo selectFacilityInfo;

		public ScriptCommandParams.BuildFacilityInfo buildFacilityInfo;

		public ScriptCommandParams.TargetFacilityInfo targetFacilityInfo;

		public ScriptCommandParams.BattleInfo battleInfo;

		public ScriptCommandParams.SoundVolumeInfo soundVolumeInfo;

		public ScriptCommandParams.BgmInfo bgmInfo;

		public ScriptCommandParams.SeInfo seInfo;

		public ScriptCommandParams.DigimonInfo digimonInfo;

		public ScriptCommandParams.ScreenEffectInfo screenEffectInfo;

		public int meatNum;

		public int digiStoneNum;

		public int linkPointNum;

		public ScriptCommandParams.DigimonExpInfo digimonExpInfo;

		public int waitOpenDetailUI;

		public ScriptCommandParams()
		{
			this.selectInfo.displayText = new List<string>();
			this.windowPictureInfo.prefabNames = new List<string>();
		}

		public struct WindowInfo
		{
			public int xFromCenter;

			public int yFromCenter;
		}

		public struct TextInfo
		{
			public string displayText;

			public int xFromCenter;

			public int yFromCenter;

			public float fadeTime;

			public bool isWindowText;
		}

		public struct SelectInfo
		{
			public List<string> displayText;

			public bool display;
		}

		public struct CharaInfo
		{
			public int type;

			public int yFromCenter;

			public string faceId;
		}

		public struct WindowPictureInfo
		{
			public List<string> prefabNames;

			public bool thumbnail;
		}

		public struct FadeInfo
		{
			public int type;

			public float time;

			public bool enable;
		}

		public struct SceneInfo
		{
			public int type;

			public bool isBattle;
		}

		public struct ShakeInfo
		{
			public float intensity;

			public float decay;
		}

		public struct UIInfo
		{
			public string type;

			public int arrowPosition;

			public bool enabled;
		}

		public struct FarmCameraMoveInfo
		{
			public int posGridX;

			public int posGridY;

			public float time;
		}

		public struct SelectFacilityInfo
		{
			public int id;

			public bool selected;
		}

		public struct BuildFacilityInfo
		{
			public int id;

			public int posGridX;

			public int posGridY;

			public int buildTime;

			public bool buildComplete;
		}

		public struct TargetFacilityInfo
		{
			public int id;

			public float adjustY;

			public bool popEnable;
		}

		public struct BattleInfo
		{
			public bool pause;

			public int type;
		}

		public struct SoundVolumeInfo
		{
			public float time;

			public bool enable;
		}

		public struct BgmInfo
		{
			public string fileName;

			public float fadeTime;

			public bool play;
		}

		public struct SeInfo
		{
			public string fileName;

			public float fadeTime;

			public float pitch;

			public bool play;

			public bool loop;
		}

		public struct DigimonInfo
		{
			public int monsterGroupID;

			public float scale;

			public Vector2 adjustPosition;
		}

		public struct ScreenEffectInfo
		{
			public int type;

			public bool start;
		}

		public struct DigimonExpInfo
		{
			public int index;

			public int level;

			public int exp;
		}
	}
}
