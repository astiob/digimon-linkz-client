using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LeadCapture : MonoBehaviour
{
	public static readonly string LEAD_CAPTURE_PREFS_KEY = "CaptureUpdateDateTimeArray";

	public static readonly string OPENED_CAPTURE_PREFS_KEY = "OpenedCaptureDateTime";

	private static LeadCapture instance;

	private DateTime? openedDateTime;

	private List<DateTime> updateDateTimeList = new List<DateTime>();

	private List<string> updateDateTimeStringList = new List<string>();

	public static LeadCapture Instance
	{
		get
		{
			return LeadCapture.instance;
		}
	}

	private List<string> UpdateDateTimeStringList
	{
		get
		{
			if (this.updateDateTimeStringList.Count == 0)
			{
				string @string = PlayerPrefs.GetString(LeadCapture.LEAD_CAPTURE_PREFS_KEY, string.Empty);
				if (!string.IsNullOrEmpty(@string))
				{
					this.updateDateTimeStringList = new List<string>(JsonMapper.ToObject<string[]>(@string));
				}
			}
			return this.updateDateTimeStringList;
		}
		set
		{
			this.updateDateTimeStringList = value;
			string value2 = JsonMapper.ToJson(this.updateDateTimeStringList.ToArray());
			PlayerPrefs.SetString(LeadCapture.LEAD_CAPTURE_PREFS_KEY, value2);
		}
	}

	private List<DateTime> UpdateDateTimeList
	{
		get
		{
			if (this.updateDateTimeList.Count == 0)
			{
				this.updateDateTimeList = this.ConvertStringIntoDateTime(this.UpdateDateTimeStringList);
			}
			return this.updateDateTimeList;
		}
		set
		{
			this.updateDateTimeList = value;
			List<string> list = new List<string>();
			foreach (DateTime dateTime in this.updateDateTimeList)
			{
				list.Add(dateTime.ToString());
			}
			this.UpdateDateTimeStringList = list;
		}
	}

	private DateTime? OpenedDateTime
	{
		get
		{
			DateTime? dateTime = this.openedDateTime;
			if (dateTime == null)
			{
				string @string = PlayerPrefs.GetString(LeadCapture.OPENED_CAPTURE_PREFS_KEY, string.Empty);
				if (!string.IsNullOrEmpty(@string))
				{
					this.openedDateTime = new DateTime?(DateTime.Parse(@string));
				}
			}
			return this.openedDateTime;
		}
		set
		{
			this.openedDateTime = value;
			PlayerPrefs.SetString(LeadCapture.OPENED_CAPTURE_PREFS_KEY, this.openedDateTime.ToString());
		}
	}

	private void Awake()
	{
		base.gameObject.SetActive(false);
		LeadCapture.instance = this;
	}

	private void OnDestroy()
	{
		LeadCapture.instance = null;
	}

	public void CheckCaptureUpdate()
	{
		if (this.UpdateDateTimeList.Count == 0)
		{
			base.gameObject.SetActive(true);
		}
		else
		{
			bool flag = false;
			foreach (DateTime dateTime in this.UpdateDateTimeList)
			{
				if (this.OpenedDateTime == null || (dateTime.CompareTo(ServerDateTime.Now) <= 0 && dateTime.CompareTo(this.OpenedDateTime) > 0))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				base.gameObject.SetActive(true);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	public void SaveCaptureUpdate(List<string> UpdateDateTimeList)
	{
		if (!this.CompareStringList(UpdateDateTimeList, this.UpdateDateTimeStringList))
		{
			this.UpdateDateTimeStringList = UpdateDateTimeList;
			base.gameObject.SetActive(false);
			this.OpenedDateTime = new DateTime?(ServerDateTime.Now);
		}
	}

	private bool CompareStringList(List<string> Value_1, List<string> Value_2)
	{
		if (Value_1.Count != Value_2.Count)
		{
			return false;
		}
		for (int i = 0; i < Value_1.Count; i++)
		{
			if (Value_1[i] != Value_2[i])
			{
				return false;
			}
		}
		return true;
	}

	private List<DateTime> ConvertStringIntoDateTime(List<string> StringList)
	{
		List<DateTime> list = new List<DateTime>();
		foreach (string s in StringList)
		{
			list.Add(DateTime.Parse(s));
		}
		list.Sort((DateTime a, DateTime b) => a.CompareTo(b));
		return list;
	}
}
