using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Specifies which <see cref="T:System.Type" /> exclusively uses an interface. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibImportClassAttribute : Attribute
	{
		private string _importClass;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.TypeLibImportClassAttribute" /> class specifying the <see cref="T:System.Type" /> that exclusively uses an interface. </summary>
		/// <param name="importClass">The <see cref="T:System.Type" /> object that exclusively uses an interface.</param>
		public TypeLibImportClassAttribute(Type importClass)
		{
			this._importClass = importClass.ToString();
		}

		/// <summary>Gets the name of a <see cref="T:System.Type" /> object that exclusively uses an interface.</summary>
		/// <returns>The name of a <see cref="T:System.Type" /> object that exclusively uses an interface.</returns>
		public string Value
		{
			get
			{
				return this._importClass;
			}
		}
	}
}
