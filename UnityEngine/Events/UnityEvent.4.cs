using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
	/// <summary>
	///   <para>Three argument version of UnityEvent.</para>
	/// </summary>
	[Serializable]
	public abstract class UnityEvent<T0, T1, T2> : UnityEventBase
	{
		private readonly object[] m_InvokeArray = new object[3];

		public void AddListener(UnityAction<T0, T1, T2> call)
		{
			base.AddCall(UnityEvent<T0, T1, T2>.GetDelegate(call));
		}

		public void RemoveListener(UnityAction<T0, T1, T2> call)
		{
			base.RemoveListener(call.Target, call.GetMethodInfo());
		}

		protected override MethodInfo FindMethod_Impl(string name, object targetObj)
		{
			return UnityEventBase.GetValidMethodInfo(targetObj, name, new Type[]
			{
				typeof(T0),
				typeof(T1),
				typeof(T2)
			});
		}

		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall<T0, T1, T2>(target, theFunction);
		}

		private static BaseInvokableCall GetDelegate(UnityAction<T0, T1, T2> action)
		{
			return new InvokableCall<T0, T1, T2>(action);
		}

		public void Invoke(T0 arg0, T1 arg1, T2 arg2)
		{
			this.m_InvokeArray[0] = arg0;
			this.m_InvokeArray[1] = arg1;
			this.m_InvokeArray[2] = arg2;
			base.Invoke(this.m_InvokeArray);
		}
	}
}
