using System;
using System.Collections;
using UnityEngine;

public class SkillTestor : MonoBehaviour
{
	[SerializeField]
	private InvocationEffectParams _effect;

	[SerializeField]
	private CharacterParams _character;

	[SerializeField]
	private CameraParams _camera;

	private Camera cameraObject;

	private GameObject stageObject;

	private LightColorChanger lightColorChanger;

	private bool _isPlaying;

	private bool _isInitialized;

	private IEnumerator _playSkillCorutine;

	private IEnumerator effectPlay;

	public bool isPlaying
	{
		get
		{
			return this._isPlaying;
		}
	}

	public bool isInitialized
	{
		get
		{
			return this._isInitialized;
		}
	}

	private void Awake()
	{
		this.cameraObject = new GameObject("Camera").AddComponent<Camera>();
		Light light = new GameObject("Directioal light").AddComponent<Light>();
		this.lightColorChanger = light.gameObject.AddComponent<LightColorChanger>();
		this.cameraObject.backgroundColor = Color.black;
		this.stageObject = new GameObject("Stage");
		base.StartCoroutine(this.isLoading());
	}

	private IEnumerator isLoading()
	{
		IEnumerator load = this._effect.SkillInitialize(this.cameraObject, this.stageObject, this.lightColorChanger);
		while (load.MoveNext())
		{
			yield return null;
		}
		this._camera.currentTargetCamera = this.cameraObject;
		this._isInitialized = true;
		yield break;
	}

	private IEnumerator PlaySkillCorutine()
	{
		this._effect.transform.position = this._character.transform.position;
		this._camera.transform.position = this._character.transform.position;
		this.effectPlay = this._effect.PlaySkillAnimation(this._character, false);
		this._camera.PlayCameraAnimation(this._character, false, false);
		while (this.effectPlay != null && this.effectPlay.MoveNext())
		{
			yield return null;
		}
		this._camera.StopCameraAnimation();
		yield break;
	}

	public void PlayEffect()
	{
		if (!this._isInitialized)
		{
			return;
		}
		if (this._isPlaying)
		{
			this.StopEffect();
		}
		this._playSkillCorutine = this.PlaySkillCorutine();
		this._isPlaying = true;
		base.StartCoroutine(this._playSkillCorutine);
	}

	public void StopEffect()
	{
		if (!this._isInitialized)
		{
			return;
		}
		if (!this._isPlaying)
		{
			return;
		}
		if (this.effectPlay != null)
		{
			base.StopCoroutine(this.effectPlay);
			this.effectPlay = null;
		}
		if (this._camera.isPlaying)
		{
			this._camera.StopCameraAnimation();
		}
		if (this._playSkillCorutine != null)
		{
			base.StopCoroutine(this._playSkillCorutine);
			this._playSkillCorutine = null;
		}
		if (this._effect.isPlaying)
		{
			this._effect.StopSkillAnimation();
		}
		if (this._camera.isPlaying)
		{
			this._camera.StopCameraAnimation();
		}
		this._isPlaying = false;
	}
}
