using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class EmotionSenderMulti : MonoBehaviour
{
	[Header("受信用の各々のエモーションアイコンのスプライト(UIAtlasSkinnerを使わない方)")]
	[SerializeField]
	private UISprite[] emotionSprites;

	[NonSerialized]
	public List<GameObject> iconSpriteParents = new List<GameObject>();

	[SerializeField]
	[Header("送信用アイコンたちが載ってる親")]
	private GameObject dialog;

	[SerializeField]
	private UIButton[] emotionDialogButtons;

	private bool isWindowOpen;

	private Action<int> sendEmotionAction;

	private Action<UIButton> sendEmotionButtonAction;

	private void Awake()
	{
		if (this.emotionSprites != null && 0 < this.emotionSprites.Length)
		{
			for (int i = 0; i < this.emotionSprites.Length; i++)
			{
				if (this.emotionSprites[i])
				{
					this.emotionSprites[i].gameObject.SetActive(false);
				}
			}
		}
		NGUITools.SetActiveSelf(this.dialog, false);
	}

	private void Initilaize()
	{
		if (this.iconSpriteParents.Count == 0 && this.emotionSprites != null && 0 < this.emotionSprites.Length)
		{
			for (int i = 0; i < this.emotionSprites.Length; i++)
			{
				GameObject spriteParentObject = this.GetSpriteParentObject(this.emotionSprites[i].transform);
				if (null != spriteParentObject)
				{
					this.iconSpriteParents.Add(spriteParentObject);
				}
			}
		}
	}

	private GameObject GetSpriteParentObject(Transform childObject)
	{
		if (!(null != childObject.parent))
		{
			return null;
		}
		Animator component = childObject.parent.GetComponent<Animator>();
		if (null != component)
		{
			return component.gameObject;
		}
		return this.GetSpriteParentObject(childObject.parent);
	}

	public void HideAll()
	{
		for (int i = 0; i < this.iconSpriteParents.Count; i++)
		{
			GameObject go = this.iconSpriteParents[i];
			NGUITools.SetActiveSelf(go, false);
		}
	}

	public void SetEmotion(int index, int emotionType, bool isOther = false)
	{
		UIButton uibutton = this.emotionDialogButtons[emotionType];
		UISprite component = uibutton.GetComponent<UISprite>();
		if (null == component)
		{
			Transform child = uibutton.transform.GetChild(0);
			if (null != child)
			{
				component = child.GetComponent<UISprite>();
			}
		}
		NGUITools.SetActiveSelf(this.iconSpriteParents[index], false);
		NGUITools.SetActiveSelf(this.iconSpriteParents[index], true);
		UISprite componentInChildren = this.iconSpriteParents[index].GetComponentInChildren<UISprite>();
		componentInChildren.gameObject.SetActive(true);
		componentInChildren.spriteName = component.spriteName;
		if (isOther)
		{
			SoundPlayer.PlayBattlePopupOtherEmotionSE();
			return;
		}
		NGUITools.SetActiveSelf(this.dialog, false);
		this.isWindowOpen = false;
	}

	public void SetEmotion(int index, string spriteName, bool isOther = false)
	{
		NGUITools.SetActiveSelf(this.iconSpriteParents[index], false);
		NGUITools.SetActiveSelf(this.iconSpriteParents[index], true);
		this.emotionSprites[index].gameObject.SetActive(true);
		UISprite uisprite = this.emotionSprites[index];
		uisprite.gameObject.SetActive(true);
		uisprite.spriteName = spriteName;
		if (isOther)
		{
			SoundPlayer.PlayBattlePopupOtherEmotionSE();
			return;
		}
		NGUITools.SetActiveSelf(this.dialog, false);
		this.isWindowOpen = false;
	}

	public void Initialize(UIButton button, Action<int> sendEmotionAction)
	{
		this.Initilaize();
		this.Initialize(this, button, sendEmotionAction);
	}

	public void Initialize(UIButton button, Action<UIButton> sendEmotionAction)
	{
		this.Initilaize();
		this.sendEmotionButtonAction = sendEmotionAction;
		this.SetupButtons(this, button);
		foreach (UIButton uibutton in this.emotionDialogButtons)
		{
			EventDelegate eventDelegate = new EventDelegate(this, "OnSendEmotionSpriteNameAction");
			eventDelegate.parameters[0] = new EventDelegate.Parameter(uibutton);
			uibutton.onClick.Add(eventDelegate);
		}
	}

	private void Initialize(MonoBehaviour target, UIButton button, Action<int> sendEmotionAction)
	{
		this.sendEmotionAction = sendEmotionAction;
		this.SetupButtons(target, button);
		this.emotionDialogButtons.Select((UIButton v, int i) => new
		{
			v,
			i
		}).ToList().ForEach(delegate(item)
		{
			EventDelegate eventDelegate = new EventDelegate(target, "OnSendEmotionAction");
			eventDelegate.parameters[0] = new EventDelegate.Parameter(item.i);
			item.v.onClick.Add(eventDelegate);
		});
	}

	private void SetupButtons(MonoBehaviour target, UIButton button)
	{
		EventDelegate item = new EventDelegate(this, "SetActiveDialog");
		button.onClick.Add(item);
		NGUITools.SetActiveSelf(this.dialog, false);
		foreach (GameObject gameObject in this.iconSpriteParents)
		{
			GameObject go = gameObject;
			AnimatorFinishEventTrigger component = gameObject.GetComponent<AnimatorFinishEventTrigger>();
			if (component == null)
			{
				break;
			}
			component.OnFinishAnimation = delegate(string str)
			{
				NGUITools.SetActiveSelf(go, false);
			};
		}
	}

	private void OnSendEmotionAction(int value)
	{
		if (this.sendEmotionAction != null)
		{
			this.sendEmotionAction(value);
		}
	}

	private void OnSendEmotionSpriteNameAction(UIButton button)
	{
		if (this.sendEmotionButtonAction != null)
		{
			this.sendEmotionButtonAction(button);
		}
	}

	private void SetActiveDialog()
	{
		this.isWindowOpen = !this.isWindowOpen;
		NGUITools.SetActiveSelf(this.dialog, this.isWindowOpen);
		if (this.isWindowOpen)
		{
			SoundPlayer.PlayButtonSelect();
		}
		else
		{
			SoundPlayer.PlayButtonCancel();
		}
	}
}
