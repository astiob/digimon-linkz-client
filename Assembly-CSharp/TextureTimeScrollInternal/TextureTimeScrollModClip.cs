using System;
using UnityEngine;

namespace TextureTimeScrollInternal
{
	[Serializable]
	public class TextureTimeScrollModClip
	{
		private const string timeScroll = "TimeScroll";

		private const string timeScrollSin = "TimeScrollSin";

		private const string timeScrollInfinity = "TimeScrollInf";

		private const string timeScrollMod = "TimeScrollMod";

		private const string timeScrollModValue = "ModValue";

		[SerializeField]
		private string _sourcePropertyName;

		[SerializeField]
		private TimeScaleMode _timeScaleMode = TimeScaleMode.Default;

		[SerializeField]
		private float _speed = 1f;

		[SerializeField]
		private TilingMode _tilingMode;

		[SerializeField]
		private float _textureTiling = 1f;

		public TextureTimeScrollModClip()
		{
			this._sourcePropertyName = string.Empty;
			this._speed = 1f;
			this._timeScaleMode = TimeScaleMode.Default;
		}

		public TextureTimeScrollModClip(string sourcePropertyName, FModMode fModMode)
		{
			this._sourcePropertyName = sourcePropertyName;
			this._speed = 1f;
			this._timeScaleMode = TimeScaleMode.Default;
			this._textureTiling = 1f;
		}

		public TextureTimeScrollModClip(string sourcePropertyName, FModMode fModMode, TimeScaleMode timeScaleMode)
		{
			this._sourcePropertyName = sourcePropertyName;
			this._speed = 1f;
			this._timeScaleMode = timeScaleMode;
			this._textureTiling = 1f;
		}

		public TextureTimeScrollModClip(string sourcePropertyName, TimeScaleMode timeScaleMode, TilingMode tilingMode)
		{
			this._sourcePropertyName = sourcePropertyName;
			this._speed = 1f;
			this._timeScaleMode = timeScaleMode;
			this._textureTiling = 1f;
			this._tilingMode = tilingMode;
		}

		public TextureTimeScrollModClip(string sourcePropertyName, TimeScaleMode timeScaleMode, TilingMode tilingMode, float textureTiling)
		{
			this._sourcePropertyName = sourcePropertyName;
			this._speed = 1f;
			this._timeScaleMode = timeScaleMode;
			this._textureTiling = textureTiling;
			this._tilingMode = tilingMode;
		}

		public TextureTimeScrollModClip(string sourcePropertyName, FModMode fModMode, TimeScaleMode timeScaleMode, float speed, TilingMode tilingMode, float textureTiling)
		{
			this._sourcePropertyName = sourcePropertyName;
			this._speed = speed;
			this._timeScaleMode = timeScaleMode;
			this._textureTiling = Mathf.Clamp(textureTiling, 0f, float.PositiveInfinity);
			this._tilingMode = tilingMode;
		}

		public void SetFModedValue(Material instancedMaterial)
		{
			if (string.IsNullOrEmpty(this._sourcePropertyName))
			{
				global::Debug.LogWarning("SourcePropertyNameの値が空です.");
				return;
			}
			if (this._sourcePropertyName[0] != '_')
			{
				this._sourcePropertyName = "_" + this._sourcePropertyName;
			}
			if (!instancedMaterial.HasProperty(this._sourcePropertyName))
			{
				return;
			}
			this.UpdateSpeed(instancedMaterial);
			this.UpdateTiling(instancedMaterial);
			float num = TextureTimeScrollRealTime.time * this._speed * this.GetModTimeScale();
			instancedMaterial.SetFloat(this._sourcePropertyName + "TimeScroll", num % this.GetModValue(FModMode.TiledTexture));
			instancedMaterial.SetFloat(this._sourcePropertyName + "TimeScrollSin", Mathf.Sin(num) % this.GetModValue(FModMode.Sin));
			instancedMaterial.SetFloat(this._sourcePropertyName + "TimeScrollInf", num % this.GetModValue(FModMode.Infinity));
			instancedMaterial.SetFloat(this._sourcePropertyName + "TimeScrollMod", num % this.GetModValue(instancedMaterial, this._sourcePropertyName + "ModValue"));
		}

		private float GetModValue(Material instancedMaterial, string key)
		{
			return (!instancedMaterial.HasProperty(key)) ? float.PositiveInfinity : instancedMaterial.GetFloat(key);
		}

		private float GetModValue(FModMode fModMode)
		{
			if (fModMode == FModMode.Sin)
			{
				return 360f;
			}
			if (fModMode != FModMode.Infinity)
			{
				return (this._textureTiling <= 0f) ? 0f : (1f / this._textureTiling);
			}
			return float.PositiveInfinity;
		}

		private float GetModTimeScale()
		{
			TimeScaleMode timeScaleMode = this._timeScaleMode;
			if (timeScaleMode != TimeScaleMode.Default)
			{
				return 0.05f;
			}
			return 1f;
		}

		public void UpdateSpeed(Material instancedMaterial)
		{
			this._speed = instancedMaterial.GetFloat(this._sourcePropertyName);
		}

		public void UpdateSpeed(MaterialController materialController)
		{
			if (materialController.ThisPropertyIsUAnimSpeed(this._sourcePropertyName))
			{
				this._speed = materialController.uvScroll.x;
			}
			if (materialController.ThisPropertyIsVAnimSpeed(this._sourcePropertyName))
			{
				this._speed = materialController.uvScroll.y;
			}
			if (materialController.useTilingOverride)
			{
				TilingMode tilingMode = this._tilingMode;
				if (tilingMode != TilingMode.V)
				{
					this._textureTiling = materialController.uvTiling.x;
				}
				else
				{
					this._textureTiling = materialController.uvTiling.y;
				}
			}
		}

		public void UpdateTiling(Material instancedMaterial)
		{
			TilingMode tilingMode = this._tilingMode;
			if (tilingMode != TilingMode.V)
			{
				this._textureTiling = instancedMaterial.GetTextureScale("_MainTex").x;
			}
			else
			{
				this._textureTiling = instancedMaterial.GetTextureScale("_MainTex").y;
			}
		}
	}
}
