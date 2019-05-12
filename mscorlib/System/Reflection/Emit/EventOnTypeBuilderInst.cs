using System;
using System.Collections;

namespace System.Reflection.Emit
{
	internal class EventOnTypeBuilderInst : EventInfo
	{
		private MonoGenericClass instantiation;

		private EventBuilder evt;

		internal EventOnTypeBuilderInst(MonoGenericClass instantiation, EventBuilder evt)
		{
			this.instantiation = instantiation;
			this.evt = evt;
		}

		public override EventAttributes Attributes
		{
			get
			{
				return this.evt.attrs;
			}
		}

		public override MethodInfo GetAddMethod(bool nonPublic)
		{
			if (this.evt.add_method == null || (!nonPublic && !this.evt.add_method.IsPublic))
			{
				return null;
			}
			return TypeBuilder.GetMethod(this.instantiation, this.evt.add_method);
		}

		public override MethodInfo GetRaiseMethod(bool nonPublic)
		{
			if (this.evt.raise_method == null || (!nonPublic && !this.evt.raise_method.IsPublic))
			{
				return null;
			}
			return TypeBuilder.GetMethod(this.instantiation, this.evt.raise_method);
		}

		public override MethodInfo GetRemoveMethod(bool nonPublic)
		{
			if (this.evt.remove_method == null || (!nonPublic && !this.evt.remove_method.IsPublic))
			{
				return null;
			}
			return TypeBuilder.GetMethod(this.instantiation, this.evt.remove_method);
		}

		public override MethodInfo[] GetOtherMethods(bool nonPublic)
		{
			if (this.evt.other_methods == null)
			{
				return new MethodInfo[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (MethodBuilder methodInfo in this.evt.other_methods)
			{
				if (nonPublic || methodInfo.IsPublic)
				{
					arrayList.Add(TypeBuilder.GetMethod(this.instantiation, methodInfo));
				}
			}
			MethodInfo[] array = new MethodInfo[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		public override Type DeclaringType
		{
			get
			{
				return this.instantiation;
			}
		}

		public override string Name
		{
			get
			{
				return this.evt.name;
			}
		}

		public override Type ReflectedType
		{
			get
			{
				return this.instantiation;
			}
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException();
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}
	}
}
