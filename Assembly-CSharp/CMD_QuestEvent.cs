using System;
using System.Collections;
using UnityEngine;

public class CMD_QuestEvent : CMD
{
	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		base.StartCoroutine(this.Initialize(delegate(bool isSuccess)
		{
			if (isSuccess)
			{
				this.ShowDLG();
				this.Show(f, sizeX, sizeY, aT);
				if (GUIScreenHome.isManualScreenFadeIn)
				{
					this.StartCoroutine(this.FadeIn());
				}
				else
				{
					RestrictionInput.EndLoad();
				}
			}
			else
			{
				this.ClosePanel(false);
				RestrictionInput.EndLoad();
			}
		}));
	}

	private IEnumerator FadeIn()
	{
		GUIBase uiHome = GUIManager.GetGUI("UIHome");
		while (uiHome == null)
		{
			uiHome = GUIManager.GetGUI("UIHome");
			yield return null;
		}
		GUIScreenHome screenHome = uiHome.GetComponent<GUIScreenHome>();
		while (!screenHome.isFinishedStartLoading)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		base.StartCoroutine(screenHome.StartScreenFadeIn(delegate
		{
			RestrictionInput.EndLoad();
		}));
		yield break;
	}

	private IEnumerator Initialize(Action<bool> OnInitialized)
	{
		SoundMng.Instance().PlayGameBGM("bgm_203");
		this.InitializeUI();
		yield break;
	}

	private void InitializeUI()
	{
	}

	public void OnTouchedRanking()
	{
		global::Debug.Log("OnTouchedRanking");
	}

	public void OnTouchedReward()
	{
		global::Debug.Log("OnTouchedReward");
	}
}
