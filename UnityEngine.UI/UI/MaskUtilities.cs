using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	public class MaskUtilities
	{
		public static void Notify2DMaskStateChanged(Component mask)
		{
			List<Component> list = ListPool<Component>.Get();
			mask.GetComponentsInChildren<Component>(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (!(list[i] == null) && !(list[i].gameObject == mask.gameObject))
				{
					IClippable clippable = list[i] as IClippable;
					if (clippable != null)
					{
						clippable.RecalculateClipping();
					}
				}
			}
			ListPool<Component>.Release(list);
		}

		public static void NotifyStencilStateChanged(Component mask)
		{
			List<Component> list = ListPool<Component>.Get();
			mask.GetComponentsInChildren<Component>(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (!(list[i] == null) && !(list[i].gameObject == mask.gameObject))
				{
					IMaskable maskable = list[i] as IMaskable;
					if (maskable != null)
					{
						maskable.RecalculateMasking();
					}
				}
			}
			ListPool<Component>.Release(list);
		}

		public static Transform FindRootSortOverrideCanvas(Transform start)
		{
			Transform transform = start;
			Transform result = null;
			while (transform != null)
			{
				Canvas component = transform.GetComponent<Canvas>();
				if (component != null && component.overrideSorting)
				{
					return transform;
				}
				if (component != null)
				{
					result = transform;
				}
				transform = transform.parent;
			}
			return result;
		}

		public static int GetStencilDepth(Transform transform, Transform stopAfter)
		{
			int num = 0;
			if (transform == stopAfter)
			{
				return num;
			}
			Transform parent = transform.parent;
			List<Component> list = ListPool<Component>.Get();
			while (parent != null)
			{
				parent.GetComponents(typeof(Mask), list);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != null && ((Mask)list[i]).IsActive() && ((Mask)list[i]).graphic.IsActive())
					{
						num++;
						break;
					}
				}
				if (parent == stopAfter)
				{
					break;
				}
				parent = parent.parent;
			}
			ListPool<Component>.Release(list);
			return num;
		}

		public static RectMask2D GetRectMaskForClippable(IClippable transform)
		{
			Transform parent = transform.rectTransform.parent;
			List<Component> list = ListPool<Component>.Get();
			while (parent != null)
			{
				parent.GetComponents(typeof(RectMask2D), list);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != null && ((RectMask2D)list[i]).IsActive())
					{
						RectMask2D result = (RectMask2D)list[i];
						ListPool<Component>.Release(list);
						return result;
					}
				}
				Canvas component = parent.GetComponent<Canvas>();
				if (component)
				{
					break;
				}
				parent = parent.parent;
			}
			ListPool<Component>.Release(list);
			return null;
		}

		public static void GetRectMasksForClip(RectMask2D clipper, List<RectMask2D> masks)
		{
			masks.Clear();
			Transform transform = clipper.transform;
			List<Component> list = ListPool<Component>.Get();
			while (transform != null)
			{
				transform.GetComponents(typeof(RectMask2D), list);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != null && ((RectMask2D)list[i]).IsActive())
					{
						masks.Add((RectMask2D)list[i]);
					}
				}
				Canvas component = transform.GetComponent<Canvas>();
				if (component)
				{
					break;
				}
				transform = transform.parent;
			}
			ListPool<Component>.Release(list);
		}
	}
}
