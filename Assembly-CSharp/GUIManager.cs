using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : Singleton<GUIManager>
{
	public static Dictionary<string, LinkedList<ITouchEvent>> gUIColliders = new Dictionary<string, LinkedList<ITouchEvent>>();

	private static Dictionary<string, GUIBase> guiBases = new Dictionary<string, GUIBase>();

	private static Dictionary<string, CommonDialog> commonDialogs = new Dictionary<string, CommonDialog>();

	private static Dictionary<Collider, ITouchEvent> colliders = new Dictionary<Collider, ITouchEvent>();

	private static Dictionary<int, ITouchEvent> spriteSelected = new Dictionary<int, ITouchEvent>();

	public static bool enebleMultiInput = false;

	private static int _generation = 0;

	private static bool extBackKeyReady = true;

	private static GUIBase isShowGUI;

	private static float DLG_START_Z = -1000f;

	private static float DLG_PITCH_Z = -200f;

	private static float DLG_BARRIER_OFS_Z = 50f;

	private static int dialogValue_;

	private static bool blockBGCollider;

	public static Action<CommonDialog> actCallShowDialog;

	private static bool barrierReqToFade;

	private static bool closeAllMode = false;

	private static int closeAll_CT = 0;

	private static List<CommonDialog> cdList_BK = null;

	private static Action actCallBackCloseAll = null;

	private static bool someOneTouch_;

	private static bool touchEndTrg = false;

	private static int touchingCount = 0;

	public static List<ITouchEvent> selectButtons;

	public event Action ActCallBackCloseAllCMD;

	public bool UseOutsideTouchControl { get; set; }

	public static bool EnableMultiInput
	{
		get
		{
			return GUIManager.enebleMultiInput;
		}
		set
		{
			GUIManager.enebleMultiInput = value;
		}
	}

	public static void AllInit()
	{
		GUIManager.gUIColliders = new Dictionary<string, LinkedList<ITouchEvent>>();
		GUIManager.guiBases = new Dictionary<string, GUIBase>();
		GUIManager.commonDialogs = new Dictionary<string, CommonDialog>();
		GUIManager.colliders = new Dictionary<Collider, ITouchEvent>();
		GUIManager.spriteSelected = new Dictionary<int, ITouchEvent>();
	}

	public static bool HaveGuiBase(string gui)
	{
		return GUIManager.guiBases.ContainsKey(gui);
	}

	private void Start()
	{
		GUIManager.InitCloseAll();
	}

	public static void AddGUIBase(GUIBase guiBase)
	{
		if (guiBase != null && !GUIManager.guiBases.ContainsKey(guiBase.gameObject.name))
		{
			GUIManager.guiBases.Add(guiBase.gameObject.name, guiBase);
		}
		guiBase.gameObject.SetActive(false);
	}

	public static void DeleteGUIBase(GUIBase guiBase)
	{
		if (guiBase != null && GUIManager.guiBases.ContainsKey(guiBase.gameObject.name))
		{
			GUIManager.guiBases.Remove(guiBase.gameObject.name);
		}
	}

	public static void AddCommonDialog(CommonDialog commonDialog)
	{
		if (GUIManager.commonDialogs.ContainsKey(commonDialog.name))
		{
			global::Debug.LogError("have commonDialogs name=" + commonDialog.gName);
		}
		else
		{
			GUIManager.commonDialogs.Add(commonDialog.gName, commonDialog);
			commonDialog.gameObject.SetActive(false);
		}
	}

	public static void DeleteCommonDialog(CommonDialog commonDialog)
	{
		if (commonDialog != null && GUIManager.commonDialogs.ContainsKey(commonDialog.gName))
		{
			GUIManager.commonDialogs.Remove(commonDialog.gName);
		}
	}

	private static void AddGUICollider(ITouchEvent iTouchEvent)
	{
		string gName = iTouchEvent.gName;
		if (!GUIManager.gUIColliders.ContainsKey(gName))
		{
			LinkedList<ITouchEvent> value = new LinkedList<ITouchEvent>();
			GUIManager.gUIColliders.Add(gName, value);
			GUIManager.gUIColliders[gName].AddFirst(iTouchEvent);
		}
		else
		{
			GUIManager.gUIColliders[gName].AddFirst(iTouchEvent);
		}
	}

	public static void AddCollider(ITouchEvent iTouchEvent)
	{
		Collider component = iTouchEvent.gObject.GetComponent<Collider>();
		if (component != null)
		{
			if (!GUIManager.colliders.ContainsKey(component))
			{
				GUIManager.colliders.Add(component, iTouchEvent);
			}
			GUIManager.AddGUICollider(iTouchEvent);
		}
	}

	public static LinkedList<ITouchEvent> GetCollider(string colName)
	{
		if (GUIManager.gUIColliders.ContainsKey(colName))
		{
			return GUIManager.gUIColliders[colName];
		}
		return null;
	}

	public static GameObject GetColliderFirstActive(string colName)
	{
		LinkedList<ITouchEvent> collider = GUIManager.GetCollider(colName);
		if (collider == null)
		{
			return null;
		}
		LinkedListNode<ITouchEvent> linkedListNode = collider.First;
		while (linkedListNode != null)
		{
			ITouchEvent value = linkedListNode.Value;
			linkedListNode = linkedListNode.Next;
			if (value.gObject.activeSelf)
			{
				return value.gObject;
			}
		}
		return null;
	}

	private static void DeleteGUICollider(ITouchEvent iTouchEvent)
	{
		string gName = iTouchEvent.gName;
		if (GUIManager.gUIColliders.ContainsKey(gName))
		{
			LinkedListNode<ITouchEvent> linkedListNode = GUIManager.gUIColliders[gName].First;
			while (linkedListNode != null)
			{
				ITouchEvent value = linkedListNode.Value;
				LinkedListNode<ITouchEvent> node = linkedListNode;
				linkedListNode = linkedListNode.Next;
				if (value == iTouchEvent)
				{
					GUIManager.gUIColliders[gName].Remove(node);
					if (GUIManager.gUIColliders[gName].Count < 1)
					{
						GUIManager.gUIColliders.Remove(gName);
					}
				}
			}
		}
	}

	public static void DeleteCollider(Collider collider)
	{
		if (collider != null)
		{
			if (GUIManager.colliders.ContainsKey(collider))
			{
				GUIManager.colliders.Remove(collider);
			}
			if (GUIManager.gUIColliders.ContainsKey(collider.name))
			{
				GUIManager.gUIColliders.Remove(collider.name);
			}
		}
	}

	public static void DeleteCollider(ITouchEvent iTouchEvent)
	{
		Collider component = iTouchEvent.gObject.GetComponent<Collider>();
		if (component != null)
		{
			if (GUIManager.colliders.ContainsKey(component))
			{
				GUIManager.colliders.Remove(component);
			}
			if (GUIManager.gUIColliders.ContainsKey(component.name))
			{
				GUIManager.gUIColliders.Remove(component.name);
			}
		}
	}

	public static void ClearColliderTouch()
	{
		foreach (ITouchEvent touchEvent in GUIManager.colliders.Values)
		{
			touchEvent.OnTouchInit();
		}
	}

	public static bool LoadGUI(string guiName, string prefabName)
	{
		if (GUIManager.guiBases != null && GUIManager.guiBases.ContainsKey(guiName))
		{
			return false;
		}
		string path = "UIScreen/" + prefabName;
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(MultiLanguageResources.Load(path, typeof(GameObject)));
		gameObject.name = guiName;
		GUIBase component = gameObject.GetComponent<GUIBase>();
		if (component == null)
		{
			return false;
		}
		Vector3 localScale = gameObject.transform.localScale;
		component.gameObject.transform.parent = Singleton<GUIMain>.Instance.gameObject.transform;
		gameObject.transform.localScale = localScale;
		GUIManager.AddGUIBase(component);
		gameObject.SetActive(true);
		return true;
	}

	public static GameObject LoadCommonGUI(string guiName, GameObject parent)
	{
		string path = "UICommon/" + guiName;
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(MultiLanguageResources.Load(path, typeof(GameObject)));
		if (gameObject == null)
		{
			return null;
		}
		gameObject.name = guiName;
		gameObject.SetActive(true);
		Vector3 localPosition = gameObject.transform.localPosition;
		if (parent != null)
		{
			Vector3 localScale = gameObject.transform.localScale;
			gameObject.transform.parent = parent.transform;
			gameObject.transform.localScale = localScale;
		}
		gameObject.transform.localPosition = localPosition;
		if (localPosition.x < 0f)
		{
			localPosition.x = -localPosition.x;
			localPosition.x = (float)((int)localPosition.x % 2000);
			if (localPosition.x > 1000f)
			{
				localPosition.x -= 2000f;
			}
			localPosition.x = -localPosition.x;
		}
		else
		{
			if (localPosition.x > 1000f)
			{
				localPosition.x -= 2000f;
			}
			localPosition.x = (float)((int)localPosition.x % 2000);
		}
		if (localPosition.y < 0f)
		{
			localPosition.y = -localPosition.y;
			localPosition.y = (float)((int)localPosition.y % 2000);
			if (localPosition.y > 1000f)
			{
				localPosition.y -= 2000f;
			}
			localPosition.y = -localPosition.y;
		}
		else
		{
			if (localPosition.y > 1000f)
			{
				localPosition.y -= 2000f;
			}
			localPosition.y = (float)((int)localPosition.y % 2000);
		}
		gameObject.transform.localPosition = localPosition;
		GUICollider component = gameObject.GetComponent<GUICollider>();
		if (component != null)
		{
			component.SetOriginalPos(localPosition);
		}
		GUIScreen component2 = gameObject.GetComponent<GUIScreen>();
		if (component2 != null)
		{
			component2.ShowGUI();
		}
		return gameObject;
	}

	public static bool IsLoadedGUI(string guiName)
	{
		return GUIManager.guiBases != null && GUIManager.guiBases.ContainsKey(guiName);
	}

	public static bool ReadyGUI(string guiName)
	{
		if (GUIManager.guiBases == null || !GUIManager.guiBases.ContainsKey(guiName))
		{
			return false;
		}
		GUIBase guibase = GUIManager.guiBases[guiName];
		return guibase.startFlag;
	}

	public static GUIBase GetGUI(string guiName)
	{
		if (GUIManager.guiBases == null || !GUIManager.guiBases.ContainsKey(guiName))
		{
			return null;
		}
		return GUIManager.guiBases[guiName];
	}

	public static bool ShowGUI(string guiName)
	{
		if (GUIManager.guiBases == null || !GUIManager.guiBases.ContainsKey(guiName))
		{
			return false;
		}
		GUIManager.isShowGUI = GUIManager.guiBases[guiName];
		GUIManager.isShowGUI.gameObject.SetActive(true);
		Vector3 position = GUIManager.isShowGUI.gameObject.transform.position;
		GUIManager.isShowGUI.gameObject.transform.localPosition = new Vector3(0f, 0f, position.z);
		GUIManager.isShowGUI.ShowGUI();
		return true;
	}

	public static bool HideGUI(string guiName)
	{
		if (GUIManager.guiBases == null || !GUIManager.guiBases.ContainsKey(guiName))
		{
			return false;
		}
		GUIBase guibase = GUIManager.guiBases[guiName];
		guibase.gameObject.SetActive(false);
		guibase.ReturnPos();
		guibase.HideGUI();
		if (!guibase.resident)
		{
			GUIManager.DeleteGUIBase(guibase);
			UnityEngine.Object.Destroy(guibase.gameObject);
			Resources.UnloadUnusedAssets();
		}
		return true;
	}

	public static GUICollider ColliderGUI(string guiName)
	{
		if (GUIManager.guiBases == null || !GUIManager.guiBases.ContainsKey(guiName))
		{
			return null;
		}
		GUIBase guibase = GUIManager.guiBases[guiName];
		return guibase.gameObject.GetComponent<GUICollider>();
	}

	public static int dialogValue
	{
		get
		{
			return GUIManager.dialogValue_;
		}
		set
		{
			GUIManager.dialogValue_ = value;
		}
	}

	public static float GetDLGPitch()
	{
		return GUIManager.DLG_PITCH_Z;
	}

	public static float GetDLGStartZ()
	{
		return GUIManager.DLG_START_Z;
	}

	public static float GetDLG_BARRIER_OFS_Z()
	{
		return GUIManager.DLG_BARRIER_OFS_Z;
	}

	public static CommonDialog GetTopDialog(CommonDialog cdself = null, bool includeAll = false)
	{
		float num = 100000f;
		CommonDialog result = null;
		foreach (string key in GUIManager.commonDialogs.Keys)
		{
			if (includeAll || !GUIManager.commonDialogs[key].dontManageZPos)
			{
				GameObject gameObject = GUIManager.commonDialogs[key].gameObject;
				if (gameObject.transform.localPosition.z < num && cdself != GUIManager.commonDialogs[key])
				{
					num = gameObject.transform.localPosition.z;
					result = GUIManager.commonDialogs[key];
				}
			}
		}
		return result;
	}

	public static CommonDialog GetTopDialogANM(CommonDialog cdself = null, bool includeAll = false)
	{
		CommonDialog topDialog = GUIManager.GetTopDialog(cdself, includeAll);
		CMD cmd = (CMD)topDialog;
		if (!(cmd != null))
		{
			return null;
		}
		if (cmd.useCMDAnim)
		{
			return topDialog;
		}
		return null;
	}

	public static CommonDialog CheckTopDialog(string name, CommonDialog cdself = null)
	{
		CommonDialog topDialog = GUIManager.GetTopDialog(cdself, false);
		if (topDialog != null && topDialog.name == name)
		{
			return topDialog;
		}
		return null;
	}

	public static bool CheckOpenDialog(string strName)
	{
		return GUIManager.commonDialogs.ContainsKey(strName);
	}

	public static GameObject GetDialog(string dialogName = "CommonDialog")
	{
		if (!GUIManager.commonDialogs.ContainsKey(dialogName))
		{
			return null;
		}
		return GUIManager.commonDialogs[dialogName].gameObject;
	}

	public static Dictionary<string, CommonDialog> GetDialogDic()
	{
		return GUIManager.commonDialogs;
	}

	public static void SetActCallShowDialog(Action<CommonDialog> act)
	{
		GUIManager.actCallShowDialog = act;
	}

	public static CommonDialog ShowCommonDialog(Action<int> action, string dialogName, Action<CommonDialog> actionReady)
	{
		bool flag = true;
		float aT = 0.2f;
		float x2 = 0f;
		float y = 0f;
		float sizeX = -1f;
		float sizeY = -1f;
		if (!GUIManager.commonDialogs.ContainsKey(dialogName))
		{
			string path = "UIDialogBox/" + dialogName;
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(MultiLanguageResources.Load(path, typeof(GameObject)));
			if (null == gameObject)
			{
				return null;
			}
			Vector3 localScale = gameObject.transform.localScale;
			gameObject.transform.parent = Singleton<GUIManager>.Instance.gameObject.transform;
			gameObject.transform.localScale = localScale;
			gameObject.SetActive(true);
			if (!GUIManager.commonDialogs.ContainsKey(dialogName))
			{
				return null;
			}
		}
		else if (GUIManager.commonDialogs[dialogName].gameObject.activeSelf)
		{
			return GUIManager.commonDialogs[dialogName];
		}
		Action<int> action2 = delegate(int x)
		{
			GUIManager.dialogValue_ = x;
		};
		if (action != null)
		{
			action2 = (Action<int>)Delegate.Combine(action2, action);
		}
		CommonDialog commonDialog = GUIManager.commonDialogs[dialogName];
		Vector3 position = commonDialog.gameObject.transform.position;
		commonDialog.gameObject.transform.localPosition = new Vector3(x2, y, position.z);
		if (actionReady != null)
		{
			actionReady(commonDialog);
		}
		commonDialog.Show(action2, sizeX, sizeY, aT);
		GUIManager.blockBGCollider = flag;
		if (GUIManager.blockBGCollider)
		{
			GUIManager.ShowGUI("CommonDialogBarrier");
		}
		CommonDialog topDialog = GUIManager.GetTopDialog(commonDialog, false);
		Vector3 localPosition = commonDialog.gameObject.transform.localPosition;
		float num;
		if (!commonDialog.dontManageZPos)
		{
			if (topDialog == null)
			{
				num = GUIManager.DLG_START_Z;
			}
			else
			{
				num = topDialog.gameObject.transform.localPosition.z + GUIManager.DLG_PITCH_Z;
			}
			localPosition.z = num;
		}
		else
		{
			num = localPosition.z;
		}
		commonDialog.SetOriginalPos(localPosition);
		GUIBase guibase = GUIManager.guiBases["CommonDialogBarrier"];
		localPosition = guibase.gameObject.transform.localPosition;
		localPosition.x = (localPosition.y = 0f);
		localPosition.z = num + GUIManager.DLG_BARRIER_OFS_Z;
		guibase.gameObject.transform.localPosition = localPosition;
		UISprite component = guibase.gameObject.GetComponent<UISprite>();
		if (component != null)
		{
			component.color = commonDialog.barrierColor;
		}
		if (GUIMain.USE_NGUI)
		{
			UIWidget component2 = guibase.gameObject.GetComponent<UIWidget>();
			if (component2 != null)
			{
				component2.depth = (int)(-(int)guibase.gameObject.transform.localPosition.z);
			}
			int add = (int)(-(int)commonDialog.gameObject.transform.localPosition.z);
			GUIManager.AddWidgetDepth(commonDialog.gameObject.transform, add);
		}
		if (GUIManager.actCallShowDialog != null)
		{
			GUIManager.actCallShowDialog(commonDialog);
		}
		return commonDialog;
	}

	public static void AddWidgetDepth(Transform tm, int add)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.depth += add;
		}
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			GUIManager.AddWidgetDepth(tm2, add);
		}
	}

	public static void SetWidgetDepth(Transform tm, int dpth)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.depth = dpth;
		}
		dpth++;
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			GUIManager.SetWidgetDepth(tm2, dpth);
		}
		dpth--;
	}

	public static void SetColorAll(Transform tm, Color col)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			UILabel component2 = tm.gameObject.GetComponent<UILabel>();
			if (component2 == null)
			{
				component.color = col;
			}
		}
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			GUIManager.SetColorAll(tm2, col);
		}
	}

	public static bool BarrierReqToFade
	{
		get
		{
			return GUIManager.barrierReqToFade;
		}
	}

	public static bool HideCommonDialog(CommonDialog cd)
	{
		if (GUIManager.blockBGCollider)
		{
			GUIBase guibase = GUIManager.guiBases["CommonDialogBarrier"];
			Vector3 localPosition = guibase.gameObject.transform.localPosition;
			int i = 0;
			if (GUIManager.closeAllMode)
			{
				for (i = 0; i < GUIManager.cdList_BK.Count; i++)
				{
					if (cd == GUIManager.cdList_BK[i])
					{
						GUIManager.closeAll_CT--;
						break;
					}
				}
				if (GUIManager.closeAll_CT <= 0)
				{
					GUIManager.closeAll_CT = 0;
					GUIManager.closeAllMode = false;
					GUIManager.HideGUI("CommonDialogBarrier");
					GUIManager.ResetTouchingCount();
					if (Singleton<GUIManager>.Instance.ActCallBackCloseAllCMD != null)
					{
						Singleton<GUIManager>.Instance.ActCallBackCloseAllCMD();
					}
					GUIManager.barrierReqToFade = true;
					if (GUIManager.actCallBackCloseAll != null)
					{
						cd.SetLastCallBack(GUIManager.actCallBackCloseAll);
					}
					if (GUIManager.actCallShowDialog != null)
					{
						GUIManager.actCallShowDialog(null);
					}
				}
			}
			if (!GUIManager.closeAllMode || i == GUIManager.cdList_BK.Count)
			{
				if (GUIManager.commonDialogs.Count == 1)
				{
					GUIManager.HideGUI("CommonDialogBarrier");
					GUIManager.ResetTouchingCount();
					if (Singleton<GUIManager>.Instance.ActCallBackCloseAllCMD != null)
					{
						Singleton<GUIManager>.Instance.ActCallBackCloseAllCMD();
					}
					GUIManager.barrierReqToFade = true;
					if (GUIManager.actCallShowDialog != null)
					{
						GUIManager.actCallShowDialog(null);
					}
				}
				else
				{
					CommonDialog topDialog = GUIManager.GetTopDialog(cd, true);
					localPosition.z = topDialog.gameObject.transform.localPosition.z + GUIManager.DLG_BARRIER_OFS_Z;
					guibase.gameObject.transform.localPosition = localPosition;
					if (GUIMain.USE_NGUI)
					{
						UIWidget component = guibase.gameObject.GetComponent<UIWidget>();
						if (component != null)
						{
							component.depth = (int)(-(int)guibase.gameObject.transform.localPosition.z);
						}
					}
					UISprite component2 = guibase.gameObject.GetComponent<UISprite>();
					if (component2 != null)
					{
						component2.color = topDialog.barrierColor;
					}
					if (GUIManager.actCallShowDialog != null)
					{
						GUIManager.actCallShowDialog(topDialog);
					}
				}
			}
		}
		return true;
	}

	public static bool IsCloseAllMode()
	{
		return GUIManager.closeAllMode;
	}

	public static void InitCloseAll()
	{
		GUIManager.closeAllMode = false;
		GUIManager.closeAll_CT = 0;
		GUIManager.cdList_BK = new List<CommonDialog>();
	}

	public static void CloseAllCommonDialog(Action actCallBack = null)
	{
		GUIManager.actCallBackCloseAll = actCallBack;
		GUIManager.closeAll_CT = 0;
		if (GUIManager.commonDialogs == null)
		{
			if (GUIManager.actCallBackCloseAll != null)
			{
				GUIManager.actCallBackCloseAll();
				GUIManager.actCallBackCloseAll = null;
			}
			return;
		}
		List<CommonDialog> list = new List<CommonDialog>();
		foreach (string key in GUIManager.commonDialogs.Keys)
		{
			if (!GUIManager.commonDialogs[key].IsClosing())
			{
				list.Add(GUIManager.commonDialogs[key]);
				GUIManager.closeAll_CT++;
			}
		}
		GUIManager.cdList_BK = list;
		if (GUIManager.closeAll_CT <= 0)
		{
			GUIManager.closeAll_CT = 0;
			if (GUIManager.actCallBackCloseAll != null)
			{
				GUIManager.actCallBackCloseAll();
				GUIManager.actCallBackCloseAll = null;
			}
		}
		else
		{
			list.Sort(new Comparison<CommonDialog>(GUIManager.CompareDLG_Z));
			GUIManager.closeAllMode = true;
			for (int i = 0; i < list.Count; i++)
			{
				if (i == list.Count - 1)
				{
					list[i].ClosePanel(true);
					List<CommonDialog> list2 = new List<CommonDialog>();
					foreach (string key2 in GUIManager.commonDialogs.Keys)
					{
						if (!GUIManager.commonDialogs[key2].IsClosing())
						{
							list2.Add(GUIManager.commonDialogs[key2]);
						}
					}
					list2.Sort(new Comparison<CommonDialog>(GUIManager.CompareDLG_Z));
					for (int j = 0; j < list2.Count; j++)
					{
						float num = GUIManager.DLG_START_Z + GUIManager.DLG_PITCH_Z * (float)j;
						Vector3 localPosition = list2[j].gameObject.transform.localPosition;
						int add = -(int)(num - list2[j].gameObject.transform.localPosition.z);
						GUIManager.AddWidgetDepth(list2[j].gameObject.transform, add);
						localPosition.z = num;
						list2[j].SetOriginalPos(localPosition);
						if (j == list2.Count - 1)
						{
							GUIBase guibase = GUIManager.guiBases["CommonDialogBarrier"];
							localPosition = guibase.gameObject.transform.localPosition;
							localPosition.z = num + GUIManager.DLG_BARRIER_OFS_Z;
							guibase.gameObject.transform.localPosition = localPosition;
							GUIManager.SetWidgetDepth(guibase.gameObject.transform, (int)(-(int)localPosition.z));
							UISprite component = guibase.gameObject.GetComponent<UISprite>();
							if (component != null)
							{
								component.color = list2[j].barrierColor;
							}
						}
					}
					if (GUIManager.actCallShowDialog != null)
					{
						GUIManager.actCallShowDialog(null);
					}
				}
				else
				{
					list[i].ClosePanel(false);
				}
			}
		}
	}

	private static int CompareDLG_Z(CommonDialog cd_x, CommonDialog cd_y)
	{
		float z = cd_x.gameObject.transform.localPosition.z;
		float z2 = cd_y.gameObject.transform.localPosition.z;
		if (z > z2)
		{
			return -1;
		}
		if (z < z2)
		{
			return 1;
		}
		return 0;
	}

	public static void ShowBarrier()
	{
		GUIManager.ShowGUI("CommonDialogBarrier");
		GUIBase guibase = GUIManager.guiBases["CommonDialogBarrier"];
		Vector3 localPosition = guibase.gameObject.transform.localPosition;
		localPosition.z = GUIManager.DLG_START_Z + GUIManager.DLG_BARRIER_OFS_Z;
		guibase.gameObject.transform.localPosition = localPosition;
	}

	public static void ShowBarrierZset(float z)
	{
		GUIManager.ShowGUI("CommonDialogBarrier");
		GUIBase guibase = GUIManager.guiBases["CommonDialogBarrier"];
		Vector3 localPosition = guibase.gameObject.transform.localPosition;
		localPosition.z = z;
		guibase.gameObject.transform.localPosition = localPosition;
		guibase.gameObject.GetComponent<UIWidget>().depth = -(int)localPosition.z;
	}

	public static void HideBarrier()
	{
		GUIManager.HideGUI("CommonDialogBarrier");
	}

	public static bool someOneTouch
	{
		get
		{
			return GUIManager.someOneTouch_;
		}
		private set
		{
			GUIManager.someOneTouch_ = value;
		}
	}

	public static void ResetTouchingCount()
	{
		GUIManager.touchingCount = 0;
		foreach (Collider key in GUIManager.colliders.Keys)
		{
			if (GUIManager.colliders.ContainsKey(key))
			{
				GUICollider guicollider = (GUICollider)GUIManager.colliders[key];
				if (guicollider.isTouching)
				{
					guicollider.isTouching = false;
				}
			}
		}
	}

	public static bool IsTouching()
	{
		if (GUIManager.touchingCount < 0)
		{
			GUIManager.touchingCount = 0;
		}
		if (GUIManager.touchingCount > 2)
		{
			GUIManager.touchingCount = 2;
		}
		if (GUIManager.touchingCount > 0)
		{
			return true;
		}
		foreach (Collider key in GUIManager.colliders.Keys)
		{
			if (GUIManager.colliders.ContainsKey(key))
			{
				GUICollider guicollider = (GUICollider)GUIManager.colliders[key];
				if (guicollider != null && guicollider.isTouching)
				{
					return true;
				}
			}
		}
		GUIBase guibase = GUIManager.guiBases["CommonDialogBarrier"];
		return guibase.gameObject.activeSelf || GUIManager.touchEndTrg;
	}

	public static int CheckTouching(out string msg)
	{
		msg = string.Empty;
		if (GUIManager.touchingCount > 0)
		{
			return 10000 + GUIManager.touchingCount;
		}
		int num = 0;
		foreach (Collider key in GUIManager.colliders.Keys)
		{
			if (GUIManager.colliders.ContainsKey(key))
			{
				GUICollider guicollider = (GUICollider)GUIManager.colliders[key];
				if (guicollider.isTouching)
				{
					msg = msg + "GCOL TOUCH TRUE NAME =  " + guicollider.gName + "\n";
					num++;
				}
			}
		}
		if (num > 0)
		{
			return 20000 + num;
		}
		GUIBase guibase = GUIManager.guiBases["CommonDialogBarrier"];
		if (guibase.gameObject.activeSelf)
		{
			return 30000;
		}
		if (GUIManager.touchEndTrg)
		{
			return 40001;
		}
		return 40000;
	}

	private void Update()
	{
		GUIManager.touchEndTrg = false;
		GUIManager.barrierReqToFade = false;
		GUIManager.someOneTouch = false;
		GUIManager.ClearColliderTouch();
		if (this.UseOutsideTouchControl)
		{
			return;
		}
		GUIManager._generation++;
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				this.lookAtTouch(Input.GetTouch(i));
			}
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, ITouchEvent> keyValuePair in GUIManager.spriteSelected)
		{
			if (keyValuePair.Value.generation >= GUIManager._generation + 2)
			{
				keyValuePair.Value.OnTouchEnded(default(Touch), new Vector2(0f, 0f), false);
				list.Add(keyValuePair.Key);
			}
		}
		foreach (int key in list)
		{
			GUIManager.spriteSelected.Remove(key);
		}
	}

	public static void RemoveTouch(Touch touch)
	{
		if (GUIManager.spriteSelected.ContainsKey(touch.fingerId))
		{
			GUIManager.spriteSelected.Remove(touch.fingerId);
		}
	}

	public static bool IsExistTouchEvent(ITouchEvent iTouchEvent)
	{
		foreach (Collider key in GUIManager.colliders.Keys)
		{
			if (GUIManager.colliders.ContainsKey(key) && GUIManager.colliders[key] == iTouchEvent)
			{
				return true;
			}
		}
		return false;
	}

	private void lookAtTouch(Touch touch)
	{
		Camera orthoCamera = GUIMain.GetOrthoCamera();
		Vector3 position = Singleton<GUIManager>.Instance.gameObject.transform.position;
		Singleton<GUIManager>.Instance.gameObject.transform.position = Vector3.zero;
		Vector2 touchPosition = new Vector2(touch.position.x, touch.position.y);
		Vector3 position2 = new Vector3(touch.position.x, touch.position.y, 0f);
		Vector3 vector = orthoCamera.ScreenToWorldPoint(position2);
		if (GUIMain.USE_NGUI && Singleton<GUIManager>.Instance.transform.parent != null)
		{
			float x = Singleton<GUIManager>.Instance.transform.parent.localScale.x;
			float y = Singleton<GUIManager>.Instance.transform.parent.localScale.y;
			vector.x /= x;
			vector.y /= y;
		}
		Vector2 pos = new Vector2(vector.x, vector.y);
		GUIManager.selectButtons = GUIManager.getButtonForScreenPositions(touchPosition);
		if (touch.phase == TouchPhase.Began)
		{
			if (GUIManager.selectButtons.Count > 0)
			{
				GUIManager.touchingCount++;
				if (!GUIManager.spriteSelected.ContainsKey(touch.fingerId))
				{
					bool flag = false;
					foreach (int key in GUIManager.spriteSelected.Keys)
					{
						if (GUIManager.spriteSelected[key] == GUIManager.selectButtons[0])
						{
							flag = true;
						}
					}
					if (!flag)
					{
						if (GUIManager.enebleMultiInput || GUIManager.spriteSelected.Count <= 0)
						{
							GUICollider guicollider = (GUICollider)GUIManager.selectButtons[0];
							if (!GUICollider.IsAllColliderDisable() || guicollider.AvoidDisableAllCollider)
							{
								GUIManager.spriteSelected.Add(touch.fingerId, GUIManager.selectButtons[0]);
								GUIManager.spriteSelected[touch.fingerId].generation = GUIManager._generation;
								GUIManager.selectButtons[0].OnTouchBegan(touch, pos);
							}
						}
					}
				}
				if (!GUIManager.selectButtons[0].useSubPhase)
				{
					GUIManager.someOneTouch = true;
				}
			}
			else
			{
				if (GUIManager.spriteSelected.ContainsKey(touch.fingerId))
				{
					GUIManager.spriteSelected[touch.fingerId].OnTouchEnded(touch, pos, false);
					GUIManager.spriteSelected.Remove(touch.fingerId);
				}
				GUIManager.someOneTouch = true;
			}
		}
		else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
		{
			if (GUIManager.spriteSelected.ContainsKey(touch.fingerId) && GUIManager.IsExistTouchEvent(GUIManager.spriteSelected[touch.fingerId]))
			{
				GUIManager.spriteSelected[touch.fingerId].OnTouchMoved(touch, pos);
			}
		}
		else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
		{
			GUIManager.touchingCount--;
			if (GUIManager.touchingCount == 0)
			{
				GUIManager.touchEndTrg = true;
			}
			if (GUIManager.touchingCount < 0)
			{
				GUIManager.touchingCount = 0;
			}
			if (GUIManager.spriteSelected.ContainsKey(touch.fingerId))
			{
				foreach (ITouchEvent touchEvent in GUIManager.selectButtons)
				{
					if (GUIManager.spriteSelected[touch.fingerId] == touchEvent)
					{
						if (touchEvent == GUIManager.selectButtons[0])
						{
							GUIManager.spriteSelected[touch.fingerId].OnTouchEnded(touch, pos, true);
						}
						if (touchEvent.useSubPhase)
						{
							GUIManager.someOneTouch = true;
						}
						GUIManager.spriteSelected.Remove(touch.fingerId);
						return;
					}
				}
				if (GUIManager.IsExistTouchEvent(GUIManager.spriteSelected[touch.fingerId]))
				{
					GUIManager.spriteSelected[touch.fingerId].OnTouchEnded(touch, pos, false);
				}
				GUIManager.spriteSelected.Remove(touch.fingerId);
			}
		}
		Singleton<GUIManager>.Instance.gameObject.transform.position = position;
	}

	public static List<ITouchEvent> getButtonForScreenPositions(Vector2 touchPosition)
	{
		Camera orthoCamera = GUIMain.GetOrthoCamera();
		List<ITouchEvent> list = new List<ITouchEvent>();
		Ray ray = orthoCamera.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0f));
		RaycastHit[] array = Physics.RaycastAll(ray);
		if (array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				ITouchEvent touchEvent;
				if (GUIManager.colliders.TryGetValue(array[i].collider, out touchEvent))
				{
					touchEvent.distance = array[i].distance;
					list.Add(touchEvent);
				}
			}
			list.Sort((ITouchEvent x, ITouchEvent y) => Math.Sign(x.distance - y.distance));
		}
		return list;
	}

	public static GameObject GetParentObject(GameObject go)
	{
		GameObject result = null;
		Transform parent = go.transform.parent;
		while (parent != null)
		{
			GUIScreen component = parent.gameObject.GetComponent<GUIScreen>();
			CommonDialog component2 = parent.gameObject.GetComponent<CommonDialog>();
			if (component != null)
			{
				result = parent.gameObject;
				break;
			}
			if (component2 != null)
			{
				result = parent.gameObject;
				break;
			}
			parent = parent.parent;
		}
		return result;
	}

	public static bool IsEnableBackKeyAndroid()
	{
		return GUIManager.extBackKeyReady && !GUICollider.IsAllColliderDisable() && !GUIMain.IsBarrierON() && GUIManager.GetDialogDic().Count <= 0;
	}

	public static bool ExtBackKeyReady
	{
		get
		{
			return GUIManager.extBackKeyReady;
		}
		set
		{
			GUIManager.extBackKeyReady = value;
		}
	}
}
