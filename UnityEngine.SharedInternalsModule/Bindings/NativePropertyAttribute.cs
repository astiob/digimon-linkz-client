using System;

namespace UnityEngine.Bindings
{
	[AttributeUsage(AttributeTargets.Property)]
	internal class NativePropertyAttribute : NativeMethodAttribute
	{
		public NativePropertyAttribute()
		{
		}

		public NativePropertyAttribute(string name) : base(name)
		{
		}

		public NativePropertyAttribute(string name, TargetType targetType) : base(name)
		{
			this.TargetType = targetType;
		}

		public NativePropertyAttribute(string name, bool isFree, TargetType targetType) : base(name, isFree)
		{
			this.TargetType = targetType;
		}

		public NativePropertyAttribute(string name, bool isFree, TargetType targetType, bool isThreadSafe) : base(name, isFree, isThreadSafe)
		{
			this.TargetType = targetType;
		}

		public TargetType TargetType { get; set; }
	}
}
