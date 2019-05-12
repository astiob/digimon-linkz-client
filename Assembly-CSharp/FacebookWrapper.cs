using Facebook.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookWrapper : MonoBehaviour
{
	private static GameObject go;

	private static FacebookWrapper instance;

	private Action<bool> initCallback;

	private Action<bool> authCallback;

	private Action getNameCallback;

	public static FacebookWrapper Instance
	{
		get
		{
			if (FacebookWrapper.instance == null)
			{
				FacebookWrapper.go = new GameObject("FacebookWrapper");
				UnityEngine.Object.DontDestroyOnLoad(FacebookWrapper.go);
				FacebookWrapper.instance = FacebookWrapper.go.AddComponent<FacebookWrapper>();
				FacebookSettings.SelectedAppIndex = 0;
			}
			return FacebookWrapper.instance;
		}
	}

	public bool IsInitialized
	{
		get
		{
			return FB.IsInitialized;
		}
	}

	public bool IsLoggedIn
	{
		get
		{
			return FB.IsLoggedIn;
		}
	}

	public string UserName { get; private set; }

	public void Init(Action<bool> callback)
	{
		this.initCallback = callback;
		if (!FB.IsInitialized)
		{
			FB.Init(new InitDelegate(this.InitCallback), new HideUnityDelegate(this.OnHideUnity), null);
		}
		else
		{
			FB.ActivateApp();
		}
	}

	private void InitCallback()
	{
		bool obj = false;
		if (FB.IsInitialized)
		{
			FB.ActivateApp();
			obj = true;
		}
		else
		{
			global::Debug.Log("Failed to Initialize the Facebook SDK");
		}
		if (this.initCallback != null)
		{
			this.initCallback(obj);
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
		}
	}

	public void LogIn(Action<bool> callBack, Action nameCallBack = null)
	{
		this.authCallback = callBack;
		this.getNameCallback = nameCallBack;
		List<string> permissions = new List<string>
		{
			"public_profile",
			"email",
			"user_friends"
		};
		FB.LogInWithReadPermissions(permissions, new FacebookDelegate<ILoginResult>(this.AuthCallback));
	}

	public string GetAccessToken()
	{
		string empty = string.Empty;
		if (FB.IsLoggedIn)
		{
			return AccessToken.CurrentAccessToken.TokenString;
		}
		return empty;
	}

	public string GetUserId()
	{
		string result = string.Empty;
		if (FB.IsLoggedIn)
		{
			result = AccessToken.CurrentAccessToken.UserId;
		}
		return result;
	}

	private void AuthCallback(ILoginResult result)
	{
		bool flag = false;
		if (FB.IsLoggedIn)
		{
			flag = true;
			if (flag)
			{
				this.GetProfileName(null);
			}
		}
		else
		{
			global::Debug.Log("User cancelled login");
		}
		if (this.authCallback != null)
		{
			this.authCallback(flag);
		}
	}

	public void LogOut()
	{
		FB.LogOut();
		this.UserName = string.Empty;
	}

	public void API(string query, HttpMethod method, FacebookDelegate<IGraphResult> callback = null, IDictionary<string, string> formData = null)
	{
		FB.API(query, method, callback, formData);
	}

	public void GetProfileName(Action nameCallBack = null)
	{
		if (nameCallBack != null)
		{
			this.getNameCallback = nameCallBack;
		}
		this.API("/me?fields=name", HttpMethod.POST, delegate(IGraphResult result)
		{
			if (result != null && result.ResultDictionary.ContainsKey("name"))
			{
				this.UserName = (string)result.ResultDictionary["name"];
				if (this.getNameCallback != null)
				{
					this.getNameCallback();
					this.getNameCallback = null;
				}
			}
		}, null);
	}
}
