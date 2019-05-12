using Master;
using System;
using System.Collections;
using UnityEngine;

public class CMD_ShortDownload : CMD
{
	[SerializeField]
	private UILabel downloadDescription;

	[SerializeField]
	private UILabel downloadProgressValue;

	[SerializeField]
	private UILabel networkDescription;

	[SerializeField]
	private UIProgressBar progressBar;

	private PartsGaugePoint partsGaugePoint;

	private Coroutine progressBarUpdatingCoroutine;

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.downloadDescription.text = StringMaster.GetString("AssetDownloadInfo");
		this.networkDescription.text = StringMaster.GetString("AssetDownloadCaution");
		this.partsGaugePoint = base.gameObject.GetComponentInChildren<PartsGaugePoint>();
		this.progressBarUpdatingCoroutine = base.StartCoroutine(this.ProgressBarUpdating());
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void WindowOpened()
	{
		base.StopCoroutine(this.progressBarUpdatingCoroutine);
		base.WindowOpened();
	}

	private IEnumerator ProgressBarUpdating()
	{
		for (;;)
		{
			this.progressBar.ForceUpdate();
			yield return null;
		}
		yield break;
	}

	public IEnumerator WaitAssetBundleDownload()
	{
		AssetDataMng manager = AssetDataMng.Instance();
		float max = (float)manager.RealABDL_TotalCount_LV();
		this.progressBar.gameObject.SetActive(true);
		while (manager.IsAssetBundleDownloading())
		{
			this.SetProgressBar((float)manager.RealABDL_NowCount_LV(), max);
			yield return new WaitForSeconds(0.25f);
		}
		this.SetProgressBar(1f, 1f);
		yield return new WaitForSeconds(0.25f);
		yield break;
	}

	public IEnumerator WaitMasterDataDownload()
	{
		MasterDataMng manager = MasterDataMng.Instance();
		int downloadNum = manager.GetMasterDataDownloadNum();
		this.progressBar.gameObject.SetActive(true);
		while (!manager.IsFinishedGetMasterData())
		{
			this.SetProgressBar((float)manager.GetMasterDataProgress(), (float)downloadNum);
			yield return null;
		}
		this.SetProgressBar(1f, 1f);
		yield return new WaitForSeconds(0.25f);
		yield break;
	}

	private void SetProgressBar(float now, float max)
	{
		this.progressBar.value = now / max;
		if (null != this.partsGaugePoint)
		{
			this.partsGaugePoint.SetGaugeValue(this.progressBar.value);
		}
		this.downloadProgressValue.text = this.progressBar.value.ToString("P0");
	}
}
