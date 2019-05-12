using System;
using UnityEngine;

namespace UniRx
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class InspectorDisplayAttribute : PropertyAttribute
	{
		public InspectorDisplayAttribute(string fieldName = "value", bool notifyPropertyChanged = true)
		{
			this.FieldName = fieldName;
			this.NotifyPropertyChanged = notifyPropertyChanged;
		}

		public string FieldName { get; private set; }

		public bool NotifyPropertyChanged { get; private set; }
	}
}
