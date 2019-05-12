using System;
using System.Collections;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
	[SerializeField]
	private TutorialMask maskBlack;

	[SerializeField]
	private TutorialFade fade;

	[SerializeField]
	private TutorialTarget targetUI;

	[SerializeField]
	private TutorialSkipButton skipButton;

	private TutorialMessageWindow messageWindow;

	private TutorialThumbnail thumbnail;

	private TutorialImageWindow imageWindow;

	private TutorialNonFrameText nonFrameText;

	private TutorialSelect selectItem;

	public TutorialMessageWindow MessageWindow
	{
		get
		{
			return this.messageWindow;
		}
	}

	public TutorialThumbnail Thumbnail
	{
		get
		{
			return this.thumbnail;
		}
	}

	public TutorialImageWindow ImageWindow
	{
		get
		{
			return this.imageWindow;
		}
	}

	public TutorialNonFrameText NonFrameText
	{
		get
		{
			return this.nonFrameText;
		}
	}

	public TutorialMask MaskBlack
	{
		get
		{
			return this.maskBlack;
		}
	}

	public TutorialFade Fade
	{
		get
		{
			return this.fade;
		}
	}

	public TutorialSelect SelectItem
	{
		get
		{
			return this.selectItem;
		}
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.messageWindow);
		this.messageWindow = null;
		UnityEngine.Object.Destroy(this.imageWindow);
		this.imageWindow = null;
		UnityEngine.Object.Destroy(this.nonFrameText);
		this.nonFrameText = null;
		UnityEngine.Object.Destroy(this.thumbnail);
		this.thumbnail = null;
		UnityEngine.Object.Destroy(this.selectItem);
		this.selectItem = null;
	}

	public IEnumerator LoadMessageWindow()
	{
		if (null == this.messageWindow)
		{
			GameObject go = AssetDataMng.Instance().LoadObject("Tutorial/TutorialMessageWindow", null, true) as GameObject;
			GameObject ui = UnityEngine.Object.Instantiate<GameObject>(go);
			yield return null;
			ui.transform.parent = base.transform;
			ui.transform.localPosition = Vector3.zero;
			ui.transform.localScale = Vector3.one;
			ui.transform.localRotation = Quaternion.identity;
			this.messageWindow = ui.GetComponent<TutorialMessageWindow>();
			go = null;
			Resources.UnloadUnusedAssets();
		}
		yield break;
	}

	public IEnumerator LoadImageWindow()
	{
		if (null == this.imageWindow)
		{
			GameObject go = AssetDataMng.Instance().LoadObject("Tutorial/TutorialImageWindow", null, true) as GameObject;
			GameObject ui = UnityEngine.Object.Instantiate<GameObject>(go);
			yield return null;
			ui.transform.parent = base.transform;
			ui.transform.localPosition = Vector3.zero;
			ui.transform.localScale = Vector3.one;
			ui.transform.localRotation = Quaternion.identity;
			this.imageWindow = ui.GetComponent<TutorialImageWindow>();
			go = null;
			Resources.UnloadUnusedAssets();
		}
		yield break;
	}

	public IEnumerator LoadNonFrameText()
	{
		if (null == this.nonFrameText)
		{
			GameObject go = AssetDataMng.Instance().LoadObject("Tutorial/TutorialNonFrameText", null, true) as GameObject;
			GameObject ui = UnityEngine.Object.Instantiate<GameObject>(go);
			yield return null;
			ui.transform.parent = base.transform;
			ui.transform.localPosition = Vector3.zero;
			ui.transform.localScale = Vector3.one;
			ui.transform.localRotation = Quaternion.identity;
			this.nonFrameText = ui.GetComponent<TutorialNonFrameText>();
			go = null;
			Resources.UnloadUnusedAssets();
		}
		yield break;
	}

	public IEnumerator LoadThumbnail()
	{
		if (null == this.thumbnail)
		{
			GameObject go = AssetDataMng.Instance().LoadObject("Tutorial/TutorialThumbnail", null, true) as GameObject;
			GameObject img = UnityEngine.Object.Instantiate<GameObject>(go);
			yield return null;
			img.transform.parent = base.transform;
			img.transform.localScale = Vector3.one;
			img.transform.localRotation = Quaternion.identity;
			img.transform.localPosition = Vector3.zero;
			this.thumbnail = img.GetComponent<TutorialThumbnail>();
			go = null;
			Resources.UnloadUnusedAssets();
		}
		yield break;
	}

	public IEnumerator LoadSelectItem()
	{
		if (null == this.selectItem)
		{
			GameObject go = AssetDataMng.Instance().LoadObject("Tutorial/TutorialSelect", null, true) as GameObject;
			GameObject ui = UnityEngine.Object.Instantiate<GameObject>(go);
			yield return null;
			ui.transform.parent = base.transform;
			ui.transform.localScale = Vector3.one;
			ui.transform.localRotation = Quaternion.identity;
			ui.transform.localPosition = Vector3.zero;
			this.selectItem = ui.GetComponent<TutorialSelect>();
			go = null;
			Resources.UnloadUnusedAssets();
		}
		yield break;
	}

	public void SetTargetUI(bool enableFrame, TutorialTarget.ArrowPositon arrowPosition, GameObject targetUI, Action onFinishedDisplayTween)
	{
		this.targetUI.StartDisplay(enableFrame, true, arrowPosition, targetUI, onFinishedDisplayTween);
	}

	public void ClearTargetUI(Action completed)
	{
		this.targetUI.StartInvisible(completed);
	}

	public void SetSkipButton(bool active, Action onPushedAction)
	{
		if (active)
		{
			this.skipButton.DisplaySkipButton();
			this.skipButton.SetPushedAction(onPushedAction);
		}
		else
		{
			this.skipButton.InvisibleSkipButton();
		}
	}

	public void SetMessageWindow(TutorialMessageWindow window)
	{
		this.messageWindow = window;
	}

	public void SetImageWindow(TutorialImageWindow window)
	{
		this.imageWindow = window;
	}

	public void SetThumbnail(TutorialThumbnail thumbnail)
	{
		this.thumbnail = thumbnail;
	}
}
