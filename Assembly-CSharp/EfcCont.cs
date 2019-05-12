using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("GUI/EffectControl")]
public class EfcCont : MonoBehaviour
{
	public bool USE_EMO_LOAD_IMAGE = true;

	public int emo_dest_X;

	public int emo_dest_Y;

	private UISprite ngSpr;

	private UIWidget ngWdg;

	private GameObject goEmo;

	private GameStringsFont gsfTXT;

	private Color col = default(Color);

	private Color paletteColor = new Color(1f, 1f, 1f, 1f);

	private float paletteValue = 1f;

	private float paletteRate;

	private Action<int> movedAct;

	private Action<int> scaledAct;

	private Action<int> coloredAct;

	private Action<int> coloredAct2;

	private Action<int> paletteAct;

	private Action<int> zoomedAct;

	private EfcCont.TSZoomDt _zdt;

	private Vector3 originalPos;

	private float wide;

	private bool isShake;

	private iTween.EaseType type;

	private Action<int> shakedAct;

	private int shakeCount;

	private float curDegree;

	private bool isBlink;

	private bool isStop;

	private Color liteCol = new Color(1f, 1f, 1f, 1f);

	private Color darkCol = new Color(1f, 1f, 1f, 0.2f);

	private float time_bk;

	protected virtual void Awake()
	{
		this.ngSpr = base.gameObject.GetComponent<UISprite>();
		if (this.ngSpr == null)
		{
			this.ngWdg = base.gameObject.GetComponent<UIWidget>();
			if (this.ngWdg == null)
			{
			}
		}
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "EMO_PARTS")
			{
				this.goEmo = transform.gameObject;
				if (this.USE_EMO_LOAD_IMAGE)
				{
					this.goEmo.transform.localPosition = new Vector3(2000f, 0f, 0f);
				}
			}
			if (transform.name == "TXT")
			{
				this.gsfTXT = transform.gameObject.GetComponent<GameStringsFont>();
			}
		}
		this.SetPaletteRate(0f);
		iTween.Init(base.gameObject);
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	public static float GetFixedTime(float time)
	{
		float result;
		if (Time.timeScale <= 0.01f)
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

	public void SetPosXY(Vector2 vP)
	{
		this.SetPosX(vP.x);
		this.SetPosY(vP.y);
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

	public void SetPosZ(float v)
	{
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.z = v;
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
		if (this.ngSpr != null)
		{
			this.ngSpr.color = c;
		}
		else if (this.ngWdg != null)
		{
			this.ngWdg.color = c;
		}
		if (this.gsfTXT != null)
		{
			this.col = this.gsfTXT.color;
			this.col.a = c.a;
			this.gsfTXT.color = this.col;
		}
	}

	public Color GetColor()
	{
		if (this.ngSpr != null)
		{
			this.col = this.ngSpr.color;
		}
		else if (this.ngWdg != null)
		{
			this.col = this.ngWdg.color;
		}
		if (this.gsfTXT != null)
		{
			this.col = this.gsfTXT.color;
		}
		return this.col;
	}

	public void ChangeSprite(string sprName)
	{
		if (this.ngSpr != null)
		{
			this.ngSpr.spriteName = sprName;
			this.ngSpr.MakePixelPerfect();
			BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
			if (component != null)
			{
				Rect rect = new Rect(0f, 0f, (float)this.ngSpr.width, (float)this.ngSpr.height);
				Vector3 size = component.size;
				size.x = rect.width;
				size.y = rect.height;
				component.size = size;
			}
		}
	}

	public void SetSpriteEmoActive(bool flg)
	{
		this.goEmo.SetActive(flg);
	}

	public void SetSpriteWH(Vector2 v2)
	{
		if (this.ngSpr != null)
		{
			this.ngSpr.width = (int)v2.x;
			this.ngSpr.height = (int)v2.y;
			this.ngSpr.MakePixelPerfect();
		}
	}

	public Vector2 GetSpriteWH()
	{
		Vector2 result = new Vector2(0f, 0f);
		if (this.ngSpr != null)
		{
			result.x = (float)this.ngSpr.width;
			result.y = (float)this.ngSpr.height;
		}
		return result;
	}

	public void SetPaletteColor(Color c)
	{
		this.paletteColor = c;
		this.paletteValue = Mathf.Sqrt(c.r * c.r + c.g * c.g + c.b * c.b);
		this.SetMaterialShaderColor("_paletteCol", this.paletteColor);
		this.SetMaterialShaderValue("_paletteValue", this.paletteValue);
	}

	public void SetPaletteRate(float rate)
	{
		this.paletteRate = rate;
		this.SetMaterialShaderValue("_paletteRate", this.paletteRate);
	}

	private void SetMaterialShaderValue(string tag, float v)
	{
		if (this.ngSpr != null)
		{
			return;
		}
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		if (component != null)
		{
			component.material.SetFloat(tag, v);
		}
		foreach (object obj in base.gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "EMO_PARTS")
			{
				component = transform.gameObject.GetComponent<MeshRenderer>();
				if (component != null)
				{
					component.material.SetFloat(tag, v);
				}
			}
		}
	}

	private void SetMaterialShaderColor(string tag, Color c)
	{
		if (this.ngSpr != null)
		{
			return;
		}
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		if (component != null)
		{
			component.material.SetColor(tag, c);
		}
		foreach (object obj in base.gameObject.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "EMO_PARTS")
			{
				component = transform.gameObject.GetComponent<MeshRenderer>();
				if (component != null)
				{
					component.material.SetColor(tag, c);
				}
			}
		}
	}

	public void MoveTo(Vector2 vP, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear, float delay = 0f)
	{
		if (time <= 0f)
		{
			this.SetPosXY(vP);
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
		if (time <= 0f)
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

	public void SetColoredAct2(Action<int> act)
	{
		this.coloredAct2 = act;
	}

	public void ColorTo(Color c, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear, float delay = 0f)
	{
		if (time <= 0f)
		{
			this.SetColor(c);
			if (act != null)
			{
				act(0);
			}
		}
		else
		{
			this.coloredAct = act;
			Hashtable hashtable = new Hashtable();
			if (this.ngSpr != null)
			{
				hashtable.Add("from", this.ngSpr.color);
			}
			else if (this.ngWdg != null)
			{
				hashtable.Add("from", this.ngWdg.color);
			}
			else if (this.gsfTXT != null)
			{
				hashtable.Add("from", this.gsfTXT.color);
				Color color = this.gsfTXT.color;
				color.a = c.a;
				c = color;
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
		if (this.coloredAct2 != null)
		{
			this.coloredAct2(id);
		}
	}

	private void UpdateColor(Color c)
	{
		if (this.ngSpr != null)
		{
			this.ngSpr.color = c;
		}
		else if (this.ngWdg != null)
		{
			this.ngWdg.color = c;
		}
		if (this.gsfTXT != null)
		{
			this.col = this.gsfTXT.color;
			this.col.a = c.a;
			this.gsfTXT.color = this.col;
		}
	}

	public void PaletteRateTo(float rate, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear)
	{
		if (time <= 0f)
		{
			this.SetPaletteRate(rate);
			if (act != null)
			{
				act(0);
			}
		}
		else
		{
			this.paletteAct = act;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("from", this.paletteRate);
			hashtable.Add("to", rate);
			hashtable.Add("time", EfcCont.GetFixedTime(time));
			hashtable.Add("onupdate", "UpdatePaletteRate");
			hashtable.Add("easetype", type);
			hashtable.Add("oncomplete", "paletteRateEnd");
			hashtable.Add("oncompleteparams", 0);
			iTween.ValueTo(base.gameObject, hashtable);
		}
	}

	private void paletteRateEnd(int id)
	{
		if (this.paletteAct != null)
		{
			this.paletteAct(id);
		}
	}

	private void UpdatePaletteRate(float rate)
	{
		this.paletteRate = rate;
		this.SetPaletteRate(this.paletteRate);
	}

	public void ZoomTo(EfcCont.TSZoomDt zdt, float time, Action<int> act, iTween.EaseType type = iTween.EaseType.linear)
	{
		this.zoomedAct = act;
		this._zdt = zdt;
		Hashtable hashtable = new Hashtable();
		float num = (this._zdt.baseW + this._zdt.zmPix) / this._zdt.baseW;
		float x = base.gameObject.transform.localScale.x;
		hashtable.Add("from", x);
		hashtable.Add("to", num);
		hashtable.Add("time", EfcCont.GetFixedTime(time));
		hashtable.Add("onupdate", "UpdateZoom");
		hashtable.Add("easetype", type);
		hashtable.Add("oncomplete", "zoomEnd");
		hashtable.Add("oncompleteparams", 0);
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void zoomEnd(int id)
	{
		if (this.zoomedAct != null)
		{
			this.zoomedAct(id);
		}
	}

	private void UpdateZoom(float scl)
	{
		Vector3 vector = base.gameObject.transform.localScale;
		vector.x = scl;
		vector.y = scl;
		base.gameObject.transform.localScale = vector;
		vector = base.gameObject.transform.localPosition;
		vector.x = -(this._zdt.ctx * scl - this._zdt.ctx);
		vector.y = -(this._zdt.cty * scl - this._zdt.cty);
		base.gameObject.transform.localPosition = vector;
	}

	public void ShakeStart(float w, Action<int> act, iTween.EaseType _type = iTween.EaseType.linear)
	{
		this.originalPos = base.gameObject.transform.localPosition;
		this.wide = w;
		this.isShake = true;
		this.shakedAct = act;
		this.type = _type;
		Hashtable hashtable = new Hashtable();
		Vector2 nextShakePosRand = this.GetNextShakePosRand(true);
		hashtable.Add("x", nextShakePosRand.x);
		hashtable.Add("y", nextShakePosRand.y);
		hashtable.Add("time", EfcCont.GetFixedTime(0.02f));
		hashtable.Add("easetype", this.type);
		hashtable.Add("oncomplete", "shakeEnd");
		hashtable.Add("oncompleteparams", 0);
		iTween.MoveTo(base.gameObject, hashtable);
		this.shakeCount = 0;
	}

	private void shakeEnd()
	{
		if (this.isShake)
		{
			Hashtable hashtable = new Hashtable();
			this.shakeCount++;
			Vector2 nextShakePosRand;
			if (this.shakeCount == 1)
			{
				if (this.shakedAct != null)
				{
					this.shakedAct(0);
				}
				nextShakePosRand = this.GetNextShakePosRand(false);
			}
			else
			{
				nextShakePosRand = this.GetNextShakePosRand(false);
			}
			hashtable.Add("x", nextShakePosRand.x);
			hashtable.Add("y", nextShakePosRand.y);
			hashtable.Add("time", EfcCont.GetFixedTime(0.03f));
			hashtable.Add("easetype", this.type);
			hashtable.Add("oncomplete", "shakeEnd");
			hashtable.Add("oncompleteparams", 0);
			iTween.MoveTo(base.gameObject, hashtable);
		}
	}

	private Vector2 GetNextShakePosRand(bool isFirst)
	{
		if (isFirst)
		{
			this.curDegree = (float)UnityEngine.Random.Range(0, 360);
		}
		else
		{
			this.curDegree = this.curDegree + 180f + (float)UnityEngine.Random.Range(-30, 30);
		}
		if (this.curDegree >= 360f)
		{
			this.curDegree -= 360f;
		}
		float x = this.originalPos.x + this.wide * Mathf.Cos(this.curDegree * 0.0174532924f);
		float y = this.originalPos.y + this.wide * Mathf.Sin(this.curDegree * 0.0174532924f);
		return new Vector2(x, y);
	}

	public void ShakeStop()
	{
		this.isShake = false;
		Hashtable hashtable = new Hashtable();
		float x = this.originalPos.x;
		float y = this.originalPos.y;
		hashtable.Add("x", x);
		hashtable.Add("y", y);
		hashtable.Add("time", EfcCont.GetFixedTime(0.1f));
		hashtable.Add("easetype", this.type);
		hashtable.Add("oncomplete", "ShakeAllEnd");
		hashtable.Add("oncompleteparams", 0);
		iTween.MoveTo(base.gameObject, hashtable);
	}

	private void ShakeAllAnd()
	{
	}

	public void StartBlink(float time)
	{
		this.time_bk = time;
		this.SetColor(this.liteCol);
		this.isBlink = true;
		this.isStop = false;
		this.ColorTo(this.darkCol, this.time_bk, new Action<int>(this.actEndDark), iTween.EaseType.linear, 0f);
	}

	private void actEndDark(int i)
	{
		if (this.isBlink)
		{
			this.ColorTo(this.liteCol, this.time_bk, new Action<int>(this.actEndLite), iTween.EaseType.linear, 0f);
		}
	}

	private void actEndLite(int i)
	{
		if (this.isStop)
		{
			this.isBlink = false;
		}
		if (this.isBlink)
		{
			this.ColorTo(this.darkCol, this.time_bk, new Action<int>(this.actEndDark), iTween.EaseType.linear, 0f);
		}
	}

	public void StopBlink()
	{
		this.isStop = true;
	}

	public class TSZoomDt
	{
		public float zmPix;

		public float ctx;

		public float cty;

		public float baseW;

		public float baseH;
	}
}
