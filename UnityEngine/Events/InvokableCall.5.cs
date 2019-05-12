using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
	internal class InvokableCall<T1, T2, T3, T4> : BaseInvokableCall
	{
		public InvokableCall(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			this.Delegate = (UnityAction<T1, T2, T3, T4>)theFunction.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4>), target);
		}

		public InvokableCall(UnityAction<T1, T2, T3, T4> action)
		{
			this.Delegate = (UnityAction<T1, T2, T3, T4>)System.Delegate.Combine(this.Delegate, action);
		}

		protected event UnityAction<T1, T2, T3, T4> Delegate;

		public override void Invoke(object[] args)
		{
			if (args.Length != 4)
			{
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
			}
			BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
			BaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
			BaseInvokableCall.ThrowOnInvalidArg<T3>(args[2]);
			BaseInvokableCall.ThrowOnInvalidArg<T4>(args[3]);
			if (BaseInvokableCall.AllowInvoke(this.Delegate))
			{
				this.Delegate((T1)((object)args[0]), (T2)((object)args[1]), (T3)((object)args[2]), (T4)((object)args[3]));
			}
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return this.Delegate.Target == targetObj && this.Delegate.GetMethodInfo() == method;
		}
	}
}
