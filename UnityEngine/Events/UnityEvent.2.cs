using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
	/// <summary>
	///   <para>One argument version of UnityEvent.</para>
	/// </summary>
	[Serializable]
	public abstract class UnityEvent<T0> : UnityEventBase
	{
		private readonly object[] m_InvokeArray = new object[1];

		public void AddListener(UnityAction<T0> call)
		{
			base.AddCall(UnityEvent<T0>.GetDelegate(call));
		}

		public void RemoveListener(UnityAction<T0> call)
		{
			base.RemoveListener(call.Target, call.GetMethodInfo());
		}

		protected override MethodInfo FindMethod_Impl(string name, object targetObj)
		{
			return UnityEventBase.GetValidMethodInfo(targetObj, name, new Type[]
			{
				typeof(T0)
			});
		}

		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall<T0>(target, theFunction);
		}

		private static BaseInvokableCall GetDelegate(UnityAction<T0> action)
		{
			return new InvokableCall<T0>(action);
		}

		public void Invoke(T0 arg0)
		{
			this.m_InvokeArray[0] = arg0;
			base.Invoke(this.m_InvokeArray);
		}
	}
}
