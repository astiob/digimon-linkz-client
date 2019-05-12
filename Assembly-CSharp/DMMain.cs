using Firebase.Messaging;
using Master;
using System;
using UnityEngine;

public class DMMain : MonoBehaviour
{
	private static bool isJustOnce;

	private void Awake()
	{
	}

	private void Start()
	{
		if (!DMMain.isJustOnce)
		{
			DMMain.isJustOnce = true;
			AdjustWrapper.Instance.StartAdjust();
			AlertMaster.Initialize();
			StringMaster.Initialize();
			FirebaseMessaging.TokenReceived += this.OnTokenReceived;
			FirebaseMessaging.MessageReceived += this.OnMessageReceived;
		}
		GUIMain.StartupScreen("UIStartupCaution");
	}

	public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
	{
		UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
	}

	public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
	{
		UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
	}
}
