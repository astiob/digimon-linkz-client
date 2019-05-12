using System;
using UniRx;
using UnityEngine;

public class UVSplitAnimationHelper : MonoBehaviour
{
	[SerializeField]
	private Material mat;

	[SerializeField]
	private float interval = 0.1f;

	[SerializeField]
	private Vector2 split = Vector2.zero;

	[SerializeField]
	private bool addU = true;

	[SerializeField]
	private bool addV = true;

	[SerializeField]
	private string shaderParamU;

	[SerializeField]
	private string shaderParamV;

	private Vector2 cellUV = Vector2.zero;

	private Vector2 nowUV = Vector2.zero;

	private SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();

	private void OnEnable()
	{
		this.disposable = new SingleAssignmentDisposable();
		this.disposable.Disposable = Observable.Interval(TimeSpan.FromSeconds((double)this.interval)).Subscribe(delegate(long _)
		{
			this.UpdateUV();
		}).AddTo(this);
		this.cellUV = new Vector2(1f / this.split.x, 1f / this.split.y);
	}

	private void OnDisable()
	{
		this.disposable.Dispose();
	}

	private void UpdateUV()
	{
		if (this.addU)
		{
			this.nowUV.x = this.nowUV.x + this.cellUV.x;
			if (this.nowUV.x >= 1f)
			{
				this.nowUV.x = 0f;
			}
		}
		else
		{
			this.nowUV.x = this.nowUV.x - this.cellUV.x;
			if (this.nowUV.x < 0f)
			{
				this.nowUV.x = 1f - this.cellUV.x;
			}
		}
		if (this.addV)
		{
			this.nowUV.y = this.nowUV.y + this.cellUV.y;
			if (this.nowUV.y >= 1f)
			{
				this.nowUV.y = 0f;
			}
		}
		else
		{
			this.nowUV.y = this.nowUV.y - this.cellUV.y;
			if (this.nowUV.y < 0f)
			{
				this.nowUV.y = 1f - this.cellUV.y;
			}
		}
		this.mat.SetFloat(this.shaderParamU, this.nowUV.x);
		this.mat.SetFloat(this.shaderParamV, this.nowUV.y);
	}
}
