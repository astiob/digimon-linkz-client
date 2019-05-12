using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("GUI/EffectControl")]
public sealed class EfcCont : MonoBehaviour
{
	private UISprite ngSpr;

	private UIWidget ngWdg;

	private Color tempColor;

	private Action<int> movedAct;

	private Action<int> scaledAct;

	private Action<int> coloredAct;

	private void Awake()
	{
		this.ngSpr = base.gameObject.GetComponent<UISprite>();
		if (null == this.ngSpr)
		{
			this.ngWdg = base.gameObject.GetComponent<UIWidget>();
		}
		this.SetMaterialShaderValue("_paletteRate", 0f);
		iTween.Init(base.gameObject);
	}

	public static float GetFixedTime(float time)
	{
		float result;
		if (0.01f >= Time.timeScale)
		{
			result = time;
		}
		else
		{
			result = time * Time.timeScale;
		}
		return result;
	}

	public void SetPos(Vector3 vP)
	{
		base.gameObject.transform.localPosition = vP;
	}

	public void SetPosX(float v)
	{
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.x = v;
		base.gameObject.transform.localPosition = localPosition;
	}

	public void SetPosY(float v)
	{
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.y = v;
		base.gameObject.transform.localPosition = localPosition;
	}

	public Vector3 GetPos()
	{
		return base.gameObject.transform.localPosition;
	}

	public void SetScale(Vector3 vS)
	{
		base.gameObject.transform.localScale = vS;
	}

	public Vector3 GetScale()
	{
		return base.gameObject.transform.localScale;
	}

	public void SetColor(Color c)
	{
		if (null != this.ngSpr)
		{
			this.ngSpr.color = c;
		}
		else if (null != this.ngWdg)
		{
			this.ngWdg.color = c;
		}
	}

	public Color GetColor()
	{
		if (null != this.ngSpr)
		{
			this.tempColor = this.ngSpr.color;
		}
		else if (null != this.ngWdg)
		{
			this.tempColor = this.ngWdg.color;
		}
		else
		{
			this.tempColor = Color.black;
		}
		return this.tempColor;
	}

	private void SetMaterialShaderValue(string tag, float v)
	{
		if (null == this.ngSpr)
		{
			MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
			if (null != component)
			{
				component.material.SetFloat(tag, v);
				global::Debug.Log("<color=pink>EfcCont.SetMaterialShaderValue()</color>");
			}
		}
	}

	public void MoveTo(Vector2 vP, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear, float delay = 0f)
	{
		if (0f >= time)
		{
			this.SetPosX(vP.x);
			this.SetPosY(vP.y);
			if (act != null)
			{
				act(0);
			}
		}
		else
		{
			this.movedAct = act;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("isLocal", true);
			hashtable.Add("x", vP.x);
			hashtable.Add("y", vP.y);
			hashtable.Add("time", EfcCont.GetFixedTime(time));
			hashtable.Add("delay", delay);
			hashtable.Add("easetype", type);
			hashtable.Add("oncomplete", "MoveEnd");
			hashtable.Add("oncompleteparams", 0);
			iTween.MoveTo(base.gameObject, hashtable);
		}
	}

	private void MoveEnd(int id)
	{
		if (this.movedAct != null)
		{
			this.movedAct(id);
		}
	}

	public void ScaleTo(Vector2 vS, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear, float delay = 0f)
	{
		if (0f >= time)
		{
			this.SetScale(vS);
			if (act != null)
			{
				act(0);
			}
		}
		else
		{
			this.scaledAct = act;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", vS.x);
			hashtable.Add("y", vS.y);
			hashtable.Add("time", EfcCont.GetFixedTime(time));
			hashtable.Add("delay", delay);
			hashtable.Add("easetype", type);
			hashtable.Add("oncomplete", "ScaleEnd");
			hashtable.Add("oncompleteparams", 0);
			iTween.ScaleTo(base.gameObject, hashtable);
		}
	}

	private void ScaleEnd(int id)
	{
		if (this.scaledAct != null)
		{
			this.scaledAct(id);
		}
	}

	public void ColorTo(Color c, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear, float delay = 0f)
	{
		if (0f >= time)
		{
			this.SetColor(c);
			if (act != null)
			{
				act(0);
			}
		}
		else if (null == this.ngSpr && null == this.ngWdg)
		{
			if (act != null)
			{
				act(0);
			}
		}
		else
		{
			this.coloredAct = act;
			Hashtable hashtable = new Hashtable();
			if (null != this.ngSpr)
			{
				hashtable.Add("from", this.ngSpr.color);
			}
			else if (null != this.ngWdg)
			{
				hashtable.Add("from", this.ngWdg.color);
			}
			hashtable.Add("to", c);
			hashtable.Add("time", EfcCont.GetFixedTime(time));
			hashtable.Add("delay", delay);
			hashtable.Add("onupdate", "UpdateColor");
			hashtable.Add("easetype", type);
			hashtable.Add("oncomplete", "colorEnd");
			hashtable.Add("oncompleteparams", 0);
			iTween.ValueTo(base.gameObject, hashtable);
		}
	}

	private void colorEnd(int id)
	{
		if (this.coloredAct != null)
		{
			this.coloredAct(id);
		}
	}

	private void UpdateColor(Color c)
	{
		if (null != this.ngSpr)
		{
			this.ngSpr.color = c;
		}
		else if (null != this.ngWdg)
		{
			this.ngWdg.color = c;
		}
	}
}
