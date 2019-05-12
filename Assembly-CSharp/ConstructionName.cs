using FarmData;
using Master;
using System;
using UnityEngine;

public class ConstructionName : MonoBehaviour
{
	[NonSerialized]
	public GameObject farmObject;

	[SerializeField]
	private UILabel farmNameUI;

	[SerializeField]
	private UISprite farmNameSprite;

	private ConstructionName.NamePlateBase objectBase;

	private void Start()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null == instance)
		{
			return;
		}
		this.Setup(instance);
	}

	private void Setup(FarmRoot farmRoot)
	{
		ConstructionName.NamePlateBase.Param param = new ConstructionName.NamePlateBase.Param
		{
			constructionName = base.gameObject,
			farmCamera = farmRoot.Camera,
			farmNameSprite = this.farmNameSprite,
			farmNameUI = this.farmNameUI,
			farmObject = this.farmObject
		};
		if (this.farmObject.tag == "Farm.Facility")
		{
			this.objectBase = new ConstructionName.Standard(param);
		}
		else
		{
			this.objectBase = new ConstructionName.Colosseum(param);
		}
	}

	private void Update()
	{
		this.objectBase.Update();
	}

	public void Close()
	{
		this.objectBase.Close();
	}

	public void SetDisplayNamePlate(bool display)
	{
		if (this.objectBase != null)
		{
			this.objectBase.SetDisplayNamePlate(display);
		}
		else
		{
			this.farmNameUI.gameObject.SetActive(display);
			this.farmNameSprite.gameObject.SetActive(display);
		}
	}

	private class NamePlateBase
	{
		protected ConstructionName.NamePlateBase.Param param;

		public NamePlateBase(ConstructionName.NamePlateBase.Param param)
		{
			this.param = param;
			this.Init();
			this.Update();
			this.SetName();
		}

		protected virtual void Init()
		{
		}

		public void Update()
		{
			this.UpdatePosition();
		}

		private void UpdatePosition()
		{
			if (this.param.farmObject != null)
			{
				Vector3 position = this.param.farmCamera.WorldToScreenPoint(this.param.farmObject.transform.position);
				Camera gUICamera = GUIManager.gUICamera;
				Vector3 vector = gUICamera.ScreenToWorldPoint(position);
				Vector3 position2 = this.param.constructionName.transform.position;
				position2.x = vector.x;
				position2.y = vector.y;
				this.param.constructionName.transform.position = position2;
				Vector3 point = this.GetPoint();
				this.param.farmNameUI.gameObject.transform.localPosition = point;
				this.param.farmNameSprite.gameObject.transform.localPosition = point;
				Vector3 size = this.GetSize();
				this.param.farmNameUI.gameObject.transform.localScale = size;
				this.param.farmNameSprite.gameObject.transform.localScale = size;
			}
			else
			{
				this.Close();
			}
		}

		protected virtual Vector3 GetPoint()
		{
			Vector3 result = new Vector3(0f, 1000f / this.param.farmCamera.orthographicSize, 0f);
			return result;
		}

		private Vector3 GetSize()
		{
			float num = 6.9f / this.param.farmCamera.orthographicSize;
			Vector3 result = new Vector3(num, num, 0f);
			return result;
		}

		private void SetName()
		{
			this.param.farmNameUI.text = this.GetName();
			int w = this.param.farmNameUI.width + 35;
			this.param.farmNameSprite.SetDimensions(w, 35);
		}

		protected virtual string GetName()
		{
			return string.Empty;
		}

		public void Close()
		{
			UnityEngine.Object.Destroy(this.param.constructionName);
			this.param = null;
		}

		public void SetDisplayNamePlate(bool display)
		{
			this.param.farmNameUI.gameObject.SetActive(display);
			this.param.farmNameSprite.gameObject.SetActive(display);
		}

		public class Param
		{
			public GameObject constructionName;

			public GameObject farmObject;

			public Camera farmCamera;

			public UILabel farmNameUI;

			public UISprite farmNameSprite;
		}
	}

	private class Standard : ConstructionName.NamePlateBase
	{
		private FarmObject farmObject;

		public Standard(ConstructionName.NamePlateBase.Param param) : base(param)
		{
		}

		protected override void Init()
		{
			this.farmObject = this.param.farmObject.GetComponent<FarmObject>();
		}

		protected override Vector3 GetPoint()
		{
			Vector3 result;
			if (this.farmObject.facilityID == 4)
			{
				result = new Vector3(0f, 1600f / this.param.farmCamera.orthographicSize, 0f);
			}
			else if (this.farmObject.facilityID == 5)
			{
				result = new Vector3(0f, 2000f / this.param.farmCamera.orthographicSize, 0f);
			}
			else
			{
				int num;
				if (this.farmObject.sizeY == 0 || this.farmObject.sizeY == 1)
				{
					num = 2;
				}
				else if (this.farmObject.sizeY > 6)
				{
					num = 6;
				}
				else
				{
					num = this.farmObject.sizeY;
				}
				result = new Vector3(0f, (float)(400 * num) / this.param.farmCamera.orthographicSize, 0f);
			}
			return result;
		}

		protected override string GetName()
		{
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.farmObject.facilityID);
			return facilityMaster.facilityName;
		}
	}

	private class Colosseum : ConstructionName.NamePlateBase
	{
		public Colosseum(ConstructionName.NamePlateBase.Param param) : base(param)
		{
		}

		protected override Vector3 GetPoint()
		{
			Vector3 result = new Vector3(0f, 1900f / this.param.farmCamera.orthographicSize, 0f);
			return result;
		}

		protected override string GetName()
		{
			return StringMaster.GetString("ColosseumTitle");
		}
	}
}
