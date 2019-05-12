using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
	[AddComponentMenu("Event/Event System")]
	public class EventSystem : UIBehaviour
	{
		private List<BaseInputModule> m_SystemInputModules = new List<BaseInputModule>();

		private BaseInputModule m_CurrentInputModule;

		private static List<EventSystem> m_EventSystems = new List<EventSystem>();

		[SerializeField]
		[FormerlySerializedAs("m_Selected")]
		private GameObject m_FirstSelected;

		[SerializeField]
		private bool m_sendNavigationEvents = true;

		[SerializeField]
		private int m_DragThreshold = 5;

		private GameObject m_CurrentSelected;

		private bool m_HasFocus = true;

		private bool m_SelectionGuard;

		private BaseEventData m_DummyData;

		private static readonly Comparison<RaycastResult> s_RaycastComparer;

		[CompilerGenerated]
		private static Comparison<RaycastResult> <>f__mg$cache0;

		protected EventSystem()
		{
		}

		public static EventSystem current
		{
			get
			{
				return (EventSystem.m_EventSystems.Count <= 0) ? null : EventSystem.m_EventSystems[0];
			}
			set
			{
				int num = EventSystem.m_EventSystems.IndexOf(value);
				if (num >= 0)
				{
					EventSystem.m_EventSystems.RemoveAt(num);
					EventSystem.m_EventSystems.Insert(0, value);
				}
			}
		}

		public bool sendNavigationEvents
		{
			get
			{
				return this.m_sendNavigationEvents;
			}
			set
			{
				this.m_sendNavigationEvents = value;
			}
		}

		public int pixelDragThreshold
		{
			get
			{
				return this.m_DragThreshold;
			}
			set
			{
				this.m_DragThreshold = value;
			}
		}

		public BaseInputModule currentInputModule
		{
			get
			{
				return this.m_CurrentInputModule;
			}
		}

		public GameObject firstSelectedGameObject
		{
			get
			{
				return this.m_FirstSelected;
			}
			set
			{
				this.m_FirstSelected = value;
			}
		}

		public GameObject currentSelectedGameObject
		{
			get
			{
				return this.m_CurrentSelected;
			}
		}

		[Obsolete("lastSelectedGameObject is no longer supported")]
		public GameObject lastSelectedGameObject
		{
			get
			{
				return null;
			}
		}

		public bool isFocused
		{
			get
			{
				return this.m_HasFocus;
			}
		}

		public void UpdateModules()
		{
			base.GetComponents<BaseInputModule>(this.m_SystemInputModules);
			for (int i = this.m_SystemInputModules.Count - 1; i >= 0; i--)
			{
				if (!this.m_SystemInputModules[i] || !this.m_SystemInputModules[i].IsActive())
				{
					this.m_SystemInputModules.RemoveAt(i);
				}
			}
		}

		public bool alreadySelecting
		{
			get
			{
				return this.m_SelectionGuard;
			}
		}

		public void SetSelectedGameObject(GameObject selected, BaseEventData pointer)
		{
			if (this.m_SelectionGuard)
			{
				Debug.LogError("Attempting to select " + selected + "while already selecting an object.");
			}
			else
			{
				this.m_SelectionGuard = true;
				if (selected == this.m_CurrentSelected)
				{
					this.m_SelectionGuard = false;
				}
				else
				{
					ExecuteEvents.Execute<IDeselectHandler>(this.m_CurrentSelected, pointer, ExecuteEvents.deselectHandler);
					this.m_CurrentSelected = selected;
					ExecuteEvents.Execute<ISelectHandler>(this.m_CurrentSelected, pointer, ExecuteEvents.selectHandler);
					this.m_SelectionGuard = false;
				}
			}
		}

		private BaseEventData baseEventDataCache
		{
			get
			{
				if (this.m_DummyData == null)
				{
					this.m_DummyData = new BaseEventData(this);
				}
				return this.m_DummyData;
			}
		}

		public void SetSelectedGameObject(GameObject selected)
		{
			this.SetSelectedGameObject(selected, this.baseEventDataCache);
		}

		private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
		{
			if (lhs.module != rhs.module)
			{
				Camera eventCamera = lhs.module.eventCamera;
				Camera eventCamera2 = rhs.module.eventCamera;
				if (eventCamera != null && eventCamera2 != null && eventCamera.depth != eventCamera2.depth)
				{
					if (eventCamera.depth < eventCamera2.depth)
					{
						return 1;
					}
					if (eventCamera.depth == eventCamera2.depth)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
					{
						return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);
					}
					if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
					{
						return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
					}
				}
			}
			int result;
			if (lhs.sortingLayer != rhs.sortingLayer)
			{
				int layerValueFromID = SortingLayer.GetLayerValueFromID(rhs.sortingLayer);
				int layerValueFromID2 = SortingLayer.GetLayerValueFromID(lhs.sortingLayer);
				result = layerValueFromID.CompareTo(layerValueFromID2);
			}
			else if (lhs.sortingOrder != rhs.sortingOrder)
			{
				result = rhs.sortingOrder.CompareTo(lhs.sortingOrder);
			}
			else if (lhs.depth != rhs.depth)
			{
				result = rhs.depth.CompareTo(lhs.depth);
			}
			else if (lhs.distance != rhs.distance)
			{
				result = lhs.distance.CompareTo(rhs.distance);
			}
			else
			{
				result = lhs.index.CompareTo(rhs.index);
			}
			return result;
		}

		public void RaycastAll(PointerEventData eventData, List<RaycastResult> raycastResults)
		{
			raycastResults.Clear();
			List<BaseRaycaster> raycasters = RaycasterManager.GetRaycasters();
			for (int i = 0; i < raycasters.Count; i++)
			{
				BaseRaycaster baseRaycaster = raycasters[i];
				if (!(baseRaycaster == null) && baseRaycaster.IsActive())
				{
					baseRaycaster.Raycast(eventData, raycastResults);
				}
			}
			raycastResults.Sort(EventSystem.s_RaycastComparer);
		}

		public bool IsPointerOverGameObject()
		{
			return this.IsPointerOverGameObject(-1);
		}

		public bool IsPointerOverGameObject(int pointerId)
		{
			return !(this.m_CurrentInputModule == null) && this.m_CurrentInputModule.IsPointerOverGameObject(pointerId);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			EventSystem.m_EventSystems.Add(this);
		}

		protected override void OnDisable()
		{
			if (this.m_CurrentInputModule != null)
			{
				this.m_CurrentInputModule.DeactivateModule();
				this.m_CurrentInputModule = null;
			}
			EventSystem.m_EventSystems.Remove(this);
			base.OnDisable();
		}

		private void TickModules()
		{
			for (int i = 0; i < this.m_SystemInputModules.Count; i++)
			{
				if (this.m_SystemInputModules[i] != null)
				{
					this.m_SystemInputModules[i].UpdateModule();
				}
			}
		}

		protected virtual void OnApplicationFocus(bool hasFocus)
		{
			this.m_HasFocus = hasFocus;
		}

		protected virtual void Update()
		{
			if (!(EventSystem.current != this))
			{
				this.TickModules();
				bool flag = false;
				for (int i = 0; i < this.m_SystemInputModules.Count; i++)
				{
					BaseInputModule baseInputModule = this.m_SystemInputModules[i];
					if (baseInputModule.IsModuleSupported() && baseInputModule.ShouldActivateModule())
					{
						if (this.m_CurrentInputModule != baseInputModule)
						{
							this.ChangeEventModule(baseInputModule);
							flag = true;
						}
						break;
					}
				}
				if (this.m_CurrentInputModule == null)
				{
					for (int j = 0; j < this.m_SystemInputModules.Count; j++)
					{
						BaseInputModule baseInputModule2 = this.m_SystemInputModules[j];
						if (baseInputModule2.IsModuleSupported())
						{
							this.ChangeEventModule(baseInputModule2);
							flag = true;
							break;
						}
					}
				}
				if (!flag && this.m_CurrentInputModule != null)
				{
					this.m_CurrentInputModule.Process();
				}
			}
		}

		private void ChangeEventModule(BaseInputModule module)
		{
			if (!(this.m_CurrentInputModule == module))
			{
				if (this.m_CurrentInputModule != null)
				{
					this.m_CurrentInputModule.DeactivateModule();
				}
				if (module != null)
				{
					module.ActivateModule();
				}
				this.m_CurrentInputModule = module;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Selected:</b>" + this.currentSelectedGameObject);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine((!(this.m_CurrentInputModule != null)) ? "No module" : this.m_CurrentInputModule.ToString());
			return stringBuilder.ToString();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static EventSystem()
		{
			if (EventSystem.<>f__mg$cache0 == null)
			{
				EventSystem.<>f__mg$cache0 = new Comparison<RaycastResult>(EventSystem.RaycastComparer);
			}
			EventSystem.s_RaycastComparer = EventSystem.<>f__mg$cache0;
		}
	}
}
