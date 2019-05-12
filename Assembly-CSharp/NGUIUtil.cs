using System;
using System.Collections;
using UnityEngine;

public class NGUIUtil : MonoBehaviour
{
	public static Texture2D LoadTexture(string texname)
	{
		return AssetDataMng.Instance().LoadObject(texname, null, true) as Texture2D;
	}

	public static void LoadTextureAsync(UITexture uiTex, string texname, Action callback = null)
	{
		if (AssetDataMng.Instance().IsAssetBundleData(texname))
		{
			PresentBoxItemIconCacheBuffer.Instance().LoadAndCacheObj(texname, delegate(UnityEngine.Object obj)
			{
				Texture2D mainTexture = obj as Texture2D;
				uiTex.mainTexture = mainTexture;
				if (callback != null)
				{
					callback();
				}
			});
		}
		else
		{
			AssetDataMng.Instance().LoadObjectASync(texname, delegate(UnityEngine.Object obj)
			{
				Texture2D mainTexture = obj as Texture2D;
				uiTex.mainTexture = mainTexture;
				Hashtable hashtable = new Hashtable();
				hashtable.Add("x", 1f);
				hashtable.Add("y", 1f);
				hashtable.Add("time", 0.4f);
				hashtable.Add("delay", 0.01f);
				hashtable.Add("easetype", "spring");
				hashtable.Add("oncomplete", "ScaleEnd");
				hashtable.Add("oncompleteparams", 0);
				iTween.ScaleTo(uiTex.gameObject, hashtable);
				if (callback != null)
				{
					callback();
				}
			});
		}
	}

	public static Sprite LoadSprite(string texname)
	{
		return AssetDataMng.Instance().LoadObject(texname, null, true) as Sprite;
	}

	public static void ChangeUITexture(UITexture uiTex, Texture2D tex, bool resize = true)
	{
		uiTex.mainTexture = tex;
		if (resize)
		{
			uiTex.MakePixelPerfect();
		}
	}

	public static void ChangeUITextureFromFile(UITexture uiTex, string texname, bool resize = true)
	{
		Texture2D mainTexture = NGUIUtil.LoadTexture(texname);
		uiTex.mainTexture = mainTexture;
		if (resize)
		{
			uiTex.MakePixelPerfect();
		}
	}

	public static void ChangeUITextureFromFileASync(UITexture uiTex, string texname, bool resize = true, Action callback = null)
	{
		NGUIUtil.LoadTextureAsync(uiTex, texname, callback);
		if (resize)
		{
			uiTex.MakePixelPerfect();
		}
	}

	public static void ChangeUI2DSpriteFromFile(UI2DSprite uiSpr, string texname)
	{
		Texture2D texture2D = NGUIUtil.LoadTexture(texname);
		Rect rect = new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height);
		Vector2 pivot = new Vector2(0f, 0f);
		uiSpr.sprite2D = Sprite.Create(texture2D, rect, pivot, 1f);
		uiSpr.MakePixelPerfect();
	}

	public static void ChangeUISpriteWithSize(UISprite uiSpr, string sprname)
	{
		uiSpr.spriteName = sprname;
		uiSpr.MakePixelPerfect();
		BoxCollider component = uiSpr.gameObject.GetComponent<BoxCollider>();
		if (component != null)
		{
			Vector3 size = component.size;
			size.x = (float)uiSpr.width;
			size.y = (float)uiSpr.height;
			component.size = size;
		}
	}
}
