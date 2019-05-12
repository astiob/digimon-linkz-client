using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TS;
using UnityEngine;

public class TutorialCreateRestartScreen
{
	private TutorialUI tutorialUI;

	private List<IEnumerator> enumeratorList;

	public TutorialCreateRestartScreen(TutorialUI tutorialUI)
	{
		this.tutorialUI = tutorialUI;
		this.enumeratorList = new List<IEnumerator>();
	}

	public IEnumerator CreateRestartScreen(int sceneType, List<ScriptCommandParams.BuildFacilityInfo> buildFacilityInfoList, int meatNum, int digiStoneNum, int linkPointNum, List<ScriptCommandParams.DigimonExpInfo> digimonExpInfo, List<string> openUI, List<string> openConfirmUI, List<string> selectUI, TutorialControlToGame controlToGame)
	{
		if (sceneType != -1)
		{
			bool isFinished = false;
			controlToGame.ChangeScene(sceneType, delegate
			{
				isFinished = true;
			}, true, null);
			while (!isFinished)
			{
				yield return null;
			}
		}
		for (int i = 0; i < buildFacilityInfoList.Count; i++)
		{
			ScriptCommandParams.BuildFacilityInfo info = buildFacilityInfoList[i];
			if (!info.buildComplete)
			{
				yield return AppCoroutine.Start(controlToGame.BuildFacility(info.id, info.posGridX, info.posGridY, false, delegate(int userFacilityID)
				{
					controlToGame.SetFacilityBuildDummyTime(userFacilityID, info.buildTime);
				}), false);
			}
			else
			{
				controlToGame.FacilityBuildComplete(info.id);
			}
		}
		controlToGame.SetPlayerMeatCount(meatNum);
		controlToGame.SetPlayerDigiStoneCount(digiStoneNum);
		controlToGame.SetPlayerLinkPointCount(linkPointNum);
		for (int j = 0; j < digimonExpInfo.Count; j++)
		{
			ScriptCommandParams.DigimonExpInfo info2 = digimonExpInfo[j];
			controlToGame.SetDigimonExp(info2.index, info2.level, info2.exp);
		}
		bool show;
		Action onOpendAction = delegate()
		{
			show = true;
		};
		for (int k = 0; k < openUI.Count; k++)
		{
			if ("CMD_GashaTOP" == openUI[k])
			{
				APIRequestTask task = APIUtil.Instance().RequestGashaInfo(GameWebAPI.GA_Req_GashaInfo.Type.TUTORIAL, true);
				yield return AppCoroutine.Start(task.Run(null, null, null), false);
			}
			show = false;
			controlToGame.OpenCommonDialog(openUI[k], onOpendAction);
			while (!show)
			{
				yield return null;
			}
			string text = openUI[k];
			if (text != null)
			{
				if (TutorialCreateRestartScreen.<>f__switch$map23 == null)
				{
					TutorialCreateRestartScreen.<>f__switch$map23 = new Dictionary<string, int>(1)
					{
						{
							"CMD_MealExecution",
							0
						}
					};
				}
				int num;
				if (TutorialCreateRestartScreen.<>f__switch$map23.TryGetValue(text, out num))
				{
					if (num == 0)
					{
						controlToGame.SetMealUI_NotServerRequest();
					}
				}
			}
		}
		if (openConfirmUI.Any((string x) => x == "GASHA_START"))
		{
			show = false;
			controlToGame.OpenCommonDialog("GashaConfirmDialog", onOpendAction);
			while (!show)
			{
				yield return null;
			}
		}
		for (int l = 0; l < selectUI.Count; l++)
		{
			controlToGame.SetInduceUI(selectUI[l], true, 0, this.tutorialUI, null);
		}
		yield break;
	}

	public void OpenMessageWindow(int xFromCenter, int yFromCenter)
	{
		this.tutorialUI.MessageWindow.DisplayWindow(xFromCenter, yFromCenter, null, false);
	}

	public void DisplayChara(int charaType, string faceId, int yFromCenter)
	{
		TutorialThumbnail.ThumbnailType thumbnailType = (charaType != 0) ? TutorialThumbnail.ThumbnailType.MONITOR : TutorialThumbnail.ThumbnailType.BODY;
		this.tutorialUI.Thumbnail.Display(thumbnailType, null, false);
		this.tutorialUI.Thumbnail.SetFace(faceId);
		if (thumbnailType == TutorialThumbnail.ThumbnailType.MONITOR)
		{
			this.tutorialUI.Thumbnail.SetMonitorPosition(yFromCenter);
		}
	}

	public void DisplayScreenMask()
	{
		this.tutorialUI.MaskBlack.SetEnable(true, null, true);
	}

	public void DisplayFadeOut(int type)
	{
		TutorialFade.FadeType type2 = (type != 0) ? TutorialFade.FadeType.BLACK : TutorialFade.FadeType.WHITE;
		this.tutorialUI.Fade.StartFade(type2, true, 0f, null);
	}

	public void ShakeBackGround(float intensity, float decay, TutorialControlToGame controlToGame)
	{
		controlToGame.ShakeBackGround(intensity, decay, null);
	}

	public void DisplayUIPop(string type, int arrowPosition, TutorialControlToGame controlToGame)
	{
		controlToGame.SetPopUI(type, arrowPosition, this.tutorialUI, null);
	}

	public void SetFarmCameraPosition(int posGridX, int posGridY, TutorialControlToGame controlToGame)
	{
		this.enumeratorList.Add(controlToGame.MoveFarmCamera(posGridX, posGridY, 0f, null));
	}

	public void FacilityBuildComplete(List<int> facilityIdList, TutorialControlToGame controlToGame)
	{
		for (int i = 0; i < facilityIdList.Count; i++)
		{
			controlToGame.FacilityBuildComplete(facilityIdList[i]);
		}
	}

	public void SetSelectFarmFacility(int id, TutorialControlToGame controlToGame)
	{
		controlToGame.SelectFacility(id);
	}

	public void SetTargetFarmFacility(int id, bool popEnable, TutorialControlToGame controlToGame, float adjustY)
	{
		this.enumeratorList.Add(controlToGame.TargetFacility(id, popEnable, null, adjustY));
	}

	public void DisplayDigimon(int monsterGroupID, float scale, Vector2 adjustPosition, TutorialControlToGame controlToGame)
	{
		this.enumeratorList.Add(controlToGame.DrawDigimon(monsterGroupID, scale, adjustPosition, null));
	}

	public void LocalDigimonEvolution(TutorialControlToGame controlToGame)
	{
		controlToGame.LocalDigimonEvolution();
	}

	public void DisplayScreenEffect(int type, TutorialControlToGame controlToGame)
	{
		controlToGame.StartEffect(type, null);
	}

	public IEnumerator CheckFinish()
	{
		bool isFinished = false;
		int count = 0;
		while (!isFinished)
		{
			isFinished = true;
			for (int i = 0; i < this.enumeratorList.Count; i++)
			{
				if (this.enumeratorList[i].MoveNext())
				{
					isFinished = false;
				}
			}
			count++;
			yield return null;
		}
		this.enumeratorList.Clear();
		yield break;
	}
}
