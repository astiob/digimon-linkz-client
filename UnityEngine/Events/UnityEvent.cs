using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
	/// <summary>
	///   <para>A zero argument persistent callback that can be saved with the scene.</para>
	/// </summary>
	[Serializable]
	public class UnityEvent : UnityEventBase
	{
		private readonly object[] m_InvokeArray = new object[0];

		/// <summary>
		///   <para>Add a non persistent listener to the UnityEvent.</para>
		/// </summary>
		/// <param name="call">Callback function.</param>
		public void AddListener(UnityAction call)
		{
			base.AddCall(UnityEvent.GetDelegate(call));
		}

		/// <summary>
		///   <para>Remove a non persistent listener from the UnityEvent.</para>
		/// </summary>
		/// <param name="call">Callback function.</param>
		public void RemoveListener(UnityAction call)
		{
			base.RemoveListener(call.Target, call.GetMethodInfo());
		}

		protected override MethodInfo FindMethod_Impl(string name, object targetObj)
		{
			return UnityEventBase.GetValidMethodInfo(targetObj, name, new Type[0]);
		}

		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall(target, theFunction);
		}

		private static BaseInvokableCall GetDelegate(UnityAction action)
		{
			return new InvokableCall(action);
		}

		/// <summary>
		///   <para>Invoke all registered callbacks (runtime and peristent).</para>
		/// </summary>
		public void Invoke()
		{
			base.Invoke(this.m_InvokeArray);
		}
	}
}
