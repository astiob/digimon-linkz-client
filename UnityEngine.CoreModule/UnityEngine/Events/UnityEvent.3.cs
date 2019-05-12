using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine.Events
{
	[Serializable]
	public abstract class UnityEvent<T0, T1> : UnityEventBase
	{
		private object[] m_InvokeArray = null;

		[RequiredByNativeCode]
		public UnityEvent()
		{
		}

		public void AddListener(UnityAction<T0, T1> call)
		{
			base.AddCall(UnityEvent<T0, T1>.GetDelegate(call));
		}

		public void RemoveListener(UnityAction<T0, T1> call)
		{
			base.RemoveListener(call.Target, call.GetMethodInfo());
		}

		protected override MethodInfo FindMethod_Impl(string name, object targetObj)
		{
			return UnityEventBase.GetValidMethodInfo(targetObj, name, new Type[]
			{
				typeof(T0),
				typeof(T1)
			});
		}

		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall<T0, T1>(target, theFunction);
		}

		private static BaseInvokableCall GetDelegate(UnityAction<T0, T1> action)
		{
			return new InvokableCall<T0, T1>(action);
		}

		public void Invoke(T0 arg0, T1 arg1)
		{
			List<BaseInvokableCall> list = base.PrepareInvoke();
			for (int i = 0; i < list.Count; i++)
			{
				InvokableCall<T0, T1> invokableCall = list[i] as InvokableCall<T0, T1>;
				if (invokableCall != null)
				{
					invokableCall.Invoke(arg0, arg1);
				}
				else
				{
					InvokableCall invokableCall2 = list[i] as InvokableCall;
					if (invokableCall2 != null)
					{
						invokableCall2.Invoke();
					}
					else
					{
						BaseInvokableCall baseInvokableCall = list[i];
						if (this.m_InvokeArray == null)
						{
							this.m_InvokeArray = new object[2];
						}
						this.m_InvokeArray[0] = arg0;
						this.m_InvokeArray[1] = arg1;
						baseInvokableCall.Invoke(this.m_InvokeArray);
					}
				}
			}
		}
	}
}
