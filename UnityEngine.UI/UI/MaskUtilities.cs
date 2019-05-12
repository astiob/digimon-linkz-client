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
			List<Canvas> list = ListPool<Canvas>.Get();
			start.GetComponentsInParent<Canvas>(false, list);
			Canvas canvas = null;
			for (int i = 0; i < list.Count; i++)
			{
				canvas = list[i];
				if (canvas.overrideSorting)
				{
					break;
				}
			}
			ListPool<Canvas>.Release(list);
			return (!(canvas != null)) ? null : canvas.transform;
		}

		public static int GetStencilDepth(Transform transform, Transform stopAfter)
		{
			int num = 0;
			if (transform == stopAfter)
			{
				return num;
			}
			Transform parent = transform.parent;
			List<Mask> list = ListPool<Mask>.Get();
			while (parent != null)
			{
				parent.GetComponents<Mask>(list);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != null && list[i].IsActive() && list[i].graphic != null && list[i].graphic.IsActive())
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
			ListPool<Mask>.Release(list);
			return num;
		}

		public static RectMask2D GetRectMaskForClippable(IClippable transform)
		{
			List<RectMask2D> list = ListPool<RectMask2D>.Get();
			RectMask2D result = null;
			transform.rectTransform.GetComponentsInParent<RectMask2D>(false, list);
			if (list.Count > 0)
			{
				result = list[0];
			}
			ListPool<RectMask2D>.Release(list);
			return result;
		}

		public static void GetRectMasksForClip(RectMask2D clipper, List<RectMask2D> masks)
		{
			masks.Clear();
			clipper.transform.GetComponentsInParent<RectMask2D>(false, masks);
		}
	}
}
