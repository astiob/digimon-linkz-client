using Master;
using System;
using UnityEngine;

public sealed class GUIBannerParts : GUIListPartBS
{
	[SerializeField]
	[Header("残り時間のラベル")]
	private UILabel timeLabel;

	[SerializeField]
	[Header("バナー読み込み失敗時のテキスト")]
	private UILabel failedTextLabel;

	[Header("ガシャの背景色")]
	[SerializeField]
	private Color gashaBGColor = new Color32(0, 80, 0, byte.MaxValue);

	[SerializeField]
	[Header("イベントの背景色")]
	private Color eventBGColor = new Color32(80, 0, 0, byte.MaxValue);

	[SerializeField]
	[Header("キャンペーンの背景色")]
	private Color campaignBGColor = new Color32(0, 0, 80, byte.MaxValue);

	[SerializeField]
	[Header("背景のスプライト")]
	private UISprite bgSprite;

	[SerializeField]
	[Header("外枠のスプライト")]
	private UISprite frameSprite;

	[SerializeField]
	private UITexture bannerImage;

	private DateTime restTimeDate;

	[Header("NEWのスプライト")]
	[SerializeField]
	private UISprite newSprite;

	[SerializeField]
	[Header("NEWをコントロールする")]
	private bool doNewControl;

	private static readonly int DAY_HOUR = 24;

	private static readonly int DAY_SECONDS = 86400;

	private static readonly int HOUR_SECONDS = 3600;

	private static readonly int MINUTES_SECONDS = 60;

	public GameWebAPI.RespDataMA_BannerM.BannerM Data { get; set; }

	public void SetBGColor()
	{
		GUIBannerParts.LinkCategoryType linkCategoryType = this.ConvertStringToEnum();
		if (linkCategoryType == GUIBannerParts.LinkCategoryType.Gasha)
		{
			this.bgSprite.color = this.gashaBGColor;
		}
		else if (linkCategoryType == GUIBannerParts.LinkCategoryType.Event)
		{
			this.bgSprite.color = this.eventBGColor;
		}
		else
		{
			this.bgSprite.color = this.campaignBGColor;
		}
	}

	private GUIBannerParts.LinkCategoryType ConvertStringToEnum()
	{
		GUIBannerParts.LinkCategoryType result = GUIBannerParts.LinkCategoryType.None;
		try
		{
			result = (GUIBannerParts.LinkCategoryType)((int)Enum.Parse(typeof(GUIBannerParts.LinkCategoryType), this.Data.linkCategoryType));
		}
		catch (ArgumentException)
		{
		}
		return result;
	}

	public void SetAction()
	{
		switch (this.ConvertStringToEnum())
		{
		case GUIBannerParts.LinkCategoryType.Quest:
			this.MethodToInvoke = "OnClickedQuest";
			break;
		case GUIBannerParts.LinkCategoryType.Shop:
			this.MethodToInvoke = "OnClickedShop";
			break;
		case GUIBannerParts.LinkCategoryType.Gasha:
			this.MethodToInvoke = "OnClickedGacha";
			break;
		case GUIBannerParts.LinkCategoryType.News:
			this.CallBackClass = base.gameObject;
			this.MethodToInvoke = "InfoShortcut";
			break;
		case GUIBannerParts.LinkCategoryType.Event:
			this.MethodToInvoke = "OnClickedQuest";
			break;
		case GUIBannerParts.LinkCategoryType.Reinforcement:
			this.MethodToInvoke = "OnClickedTraining";
			break;
		case GUIBannerParts.LinkCategoryType.Evolution:
			this.MethodToInvoke = "OnClickedEvo";
			break;
		case GUIBannerParts.LinkCategoryType.Meal:
			this.MethodToInvoke = "OnClickedMeal";
			break;
		case GUIBannerParts.LinkCategoryType.Schedule:
			this.CallBackClass = base.gameObject;
			this.MethodToInvoke = "ScheduleShortcut";
			break;
		default:
			global::Debug.LogError("ER : ===== 想定外遷移 =====");
			break;
		}
		this.restTimeDate = DateTime.Parse(this.Data.endTime);
		int restTimeSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		GUIBannerParts.SetTimeText(this.timeLabel, restTimeSeconds, this.restTimeDate);
		if (0 < restTimeSeconds)
		{
			base.InvokeRepeating("CountDown", 1f, 1f);
		}
		if (this.doNewControl)
		{
			this.SetNew();
			GUICollider component = base.gameObject.GetComponent<GUICollider>();
			component.onTouchEnded += delegate(Touch touch, Vector2 pos, bool flag)
			{
				if (flag)
				{
					this.ResetNew();
				}
			};
		}
	}

	private string GetPrefsID()
	{
		return "BANNER_IS_SHOWED_CAT_" + this.Data.linkCategoryType + "_ID_" + this.Data.bannerId;
	}

	public void SetNew()
	{
		string prefsID = this.GetPrefsID();
		if (PlayerPrefs.HasKey(prefsID))
		{
			if (this.newSprite != null)
			{
				this.newSprite.gameObject.SetActive(false);
			}
		}
		else if (this.newSprite != null)
		{
			this.newSprite.gameObject.SetActive(true);
		}
	}

	private void ResetNew()
	{
		if (this.newSprite != null)
		{
			this.newSprite.gameObject.SetActive(false);
		}
		string prefsID = this.GetPrefsID();
		PlayerPrefs.SetString(prefsID, DateTime.Now.ToString());
	}

	private void CountDown()
	{
		int restTimeSeconds = GUIBannerParts.GetRestTimeSeconds(this.restTimeDate);
		GUIBannerParts.SetTimeText(this.timeLabel, restTimeSeconds, this.restTimeDate);
		if (restTimeSeconds <= 0)
		{
			base.CancelInvoke("CountDown");
			this.CallBackClass = base.gameObject;
			this.MethodToInvoke = "CheckUpdateTime";
		}
	}

	public static int GetRestTimeSeconds(DateTime restDate)
	{
		int num = 0;
		if (restDate != DateTime.MinValue)
		{
			TimeSpan timeSpan = restDate - ServerDateTime.Now;
			if (2147483647.0 <= timeSpan.TotalSeconds)
			{
				num = 99999999;
			}
			else
			{
				num = (int)timeSpan.TotalSeconds;
				if (0 > num)
				{
					num = 0;
				}
			}
		}
		return num;
	}

	public static int GetRestTimeOneDaySeconds(DateTime restDate)
	{
		int num = 0;
		if (restDate != DateTime.MinValue)
		{
			DateTime now = ServerDateTime.Now;
			DateTime d = new DateTime(now.Year, now.Month, now.Day, restDate.Hour, restDate.Minute, restDate.Second);
			d = d.AddDays(1.0);
			num = (int)(d - now).TotalSeconds;
			if (0 > num)
			{
				num = 0;
			}
		}
		return num;
	}

	private static void SetDateFormat(UILabel label, DateTime restTimeDate)
	{
		label.text = string.Format(StringMaster.GetString("CountDownYMDhm"), new object[]
		{
			restTimeDate.Year,
			restTimeDate.Month,
			restTimeDate.Day,
			restTimeDate.Hour,
			restTimeDate.Minute
		});
	}

	public static void SetTimeText(UILabel label, int totalSeconds, DateTime restTimeDate)
	{
		if (totalSeconds > 0)
		{
			int num = totalSeconds / GUIBannerParts.DAY_SECONDS;
			if (1 <= num)
			{
				GUIBannerParts.SetActiveRestTime(true, label);
				GUIBannerParts.SetDateFormat(label, restTimeDate);
			}
			else
			{
				int num2 = (totalSeconds - GUIBannerParts.DAY_SECONDS * num) / GUIBannerParts.HOUR_SECONDS;
				if (1 <= num2)
				{
					GUIBannerParts.SetActiveRestTime(true, label);
					GUIBannerParts.SetDateFormat(label, restTimeDate);
				}
				else
				{
					GUIBannerParts.SetActiveRestTime(true, label);
					int num3 = (totalSeconds - GUIBannerParts.DAY_SECONDS * num - GUIBannerParts.HOUR_SECONDS * num2) / GUIBannerParts.MINUTES_SECONDS;
					int num4 = totalSeconds % GUIBannerParts.MINUTES_SECONDS;
					if (num3 >= 1)
					{
						label.text = string.Format(StringMaster.GetString("CountDownMS"), num3, num4);
					}
					else
					{
						label.text = string.Format(StringMaster.GetString("CountDownS"), num4);
					}
				}
			}
		}
		else
		{
			GUIBannerParts.SetActiveRestTime(true, label);
			label.text = StringMaster.GetString("CountDownEnd");
		}
	}

	public static void SetTimeTextForDayOfWeek(UILabel label, int totalSeconds, DateTime restTimeDate, bool useDateFormat = true)
	{
		if (totalSeconds > 0)
		{
			GUIBannerParts.SetActiveRestTime(true, label);
			int num = totalSeconds / GUIBannerParts.DAY_SECONDS;
			int num2 = (totalSeconds - GUIBannerParts.DAY_SECONDS * num) / GUIBannerParts.HOUR_SECONDS;
			int num3 = (totalSeconds - GUIBannerParts.DAY_SECONDS * num - GUIBannerParts.HOUR_SECONDS * num2) / GUIBannerParts.MINUTES_SECONDS;
			int num4 = totalSeconds % GUIBannerParts.MINUTES_SECONDS;
			if (useDateFormat && 1 <= num)
			{
				GUIBannerParts.SetDateFormat(label, restTimeDate);
			}
			else if (num2 >= 1)
			{
				label.text = string.Format(StringMaster.GetString("CountDownHM"), num2 + GUIBannerParts.DAY_HOUR * num, num3);
			}
			else if (num3 >= 1)
			{
				label.text = string.Format(StringMaster.GetString("CountDownMS"), num3, num4);
			}
			else
			{
				label.text = string.Format(StringMaster.GetString("CountDownS"), num4);
			}
		}
		else
		{
			GUIBannerParts.SetActiveRestTime(true, label);
			label.text = StringMaster.GetString("CountDownEnd");
		}
	}

	public static int GetSecondToDays(int totalSeconds)
	{
		return totalSeconds / GUIBannerParts.DAY_SECONDS;
	}

	public static int GetSecondToHours(int totalSeconds)
	{
		return (totalSeconds - GUIBannerParts.DAY_SECONDS * GUIBannerParts.GetSecondToDays(totalSeconds)) / GUIBannerParts.HOUR_SECONDS;
	}

	public static int GetSecondToMinutes(int totalSeconds)
	{
		return (totalSeconds - GUIBannerParts.DAY_SECONDS * GUIBannerParts.GetSecondToDays(totalSeconds) - GUIBannerParts.HOUR_SECONDS * GUIBannerParts.GetSecondToHours(totalSeconds)) / GUIBannerParts.MINUTES_SECONDS;
	}

	private static void SetActiveRestTime(bool active, UILabel text)
	{
		if (!active)
		{
			if (text.gameObject.activeSelf)
			{
				text.gameObject.SetActive(false);
			}
		}
		else if (!text.gameObject.activeSelf)
		{
			text.gameObject.SetActive(true);
		}
	}

	private void SetBannerErrorText()
	{
		this.failedTextLabel.text = this.Data.name;
		this.failedTextLabel.gameObject.SetActive(true);
	}

	private void InfoShortcut()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		cmdwebWindow.TitleText = this.Data.name;
		cmdwebWindow.Url = ConstValue.APP_WEB_DOMAIN + this.Data.url;
	}

	private void ScheduleShortcut()
	{
		this.InfoShortcut();
		PartsMenu.instance.RefreshMenuBannerNewAlert();
	}

	public void OnBannerReceived(Texture2D texture)
	{
		if (null == texture)
		{
			this.SetBannerErrorText();
		}
		else if (null != this.bannerImage)
		{
			this.bannerImage.mainTexture = texture;
		}
		this.SetAction();
	}

	private enum LinkCategoryType
	{
		None,
		Quest,
		Shop,
		Gasha,
		News,
		Event,
		Reinforcement,
		Evolution,
		Meal,
		Schedule
	}
}
