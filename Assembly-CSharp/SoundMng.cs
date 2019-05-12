using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioListener))]
public class SoundMng : MonoBehaviour
{
	private static SoundMng instance;

	public bool isInitialized;

	private string previousBgmPath = string.Empty;

	[SerializeField]
	private AudioMixerGroup mixer;

	public GameObject goSE;

	public List<GameObject> goAudioBGM_List;

	private List<AudioSource> audioBGM_List;

	private float bgmValue;

	private int curBgmNum;

	private int SE_MAX = 9;

	private int TS_SE_MAX = 5;

	private List<SoundCtr> audioSEList;

	private float previousTimeScale;

	private float volumeSE = 7f;

	private float volumeBGM = 7f;

	private Dictionary<string, AudioClip> audioCacheTable = new Dictionary<string, AudioClip>();

	private Action<int> endFadeInBGM;

	private Action<int> endFadeOutBGM;

	public static SoundMng Instance()
	{
		return SoundMng.instance;
	}

	protected virtual void Awake()
	{
		if (this.isInitialized)
		{
			return;
		}
		SoundMng.instance = this;
		this.previousTimeScale = Time.timeScale;
		this.audioSEList = new List<SoundCtr>();
		if (this.mixer == null)
		{
			AudioMixer audioMixer = Resources.Load<AudioMixer>("AudioMixer/default");
			this.mixer = audioMixer.FindMatchingGroups("Master")[0];
		}
		if (this.goSE == null)
		{
			this.goSE = new GameObject();
			this.goSE.AddComponent<AudioSource>();
			this.goSE.AddComponent<SoundCtr>();
		}
		for (int i = 0; i < this.SE_MAX + this.TS_SE_MAX; i++)
		{
			GameObject gameObject = (i <= 0) ? this.goSE : UnityEngine.Object.Instantiate<GameObject>(this.goSE);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector2.one;
			AudioSource component = gameObject.GetComponent<AudioSource>();
			SoundCtr component2 = gameObject.GetComponent<SoundCtr>();
			if (i >= this.SE_MAX)
			{
				gameObject.name = "R_TSSE" + i.ToString();
				component.outputAudioMixerGroup = this.mixer;
			}
			else
			{
				gameObject.name = "R_SE" + i.ToString();
			}
			component2.seName = string.Empty;
			component2.go = gameObject;
			component2.ads = component;
			component2.value = 0f;
			component2.autoDel = false;
			component2.time = 0f;
			component2.act = null;
			component.volume = 0f;
			this.audioSEList.Add(component2);
		}
		this.bgmValue = 0f;
		this.audioBGM_List = new List<AudioSource>();
		if (this.goAudioBGM_List == null || this.goAudioBGM_List.Count == 0)
		{
			this.goAudioBGM_List = new List<GameObject>();
			for (int i = 0; i < 2; i++)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = "R_BGM" + i.ToString();
				gameObject2.transform.parent = base.transform;
				gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject2.AddComponent<AudioSource>();
				this.goAudioBGM_List.Add(gameObject2);
				AudioSource component = this.goAudioBGM_List[i].GetComponent<AudioSource>();
				component.volume = 0f;
				this.audioBGM_List.Add(component);
				this.goAudioBGM_List[i].SetActive(false);
			}
		}
		else
		{
			for (int i = 0; i < this.goAudioBGM_List.Count; i++)
			{
				AudioSource component = this.goAudioBGM_List[i].GetComponent<AudioSource>();
				component.outputAudioMixerGroup = this.mixer;
				component.volume = 0f;
				this.audioBGM_List.Add(component);
				this.goAudioBGM_List[i].SetActive(false);
			}
		}
		this.ReleaseAudio();
		if (AssetDataMng.Instance() == null)
		{
			AssetDataMng assetDataMng = base.gameObject.AddComponent<AssetDataMng>();
			assetDataMng.Initialize();
		}
		OptionSetting.LoadSoundVolume();
		this.isInitialized = true;
	}

	protected virtual void Update()
	{
		this.RePichShifting();
	}

	protected virtual void OnDestroy()
	{
		SoundMng.instance = null;
	}

	private void OnApplicationPause(bool PauseStatus)
	{
		if (PauseStatus)
		{
			foreach (AudioSource audioSource in this.audioBGM_List)
			{
				if (audioSource.gameObject.activeSelf)
				{
					audioSource.Stop();
				}
			}
		}
		else
		{
			foreach (AudioSource audioSource2 in this.audioBGM_List)
			{
				if (audioSource2.gameObject.activeSelf)
				{
					audioSource2.Play();
				}
			}
		}
	}

	public float VolumeSE
	{
		get
		{
			return this.volumeSE;
		}
		set
		{
			this.volumeSE = value;
			for (int i = 0; i < this.audioSEList.Count; i++)
			{
				this.audioSEList[i].ads.volume = this.audioSEList[i].value * this.volumeSE * 0.1f;
			}
		}
	}

	public float VolumeBGM
	{
		get
		{
			return this.volumeBGM;
		}
		set
		{
			this.volumeBGM = value;
			this.audioBGM_List[this.curBgmNum].volume = this.bgmValue * this.volumeBGM * 0.1f;
		}
	}

	public void Initialize()
	{
		this.Awake();
	}

	public void PreLoadAudio(string[] pathList)
	{
		this.PreLoadAudio(pathList, pathList);
	}

	public void PreLoadAudio(string[] pathList, string[] keyList)
	{
		for (int i = 0; i < keyList.Length; i++)
		{
			if (!keyList[i].Equals(string.Empty))
			{
				if (!this.audioCacheTable.ContainsKey(keyList[i]))
				{
					AudioClip audioClip = AssetDataMng.Instance().LoadObject(pathList[i], null, true) as AudioClip;
					if (audioClip == null)
					{
						global::Debug.LogError("======================SoundMng::Audio ファイル " + pathList[i] + " 見つかりません");
					}
					this.audioCacheTable.Add(keyList[i], audioClip);
				}
			}
		}
	}

	public void ReleaseAndPreLoadAudio(string[] pathList, string[] keyList)
	{
		this.ReleaseAudio();
		this.PreLoadAudio(pathList, keyList);
	}

	public void ReleaseAndPreLoadAudio(string[] pathList)
	{
		this.ReleaseAndPreLoadAudio(pathList, pathList);
	}

	public void ReleaseAudio()
	{
		this.audioCacheTable.Clear();
	}

	private AudioClip LoadAudio(string path)
	{
		AudioClip audioClip;
		if (this.audioCacheTable.ContainsKey(path))
		{
			audioClip = this.audioCacheTable[path];
		}
		else
		{
			if (AssetDataMng.Instance() == null)
			{
				return null;
			}
			audioClip = (AssetDataMng.Instance().LoadObject(path, null, true) as AudioClip);
			if (audioClip == null)
			{
				global::Debug.LogError("======================SoundMng::Audio ファイル " + path + " 見つかりません");
			}
			this.audioCacheTable.Add(path, audioClip);
		}
		return audioClip;
	}

	public void TryPlaySE(string path, float time = 0f, bool loop = false, bool isAutoDel = true, Action<int> act = null, int selfVolume = -1)
	{
		if (path == string.Empty)
		{
			return;
		}
		int i;
		for (i = 0; i < this.audioSEList.Count; i++)
		{
			if (this.audioSEList[i].seName == path)
			{
				break;
			}
		}
		if (i < this.audioSEList.Count)
		{
			this.TryStopSE(path, 0f, null);
		}
		this.PlaySE(path, time, loop, isAutoDel, act, selfVolume, 1f);
	}

	public void PlaySE(string path, float time = 0f, bool loop = false, bool isAutoDel = true, Action<int> act = null, int selfVolume = -1, float pitch = 1f)
	{
		AudioClip clip = this.LoadAudio(path);
		int i;
		for (i = 0; i < this.audioSEList.Count - this.TS_SE_MAX; i++)
		{
			if (this.audioSEList[i].seName == string.Empty)
			{
				break;
			}
		}
		if (i >= this.audioSEList.Count - this.TS_SE_MAX)
		{
			global::Debug.LogWarning("<color=red>======================SoundMng::PlaySE ファイル </color>" + path + "<color=red> SE OVER</color>");
			return;
		}
		SoundCtr soundCtr = this.audioSEList[i];
		soundCtr.value = 0f;
		soundCtr.seName = path;
		soundCtr.autoDel = isAutoDel;
		soundCtr.time = time;
		soundCtr.act = act;
		soundCtr.ads.clip = clip;
		soundCtr.ads.loop = loop;
		soundCtr.ads.pitch = pitch;
		soundCtr.selfVolume = selfVolume;
		soundCtr.Play(true);
	}

	public void PlayTimeScaleSE(string path, float time = 0f, bool loop = false, bool isAutoDel = true, Action<int> act = null, int selfVolume = -1)
	{
		AudioClip clip = this.LoadAudio(path);
		int i;
		for (i = this.SE_MAX; i < this.audioSEList.Count; i++)
		{
			if (this.audioSEList[i].seName == string.Empty)
			{
				break;
			}
		}
		if (i < this.SE_MAX || i >= this.audioSEList.Count)
		{
			global::Debug.LogWarning("<color=red>======================SoundMng::PlaySE ファイル </color>" + path + "<color=red> SE OVER</color>");
			return;
		}
		SoundCtr soundCtr = this.audioSEList[i];
		soundCtr.value = 0f;
		soundCtr.seName = path;
		soundCtr.autoDel = isAutoDel;
		soundCtr.time = time;
		soundCtr.act = act;
		soundCtr.ads.clip = clip;
		soundCtr.ads.loop = loop;
		soundCtr.ads.pitch = 1f;
		soundCtr.selfVolume = selfVolume;
		soundCtr.Play(true);
	}

	public void TryStopSE(string path, float time = 0f, Action<int> act = null)
	{
		int i;
		for (i = 0; i < this.audioSEList.Count; i++)
		{
			if (this.audioSEList[i].seName == path)
			{
				break;
			}
		}
		if (i != this.audioSEList.Count)
		{
			this.StopSE(path, time, act);
		}
	}

	public void StopSE(string path, float time = 0f, Action<int> act = null)
	{
		int i;
		for (i = 0; i < this.audioSEList.Count; i++)
		{
			if (this.audioSEList[i].seName == path)
			{
				break;
			}
		}
		if (i == this.audioSEList.Count)
		{
			global::Debug.Log("======================SoundMng::PlaySE 番号 " + path + " NOT PLAYING SE");
			return;
		}
		SoundCtr soundCtr = this.audioSEList[i];
		soundCtr.time = time;
		soundCtr.act = act;
		soundCtr.Play(false);
	}

	public void StopAllSE(float time = 0f)
	{
		for (int i = 0; i < this.audioSEList.Count; i++)
		{
			if (this.audioSEList[i].seName != string.Empty)
			{
				SoundCtr soundCtr = this.audioSEList[i];
				soundCtr.time = time;
				soundCtr.act = null;
				soundCtr.Play(false);
			}
		}
	}

	public bool IsPlayingSE(string path)
	{
		if (path != string.Empty)
		{
			for (int i = 0; i < this.audioSEList.Count; i++)
			{
				if (this.audioSEList[i].seName == path)
				{
					return this.audioSEList[i].ads.isPlaying;
				}
			}
		}
		return false;
	}

	public int PlaySE_Ex(string path)
	{
		int num = -1;
		for (int i = 0; i < this.audioSEList.Count; i++)
		{
			if (!this.audioSEList[i].ads.isPlaying)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			global::Debug.LogError("CAN NOT PLAY SE");
		}
		else
		{
			AudioClip clip = this.LoadAudio(path);
			SoundCtr soundCtr = this.audioSEList[num];
			soundCtr.value = 0f;
			soundCtr.seName = path;
			soundCtr.autoDel = true;
			soundCtr.time = 0f;
			soundCtr.act = null;
			soundCtr.ads.clip = clip;
			soundCtr.ads.loop = false;
			soundCtr.ads.pitch = 1f;
			soundCtr.selfVolume = -1;
			soundCtr.Play(true);
		}
		return num;
	}

	public void StopSE_Ex(int handle)
	{
		if (this.audioSEList.Count <= handle)
		{
			global::Debug.LogErrorFormat("INVALID HANDLE : {0}", new object[]
			{
				handle
			});
			return;
		}
		if (this.audioSEList[handle].ads.isPlaying)
		{
			SoundCtr soundCtr = this.audioSEList[handle];
			soundCtr.time = 0f;
			soundCtr.act = null;
			soundCtr.Stop();
		}
	}

	public bool IsPlayingSE_Ex(int handle)
	{
		bool result = false;
		if (handle < this.audioSEList.Count)
		{
			result = this.audioSEList[handle].ads.isPlaying;
		}
		return result;
	}

	public string GetNowBGMPath()
	{
		return this.previousBgmPath;
	}

	public void PlayGameBGM(string fileName)
	{
		float time = 0.3f;
		this.PlayBGM(string.Format("BGM/{0}/sound", fileName), time, null);
	}

	public void PlayBGM(string path, float time = 0f, Action<int> act = null)
	{
		if (this.IsMatchPreviousPlayedBGM(path))
		{
			return;
		}
		if (this.goAudioBGM_List[this.curBgmNum].activeSelf)
		{
			this.curBgmNum = 1 - this.curBgmNum;
		}
		this.goAudioBGM_List[this.curBgmNum].SetActive(true);
		AudioClip clip = this.LoadAudio(path);
		this.previousBgmPath = path;
		this.audioBGM_List[this.curBgmNum].clip = clip;
		this.audioBGM_List[this.curBgmNum].loop = true;
		this.endFadeInBGM = act;
		if (time == 0f)
		{
			this.bgmValue = 1f;
			if (this.IsPlayingStandardPlayer())
			{
				this.bgmValue = 0f;
			}
			this.audioBGM_List[this.curBgmNum].volume = this.bgmValue * this.volumeBGM * 0.1f;
			if (act != null)
			{
				act(0);
			}
		}
		else
		{
			Hashtable hashtable = new Hashtable();
			this.bgmValue = 0f;
			this.audioBGM_List[this.curBgmNum].volume = 0f;
			hashtable.Add("from", 0f);
			hashtable.Add("to", 1f);
			hashtable.Add("time", time);
			hashtable.Add("onupdate", "UpdateBGM");
			hashtable.Add("easetype", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "EndFadeInBGM");
			hashtable.Add("oncompleteparams", 0);
			iTween.ValueTo(base.gameObject, hashtable);
		}
		this.audioBGM_List[this.curBgmNum].Play();
	}

	private void UpdateBGM(float v)
	{
		this.bgmValue = v;
		this.audioBGM_List[this.curBgmNum].volume = this.bgmValue * this.volumeBGM * 0.1f;
		int index = 1 - this.curBgmNum;
		if (this.goAudioBGM_List[index].activeSelf)
		{
			this.audioBGM_List[index].volume = (1f - this.bgmValue) * this.volumeBGM * 0.1f;
			if (this.bgmValue >= 1f)
			{
				this.audioBGM_List[index].volume = 0f;
				this.audioBGM_List[index].Stop();
				this.audioBGM_List[index].clip = null;
				this.goAudioBGM_List[index].SetActive(false);
			}
		}
	}

	private void EndFadeInBGM()
	{
		if (this.endFadeInBGM != null)
		{
			this.endFadeInBGM(0);
		}
	}

	public void StopBGM(float time = 0f, Action<int> act = null)
	{
		this.endFadeOutBGM = act;
		if (time == 0f)
		{
			this.bgmValue = 0f;
			this.audioBGM_List[this.curBgmNum].volume = 0f;
			this.audioBGM_List[this.curBgmNum].Stop();
			this.audioBGM_List[this.curBgmNum].clip = null;
			this.previousBgmPath = string.Empty;
			if (act != null)
			{
				act(0);
			}
		}
		else
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("from", this.bgmValue);
			this.audioBGM_List[this.curBgmNum].volume = this.bgmValue * this.volumeBGM * 0.1f;
			hashtable.Add("to", 0f);
			hashtable.Add("time", time);
			hashtable.Add("onupdate", "UpdateBGM");
			hashtable.Add("easetype", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "EndFadeOutBGM");
			hashtable.Add("oncompleteparams", 0);
			iTween.ValueTo(base.gameObject, hashtable);
		}
	}

	public void CertStopBGM()
	{
		this.bgmValue = 0f;
		foreach (AudioSource audioSource in this.audioBGM_List)
		{
			audioSource.volume = 0f;
			audioSource.Stop();
			audioSource.clip = null;
		}
	}

	private void EndFadeOutBGM()
	{
		this.bgmValue = 0f;
		this.audioBGM_List[this.curBgmNum].volume = 0f;
		this.audioBGM_List[this.curBgmNum].Stop();
		this.audioBGM_List[this.curBgmNum].clip = null;
		this.previousBgmPath = string.Empty;
		this.goAudioBGM_List[this.curBgmNum].SetActive(false);
		if (this.endFadeOutBGM != null)
		{
			this.endFadeOutBGM(0);
		}
	}

	public bool IsPlayingBGM
	{
		get
		{
			return this.audioBGM_List[this.curBgmNum].isPlaying;
		}
	}

	public bool IsMatchPreviousPlayedBGM(string path)
	{
		return this.previousBgmPath == path;
	}

	private void RePichShifting()
	{
		if (this.previousTimeScale != Time.timeScale)
		{
			float timeScale = Time.timeScale;
			this.mixer.audioMixer.SetFloat("MainPitch", 1f * timeScale);
			this.mixer.audioMixer.SetFloat("ShiftPitch", 1f / timeScale);
			this.previousTimeScale = Time.timeScale;
		}
	}

	private bool IsPlayingStandardPlayer()
	{
		return false;
	}
}
