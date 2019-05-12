using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
	[AddComponentMenu("Event/Event System")]
	public class EventSystem : UIBehaviour
	{
		private List<BaseInputModule> m_SystemInputModules = new List<BaseInputModule>();

		private BaseInputModule m_CurrentInputModule;

		[SerializeField]
		[FormerlySerializedAs("m_Selected")]
		private GameObject m_FirstSelected;

		[SerializeField]
		private bool m_sendNavigationEvents = true;

		[SerializeField]
		private int m_DragThreshold = 5;

		private GameObject m_CurrentSelected;

		private bool m_SelectionGuard;

		private BaseEventData m_DummyData;

		private static readonly Comparison<RaycastResult> s_RaycastComparer = new Comparison<RaycastResult>(EventSystem.RaycastComparer);

		protected EventSystem()
		{
		}

		public static EventSystem current { get; set; }

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
				return;
			}
			this.m_SelectionGuard = true;
			if (selected == this.m_CurrentSelected)
			{
				this.m_SelectionGuard = false;
				return;
			}
			ExecuteEvents.Execute<IDeselectHandler>(this.m_CurrentSelected, pointer, ExecuteEvents.deselectHandler);
			this.m_CurrentSelected = selected;
			ExecuteEvents.Execute<ISelectHandler>(this.m_CurrentSelected, pointer, ExecuteEvents.selectHandler);
			this.m_SelectionGuard = false;
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
				if (lhs.module.eventCamera != null && rhs.module.eventCamera != null && lhs.module.eventCamera.depth != rhs.module.eventCamera.depth)
				{
					if (lhs.module.eventCamera.depth < rhs.module.eventCamera.depth)
					{
						return 1;
					}
					if (lhs.module.eventCamera.depth == rhs.module.eventCamera.depth)
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
			if (lhs.sortingLayer != rhs.sortingLayer)
			{
				int layerValueFromID = SortingLayer.GetLayerValueFromID(rhs.sortingLayer);
				int layerValueFromID2 = SortingLayer.GetLayerValueFromID(lhs.sortingLayer);
				return layerValueFromID.CompareTo(layerValueFromID2);
			}
			if (lhs.sortingOrder != rhs.sortingOrder)
			{
				return rhs.sortingOrder.CompareTo(lhs.sortingOrder);
			}
			if (lhs.depth != rhs.depth)
			{
				return rhs.depth.CompareTo(lhs.depth);
			}
			if (lhs.distance != rhs.distance)
			{
				return lhs.distance.CompareTo(rhs.distance);
			}
			return lhs.index.CompareTo(rhs.index);
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
			if (EventSystem.current == null)
			{
				EventSystem.current = this;
			}
		}

		protected override void OnDisable()
		{
			if (this.m_CurrentInputModule != null)
			{
				this.m_CurrentInputModule.DeactivateModule();
				this.m_CurrentInputModule = null;
			}
			if (EventSystem.current == this)
			{
				EventSystem.current = null;
			}
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

		protected virtual void Update()
		{
			if (EventSystem.current != this)
			{
				return;
			}
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

		private void ChangeEventModule(BaseInputModule module)
		{
			if (this.m_CurrentInputModule == module)
			{
				return;
			}
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

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Selected:</b>" + this.currentSelectedGameObject);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine((!(this.m_CurrentInputModule != null)) ? "No module" : this.m_CurrentInputModule.ToString());
			return stringBuilder.ToString();
		}
	}
}
