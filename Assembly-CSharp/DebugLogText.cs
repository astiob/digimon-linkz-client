using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DebugLogText : MonoBehaviour
{
	private readonly int logMax = 50;

	private List<string> logList = new List<string>();

	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void DebugLogHandler(string Condition, string StackTrace, LogType Type)
	{
		if (this.logList.Count > this.logMax)
		{
			this.logList.RemoveAt(0);
		}
		string text = string.Concat(new object[]
		{
			"【",
			Type,
			"】\n",
			Condition,
			"\n"
		});
		if (Type == LogType.Error || Type == LogType.Exception)
		{
			text = text + StackTrace + "\n";
		}
		this.logList.Add(text);
		text = string.Empty;
		foreach (string str in this.logList)
		{
			text += str;
		}
		File.WriteAllText(Application.persistentDataPath + "/DebugLogText.txt", text, Encoding.UTF8);
	}
}
