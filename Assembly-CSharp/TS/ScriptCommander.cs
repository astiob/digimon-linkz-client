using System;

namespace TS
{
	public sealed class ScriptCommander
	{
		public ScriptCommandParams commandParams;

		public ScriptCommander()
		{
			this.commandParams = new ScriptCommandParams();
		}

		public void Window(string[] commandParams)
		{
			if (3 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.windowInfo.xFromCenter = ScriptUtil.GetInt(commandParams[1]);
			this.commandParams.windowInfo.yFromCenter = ScriptUtil.GetInt(commandParams[2]);
		}

		public void Msg(string[] commandParams)
		{
			this.commandParams.textInfo.displayText = string.Empty;
			if (2 < commandParams.Length)
			{
				this.commandParams.textInfo.isWindowText = false;
				this.commandParams.textInfo.xFromCenter = ScriptUtil.GetInt(commandParams[1]);
				this.commandParams.textInfo.yFromCenter = ScriptUtil.GetInt(commandParams[2]);
				this.commandParams.textInfo.fadeTime = ScriptUtil.GetFloat(commandParams[3]);
			}
			else
			{
				this.commandParams.textInfo.isWindowText = true;
			}
		}

		public void Text(string[] commandParams)
		{
			this.commandParams.textInfo.displayText = commandParams[1];
		}

		public void MsgEnd(string[] commandParams)
		{
			if (2 <= commandParams.Length)
			{
				this.commandParams.textInfo.autoFeedTime = ScriptUtil.GetFloat(commandParams[1]);
			}
			else
			{
				this.commandParams.textInfo.autoFeedTime = 0f;
			}
		}

		public void Select(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			if (this.commandParams.selectInfo.display)
			{
				this.commandParams.selectInfo.display = false;
				this.commandParams.selectInfo.displayText.Clear();
			}
			this.commandParams.selectInfo.displayText.Add(commandParams[1]);
		}

		public void SelectDisplay()
		{
			this.commandParams.selectInfo.display = true;
		}

		public void Chara(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.charaInfo.type = ScriptUtil.GetIndex(new string[]
			{
				"body",
				"monitor"
			}, commandParams[1]);
			if (3 <= commandParams.Length)
			{
				this.commandParams.charaInfo.faceId = commandParams[2];
			}
			else
			{
				this.commandParams.charaInfo.faceId = "5";
			}
		}

		public void CharaPos(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.charaInfo.yFromCenter = ScriptUtil.GetInt(commandParams[1]);
		}

		public void CharaFace(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.charaInfo.faceId = commandParams[1];
		}

		public void CharaDelete(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.charaInfo.type = ScriptUtil.GetIndex(new string[]
			{
				"body",
				"monitor"
			}, commandParams[1]);
			this.commandParams.charaInfo.yFromCenter = 0;
			this.commandParams.charaInfo.faceId = "5";
		}

		public void WindowPicture(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.windowPictureInfo.thumbnail = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[1]));
			this.commandParams.windowPictureInfo.prefabNames.Clear();
			for (int i = 2; i < commandParams.Length; i++)
			{
				this.commandParams.windowPictureInfo.prefabNames.Add(commandParams[i]);
			}
		}

		public ScriptEngine.Status Wait(string[] commandParams)
		{
			ScriptEngine.Status result = ScriptEngine.Status.NONE;
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return result;
			}
			string text = commandParams[1];
			switch (text)
			{
			case "time":
			{
				if (3 > commandParams.Length)
				{
					Debug.LogError("引数が足りない");
					return result;
				}
				float num2 = ScriptUtil.GetFloat(commandParams[2]);
				if (0f > num2)
				{
					num2 = 0f;
				}
				this.commandParams.waitTime = num2;
				return ScriptEngine.Status.WAIT_TIME;
			}
			case "touch":
				return ScriptEngine.Status.WAIT_TOUCH;
			case "battle_start":
				return ScriptEngine.Status.WAIT_BATTLE_START;
			case "battle_result":
				return ScriptEngine.Status.WAIT_BATTLE_RESULT_START;
			case "battle_action":
				return ScriptEngine.Status.WAIT_BATTLE_ACTION_SELECT;
			case "harvest":
				if (3 > commandParams.Length)
				{
					Debug.LogError("引数が足りない");
					return result;
				}
				this.commandParams.meatNum = ScriptUtil.GetInt(commandParams[2]);
				return ScriptEngine.Status.WAIT_FARM_HARVEST;
			case "result_end":
				return ScriptEngine.Status.WAIT_BATTLE_RESULT_END;
			case "first_clear":
				return ScriptEngine.Status.WAIT_FIRST_CLEAR;
			case "level_up":
				return ScriptEngine.Status.WAIT_MEAL_LEVEL_UP;
			case "training_open":
				return ScriptEngine.Status.WAIT_OPEN_TRAINING_MENU;
			case "meal_digimon":
				return ScriptEngine.Status.WAIT_OPEN_MEAL_DIGIMON_SELECT;
			case "meal":
				return ScriptEngine.Status.WAIT_OPEN_MEAL_GIVE;
			case "gasha_top":
				return ScriptEngine.Status.WAIT_OPEN_GASHA_TOP;
			case "detail":
				if (3 > commandParams.Length)
				{
					Debug.LogError("引数が足りない");
					return result;
				}
				this.commandParams.waitOpenDetailUI = ScriptUtil.GetIndex(new string[]
				{
					"gasha",
					"evolution"
				}, commandParams[2]);
				return ScriptEngine.Status.WAIT_OPEN_DIGIMON_DETAIL;
			case "download":
				return ScriptEngine.Status.WAIT_DOWNLOAD;
			case "meat_farm":
				return ScriptEngine.Status.WAIT_BUILD_MEAT_FARM;
			case "digivice_open":
				return ScriptEngine.Status.WAIT_DIGIVICE_OPEN;
			case "digi_garden":
				return ScriptEngine.Status.WAIT_DIGI_GARDEN_OPEN;
			case "digi_garden_list":
				return ScriptEngine.Status.WAIT_DIGI_GARDEN_CHANGE_LIST;
			case "digi_garden_set_list":
				return ScriptEngine.Status.WAIT_DIGI_GARDEN_CHANGE_SET_LIST;
			case "colosseum_open":
				return ScriptEngine.Status.WAIT_COLOSSEUM_OPEN;
			case "build_tap":
				if (3 > commandParams.Length)
				{
					Debug.LogError("引数が足りない");
					return result;
				}
				this.commandParams.buildFacilityInfo.id = ScriptUtil.GetInt(commandParams[2]);
				return ScriptEngine.Status.WAIT_FARM_BUILD_TAP;
			case "facility_shop":
				return ScriptEngine.Status.WAIT_FACILITY_SHOP_OPEN;
			case "mission":
				return ScriptEngine.Status.WAIT_MISSION_OPEN;
			}
			Debug.LogError("引数が対応外");
			return result;
		}

		public void Mask(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.enableMask = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[1]));
		}

		public void Fade(string[] commandParams)
		{
			if (4 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.fadeInfo.enable = (0 == ScriptUtil.GetIndex(new string[]
			{
				"out",
				"in"
			}, commandParams[1]));
			this.commandParams.fadeInfo.type = ScriptUtil.GetIndex(new string[]
			{
				"white",
				"black"
			}, commandParams[2]);
			this.commandParams.fadeInfo.time = ScriptUtil.GetFloat(commandParams[3]);
		}

		public void Save(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.tutorialStateID = commandParams[1];
		}

		public void Scene(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.sceneInfo.type = ScriptUtil.GetIndex(new string[]
			{
				"black",
				"tutorial",
				"warp",
				"world",
				"world_collapse",
				"battle",
				"farm",
				"white"
			}, commandParams[1]);
			this.commandParams.sceneInfo.isBattle = ("battle" == commandParams[1]);
		}

		public void Shake(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.shakeInfo.intensity = ScriptUtil.GetFloat(commandParams[1]);
			this.commandParams.shakeInfo.decay = 0f;
		}

		public void ShakeStop(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.shakeInfo.decay = ScriptUtil.GetFloat(commandParams[1]);
		}

		public void UI(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.uiInfo.type = commandParams[1];
		}

		public void UI_Induce(string[] commandParams)
		{
			if (3 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.uiInfo.type = commandParams[1];
			this.commandParams.uiInfo.enabled = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[2]));
			if (3 < commandParams.Length)
			{
				this.commandParams.uiInfo.arrowPosition = ScriptUtil.GetIndex(new string[]
				{
					"top",
					"bottom",
					"left",
					"right"
				}, commandParams[3]);
			}
			else
			{
				this.commandParams.uiInfo.arrowPosition = 0;
			}
		}

		public void FarmCamera(string[] commandParams)
		{
			if (4 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.farmCameraMoveInfo.posGridX = ScriptUtil.GetInt(commandParams[1]);
			this.commandParams.farmCameraMoveInfo.posGridY = ScriptUtil.GetInt(commandParams[2]);
			this.commandParams.farmCameraMoveInfo.time = ScriptUtil.GetFloat(commandParams[3]);
		}

		public void FarmSelect(string[] commandParams)
		{
			if (3 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.selectFacilityInfo.id = ScriptUtil.GetInt(commandParams[1]);
			this.commandParams.selectFacilityInfo.selected = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[2]));
		}

		public void FarmBuild(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.buildFacilityInfo.id = ScriptUtil.GetInt(commandParams[1]);
			if (3 <= commandParams.Length)
			{
				this.commandParams.buildFacilityInfo.posGridX = ScriptUtil.GetInt(commandParams[2]);
				this.commandParams.buildFacilityInfo.posGridY = ScriptUtil.GetInt(commandParams[3]);
				this.commandParams.buildFacilityInfo.buildTime = ScriptUtil.GetInt(commandParams[4]);
				this.commandParams.buildFacilityInfo.buildComplete = false;
			}
			else
			{
				this.commandParams.buildFacilityInfo.buildComplete = true;
			}
		}

		public void FarmTarget(string[] commandParams)
		{
			if (3 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.targetFacilityInfo.id = ScriptUtil.GetInt(commandParams[1]);
			this.commandParams.targetFacilityInfo.popEnable = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[2]));
			if (4 <= commandParams.Length)
			{
				this.commandParams.targetFacilityInfo.adjustY = ScriptUtil.GetFloat(commandParams[3]);
			}
			else
			{
				this.commandParams.targetFacilityInfo.adjustY = 0f;
			}
		}

		public void Quest(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.battleInfo.type = ScriptUtil.GetInt(commandParams[1]);
		}

		public void BattlePause(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.battleInfo.pause = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[1]));
		}

		public void SoundVolume(string[] commandParams)
		{
			if (3 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.soundVolumeInfo.enable = (0 == ScriptUtil.GetIndex(new string[]
			{
				"in",
				"out"
			}, commandParams[1]));
			this.commandParams.soundVolumeInfo.time = ScriptUtil.GetFloat(commandParams[2]);
		}

		public void Bgm(string[] commandParams)
		{
			if (4 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.bgmInfo.play = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[1]));
			this.commandParams.bgmInfo.fadeTime = ScriptUtil.GetFloat(commandParams[2]);
			this.commandParams.bgmInfo.fileName = commandParams[3];
		}

		public void Se(string[] commandParams)
		{
			if (5 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.seInfo.play = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[1]));
			this.commandParams.seInfo.fadeTime = ScriptUtil.GetFloat(commandParams[2]);
			this.commandParams.seInfo.loop = (0 == ScriptUtil.GetIndex(new string[]
			{
				"loop",
				"one"
			}, commandParams[3]));
			this.commandParams.seInfo.fileName = commandParams[4];
			if (6 > commandParams.Length)
			{
				this.commandParams.seInfo.pitch = 1f;
			}
			else
			{
				this.commandParams.seInfo.pitch = ScriptUtil.GetFloat(commandParams[5]);
			}
		}

		public void Digimon(string[] commandParams)
		{
			if (5 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.digimonInfo.monsterGroupID = ScriptUtil.GetInt(commandParams[1]);
			this.commandParams.digimonInfo.scale = ScriptUtil.GetFloat(commandParams[2]);
			this.commandParams.digimonInfo.adjustPosition.x = ScriptUtil.GetFloat(commandParams[3]);
			this.commandParams.digimonInfo.adjustPosition.y = ScriptUtil.GetFloat(commandParams[4]);
		}

		public void Effect(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.screenEffectInfo.start = (0 == ScriptUtil.GetIndex(new string[]
			{
				"on",
				"off"
			}, commandParams[1]));
			this.commandParams.screenEffectInfo.type = ScriptUtil.GetIndex(new string[]
			{
				"siren",
				"connect",
				"shutdown"
			}, commandParams[2]);
		}

		public void Meat(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.meatNum = ScriptUtil.GetInt(commandParams[1]);
		}

		public void DigiStone(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.digiStoneNum = ScriptUtil.GetInt(commandParams[1]);
		}

		public void DigimonExp(string[] commandParams)
		{
			if (4 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.digimonExpInfo.index = ScriptUtil.GetInt(commandParams[1]);
			this.commandParams.digimonExpInfo.level = ScriptUtil.GetInt(commandParams[2]);
			this.commandParams.digimonExpInfo.exp = ScriptUtil.GetInt(commandParams[3]);
		}

		public void SetLinkPoint(string[] commandParams)
		{
			if (2 > commandParams.Length)
			{
				Debug.LogError("引数が足りない");
				return;
			}
			this.commandParams.linkPointNum = ScriptUtil.GetInt(commandParams[1]);
		}
	}
}
