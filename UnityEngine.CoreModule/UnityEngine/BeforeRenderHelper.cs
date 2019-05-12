using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine
{
	internal static class BeforeRenderHelper
	{
		private static List<BeforeRenderHelper.OrderBlock> s_OrderBlocks = new List<BeforeRenderHelper.OrderBlock>();

		private static int GetUpdateOrder(UnityAction callback)
		{
			object[] customAttributes = callback.Method.GetCustomAttributes(typeof(BeforeRenderOrderAttribute), true);
			BeforeRenderOrderAttribute beforeRenderOrderAttribute = (customAttributes == null || customAttributes.Length <= 0) ? null : (customAttributes[0] as BeforeRenderOrderAttribute);
			return (beforeRenderOrderAttribute == null) ? 0 : beforeRenderOrderAttribute.order;
		}

		public static void RegisterCallback(UnityAction callback)
		{
			int updateOrder = BeforeRenderHelper.GetUpdateOrder(callback);
			object obj = BeforeRenderHelper.s_OrderBlocks;
			lock (obj)
			{
				int num = 0;
				while (num < BeforeRenderHelper.s_OrderBlocks.Count && BeforeRenderHelper.s_OrderBlocks[num].order <= updateOrder)
				{
					if (BeforeRenderHelper.s_OrderBlocks[num].order == updateOrder)
					{
						BeforeRenderHelper.OrderBlock value = BeforeRenderHelper.s_OrderBlocks[num];
						value.callback = (UnityAction)Delegate.Combine(value.callback, callback);
						BeforeRenderHelper.s_OrderBlocks[num] = value;
						return;
					}
					num++;
				}
				BeforeRenderHelper.OrderBlock item = default(BeforeRenderHelper.OrderBlock);
				item.order = updateOrder;
				item.callback = (UnityAction)Delegate.Combine(item.callback, callback);
				BeforeRenderHelper.s_OrderBlocks.Insert(num, item);
			}
		}

		public static void UnregisterCallback(UnityAction callback)
		{
			int updateOrder = BeforeRenderHelper.GetUpdateOrder(callback);
			object obj = BeforeRenderHelper.s_OrderBlocks;
			lock (obj)
			{
				int num = 0;
				while (num < BeforeRenderHelper.s_OrderBlocks.Count && BeforeRenderHelper.s_OrderBlocks[num].order <= updateOrder)
				{
					if (BeforeRenderHelper.s_OrderBlocks[num].order == updateOrder)
					{
						BeforeRenderHelper.OrderBlock value = BeforeRenderHelper.s_OrderBlocks[num];
						value.callback = (UnityAction)Delegate.Remove(value.callback, callback);
						BeforeRenderHelper.s_OrderBlocks[num] = value;
						if (value.callback == null)
						{
							BeforeRenderHelper.s_OrderBlocks.RemoveAt(num);
						}
						break;
					}
					num++;
				}
			}
		}

		public static void Invoke()
		{
			object obj = BeforeRenderHelper.s_OrderBlocks;
			lock (obj)
			{
				for (int i = 0; i < BeforeRenderHelper.s_OrderBlocks.Count; i++)
				{
					UnityAction callback = BeforeRenderHelper.s_OrderBlocks[i].callback;
					if (callback != null)
					{
						callback();
					}
				}
			}
		}

		private struct OrderBlock
		{
			internal int order;

			internal UnityAction callback;
		}
	}
}
