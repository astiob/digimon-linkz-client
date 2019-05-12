using System;
using UnityEngine;

public sealed class TutorialMessageWindowText : MonoBehaviour
{
	private TutorialMessageWindowText.ShowTextInfo showTextInfo;

	public void SetText(string str)
	{
		this.showTextInfo.isColor = false;
		this.showTextInfo.count = 0;
		this.showTextInfo.message = this.ReplaceArguments(str);
	}

	public void ClearText()
	{
		UILabel component = base.GetComponent<UILabel>();
		component.text = string.Empty;
	}

	public bool UpdateDisplayText()
	{
		this.showTextInfo.count = this.GetShowTextCount(this.showTextInfo.message, this.showTextInfo.count);
		this.SetLabelText(this.showTextInfo.message.Substring(0, this.showTextInfo.count), this.showTextInfo.isColor);
		return this.showTextInfo.message.Length <= this.showTextInfo.count;
	}

	public void DisplayText()
	{
		this.showTextInfo.count = this.showTextInfo.message.Length;
		this.showTextInfo.isColor = false;
		this.SetLabelText(this.showTextInfo.message, false);
	}

	private void SetLabelText(string text, bool isColorEndTag)
	{
		UILabel component = base.GetComponent<UILabel>();
		component.text = text;
		if (isColorEndTag)
		{
			UILabel uilabel = component;
			uilabel.text += "[-]";
		}
	}

	private string ReplaceArguments(string message)
	{
		return message.Replace("$USER_INPUT_NAME$", DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.nickname);
	}

	private bool IsColorCode(char c)
	{
		return '[' == c;
	}

	private int SkipLineCode(string text, int index)
	{
		int num = index;
		if (text[index] == '\r' && text.Length > index + 1 && text[index + 1] == '\n')
		{
			num += 2;
		}
		else if (text[index] == '\n')
		{
			num++;
		}
		return num;
	}

	private int GetNextIndexToChara(string text, int index, char findChara)
	{
		int result = index;
		for (int i = index; i < text.Length; i++)
		{
			if (findChara == text[i])
			{
				result = i + 1;
				break;
			}
		}
		return result;
	}

	private bool IsStartColorCode(string text, int index)
	{
		return "[-]" != text.Substring(index, 3);
	}

	private TutorialMessageWindowText.CharaType GetCharaType(string text, int index)
	{
		if ((text[index] == '\r' && text.Length > index + 1 && text[index + 1] == '\n') || text[index] == '\n')
		{
			return TutorialMessageWindowText.CharaType.LINE;
		}
		if (text[index] == '[')
		{
			return TutorialMessageWindowText.CharaType.COLOR;
		}
		return TutorialMessageWindowText.CharaType.SHOW_CHARA;
	}

	private int GetShowTextCount(string text, int index)
	{
		bool flag = false;
		int num = index;
		while (!flag && text.Length > num)
		{
			TutorialMessageWindowText.CharaType charaType = this.GetCharaType(text, num);
			if (charaType != TutorialMessageWindowText.CharaType.LINE)
			{
				if (charaType != TutorialMessageWindowText.CharaType.COLOR)
				{
					if (charaType == TutorialMessageWindowText.CharaType.SHOW_CHARA)
					{
						num++;
						flag = true;
					}
				}
				else
				{
					this.showTextInfo.isColor = this.IsStartColorCode(text, num);
					num = this.GetNextIndexToChara(text, num, ']');
				}
			}
			else
			{
				num = this.SkipLineCode(text, num);
			}
		}
		if (text.Length < num)
		{
			num = text.Length;
		}
		return num;
	}

	private enum CharaType
	{
		LINE,
		COLOR,
		SHOW_CHARA
	}

	private struct ShowTextInfo
	{
		public string message;

		public int count;

		public bool isColor;
	}
}
