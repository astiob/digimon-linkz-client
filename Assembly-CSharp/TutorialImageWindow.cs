using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialImageWindow : MonoBehaviour
{
	[SerializeField]
	private GameObject windowLocator;

	[SerializeField]
	private GameObject closeButton;

	[SerializeField]
	private UILabel closeButtonLabel;

	[SerializeField]
	private BoxCollider pageButton;

	[SerializeField]
	private TutorialImageWindowArrow pageArrow;

	[SerializeField]
	private TutorialImageWindowPageMark pageMark;

	[SerializeField]
	private GameObject[] pages;

	private int pageNum;

	private int nowPageIndex;

	private Action actionCloseWindow;

	private Action actionCloseButtonClick;

	private SwipeControllerLR swipeCont;

	private bool isEnableBackButton;

	private void Start()
	{
		this.closeButtonLabel.text = StringMaster.GetString("SystemButtonClose");
	}

	public IEnumerator OpenWindow(List<string> pageResourcesNames, bool isDisplayThumbnail, Action closedAction, Action closeButtonClickAction = null)
	{
		this.nowPageIndex = 0;
		this.pageNum = pageResourcesNames.Count;
		if (isDisplayThumbnail)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.x += 100f;
			base.transform.localPosition = localPosition;
		}
		for (int i = 0; i < pageResourcesNames.Count; i++)
		{
			if (i >= this.pages.Length)
			{
				this.AddPages();
			}
			yield return base.StartCoroutine(this.LoadPage(pageResourcesNames[i], this.pages[i]));
		}
		this.pageMark.Initialize(pageResourcesNames.Count);
		this.pageArrow.Initialize(pageResourcesNames.Count);
		this.actionCloseWindow = closedAction;
		this.actionCloseButtonClick = closeButtonClickAction;
		this.pages[0].SetActive(true);
		this.StartWindowAnimation(true, TutorialImageWindow.WindowMoveType.ACTIVE);
		this.pageMark.StartDisplay();
		if (this.swipeCont == null)
		{
			this.swipeCont = base.gameObject.AddComponent<SwipeControllerLR>();
		}
		this.swipeCont.SetThreshold(30f);
		this.SetSwipeActionByEnd();
		this.closeButton.SetActive(true);
		TweenAlpha tweenAlpha = this.closeButton.GetComponent<TweenAlpha>();
		tweenAlpha.from = 0f;
		tweenAlpha.to = 1f;
		tweenAlpha.PlayForward();
		this.pageButton.gameObject.SetActive(true);
		this.DisplayedCloseButton();
		yield break;
	}

	public void CloseWindow()
	{
		this.OnPushedCloseButton();
	}

	private void AddPages()
	{
		GameObject gameObject = this.pages[0];
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.SetParent(gameObject.transform.parent);
		gameObject2.transform.localScale = gameObject.transform.localScale;
		gameObject2.transform.localPosition = gameObject.transform.localPosition;
		gameObject2.transform.localRotation = gameObject.transform.localRotation;
		gameObject2.SetActive(false);
		gameObject2.name = "Page" + (this.pages.Length + 1);
		this.pages = new List<GameObject>(this.pages)
		{
			gameObject2
		}.ToArray();
	}

	private void SetSwipeActionByEnd()
	{
		if (this.pageNum <= 1)
		{
			this.swipeCont.SetActionSwipe(null, null);
		}
		else if (this.nowPageIndex == 0)
		{
			this.swipeCont.SetActionSwipe(new Action(this.OnPushedArrowRight), null);
		}
		else if (this.nowPageIndex == this.pageNum - 1)
		{
			this.swipeCont.SetActionSwipe(null, new Action(this.OnPushedArrowLeft));
		}
		else
		{
			this.swipeCont.SetActionSwipe(new Action(this.OnPushedArrowRight), new Action(this.OnPushedArrowLeft));
		}
	}

	private IEnumerator LoadPage(string fileName, GameObject page)
	{
		UITexture texture = page.GetComponent<UITexture>();
		NGUIUtil.ChangeUITextureFromFile(texture, fileName, false);
		page.SetActive(false);
		yield return null;
		Resources.UnloadUnusedAssets();
		yield break;
	}

	private void ReleasePage(GameObject page)
	{
		if (page != null)
		{
			UITexture component = page.GetComponent<UITexture>();
			if (component != null)
			{
				component.mainTexture = null;
				Resources.UnloadUnusedAssets();
			}
		}
	}

	private void OnPushedArrowLeft()
	{
		this.StartWindowAnimation(false, TutorialImageWindow.WindowMoveType.RIGHT);
		this.pageArrow.StartInvisible();
		BoxCollider component = this.closeButton.GetComponent<BoxCollider>();
		component.enabled = false;
		this.swipeCont.SetActionSwipe(null, null);
		this.isEnableBackButton = false;
		this.pageButton.enabled = false;
	}

	private void OnPushedArrowRight()
	{
		this.StartWindowAnimation(false, TutorialImageWindow.WindowMoveType.LEFT);
		this.pageArrow.StartInvisible();
		BoxCollider component = this.closeButton.GetComponent<BoxCollider>();
		component.enabled = false;
		this.swipeCont.SetActionSwipe(null, null);
		this.isEnableBackButton = false;
		this.pageButton.enabled = false;
	}

	private void StartWindowAnimation(bool fade, TutorialImageWindow.WindowMoveType type)
	{
		TweenPosition component = this.windowLocator.GetComponent<TweenPosition>();
		if (type == TutorialImageWindow.WindowMoveType.LEFT)
		{
			if (fade)
			{
				component.tweenFactor = 0f;
				component.from = new Vector3(40f, 0f, 0f);
				component.to = Vector3.zero;
			}
			else
			{
				component.tweenFactor = 1f;
				component.to = Vector3.zero;
				component.from = new Vector3(-40f, 0f, 0f);
				EventDelegate.Set(component.onFinished, new EventDelegate.Callback(this.OnFinishedTweenMoveLeft));
			}
		}
		else if (type == TutorialImageWindow.WindowMoveType.RIGHT)
		{
			if (fade)
			{
				component.tweenFactor = 0f;
				component.from = new Vector3(-40f, 0f, 0f);
				component.to = Vector3.zero;
			}
			else
			{
				component.tweenFactor = 1f;
				component.to = Vector3.zero;
				component.from = new Vector3(40f, 0f, 0f);
				EventDelegate.Set(component.onFinished, new EventDelegate.Callback(this.OnFinishedTweenMoveRight));
			}
		}
		else
		{
			component.to = Vector3.zero;
			component.from = Vector3.zero;
		}
		UIPlayTween component2 = this.windowLocator.GetComponent<UIPlayTween>();
		component2.Play(fade);
	}

	private void OnFinishedTweenMoveLeft()
	{
		base.StartCoroutine(this.OpenNewRightPage());
	}

	private void OnFinishedTweenMoveRight()
	{
		base.StartCoroutine(this.OpenNewLeftPage());
	}

	private IEnumerator OpenNewLeftPage()
	{
		yield return base.StartCoroutine(this.Wait001Second());
		TweenPosition tween = this.windowLocator.GetComponent<TweenPosition>();
		EventDelegate.Remove(tween.onFinished, new EventDelegate.Callback(this.OnFinishedTweenMoveRight));
		this.pages[this.nowPageIndex].SetActive(false);
		if (0 < this.nowPageIndex)
		{
			this.nowPageIndex--;
		}
		GameObject page = this.pages[this.nowPageIndex];
		page.SetActive(true);
		this.windowLocator.transform.localPosition = Vector3.zero;
		this.pageMark.SetActiveMark(this.nowPageIndex);
		this.pageArrow.StartDisplay(this.nowPageIndex, this.pageNum);
		this.StartWindowAnimation(true, TutorialImageWindow.WindowMoveType.RIGHT);
		this.SetSwipeActionByEnd();
		this.DisplayedCloseButton();
		yield break;
	}

	private IEnumerator OpenNewRightPage()
	{
		yield return base.StartCoroutine(this.Wait001Second());
		TweenPosition tween = this.windowLocator.GetComponent<TweenPosition>();
		EventDelegate.Remove(tween.onFinished, new EventDelegate.Callback(this.OnFinishedTweenMoveLeft));
		this.pages[this.nowPageIndex].SetActive(false);
		this.nowPageIndex++;
		if (this.pageNum <= this.nowPageIndex)
		{
			this.nowPageIndex = this.pageNum - 1;
		}
		GameObject page = this.pages[this.nowPageIndex];
		page.SetActive(true);
		this.windowLocator.transform.localPosition = Vector3.zero;
		this.pageMark.SetActiveMark(this.nowPageIndex);
		this.pageArrow.StartDisplay(this.nowPageIndex, this.pageNum);
		this.StartWindowAnimation(true, TutorialImageWindow.WindowMoveType.LEFT);
		this.SetSwipeActionByEnd();
		this.DisplayedCloseButton();
		yield break;
	}

	private void OnPushedCloseButton()
	{
		UIPlayTween component = this.windowLocator.GetComponent<UIPlayTween>();
		EventDelegate.Set(component.onFinished, new EventDelegate.Callback(this.OnFinishedCloseTween));
		if (this.actionCloseButtonClick != null)
		{
			this.actionCloseButtonClick();
		}
		this.StartWindowAnimation(false, TutorialImageWindow.WindowMoveType.ACTIVE);
		this.pageMark.StartInvisible();
		this.pageArrow.StartInvisible();
		BoxCollider component2 = this.closeButton.GetComponent<BoxCollider>();
		component2.enabled = false;
		this.swipeCont.SetActionSwipe(null, null);
		this.isEnableBackButton = false;
		this.pageButton.gameObject.SetActive(false);
		TweenAlpha component3 = this.closeButton.GetComponent<TweenAlpha>();
		component3.from = 1f;
		component3.to = 0f;
		component3.PlayForward();
	}

	private void OnFinishedCloseTween()
	{
		base.StartCoroutine(this.OnCloseWindow());
	}

	private IEnumerator OnCloseWindow()
	{
		for (int i = 0; i < this.pageNum; i++)
		{
			this.ReleasePage(this.pages[i]);
		}
		yield return base.StartCoroutine(this.Wait001Second());
		UIPlayTween uiPlayTween = this.windowLocator.GetComponent<UIPlayTween>();
		EventDelegate.Remove(uiPlayTween.onFinished, new EventDelegate.Callback(this.OnFinishedCloseTween));
		this.pageNum = 0;
		this.nowPageIndex = 0;
		this.closeButton.SetActive(false);
		this.pageArrow.DeactiveArrows();
		if (this.actionCloseWindow != null)
		{
			this.actionCloseWindow();
			this.actionCloseWindow = null;
		}
		yield break;
	}

	private IEnumerator Wait001Second()
	{
		for (float time = 0f; time >= 0.01f; time += Time.unscaledDeltaTime)
		{
			yield return null;
		}
		yield break;
	}

	private void DisplayedCloseButton()
	{
		BoxCollider component = this.closeButton.GetComponent<BoxCollider>();
		component.enabled = true;
		this.isEnableBackButton = true;
		this.pageButton.enabled = (this.nowPageIndex != this.pageNum - 1);
	}

	private void Update()
	{
		if (this.isEnableBackButton && Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnPushedCloseButton();
		}
	}

	private enum WindowMoveType
	{
		ACTIVE,
		LEFT,
		RIGHT
	}
}
