using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class EmotionSenderMulti : MonoBehaviour
{
	[Header("受信用の各々のエモーションアイコンのスプライト")]
	[SerializeField]
	private UIAtlasSkinner[] iconSprites;

	[SerializeField]
	[Header("受信用の各々のエモーションアイコンのスプライト(PvP用)")]
	private UISprite[] emotionSprites;

	[NonSerialized]
	public List<GameObject> iconSpriteParents = new List<GameObject>();

	[Header("送信用アイコンたちが載ってる親")]
	[SerializeField]
	private GameObject dialog;

	[SerializeField]
	private UIButton[] emotionDialogButtons;

	private bool isWindowOpen;

	private Action<int> sendEmotionAction;

	private Action<UIButton> sendEmotionButtonAction;

	private void Initilaize()
	{
		if (this.iconSpriteParents.Count == 0)
		{
			if (this.iconSprites != null && 0 < this.iconSprites.Length)
			{
				for (int i = 0; i < this.iconSprites.Length; i++)
				{
					GameObject spriteParentObject = this.GetSpriteParentObject(this.iconSprites[i].transform);
					if (null != spriteParentObject)
					{
						this.iconSpriteParents.Add(spriteParentObject);
					}
				}
			}
			else if (this.emotionSprites != null && 0 < this.emotionSprites.Length)
			{
				for (int j = 0; j < this.emotionSprites.Length; j++)
				{
					GameObject spriteParentObject2 = this.GetSpriteParentObject(this.emotionSprites[j].transform);
					if (null != spriteParentObject2)
					{
						this.iconSpriteParents.Add(spriteParentObject2);
					}
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
		UISprite uisprite = this.iconSpriteParents[index].GetComponentInChildren<UISprite>();
		if (null == uisprite)
		{
			uisprite = this.iconSprites[index].GetComponent<UISprite>();
		}
		uisprite.spriteName = component.spriteName;
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
		UISprite uisprite = this.emotionSprites[index];
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
