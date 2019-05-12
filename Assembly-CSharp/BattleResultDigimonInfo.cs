using Master;
using System;
using UnityEngine;

public sealed class BattleResultDigimonInfo : MonoBehaviour
{
	private const int DIGIMON_COUNT = 2;

	private const float PARTICLE_POS_X_DELTA = 1f;

	private const float PARTICLE_POS_Y = 240f;

	[SerializeField]
	[Header("元レベルのLv部分")]
	private GameObject oldLevelTitle;

	[SerializeField]
	[Header("元レベルのテキスト")]
	private UILabel oldLevel;

	[SerializeField]
	[Header("新しいレベルのLv部分")]
	private GameObject newLevelTitle;

	[Header("新しいレベルのテキスト")]
	[SerializeField]
	private UILabel newLevel;

	[Header("経験値のテキスト")]
	[SerializeField]
	private UILabel exp;

	[Header("経験値の数字のテキスト")]
	[SerializeField]
	private UILabel expNum;

	[SerializeField]
	[Header("レベルアップアイコン")]
	private GameObject levelUpIcon;

	private Transform levelUpIconTrans;

	private Animation levelUpAnimtion;

	[Header("友情度アップアイコン")]
	[SerializeField]
	private GameObject friendUpIcon;

	private Transform friendUpIconTrans;

	private Animation friendUpAnimtion;

	[Header("下向きの矢印")]
	[SerializeField]
	private UISprite arrow;

	[Header("新しいレベルの左にある「Lv.」のテキスト自体")]
	[SerializeField]
	private UILabel newLevelText;

	[SerializeField]
	[Header("ゲージの背景")]
	private GameObject expGaugeBG;

	[SerializeField]
	[Header("黄色の経験値ゲージ")]
	private UIProgressBar expGauge;

	[SerializeField]
	[Header("アイコンのアンカー")]
	private Transform iconAnchor;

	[Header("レベルマックスの時のマーク")]
	[SerializeField]
	private GameObject levelMaxMark;

	private GameObject levelUpParticlePref;

	private GameObject expUpParticlePref;

	private bool isLevelMax;

	private int nowExp;

	private Transform myTransform;

	private DataMng.ExperienceInfo maxLevelInfo;

	private GameObject friendUpDigitIcon;

	public int DigimonNo { get; set; }

	private void Awake()
	{
		this.myTransform = base.transform;
		this.newLevel.gameObject.SetActive(false);
		NGUITools.SetActiveSelf(this.levelMaxMark, false);
		NGUITools.SetActiveSelf(this.arrow.gameObject, false);
		this.newLevelText.gameObject.SetActive(false);
		this.exp.text = string.Empty;
		this.expNum.text = string.Empty;
		string path = "UICommon/Parts/LevelUp";
		GameObject original = Resources.Load(path, typeof(GameObject)) as GameObject;
		this.levelUpIcon = UnityEngine.Object.Instantiate<GameObject>(original);
		this.levelUpIconTrans = this.levelUpIcon.transform;
		this.levelUpIconTrans.parent = this.myTransform;
		this.levelUpIconTrans.localPosition = Vector3.zero;
		this.levelUpIconTrans.localScale = Vector3.one;
		this.levelUpAnimtion = this.levelUpIcon.GetComponent<Animation>();
		this.ShowLevelUpIcon(false);
		string path2 = "UICommon/Parts/Friendship";
		GameObject original2 = Resources.Load(path2, typeof(GameObject)) as GameObject;
		this.friendUpIcon = UnityEngine.Object.Instantiate<GameObject>(original2);
		this.friendUpIconTrans = this.friendUpIcon.transform;
		this.friendUpIconTrans.parent = this.myTransform;
		this.friendUpIconTrans.localPosition = Vector3.zero;
		this.friendUpIconTrans.localScale = Vector3.one;
		this.friendUpAnimtion = this.friendUpIcon.GetComponent<Animation>();
		this.ShowFriendUpIcon(false);
	}

	public void CreateDetails(string userMonsterID)
	{
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM[] monsterExperienceM = MasterDataMng.Instance().RespDataMA_MonsterExperienceM.monsterExperienceM;
		MonsterData monsterDataByUserMonsterID = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(userMonsterID, false);
		this.nowExp = int.Parse(monsterDataByUserMonsterID.userMonster.ex);
		this.maxLevelInfo = new DataMng.ExperienceInfo();
		this.maxLevelInfo.lev = int.Parse(monsterDataByUserMonsterID.monsterM.maxLevel);
		if (this.maxLevelInfo.lev <= monsterExperienceM.Length)
		{
			string experienceNum = monsterExperienceM[this.maxLevelInfo.lev - 1].experienceNum;
			this.maxLevelInfo.exp = int.Parse(experienceNum);
		}
		this.oldLevel.text = monsterDataByUserMonsterID.userMonster.level;
		this.UpdateDetails(DataMng.Instance().GetExperienceInfo(this.nowExp));
	}

	public void CreateDetails(MonsterData monsterData)
	{
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM[] monsterExperienceM = MasterDataMng.Instance().RespDataMA_MonsterExperienceM.monsterExperienceM;
		this.nowExp = int.Parse(monsterData.userMonster.ex);
		this.maxLevelInfo = new DataMng.ExperienceInfo();
		this.maxLevelInfo.lev = int.Parse(monsterData.monsterM.maxLevel);
		if (this.maxLevelInfo.lev <= monsterExperienceM.Length)
		{
			string experienceNum = monsterExperienceM[this.maxLevelInfo.lev - 1].experienceNum;
			this.maxLevelInfo.exp = int.Parse(experienceNum);
		}
		this.oldLevel.text = monsterData.userMonster.level;
		this.UpdateDetails(DataMng.Instance().GetExperienceInfo(this.nowExp));
	}

	public void CreateDetails(int nowExp, int currentLevel, int maxLevel)
	{
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM[] monsterExperienceM = MasterDataMng.Instance().RespDataMA_MonsterExperienceM.monsterExperienceM;
		this.nowExp = nowExp;
		this.maxLevelInfo = new DataMng.ExperienceInfo();
		this.maxLevelInfo.lev = maxLevel;
		if (this.maxLevelInfo.lev <= monsterExperienceM.Length)
		{
			string experienceNum = monsterExperienceM[this.maxLevelInfo.lev - 1].experienceNum;
			this.maxLevelInfo.exp = int.Parse(experienceNum);
		}
		this.oldLevel.text = currentLevel.ToString();
		this.UpdateDetails(DataMng.Instance().GetExperienceInfo(this.nowExp));
	}

	public void AddExp(int addPoint, Action levelUpAction)
	{
		this.ShowExpUpParticle();
		DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(this.nowExp);
		int lev = experienceInfo.lev;
		if (lev < this.maxLevelInfo.lev)
		{
			this.nowExp += addPoint;
			this.nowExp = ((this.nowExp <= this.maxLevelInfo.exp) ? this.nowExp : this.maxLevelInfo.exp);
			experienceInfo = DataMng.Instance().GetExperienceInfo(this.nowExp);
			this.UpdateDetails(experienceInfo);
			if (lev < experienceInfo.lev)
			{
				this.SetLevelUp(experienceInfo);
				if (levelUpAction != null)
				{
					levelUpAction();
				}
			}
		}
	}

	public void AddFriend(int addPoint)
	{
		this.SetFriendUp(addPoint);
	}

	public bool IsFinishExpCountUp()
	{
		return this.nowExp >= this.maxLevelInfo.exp;
	}

	public bool IsFinishFriendShipUp()
	{
		return !this.friendUpAnimtion.isPlaying;
	}

	private void UpdateDetails(DataMng.ExperienceInfo experienceInfo)
	{
		if (this.maxLevelInfo.lev <= experienceInfo.lev || experienceInfo.expLevNext == 0)
		{
			NGUITools.SetActiveSelf(this.arrow.gameObject, false);
			NGUITools.SetActiveSelf(this.oldLevelTitle, false);
			NGUITools.SetActiveSelf(this.newLevelTitle, false);
			NGUITools.SetActiveSelf(this.oldLevel.gameObject, false);
			NGUITools.SetActiveSelf(this.newLevel.gameObject, false);
			NGUITools.SetActiveSelf(this.levelMaxMark, true);
			this.exp.text = StringMaster.GetString("BattleResult-08");
			this.expNum.text = StringMaster.GetString("CharaStatus-12");
			this.expGauge.value = 1f;
			this.isLevelMax = true;
		}
		else
		{
			this.exp.text = StringMaster.GetString("BattleResult-02");
			int num = experienceInfo.expLevAll - experienceInfo.expLev;
			this.expNum.text = num.ToString();
			this.expGauge.value = (float)experienceInfo.expLev / (float)experienceInfo.expLevAll;
		}
	}

	private void SetLevelUp(DataMng.ExperienceInfo experienceInfo)
	{
		if (!this.newLevel.gameObject.activeSelf && !this.isLevelMax)
		{
			this.newLevel.gameObject.SetActive(true);
			NGUITools.SetActiveSelf(this.arrow.gameObject, true);
			this.newLevelText.gameObject.SetActive(true);
		}
		this.ShowLevelUpIcon(true);
		this.newLevel.text = experienceInfo.lev.ToString();
		this.ShowLevelUpParticle();
	}

	private void SetFriendUp(int upVal)
	{
		this.ShowFriendUpIcon(true);
		if (this.friendUpDigitIcon != null)
		{
			UnityEngine.Object.Destroy(this.friendUpDigitIcon);
			this.friendUpDigitIcon = null;
		}
		this.friendUpDigitIcon = new GameObject();
		this.friendUpDigitIcon.transform.SetParent(this.iconAnchor);
		this.friendUpDigitIcon.transform.localPosition = Vector3.zero;
		this.friendUpDigitIcon.transform.localScale = Vector3.one;
		UISprite component = this.friendUpIcon.GetComponent<UISprite>();
		component.transform.SetParent(this.friendUpDigitIcon.transform);
		component.transform.localScale = Vector3.one;
		component.spriteName = "Common02_Friendship";
		component.MakePixelPerfect();
		int num = 1;
		foreach (char c in upVal.ToString())
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(this.friendUpDigitIcon.transform);
			gameObject.transform.localScale = Vector3.one;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(component.gameObject);
			gameObject2.SetActive(true);
			gameObject2.transform.SetParent(gameObject.transform);
			Animation component2 = gameObject2.GetComponent<Animation>();
			component2["FriendshipUP"].time = 0f;
			component2.Play("FriendshipUP");
			UISprite component3 = gameObject2.GetComponent<UISprite>();
			component3.spriteName = "Common02_FriendshipN_" + c;
			component3.MakePixelPerfect();
			gameObject.transform.localPosition = new Vector3((float)component.width - 85.4f + (float)(component3.width * num), 0f, 0f);
			float num2 = (float)component.width + ((float)component.width - 85.4f + (float)(component3.width * num));
			this.friendUpDigitIcon.transform.localPosition = new Vector3(-num2 * 0.5f, 0f, 0f);
			num++;
		}
	}

	private void ShowLevelUpParticle()
	{
		if (this.DigimonNo > 2)
		{
			global::Debug.LogError("DigimonNo is not valid.");
			return;
		}
		string path = "Cutscenes/NewFX6";
		if (this.levelUpParticlePref == null)
		{
			this.levelUpParticlePref = (Resources.Load(path, typeof(GameObject)) as GameObject);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.levelUpParticlePref);
		gameObject.name = string.Format("DigimonLevelUpParticle_{0}", this.DigimonNo);
		Transform transform = gameObject.transform;
		int num = this.DigimonNo + 1 - 2;
		transform.parent = this.myTransform;
		transform.localPosition = new Vector3((float)num * 1f, -90f, 0f);
	}

	private void ShowExpUpParticle()
	{
		if (this.DigimonNo > 2)
		{
			global::Debug.LogError("DigimonNo is not valid.");
			return;
		}
		string path = "Cutscenes/NewFX9";
		if (this.expUpParticlePref == null)
		{
			this.expUpParticlePref = (Resources.Load(path, typeof(GameObject)) as GameObject);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.expUpParticlePref);
			gameObject.name = string.Format("DigimonExpUpParticle_{0}", this.DigimonNo);
			Transform transform = gameObject.transform;
			int num = this.DigimonNo + 1 - 2;
			transform.parent = this.myTransform;
			transform.localPosition = new Vector3((float)num * 1f, 240f, 0f);
			transform.localScale = new Vector3(1200f, 320f, 320f);
			return;
		}
	}

	public void SetDepth(int depth)
	{
		DepthController depthController = this.levelUpIcon.AddComponent<DepthController>();
		depthController.AddWidgetDepth(this.levelUpIconTrans, depth + 10);
	}

	private void ShowLevelUpIcon(bool isShow)
	{
		NGUITools.SetActiveSelf(this.levelUpIcon.gameObject, isShow);
		NGUITools.SetActiveSelf(this.levelUpIcon, isShow);
		if (isShow)
		{
			this.levelUpAnimtion["LevelUp"].time = 0f;
			this.levelUpAnimtion.Play("LevelUp");
		}
	}

	private void ShowFriendUpIcon(bool isShow)
	{
		NGUITools.SetActiveSelf(this.friendUpIcon.gameObject, isShow);
		NGUITools.SetActiveSelf(this.friendUpIcon, isShow);
		if (this.friendUpDigitIcon != null)
		{
			NGUITools.SetActiveSelf(this.friendUpDigitIcon, isShow);
		}
		if (isShow)
		{
			this.friendUpAnimtion["FriendshipUP"].time = 0f;
			this.friendUpAnimtion.Play("FriendshipUP");
		}
	}

	public void FixExp(int addPoint)
	{
		DataMng.ExperienceInfo experienceInfo = DataMng.Instance().GetExperienceInfo(this.nowExp);
		int lev = experienceInfo.lev;
		while (0 < addPoint && lev < this.maxLevelInfo.lev)
		{
			int num = Mathf.Min(addPoint, experienceInfo.expLevNext);
			this.nowExp += num;
			if (this.nowExp > this.maxLevelInfo.exp)
			{
				this.nowExp = this.maxLevelInfo.exp;
				addPoint = 0;
			}
			else
			{
				addPoint -= num;
			}
			experienceInfo = DataMng.Instance().GetExperienceInfo(this.nowExp);
			this.UpdateDetails(experienceInfo);
		}
		this.ShowLevelUpIcon(false);
		this.ShowFriendUpIcon(false);
	}

	public void FixFriend()
	{
		this.ShowFriendUpIcon(false);
	}

	public Transform GetIconLocator()
	{
		return this.iconAnchor;
	}
}
