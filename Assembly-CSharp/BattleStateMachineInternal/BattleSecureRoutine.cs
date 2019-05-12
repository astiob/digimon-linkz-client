using System;
using System.Collections;
using UnityEngine;

namespace BattleStateMachineInternal
{
	public class BattleSecureRoutine : MonoBehaviour
	{
		private static bool isPlayed;

		private AudioSource audioS;

		private int tapCount;

		private float timeCount;

		private Camera uiCamera;

		private IEnumerator mainRoutine;

		private GameObject skillSelectUI;

		private GameObject alwaysUI;

		private UIRoot[] rootList;

		[SerializeField]
		private BattleSecureRoutine.RoutineState state;

		private BattleStateManager manager;

		private void Awake()
		{
			this.state = BattleSecureRoutine.RoutineState.Awake;
			if (BattleSecureRoutine.isPlayed)
			{
				this.MyDestroy();
				return;
			}
			this.rootList = UIRoot.list.ToArray();
			foreach (UIRoot uiroot in this.rootList)
			{
				if (!(uiroot == null))
				{
					Transform findObject = this.GetFindObject("SkillSelect", uiroot.transform);
					Transform findObject2 = this.GetFindObject("Always", uiroot.transform);
					if (findObject != null && findObject2 != null)
					{
						this.skillSelectUI = findObject.gameObject;
						this.alwaysUI = findObject2.gameObject;
						break;
					}
				}
			}
		}

		private void Start()
		{
			this.manager = BattleStateManager.current;
			this.state = BattleSecureRoutine.RoutineState.Start;
			this.uiCamera = UnityEngine.Object.FindObjectOfType<UICamera>().cachedCamera;
			if (this.skillSelectUI == null || this.alwaysUI == null)
			{
				this.MyDestroy();
				return;
			}
			this.mainRoutine = this.MainRoutine();
			base.StartCoroutine(this.mainRoutine);
		}

		private IEnumerator MainRoutine()
		{
			if (this.manager.battleMode != BattleMode.Single)
			{
				this.MyDestroy();
				yield break;
			}
			AudioClip clip = Resources.Load<AudioClip>("SND");
			if (clip == null)
			{
				this.MyDestroy();
				yield break;
			}
			Transform helpUI = this.GetFindObject("DialogHelp", this.alwaysUI.transform);
			Transform retrieUI = this.GetFindObject("DialogRetire", this.alwaysUI.transform);
			Transform menuUI = this.GetFindObject("DialogMENU", this.alwaysUI.transform);
			if (helpUI == null || retrieUI == null || menuUI == null)
			{
				this.MyDestroy();
				yield break;
			}
			if (this.manager == null)
			{
				this.MyDestroy();
				yield break;
			}
			LightColorChanger lightColorChanger = this.manager.hierarchyData.cameraObject.sunLightColorChanger;
			Light light = this.manager.hierarchyData.cameraObject.sunLight;
			if (lightColorChanger == null || light == null)
			{
				this.MyDestroy();
				yield break;
			}
			this.audioS = base.gameObject.AddComponent<AudioSource>();
			this.audioS.loop = false;
			this.audioS.spatialBlend = 0f;
			this.audioS.playOnAwake = false;
			this.audioS.clip = clip;
			bool next = false;
			this.state = BattleSecureRoutine.RoutineState.WaitSkillSelectActive;
			while (this.manager.battleScreen != BattleScreen.SkillSelects)
			{
				yield return null;
			}
			GameObject hudUI = null;
			foreach (UIRoot uiroot in this.rootList)
			{
				if (!(uiroot == null))
				{
					Transform findObject = this.GetFindObject("HUD", uiroot.transform);
					if (!(findObject == null))
					{
						hudUI = findObject.gameObject;
						break;
					}
				}
			}
			if (hudUI == null)
			{
				this.MyDestroy();
				yield break;
			}
			this.state = BattleSecureRoutine.RoutineState.WaitMenuUIActive;
			while (menuUI != null && !menuUI.gameObject.activeSelf)
			{
				yield return null;
			}
			if (!(menuUI != null) || !(menuUI.gameObject != null))
			{
				this.MyDestroy();
				yield break;
			}
			bool isThreeSecondWait = false;
			this.state = BattleSecureRoutine.RoutineState.LongTapWait;
			CameraParams cameraParams;
			CharacterParams[] characters;
			while (!(helpUI == null) && !(retrieUI == null) && !(menuUI == null))
			{
				if (this.manager.hierarchyData.on2xSpeedPlay || this.manager.hierarchyData.onAutoPlay != 0)
				{
					this.MyDestroy();
					yield break;
				}
				if (!isThreeSecondWait && Input.GetMouseButton(0))
				{
					Vector3 vector = this.uiCamera.ScreenToViewportPoint(Input.mousePosition);
					if (vector.x <= 0.9f || vector.y <= 0.9f)
					{
						this.MyDestroy();
						yield break;
					}
					this.timeCount += Time.unscaledDeltaTime;
					if (this.timeCount > 3f)
					{
						isThreeSecondWait = true;
					}
				}
				if (isThreeSecondWait && Input.GetMouseButtonDown(0))
				{
					this.state = BattleSecureRoutine.RoutineState.LongTapWait;
					Vector3 vector2 = this.uiCamera.ScreenToViewportPoint(Input.mousePosition);
					if (vector2.x >= 0.1f || vector2.y >= 0.1f)
					{
						this.MyDestroy();
						yield break;
					}
					this.tapCount++;
					if (Time.timeScale == 0f && this.tapCount > 21)
					{
						next = true;
					}
				}
				if (helpUI.gameObject.activeSelf || retrieUI.gameObject.activeSelf || !menuUI.gameObject.activeSelf)
				{
					this.MyDestroy();
					yield break;
				}
				if (next)
				{
					characters = UnityEngine.Object.FindObjectsOfType<CharacterParams>();
					cameraParams = CameraParams.current;
					if (cameraParams == null)
					{
						this.MyDestroy();
						yield break;
					}
					Camera camera3D = this.manager.hierarchyData.cameraObject.camera3D;
					Action PlayAnimation = delegate()
					{
						cameraParams.PlayCameraShake();
						foreach (CharacterParams characterParams2 in characters)
						{
							if (characterParams2.gameObject.activeInHierarchy)
							{
								characterParams2.PlayAnimation(CharacterAnimationType.attacks, SkillType.Attack, 0, null, null);
							}
						}
					};
					Action ChangeLightColor = delegate()
					{
						light.color = new Color(UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f), UnityEngine.Random.Range(0.5f, 1f), 1f);
					};
					this.skillSelectUI.SetActive(false);
					this.alwaysUI.SetActive(false);
					hudUI.SetActive(false);
					lightColorChanger.enabled = false;
					float time = Time.timeScale;
					Time.timeScale = 1f;
					float volume = SoundMng.Instance().VolumeBGM;
					SoundMng.Instance().VolumeBGM = 0f;
					this.audioS.volume = volume * 0.01f * 2f;
					this.audioS.Play();
					Vector3 pos = camera3D.transform.position;
					Quaternion rot = camera3D.transform.rotation;
					float fov = camera3D.fieldOfView;
					cameraParams.PlayCameraAnimation(Vector3.zero, Vector3.zero, false, true);
					this.state = BattleSecureRoutine.RoutineState.PlayAnimation;
					float bpmWait = 0.46f;
					while (this.audioS.isPlaying)
					{
						ChangeLightColor();
						PlayAnimation();
						yield return new WaitForSeconds(bpmWait * 0.5f);
						ChangeLightColor();
						yield return new WaitForSeconds(bpmWait * 0.5f);
					}
					foreach (CharacterParams characterParams in characters)
					{
						if (characterParams.gameObject.activeInHierarchy)
						{
							characterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
						}
					}
					yield return new WaitForSeconds(1f);
					this.audioS.Stop();
					SoundMng.Instance().VolumeBGM = volume;
					this.skillSelectUI.SetActive(true);
					this.alwaysUI.SetActive(true);
					hudUI.SetActive(true);
					Time.timeScale = time;
					lightColorChanger.enabled = true;
					cameraParams.StopCameraAnimation();
					camera3D.transform.position = pos;
					camera3D.transform.rotation = rot;
					camera3D.fieldOfView = fov;
					cameraParams.StopCameraShake();
					this.MyDestroy();
					yield break;
				}
				else
				{
					yield return null;
				}
			}
			this.MyDestroy();
			yield break;
		}

		private void MyDestroy()
		{
			BattleSecureRoutine.isPlayed = false;
			if (this.mainRoutine != null)
			{
				base.StopCoroutine(this.mainRoutine);
			}
			UnityEngine.Object.Destroy(this.audioS);
			UnityEngine.Object.Destroy(this);
		}

		private Transform GetFindObject(string targetName, Transform target)
		{
			if (target.name.Equals(targetName))
			{
				return target;
			}
			for (int i = 0; i < target.childCount; i++)
			{
				Transform findObject = this.GetFindObject(targetName, target.GetChild(i));
				if (findObject != null)
				{
					return findObject;
				}
			}
			return null;
		}

		public void DestroyThisObject()
		{
			this.MyDestroy();
		}

		private enum RoutineState
		{
			Awake,
			Start,
			WaitSkillSelectActive,
			WaitMenuUIActive,
			LongTapWait,
			CountTapWait,
			PlayAnimation
		}
	}
}
