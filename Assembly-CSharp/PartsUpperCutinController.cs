using System;
using System.Collections;
using UnityEngine;

public class PartsUpperCutinController : MonoBehaviour
{
	[SerializeField]
	private Animator cutinAnimator;

	private static PartsUpperCutinController instance;

	private bool isLock;

	public bool IsPlaying
	{
		get
		{
			return this.isLock;
		}
	}

	public static PartsUpperCutinController Instance
	{
		get
		{
			return PartsUpperCutinController.instance;
		}
	}

	private void Awake()
	{
		PartsUpperCutinController.instance = this;
	}

	private void Start()
	{
		this.cutinAnimator.enabled = false;
	}

	private void OnDestroy()
	{
		PartsUpperCutinController.instance = null;
	}

	public static PartsUpperCutinController Create(Transform parentObject, int sortingOrder)
	{
		PartsUpperCutinController original = Resources.Load<PartsUpperCutinController>("UI/PartsUpperCutin");
		PartsUpperCutinController partsUpperCutinController = UnityEngine.Object.Instantiate<PartsUpperCutinController>(original);
		Transform transform = partsUpperCutinController.transform;
		transform.parent = parentObject;
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		UIPanel component = partsUpperCutinController.GetComponent<UIPanel>();
		component.sortingOrder = sortingOrder;
		return partsUpperCutinController;
	}

	public Coroutine PlayAnimator(PartsUpperCutinController.AnimeType PlayType, Action OnPlayed = null)
	{
		if (!this.Lock())
		{
			return null;
		}
		return base.StartCoroutine(this.PlayAnimator_(PlayType, OnPlayed));
	}

	private IEnumerator PlayAnimator_(PartsUpperCutinController.AnimeType PlayType, Action OnPlayed)
	{
		this.cutinAnimator.enabled = true;
		this.cutinAnimator.Play(PlayType.ToString());
		SoundMng.Instance().TryPlaySE("SEInternal/CutScene/se_214", 0f, false, true, null, -1);
		if (this.cutinAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
		{
			while (this.cutinAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
			{
				yield return null;
			}
		}
		while (this.cutinAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			yield return null;
		}
		this.cutinAnimator.StopPlayback();
		this.cutinAnimator.enabled = false;
		this.UnLock();
		if (OnPlayed != null)
		{
			OnPlayed();
		}
		yield break;
	}

	private bool Lock()
	{
		if (this.isLock)
		{
			return false;
		}
		this.isLock = true;
		return true;
	}

	private void UnLock()
	{
		this.isLock = false;
	}

	public enum AnimeType
	{
		LevelUp,
		LuckUp,
		ResistanceChange,
		InheritanceComplete,
		EvolutionComplete,
		ModeChangeComplete,
		AwakeningComplete,
		ResearchComplete,
		Jogress,
		Combine,
		TalentBlooms,
		BloomsFailed,
		NONE,
		StageClear,
		MissionClear,
		Versionup
	}
}
