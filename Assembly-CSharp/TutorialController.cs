using System;
using System.Collections;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
	public bool endFlg;

	[SerializeField]
	[Header("カメラ")]
	private GameObject obj_cam2;

	private int skipLock;

	private Action endCallBack;

	public Action EndCallBack
	{
		set
		{
			this.endCallBack = value;
		}
	}

	private void Awake()
	{
		base.name = "Cutscene";
	}

	private void Start()
	{
		this.obj_cam2.transform.position = new Vector3(0f, 2f, 0f);
	}

	private IEnumerator Fade()
	{
		this.obj_cam2.SendMessage("fadeOut");
		yield return new WaitForSeconds(3f);
		this.skipLock = 1;
		Time.timeScale = 1f;
		if (this.endCallBack != null)
		{
			this.endCallBack();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	private void Update()
	{
		if (this.endFlg && this.skipLock == 0)
		{
			this.skipLock = 1;
			base.StartCoroutine("Fade");
		}
	}
}
