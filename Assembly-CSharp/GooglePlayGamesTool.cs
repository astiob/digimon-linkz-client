using Neptune.GameService;
using System;
using UnityEngine;

public class GooglePlayGamesTool
{
	private readonly string achievementIdClearQuest = "CgkI0fSur-kWEAIQAQ";

	private readonly string achievementIdMeal = "CgkI0fSur-kWEAIQAg";

	private readonly string achievementIdReinforce = "CgkI0fSur-kWEAIQAw";

	private readonly string achievementIdEvolution = "CgkI0fSur-kWEAIQBA";

	private readonly string achievementIdLaboratory = "CgkI0fSur-kWEAIQBQ";

	private static GooglePlayGamesTool instance;

	private GooglePlayGamesTool.SignState signState = GooglePlayGamesTool.SignState.Non;

	private bool isInitialized;

	private GooglePlayGamesTool()
	{
	}

	public static GooglePlayGamesTool Instance
	{
		get
		{
			if (GooglePlayGamesTool.instance == null)
			{
				GooglePlayGamesTool.instance = new GooglePlayGamesTool();
			}
			return GooglePlayGamesTool.instance;
		}
	}

	public bool IsSignIn
	{
		get
		{
			return NpSingleton<NpGameService>.Instance.IsSignedIn();
		}
	}

	public void Initialize(Action<bool> SignInResult)
	{
		this.signState = (GooglePlayGamesTool.SignState)PlayerPrefs.GetInt("GooglePlaySignState", -1);
		this.isInitialized = true;
		if (this.signState != GooglePlayGamesTool.SignState.Out)
		{
			this.SignIn(SignInResult);
		}
	}

	public void SignIn(Action<bool> SignInResult = null)
	{
		if (!this.isInitialized)
		{
			return;
		}
		NpSingleton<NpGameService>.Instance.SignedIn(NpGameServiceAndroid.CLIENT_TYPE.CLIENT_SNAPSHOT, delegate
		{
			PlayerPrefs.SetInt("GooglePlaySignState", 1);
			this.signState = GooglePlayGamesTool.SignState.In;
			if (SignInResult != null)
			{
				SignInResult(true);
			}
		}, delegate(string errorText)
		{
			global::Debug.LogWarning("NpGameService.Instance.SignedIn failed : " + errorText);
			PlayerPrefs.SetInt("GooglePlaySignState", 0);
			this.signState = GooglePlayGamesTool.SignState.Out;
			if (SignInResult != null)
			{
				SignInResult(false);
			}
		});
	}

	public void SignOut()
	{
		if (!this.isInitialized)
		{
			return;
		}
		NpSingleton<NpGameService>.Instance.SignOut();
		PlayerPrefs.SetInt("GooglePlaySignState", 0);
		this.signState = GooglePlayGamesTool.SignState.Out;
	}

	public void ShowAchievementsUI()
	{
		if (this.signState == GooglePlayGamesTool.SignState.Out || !this.isInitialized)
		{
			return;
		}
		NpSingleton<NpGameService>.Instance.ShowAchievements();
	}

	public void ClearQuest()
	{
		if (this.signState == GooglePlayGamesTool.SignState.Out || !this.isInitialized)
		{
			return;
		}
		NpSingleton<NpGameService>.Instance.IncrementAchievements(this.achievementIdClearQuest, 1, delegate
		{
		}, delegate(string errorText)
		{
			global::Debug.LogWarning(errorText);
		});
	}

	public void Meal(int MeatNum)
	{
		if (this.signState == GooglePlayGamesTool.SignState.Out || !this.isInitialized)
		{
			return;
		}
		NpSingleton<NpGameService>.Instance.IncrementAchievements(this.achievementIdMeal, MeatNum, delegate
		{
		}, delegate(string errorText)
		{
			global::Debug.LogWarning(errorText);
		});
	}

	public void Reinforce()
	{
		if (this.signState == GooglePlayGamesTool.SignState.Out || !this.isInitialized)
		{
			return;
		}
		NpSingleton<NpGameService>.Instance.IncrementAchievements(this.achievementIdReinforce, 1, delegate
		{
		}, delegate(string errorText)
		{
			global::Debug.LogWarning(errorText);
		});
	}

	public void Evolution()
	{
		if (this.signState == GooglePlayGamesTool.SignState.Out || !this.isInitialized)
		{
			return;
		}
		NpSingleton<NpGameService>.Instance.IncrementAchievements(this.achievementIdEvolution, 1, delegate
		{
		}, delegate(string errorText)
		{
			global::Debug.LogWarning(errorText);
		});
	}

	public void Laboratory()
	{
		if (this.signState == GooglePlayGamesTool.SignState.Out || !this.isInitialized)
		{
			return;
		}
		NpSingleton<NpGameService>.Instance.IncrementAchievements(this.achievementIdLaboratory, 1, delegate
		{
		}, delegate(string errorText)
		{
			global::Debug.LogWarning(errorText);
		});
	}

	private enum SignState
	{
		Non = -1,
		Out,
		In
	}
}
