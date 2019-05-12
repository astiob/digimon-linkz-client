using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleStateMachineInternal.HelpImage
{
	public class BattleHelpImages : ScriptableObject
	{
		private static BattleHelpImages _instance;

		[Header("ヘルプの画像を順に指定. 順番が若いほうが左に来る.")]
		[NonSerialized]
		private Texture2D[] _images = new Texture2D[0];

		[SerializeField]
		private string[] _imageNames = new string[]
		{
			string.Empty
		};

		public static Texture2D[] images
		{
			get
			{
				if (BattleHelpImages._instance != null)
				{
					return BattleHelpImages._instance._images;
				}
				global::Debug.LogError("BattleHelpImages.Initialize(BattleHelpImageType type); がコールされていません.");
				return new Texture2D[0];
			}
		}

		public static string[] imageNames
		{
			get
			{
				if (BattleHelpImages._instance != null)
				{
					return BattleHelpImages._instance._imageNames;
				}
				global::Debug.LogError("BattleHelpImages.Initialize(BattleHelpImageType type); がコールされていません.");
				return new string[0];
			}
		}

		public static void Initialize(BattleHelpImageType type)
		{
			if (BattleHelpImages._instance != null)
			{
				return;
			}
			BattleHelpImages._instance = Resources.Load<BattleHelpImages>(BattleHelpImages.GetObjectPath(type));
			BattleHelpImages._instance.LoadTexturesFromAssetBundle();
		}

		public static void UnloadInstance()
		{
			BattleHelpImages._instance = null;
			Resources.UnloadUnusedAssets();
		}

		private static string GetObjectPath(BattleHelpImageType type)
		{
			if (type == BattleHelpImageType.Multi)
			{
				return "BattleHelpImagesMulti";
			}
			if (type != BattleHelpImageType.PvP)
			{
				return "BattleHelpImages";
			}
			return "BattleHelpImagesPvP";
		}

		private void LoadTexturesFromAssetBundle()
		{
			List<Texture2D> list = new List<Texture2D>(this._images);
			list.Remove(null);
			this._images = list.ToArray();
			if (this._images.Length == this._imageNames.Length)
			{
				return;
			}
			this._images = new Texture2D[this._imageNames.Length];
			for (int i = 0; i < this._imageNames.Length; i++)
			{
				this._images[i] = (AssetDataMng.Instance().LoadObject("Tutorial_info/" + this._imageNames[i], null, true) as Texture2D);
			}
		}
	}
}
