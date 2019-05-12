using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode(Name = "ExposedReference")]
	[Serializable]
	public struct ExposedReference<T> where T : Object
	{
		[SerializeField]
		public PropertyName exposedName;

		[SerializeField]
		public Object defaultValue;

		public T Resolve(IExposedPropertyTable resolver)
		{
			if (resolver != null)
			{
				bool flag;
				Object referenceValue = resolver.GetReferenceValue(this.exposedName, out flag);
				if (flag)
				{
					return referenceValue as T;
				}
			}
			return this.defaultValue as T;
		}
	}
}
