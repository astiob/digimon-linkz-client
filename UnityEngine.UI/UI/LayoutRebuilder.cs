using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI
{
	public class LayoutRebuilder : ICanvasElement
	{
		private RectTransform m_ToRebuild;

		private int m_CachedHashFromTransform;

		private static ObjectPool<LayoutRebuilder> s_Rebuilders = new ObjectPool<LayoutRebuilder>(null, delegate(LayoutRebuilder x)
		{
			x.Clear();
		});

		static LayoutRebuilder()
		{
			RectTransform.reapplyDrivenProperties += LayoutRebuilder.ReapplyDrivenProperties;
		}

		private void Initialize(RectTransform controller)
		{
			this.m_ToRebuild = controller;
			this.m_CachedHashFromTransform = controller.GetHashCode();
		}

		private void Clear()
		{
			this.m_ToRebuild = null;
			this.m_CachedHashFromTransform = 0;
		}

		private static void ReapplyDrivenProperties(RectTransform driven)
		{
			LayoutRebuilder.MarkLayoutForRebuild(driven);
		}

		public Transform transform
		{
			get
			{
				return this.m_ToRebuild;
			}
		}

		public bool IsDestroyed()
		{
			return this.m_ToRebuild == null;
		}

		private static void StripDisabledBehavioursFromList(List<Component> components)
		{
			components.RemoveAll((Component e) => e is Behaviour && !((Behaviour)e).isActiveAndEnabled);
		}

		public static void ForceRebuildLayoutImmediate(RectTransform layoutRoot)
		{
			LayoutRebuilder layoutRebuilder = LayoutRebuilder.s_Rebuilders.Get();
			layoutRebuilder.Initialize(layoutRoot);
			layoutRebuilder.Rebuild(CanvasUpdate.Layout);
			LayoutRebuilder.s_Rebuilders.Release(layoutRebuilder);
		}

		public void Rebuild(CanvasUpdate executing)
		{
			if (executing == CanvasUpdate.Layout)
			{
				this.PerformLayoutCalculation(this.m_ToRebuild, delegate(Component e)
				{
					(e as ILayoutElement).CalculateLayoutInputHorizontal();
				});
				this.PerformLayoutControl(this.m_ToRebuild, delegate(Component e)
				{
					(e as ILayoutController).SetLayoutHorizontal();
				});
				this.PerformLayoutCalculation(this.m_ToRebuild, delegate(Component e)
				{
					(e as ILayoutElement).CalculateLayoutInputVertical();
				});
				this.PerformLayoutControl(this.m_ToRebuild, delegate(Component e)
				{
					(e as ILayoutController).SetLayoutVertical();
				});
			}
		}

		private void PerformLayoutControl(RectTransform rect, UnityAction<Component> action)
		{
			if (rect == null)
			{
				return;
			}
			List<Component> list = ListPool<Component>.Get();
			rect.GetComponents(typeof(ILayoutController), list);
			LayoutRebuilder.StripDisabledBehavioursFromList(list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] is ILayoutSelfController)
					{
						action(list[i]);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (!(list[j] is ILayoutSelfController))
					{
						action(list[j]);
					}
				}
				for (int k = 0; k < rect.childCount; k++)
				{
					this.PerformLayoutControl(rect.GetChild(k) as RectTransform, action);
				}
			}
			ListPool<Component>.Release(list);
		}

		private void PerformLayoutCalculation(RectTransform rect, UnityAction<Component> action)
		{
			if (rect == null)
			{
				return;
			}
			List<Component> list = ListPool<Component>.Get();
			rect.GetComponents(typeof(ILayoutElement), list);
			LayoutRebuilder.StripDisabledBehavioursFromList(list);
			if (list.Count > 0)
			{
				for (int i = 0; i < rect.childCount; i++)
				{
					this.PerformLayoutCalculation(rect.GetChild(i) as RectTransform, action);
				}
				for (int j = 0; j < list.Count; j++)
				{
					action(list[j]);
				}
			}
			ListPool<Component>.Release(list);
		}

		public static void MarkLayoutForRebuild(RectTransform rect)
		{
			if (rect == null)
			{
				return;
			}
			RectTransform rectTransform = rect;
			for (;;)
			{
				RectTransform rectTransform2 = rectTransform.parent as RectTransform;
				if (!LayoutRebuilder.ValidLayoutGroup(rectTransform2))
				{
					break;
				}
				rectTransform = rectTransform2;
			}
			if (rectTransform == rect && !LayoutRebuilder.ValidController(rectTransform))
			{
				return;
			}
			LayoutRebuilder.MarkLayoutRootForRebuild(rectTransform);
		}

		private static bool ValidLayoutGroup(RectTransform parent)
		{
			if (parent == null)
			{
				return false;
			}
			List<Component> list = ListPool<Component>.Get();
			parent.GetComponents(typeof(ILayoutGroup), list);
			LayoutRebuilder.StripDisabledBehavioursFromList(list);
			bool result = list.Count > 0;
			ListPool<Component>.Release(list);
			return result;
		}

		private static bool ValidController(RectTransform layoutRoot)
		{
			if (layoutRoot == null)
			{
				return false;
			}
			List<Component> list = ListPool<Component>.Get();
			layoutRoot.GetComponents(typeof(ILayoutController), list);
			LayoutRebuilder.StripDisabledBehavioursFromList(list);
			bool result = list.Count > 0;
			ListPool<Component>.Release(list);
			return result;
		}

		private static void MarkLayoutRootForRebuild(RectTransform controller)
		{
			if (controller == null)
			{
				return;
			}
			LayoutRebuilder layoutRebuilder = LayoutRebuilder.s_Rebuilders.Get();
			layoutRebuilder.Initialize(controller);
			if (!CanvasUpdateRegistry.TryRegisterCanvasElementForLayoutRebuild(layoutRebuilder))
			{
				LayoutRebuilder.s_Rebuilders.Release(layoutRebuilder);
			}
		}

		public void LayoutComplete()
		{
			LayoutRebuilder.s_Rebuilders.Release(this);
		}

		public void GraphicUpdateComplete()
		{
		}

		public override int GetHashCode()
		{
			return this.m_CachedHashFromTransform;
		}

		public override bool Equals(object obj)
		{
			return obj.GetHashCode() == this.GetHashCode();
		}

		public override string ToString()
		{
			return "(Layout Rebuilder for) " + this.m_ToRebuild;
		}
	}
}
