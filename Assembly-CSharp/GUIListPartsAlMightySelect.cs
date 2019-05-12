using Evolution;
using System;
using System.Collections;
using UnityEngine;

public class GUIListPartsAlMightySelect : GUIListPartBS
{
	[SerializeField]
	[Header("素材アイコン")]
	public UITexture texIcon;

	[Header("選択アイコン")]
	[SerializeField]
	public UISprite spSelectIcon;

	[Header("個数プレート")]
	[SerializeField]
	public UISprite spNumPlate;

	[Header("個数表示")]
	[SerializeField]
	public UILabel lbNum;

	private Vector2 beganPosition;

	private float touchBeganTime;

	private bool isTouching_mi;

	private bool isLongTouched;

	private bool _LongTouch = true;

	public HaveSoulData Data { get; set; }

	public bool LongTouch
	{
		get
		{
			return this._LongTouch;
		}
		set
		{
			this._LongTouch = value;
		}
	}

	public override void SetData()
	{
		CMD_AlMightySelect cmd_AlMightySelect = (CMD_AlMightySelect)base.GetInstanceCMD();
		this.Data = cmd_AlMightySelect.GetSoulDataByIDX(base.IDX);
	}

	public override void InitParts()
	{
		this.SetDetail();
	}

	public override void RefreshParts()
	{
		this.SetDetail();
	}

	private void SetDetail()
	{
		CMD_AlMightySelect cmd_AlMightySelect = (CMD_AlMightySelect)base.GetInstanceCMD();
		GameWebAPI.RespDataMA_GetSoulM.SoulM soulM = this.Data.soulM;
		int haveNum = this.Data.haveNum;
		string evolveItemIconPathByID = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(soulM.soulId);
		this.LoadObjectASync(evolveItemIconPathByID);
		if (cmd_AlMightySelect.CurSelectedSoulId == soulM.soulId)
		{
			this.spSelectIcon.gameObject.SetActive(true);
		}
		else
		{
			this.spSelectIcon.gameObject.SetActive(false);
		}
		this.lbNum.text = haveNum.ToString();
	}

	private void LoadObjectASync(string path)
	{
		CMD_AlMightySelect cmd = (CMD_AlMightySelect)base.GetInstanceCMD();
		Vector3 vScl = base.gameObject.transform.localScale;
		Vector3 localScale = new Vector3(0f, 0f, 1f);
		base.gameObject.transform.localScale = localScale;
		AssetDataMng.Instance().LoadObjectASync(path, delegate(UnityEngine.Object obj)
		{
			Texture2D mainTexture = obj as Texture2D;
			this.texIcon.mainTexture = mainTexture;
			if (this.Data.haveNum - this.Data.curUsedNum >= cmd.NeedNum)
			{
				this.texIcon.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				this.texIcon.color = new Color(0.6f, 0.6f, 0.6f, 1f);
			}
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", vScl.x);
			hashtable.Add("y", vScl.y);
			hashtable.Add("time", 0.4f);
			hashtable.Add("delay", 0.01f);
			hashtable.Add("easetype", "spring");
			hashtable.Add("oncomplete", "ScaleEnd");
			hashtable.Add("oncompleteparams", 0);
			iTween.ScaleTo(this.gameObject, hashtable);
			ITweenResumer component = this.gameObject.GetComponent<ITweenResumer>();
			if (component == null)
			{
				this.gameObject.AddComponent<ITweenResumer>();
			}
		});
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.texIcon.mainTexture != null)
		{
			this.texIcon.mainTexture = null;
		}
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		this.beganPosition = pos;
		base.OnTouchBegan(touch, pos);
		this.isTouching_mi = true;
		this.isLongTouched = false;
		this.touchBeganTime = Time.realtimeSinceStartup;
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable())
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		base.OnTouchMoved(touch, pos);
		float magnitude = (this.beganPosition - pos).magnitude;
		if (magnitude > 40f)
		{
			this.isTouching_mi = false;
		}
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable() && !base.AvoidDisableAllCollider)
		{
			return;
		}
		if (!base.activeCollider)
		{
			return;
		}
		this.isTouching_mi = false;
		if (this.isLongTouched)
		{
			this.isLongTouched = false;
			return;
		}
		if (flag)
		{
			base.OnTouchEnded(touch, pos, flag);
			float magnitude = (this.beganPostion - pos).magnitude;
			if (magnitude < 40f)
			{
				CMD_AlMightySelect cmd_AlMightySelect = (CMD_AlMightySelect)base.GetInstanceCMD();
				cmd_AlMightySelect.SetSelected(this.Data.soulM.soulId);
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.isTouching_mi && this.LongTouch && Time.realtimeSinceStartup - this.touchBeganTime >= 0.5f)
		{
			CMD_QuestItemPOP.Create(this.Data.soulM);
			base.isTouching = false;
			this.isLongTouched = true;
			this.isTouching_mi = false;
		}
	}
}
