using Master;
using System;
using System.Collections;
using UnityEngine;

public sealed class GUIScreenAssetBundleDownLoad : GUIScreen
{
	private const float deltaX = 5f;

	private const float tweenTime = 1f;

	private const string easeType = "easeOutCubic";

	[SerializeField]
	private GameObject line;

	[SerializeField]
	private GUIScreenAssetBundleDownLoad.DigimonInfo[] monsterInfos;

	private Transform[] monsterTransforms;

	private bool rotating;

	private int selectedIndex;

	[SerializeField]
	private UILabel nameLabel;

	[SerializeField]
	private UILabel detailLabel;

	[SerializeField]
	private UILabel downloadingLabel;

	[SerializeField]
	private UILabel adviceLabel;

	[SerializeField]
	private GameObject leftArrowButton;

	[SerializeField]
	private GameObject rightArrowButton;

	[SerializeField]
	private Transform threeDCameraTrans;

	private Transform monsterBaseTrans;

	private Transform circleTrans;

	private int futureIndex = -1;

	[SerializeField]
	private UIProgressBar downloadProgressBar;

	[SerializeField]
	private UILabel downloadProgressValue;

	[SerializeField]
	private UILabel downloadFileValue;

	private PartsGaugePoint partsGaugePoint;

	private SwipeControllerLR swipeController;

	protected override void Awake()
	{
		base.Awake();
		this.downloadingLabel.text = StringMaster.GetString("AssetDownloadTitle");
		this.adviceLabel.text = StringMaster.GetString("AssetDownloadCaution");
		this.SetFrontUIs(false);
		this.partsGaugePoint = base.GetComponentInChildren<PartsGaugePoint>();
		this.swipeController = base.gameObject.AddComponent<SwipeControllerLR>();
		this.swipeController.SetThreshold(30f);
		this.swipeController.SetActionSwipe(new Action(this.OnPushedRightButton), new Action(this.OnPushedLeftButton));
	}

	private void OnEnable()
	{
		Screen.sleepTimeout = -1;
	}

	private void OnDisable()
	{
		Screen.sleepTimeout = -2;
	}

	private void SetFrontUIs(bool isActive)
	{
		this.nameLabel.gameObject.SetActive(isActive);
		this.detailLabel.gameObject.SetActive(isActive);
		this.leftArrowButton.SetActive(isActive);
		this.rightArrowButton.SetActive(isActive);
	}

	public override void ShowGUI()
	{
		base.StartCoroutine(this.InitSHowGUI());
	}

	private IEnumerator InitSHowGUI()
	{
		yield return base.StartCoroutine(this.PreloadSpawnMonsters());
		this.InitAndSpawnOthers();
		GUIFadeControll.StartFadeIn(1f);
		RestrictionInput.EndLoad();
		yield return base.StartCoroutine(this.StartDownload());
		base.StartCoroutine(this.EndAssetBundleDownload());
		yield break;
	}

	private IEnumerator PreloadSpawnMonsters()
	{
		AssetBundleMng.Instance().SetLevel(string.Empty);
		string p17 = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId("17");
		string p18 = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId("74");
		string p19 = MonsterDataMng.Instance().GetMonsterCharaPathByMonsterGroupId("152");
		bool finish = false;
		AssetDataMng.Instance().LoadObjectASync(p17, delegate(UnityEngine.Object _obj)
		{
			finish = true;
		});
		while (!finish)
		{
			yield return null;
		}
		finish = false;
		AssetDataMng.Instance().LoadObjectASync(p18, delegate(UnityEngine.Object _obj)
		{
			finish = true;
		});
		while (!finish)
		{
			yield return null;
		}
		finish = false;
		AssetDataMng.Instance().LoadObjectASync(p19, delegate(UnityEngine.Object _obj)
		{
			finish = true;
		});
		while (!finish)
		{
			yield return null;
		}
		yield break;
	}

	private void InitAndSpawnOthers()
	{
		this.SetFrontUIs(true);
		this.monsterTransforms = new Transform[this.monsterInfos.Length];
		this.SpawnEvolveCircle();
		this.SpawnMonsters();
		this.SetDetails();
	}

	private void SetDetails()
	{
		string groupId = this.monsterInfos[this.selectedIndex].groupId;
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterGroupId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterGroupId(groupId);
		this.nameLabel.text = monsterGroupMasterByMonsterGroupId.monsterName;
		this.detailLabel.text = monsterGroupMasterByMonsterGroupId.description;
		this.line.SetActive(true);
		this.GetLeftDigimonTransform().localPosition = this.circleTrans.localPosition - Vector3.right * 5f;
		this.monsterTransforms[this.selectedIndex].localPosition = this.circleTrans.localPosition;
		this.GetRightDigimonTransform().localPosition = this.circleTrans.localPosition + Vector3.right * 5f;
	}

	private IEnumerator StartDownload()
	{
		AssetDataMng assetDataMng = AssetDataMng.Instance();
		if (!assetDataMng.IsAssetBundleDownloading())
		{
			int count = assetDataMng.GetDownloadAssetBundleCount(string.Empty);
			assetDataMng.StartDownloadAssetBundle(count, 4);
		}
		yield return base.StartCoroutine(this.UpdateProgressBar());
		yield break;
	}

	private IEnumerator UpdateProgressBar()
	{
		AssetDataMng assetDataMng = AssetDataMng.Instance();
		float downloadFileNum = (float)assetDataMng.RealABDL_TotalCount_LV();
		while (assetDataMng.IsAssetBundleDownloading())
		{
			float downloadedFileCount = (float)assetDataMng.RealABDL_NowCount_LV();
			this.SetProgressBar(downloadedFileCount, downloadFileNum);
			yield return null;
		}
		this.SetProgressBar(downloadFileNum, downloadFileNum);
		yield return new WaitForSeconds(0.25f);
		yield break;
	}

	private void SetProgressBar(float now, float max)
	{
		this.downloadProgressBar.value = now / max;
		this.downloadProgressValue.text = this.downloadProgressBar.value.ToString("P0");
		if (null != this.partsGaugePoint)
		{
			this.partsGaugePoint.SetGaugeValue(this.downloadProgressBar.value);
		}
		this.downloadFileValue.text = string.Format(StringMaster.GetString("SystemFraction"), (int)now, (int)max);
	}

	private IEnumerator EndAssetBundleDownload()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if ("1" != DataMng.Instance().RespDataCM_Login.tutorialStatus.endFlg)
		{
			global::Debug.Log("PartyTrack送信：チュートリアルABダウンロード終了");
			Partytrack.sendEvent(65600);
			TutorialFirstFinishRequest request = new TutorialFirstFinishRequest();
			yield return base.StartCoroutine(request.RequestFirstTutorialFinish());
		}
		ScreenController.ChangeHomeScreen(CMD_Tips.DISPLAY_PLACE.TitleToFarm);
		yield break;
	}

	private void SpawnEvolveCircle()
	{
		GameObject original = Resources.Load("Tutorial/EvolveCircle", typeof(GameObject)) as GameObject;
		this.circleTrans = UnityEngine.Object.Instantiate<GameObject>(original).transform;
		this.threeDCameraTrans.SetParent(null);
		this.threeDCameraTrans.localScale = Vector3.one;
		this.threeDCameraTrans.position = Vector3.forward * 3000f;
		this.circleTrans.SetParent(this.threeDCameraTrans);
		float num = 0.9f;
		this.circleTrans.localScale = new Vector3(num, 1f, num);
		this.circleTrans.localPosition = new Vector3(-0.68f, -0.68f, 2.37f);
	}

	private void SpawnMonsters()
	{
		int num = this.monsterInfos.Length;
		if (num < 3)
		{
			global::Debug.LogError("最低3匹必須エラー" + num.ToString());
			return;
		}
		int num2 = 0;
		foreach (GUIScreenAssetBundleDownLoad.DigimonInfo digimonInfo in this.monsterInfos)
		{
			this.SpawnMonster(digimonInfo.groupId, digimonInfo.scale, num2);
			num2++;
		}
	}

	private void SpawnMonster(string monsterGroupId, float scale, int index)
	{
		string path = "Characters/" + monsterGroupId + "/prefab";
		GameObject gameObject = UnityEngine.Object.Instantiate(AssetDataMng.Instance().LoadObject(path, null, true)) as GameObject;
		gameObject.name = "DIGIMON_" + monsterGroupId;
		int mask = LayerMask.NameToLayer("UI3D");
		Util.SetLayer(gameObject, mask);
		Transform transform = gameObject.transform;
		this.monsterTransforms[index] = transform;
		transform.SetParent(this.threeDCameraTrans);
		transform.localScale = new Vector3(scale, scale, scale);
		transform.localPosition = Vector3.up * 3000f;
		transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		CharacterParams component = gameObject.GetComponent<CharacterParams>();
		component.PlayIdleAnimation();
	}

	public override void OnDestroy()
	{
		if (this.threeDCameraTrans != null)
		{
			UnityEngine.Object.DestroyImmediate(this.threeDCameraTrans.gameObject);
		}
		base.OnDestroy();
	}

	private void OnPushedLeftButton()
	{
		if (this.monsterTransforms != null)
		{
			this.RunRotateMonsters(-1);
		}
	}

	private void OnPushedRightButton()
	{
		if (this.monsterTransforms != null)
		{
			this.RunRotateMonsters(1);
		}
	}

	private void RunRotateMonsters(int plusMinus)
	{
		if (this.rotating)
		{
			return;
		}
		this.rotating = true;
		this.nameLabel.text = string.Empty;
		this.detailLabel.text = string.Empty;
		this.line.SetActive(false);
		this.RunITween(plusMinus);
		this.futureIndex = this.selectedIndex + plusMinus;
		if (this.futureIndex > this.monsterInfos.Length - 1)
		{
			this.futureIndex = 0;
		}
		else if (this.futureIndex < 0)
		{
			this.futureIndex = this.monsterInfos.Length - 1;
		}
	}

	private Transform GetLeftDigimonTransform()
	{
		int num = (this.selectedIndex - 1 >= 0) ? (this.selectedIndex - 1) : (this.monsterTransforms.Length - 1);
		return this.monsterTransforms[num];
	}

	private Transform GetRightDigimonTransform()
	{
		int num = (this.selectedIndex + 1 != this.monsterTransforms.Length) ? (this.selectedIndex + 1) : 0;
		return this.monsterTransforms[num];
	}

	private void RunITween(int plusMinus)
	{
		Transform transform = this.monsterTransforms[this.selectedIndex];
		Transform leftDigimonTransform = this.GetLeftDigimonTransform();
		leftDigimonTransform.SetLocalX(transform.localPosition.x - 5f);
		iTween.MoveTo(leftDigimonTransform.gameObject, iTween.Hash(new object[]
		{
			"x",
			leftDigimonTransform.localPosition.x - (float)plusMinus * 5f,
			"time",
			1f,
			"easeType",
			"easeOutCubic",
			"islocal",
			true
		}));
		iTween.MoveTo(transform.gameObject, iTween.Hash(new object[]
		{
			"x",
			transform.localPosition.x - (float)plusMinus * 5f,
			"time",
			1f,
			"easeType",
			"easeOutCubic",
			"islocal",
			true,
			"oncomplete",
			"ITweenCallBack",
			"oncompletetarget",
			base.gameObject
		}));
		Transform rightDigimonTransform = this.GetRightDigimonTransform();
		rightDigimonTransform.SetLocalX(transform.localPosition.x + 5f);
		iTween.MoveTo(rightDigimonTransform.gameObject, iTween.Hash(new object[]
		{
			"x",
			rightDigimonTransform.localPosition.x - (float)plusMinus * 5f,
			"time",
			1f,
			"easeType",
			"easeOutCubic",
			"islocal",
			true
		}));
	}

	private void ITweenCallBack()
	{
		this.rotating = false;
		this.selectedIndex = this.futureIndex;
		this.SetDetails();
	}

	[Serializable]
	private struct DigimonInfo
	{
		[SerializeField]
		public string groupId;

		[SerializeField]
		public float scale;
	}
}
