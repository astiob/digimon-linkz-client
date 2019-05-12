using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUIListDropItemParts : GUIListPartBS
{
	private GUIListDropItemParts.Data data;

	private Vector2 beganPosition = Vector2.zero;

	private bool isTouchEndFromChild;

	private bool isTouching_mi;

	private bool isLongTouched;

	private float touchBeganTime;

	private bool _LongTouch = true;

	[Header("箱のアイコン")]
	[SerializeField]
	private UISprite boxIcons;

	[SerializeField]
	[Header("ドロップアイテム")]
	private PresentBoxItem dropItemItems;

	[SerializeField]
	[Header("運の文字アイコン")]
	private UILabel luckIcons;

	[SerializeField]
	[Header("マルチ運プレイヤーアイコン")]
	private GameObject goLuckPlayerIcons;

	[SerializeField]
	[Header("マルチの文字アイコン")]
	private UILabel multiIcons;

	[Header("マルチのオーナー報酬アイコン")]
	[SerializeField]
	private UILabel ownerRewardIcon;

	[SerializeField]
	[Header("マルチのゲスト報酬アイコン")]
	private UILabel guestRewardIcon;

	[SerializeField]
	[Header("チャレンジの文字アイコン")]
	private UILabel challengeIcon;

	[Header("イベントチップドロップのアイコン")]
	[SerializeField]
	private UILabel eventChipDropIcon;

	[Header("ドロップ数のラベル")]
	[SerializeField]
	private UILabel dropNumLabel;

	private ParticleSystem dropParticleSystem;

	private bool isPlayIconAnimation;

	private List<UIWidget> iconAnimationList = new List<UIWidget>();

	private int index;

	private float time;

	private float maxTime = 2f;

	private bool isNotShortTouchable;

	public bool LongTouch
	{
		get
		{
			return this._LongTouch;
		}
		set
		{
			this._LongTouch = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.boxIcons.gameObject.SetActive(false);
		this.luckIcons.gameObject.SetActive(false);
		this.goLuckPlayerIcons.SetActive(false);
		this.multiIcons.gameObject.SetActive(false);
		this.ownerRewardIcon.gameObject.SetActive(false);
		this.guestRewardIcon.gameObject.SetActive(false);
		this.challengeIcon.gameObject.SetActive(false);
		this.eventChipDropIcon.gameObject.SetActive(false);
		this.dropNumLabel.gameObject.SetActive(false);
	}

	public override void ShowGUI()
	{
		this.Init();
		base.ShowGUI();
	}

	private void Init()
	{
		IEnumerator enumerator = this.SetDrops(true, null);
		while (enumerator.MoveNext())
		{
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		this.beganPosition = pos;
		base.OnTouchBegan(touch, pos);
		this.isTouchEndFromChild = false;
		this.isTouching_mi = true;
		this.isLongTouched = false;
		this.touchBeganTime = Time.realtimeSinceStartup;
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
		float magnitude = (this.beganPosition - pos).magnitude;
		if (magnitude > 40f)
		{
			this.isTouching_mi = false;
		}
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		if (this.data == null)
		{
			return;
		}
		this.isTouching_mi = false;
		if (this.isLongTouched)
		{
			this.isLongTouched = false;
			return;
		}
		base.OnTouchEnded(touch, pos, flag);
		if (flag)
		{
			float magnitude = (this.beganPosition - pos).magnitude;
			if (magnitude < 40f && !this.isTouchEndFromChild)
			{
				if (this.isNotShortTouchable)
				{
					return;
				}
				if (this.data.actTouchShort != null)
				{
					this.data.actTouchShort(this.data);
				}
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.data == null)
		{
			return;
		}
		if (this.isTouching_mi && this.LongTouch && Time.realtimeSinceStartup - this.touchBeganTime >= 0.5f)
		{
			if (this.data.actTouchLong != null)
			{
				this.data.actTouchLong(this.data);
			}
			base.isTouching = false;
			this.isLongTouched = true;
			this.isTouching_mi = false;
		}
		if (this.dropParticleSystem != null && !this.dropParticleSystem.isPlaying)
		{
			this.dropParticleSystem.gameObject.SetActive(false);
		}
		this.UpdateIconAnimation();
	}

	public override void SetData()
	{
		this.data = GUISelectPanelDropItemList.partsDataList[base.IDX];
	}

	public override void InitParts()
	{
		this.ShowGUI();
	}

	public override void RefreshParts()
	{
		this.ShowGUI();
	}

	public IEnumerator SetDrops(bool isSkip = false, Action callBack = null)
	{
		this.index = 0;
		this.time = 0f;
		this.isPlayIconAnimation = false;
		this.iconAnimationList = new List<UIWidget>();
		if (!isSkip)
		{
			yield return new WaitForSeconds(0.6f);
		}
		switch (this.data.dropType)
		{
		case GUIListDropItemParts.DropType.Luck:
			this.AddIconAnimationList(this.luckIcons);
			break;
		case GUIListDropItemParts.DropType.Owner:
			this.AddIconAnimationList(this.ownerRewardIcon);
			break;
		case GUIListDropItemParts.DropType.Multi:
			this.AddIconAnimationList(this.multiIcons);
			break;
		case GUIListDropItemParts.DropType.LuckMulti:
		{
			this.AddIconAnimationList(this.luckIcons);
			GUIListDropItemParts.LuckDropUserInfo info = this.GetLuckDropUserInfo(0, this.data.multiLuckDropUserId);
			if (info != null)
			{
				int playerIconIndex = 0;
				MultiBattleData multiBattleData = ClassSingleton<MultiBattleData>.Instance;
				string[] uniqMultiUsers = multiBattleData.MultiUsers.Select((MultiUser item) => item.userId).Distinct<string>().ToArray<string>();
				int uniqUserLength = (uniqMultiUsers == null) ? 0 : uniqMultiUsers.Length;
				for (int i = 0; i < uniqUserLength; i++)
				{
					if (info.userId == uniqMultiUsers[i])
					{
						playerIconIndex = i;
						break;
					}
				}
				GameObject luckPlayerIcon = this.goLuckPlayerIcons;
				BattleResultLuckPlayerIcon playerIcon = luckPlayerIcon.GetComponent<BattleResultLuckPlayerIcon>();
				playerIcon.SetLuckPlayerIcon(playerIconIndex);
			}
			break;
		}
		case GUIListDropItemParts.DropType.Challenge:
			this.AddIconAnimationList(this.challengeIcon);
			break;
		case GUIListDropItemParts.DropType.EventChip:
			this.AddIconAnimationList(this.eventChipDropIcon);
			break;
		}
		PresentBoxItem presentBoxItem = this.dropItemItems;
		int assetCategoryId = (int)this.data.assetCategoryId;
		presentBoxItem.SetItem(assetCategoryId.ToString(), this.data.assetValue.ToString(), "1", false, null);
		this.dropItemItems.gameObject.SetActive(this.data.isEndOpenAnimation);
		this.dropNumLabel.text = "× " + this.data.assetNum;
		this.AddIconAnimationList(this.dropNumLabel);
		if (!isSkip)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (callBack != null)
		{
			callBack();
		}
		yield break;
	}

	private GUIListDropItemParts.LuckDropUserInfo GetLuckDropUserInfo(int no, string id)
	{
		GUIListDropItemParts.LuckDropUserInfo luckDropUserInfo = new GUIListDropItemParts.LuckDropUserInfo();
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		int num = -1;
		for (int i = 0; i < respData_WorldMultiStartInfo.party.Length; i++)
		{
			if (respData_WorldMultiStartInfo.party[i].userId == id)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return null;
		}
		luckDropUserInfo.no = no;
		luckDropUserInfo.userName = respData_WorldMultiStartInfo.party[num].nickname;
		luckDropUserInfo.userId = id;
		luckDropUserInfo.leaderMonsterId = respData_WorldMultiStartInfo.party[num].userMonsters[0].monsterId;
		luckDropUserInfo.leaderMonsterLuckNum = int.Parse(respData_WorldMultiStartInfo.party[num].userMonsters[0].luck);
		return luckDropUserInfo;
	}

	public void SetupDropIcon()
	{
		if (this.data.dropBoxType == GUIListDropItemParts.BoxType.NORMAL)
		{
			this.boxIcons.spriteName = "Common02_Icon_DropB";
		}
		else if (this.data.dropBoxType == GUIListDropItemParts.BoxType.RARE)
		{
			this.boxIcons.spriteName = "Common02_Icon_DropG";
		}
		else
		{
			global::Debug.LogError("boxType is is not valid.");
			this.boxIcons.spriteName = "Common02_Icon_DropB";
		}
		NGUITools.SetActiveSelf(this.boxIcons.gameObject, true);
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_302", 0f, false, true, null, -1);
		if (this.data.dropType == GUIListDropItemParts.DropType.LuckMulti)
		{
			this.goLuckPlayerIcons.SetActive(true);
		}
		this.ownerRewardIcon.alpha = 1f;
		this.multiIcons.alpha = 1f;
		this.luckIcons.alpha = 1f;
		this.challengeIcon.alpha = 1f;
		this.eventChipDropIcon.alpha = 1f;
		this.dropNumLabel.alpha = 0f;
	}

	public void DrawDropIcon(bool isSkip = false, Action callback = null)
	{
		MasterDataMng.AssetCategory assetCategoryId = this.data.assetCategoryId;
		if (assetCategoryId != MasterDataMng.AssetCategory.ITEM)
		{
			NGUITools.SetActiveSelf(this.boxIcons.gameObject, false);
		}
		else
		{
			this.boxIcons.spriteName = "Common02_item_flame";
			this.boxIcons.depth = this.dropItemItems.depth - 1;
		}
		this.isPlayIconAnimation = true;
		this.data.isEndOpenAnimation = true;
		if (isSkip)
		{
			NGUITools.SetActiveSelf(this.dropItemItems.gameObject, true);
			if (callback != null)
			{
				callback();
			}
		}
		else
		{
			this.SetFade(this.dropItemItems.gameObject, callback);
			this.ShowParticle();
		}
	}

	private void SetFade(GameObject go, Action callback = null)
	{
		NGUITools.SetActiveSelf(go, false);
		TweenAlpha tweenAlpha = go.GetComponent<TweenAlpha>();
		if (tweenAlpha == null)
		{
			tweenAlpha = go.AddComponent<TweenAlpha>();
		}
		tweenAlpha.from = 0f;
		tweenAlpha.to = 1f;
		if (callback != null)
		{
			tweenAlpha.onFinished.Clear();
			EventDelegate.Set(tweenAlpha.onFinished, delegate()
			{
				callback();
			});
		}
		tweenAlpha.PlayForward();
		NGUITools.SetActiveSelf(go, true);
	}

	private void ShowParticle()
	{
		string path = "Cutscenes/NewFX7";
		SoundMng.Instance().TryPlaySE("SEInternal/Common/se_303", 0f, false, true, null, -1);
		if (this.dropParticleSystem == null)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(path, typeof(GameObject)));
			gameObject.name = string.Format("DropParticle_{0}", 0);
			Transform transform = gameObject.transform;
			transform.parent = base.transform;
			transform.localPosition = new Vector3(0f, -45f, 0f);
			this.dropParticleSystem = gameObject.GetComponent<ParticleSystem>();
		}
		else
		{
			this.dropParticleSystem.gameObject.SetActive(true);
		}
	}

	private void AddIconAnimationList(UIWidget icon)
	{
		NGUITools.SetActiveSelf(icon.gameObject, true);
		icon.alpha = 0f;
		this.iconAnimationList.Add(icon);
	}

	private void UpdateIconAnimation()
	{
		if (this.isPlayIconAnimation)
		{
			if (this.iconAnimationList.Count > 1)
			{
				UIWidget uiwidget = this.iconAnimationList[this.index];
				uiwidget.alpha = Mathf.Sin(3.14159274f * this.time / this.maxTime);
				this.time += Time.deltaTime;
				if (this.time >= this.maxTime)
				{
					this.time = 0f;
					uiwidget.alpha = 0f;
					this.index++;
					if (this.index >= this.iconAnimationList.Count)
					{
						this.index = 0;
					}
				}
			}
			else
			{
				this.iconAnimationList[this.index].alpha = 1f;
			}
		}
	}

	public enum DropType
	{
		Standard,
		Luck,
		Owner,
		Multi,
		LuckMulti,
		Challenge,
		EventChip
	}

	public enum BoxType
	{
		NONE,
		NORMAL,
		RARE
	}

	public class LuckDropUserInfo
	{
		public int no;

		public string userId;

		public string userName;

		public string leaderMonsterId;

		public int leaderMonsterLuckNum;
	}

	public class Data
	{
		public int index;

		public Action<GUIListDropItemParts.Data> actTouchShort;

		public Action<GUIListDropItemParts.Data> actTouchLong;

		public GUIListDropItemParts.DropType dropType;

		public GUIListDropItemParts.BoxType dropBoxType;

		public MasterDataMng.AssetCategory assetCategoryId = MasterDataMng.AssetCategory.MONSTER;

		public int assetValue;

		public int assetNum;

		public string multiLuckDropUserId = string.Empty;

		public bool isEndOpenAnimation;
	}
}
