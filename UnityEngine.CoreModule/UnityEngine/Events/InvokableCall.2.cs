using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
	internal class InvokableCall<T1> : BaseInvokableCall
	{
		public InvokableCall(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			this.Delegate += (UnityAction<T1>)theFunction.CreateDelegate(typeof(UnityAction<T1>), target);
		}

		public InvokableCall(UnityAction<T1> action)
		{
			this.Delegate += action;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected event UnityAction<T1> Delegate;

		public override void Invoke(object[] args)
		{
			if (args.Length != 1)
			{
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
			}
			BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
			if (BaseInvokableCall.AllowInvoke(this.Delegate))
			{
				this.Delegate((T1)((object)args[0]));
			}
		}

		public virtual void Invoke(T1 args0)
		{
			if (BaseInvokableCall.AllowInvoke(this.Delegate))
			{
				this.Delegate(args0);
			}
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return this.Delegate.Target == targetObj && this.Delegate.GetMethodInfo().Equals(method);
		}
	}
}
