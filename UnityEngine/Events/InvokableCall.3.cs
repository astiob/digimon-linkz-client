using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
	internal class InvokableCall<T1, T2> : BaseInvokableCall
	{
		public InvokableCall(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			this.Delegate = (UnityAction<T1, T2>)theFunction.CreateDelegate(typeof(UnityAction<T1, T2>), target);
		}

		public InvokableCall(UnityAction<T1, T2> action)
		{
			this.Delegate = (UnityAction<T1, T2>)System.Delegate.Combine(this.Delegate, action);
		}

		protected event UnityAction<T1, T2> Delegate;

		public override void Invoke(object[] args)
		{
			if (args.Length != 2)
			{
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
			}
			BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
			BaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
			if (BaseInvokableCall.AllowInvoke(this.Delegate))
			{
				this.Delegate((T1)((object)args[0]), (T2)((object)args[1]));
			}
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return this.Delegate.Target == targetObj && this.Delegate.GetMethodInfo() == method;
		}
	}
}
