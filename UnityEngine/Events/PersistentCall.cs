using System;
using System.Reflection;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	[Serializable]
	internal class PersistentCall
	{
		[SerializeField]
		[FormerlySerializedAs("instance")]
		private Object m_Target;

		[SerializeField]
		[FormerlySerializedAs("methodName")]
		private string m_MethodName;

		[SerializeField]
		[FormerlySerializedAs("mode")]
		private PersistentListenerMode m_Mode;

		[FormerlySerializedAs("arguments")]
		[SerializeField]
		private ArgumentCache m_Arguments = new ArgumentCache();

		[FormerlySerializedAs("enabled")]
		[SerializeField]
		[FormerlySerializedAs("m_Enabled")]
		private UnityEventCallState m_CallState = UnityEventCallState.RuntimeOnly;

		public Object target
		{
			get
			{
				return this.m_Target;
			}
		}

		public string methodName
		{
			get
			{
				return this.m_MethodName;
			}
		}

		public PersistentListenerMode mode
		{
			get
			{
				return this.m_Mode;
			}
			set
			{
				this.m_Mode = value;
			}
		}

		public ArgumentCache arguments
		{
			get
			{
				return this.m_Arguments;
			}
		}

		public UnityEventCallState callState
		{
			get
			{
				return this.m_CallState;
			}
			set
			{
				this.m_CallState = value;
			}
		}

		public bool IsValid()
		{
			return this.target != null && !string.IsNullOrEmpty(this.methodName);
		}

		public BaseInvokableCall GetRuntimeCall(UnityEventBase theEvent)
		{
			if (this.m_CallState == UnityEventCallState.Off || theEvent == null)
			{
				return null;
			}
			MethodInfo methodInfo = theEvent.FindMethod(this);
			if (methodInfo == null)
			{
				return null;
			}
			switch (this.m_Mode)
			{
			case PersistentListenerMode.EventDefined:
				return theEvent.GetDelegate(this.target, methodInfo);
			case PersistentListenerMode.Void:
				return new InvokableCall(this.target, methodInfo);
			case PersistentListenerMode.Object:
				return PersistentCall.GetObjectCall(this.target, methodInfo, this.m_Arguments);
			case PersistentListenerMode.Int:
				return new CachedInvokableCall<int>(this.target, methodInfo, this.m_Arguments.intArgument);
			case PersistentListenerMode.Float:
				return new CachedInvokableCall<float>(this.target, methodInfo, this.m_Arguments.floatArgument);
			case PersistentListenerMode.String:
				return new CachedInvokableCall<string>(this.target, methodInfo, this.m_Arguments.stringArgument);
			case PersistentListenerMode.Bool:
				return new CachedInvokableCall<bool>(this.target, methodInfo, this.m_Arguments.boolArgument);
			default:
				return null;
			}
		}

		private static BaseInvokableCall GetObjectCall(Object target, MethodInfo method, ArgumentCache arguments)
		{
			Type type = typeof(Object);
			if (!string.IsNullOrEmpty(arguments.unityObjectArgumentAssemblyTypeName))
			{
				type = (Type.GetType(arguments.unityObjectArgumentAssemblyTypeName, false) ?? typeof(Object));
			}
			Type typeFromHandle = typeof(CachedInvokableCall<>);
			Type type2 = typeFromHandle.MakeGenericType(new Type[]
			{
				type
			});
			ConstructorInfo constructor = type2.GetConstructor(new Type[]
			{
				typeof(Object),
				typeof(MethodInfo),
				type
			});
			Object @object = arguments.unityObjectArgument;
			if (@object != null && !type.IsAssignableFrom(@object.GetType()))
			{
				@object = null;
			}
			return constructor.Invoke(new object[]
			{
				target,
				method,
				@object
			}) as BaseInvokableCall;
		}

		public void RegisterPersistentListener(Object ttarget, string mmethodName)
		{
			this.m_Target = ttarget;
			this.m_MethodName = mmethodName;
		}

		public void UnregisterPersistentListener()
		{
			this.m_MethodName = string.Empty;
			this.m_Target = null;
		}
	}
}
