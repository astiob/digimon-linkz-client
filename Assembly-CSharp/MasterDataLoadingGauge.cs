using Master;
using System;
using System.Collections;
using UnityEngine;

public class MasterDataLoadingGauge : MonoBehaviour
{
	[SerializeField]
	private UILabel loadDescription;

	[SerializeField]
	private UILabel downloadProgressValue;

	[SerializeField]
	private UIProgressBar progressBar;

	private PartsGaugePoint partsGaugePoint;

	[SerializeField]
	private int gaugePointDepth;

	private void Awake()
	{
		this.loadDescription.text = StringMaster.GetString("SystemLoadingFarm");
		this.partsGaugePoint = base.gameObject.GetComponentInChildren<PartsGaugePoint>();
		this.partsGaugePoint.SetDepthDigimonIcons(this.gaugePointDepth);
	}

	public void SetLoadProgress(int now, int max)
	{
		this.SetProgressBar((float)now, (float)max);
	}

	public IEnumerator WaitMasterDataLoad()
	{
		this.progressBar.gameObject.SetActive(true);
		while (this.progressBar.value < 1f)
		{
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
