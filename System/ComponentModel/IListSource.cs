using System;
using System.Collections;

namespace System.ComponentModel
{
	/// <summary>Provides functionality to an object to return a list that can be bound to a data source.</summary>
	[TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[MergableProperty(false)]
	public interface IListSource
	{
		/// <summary>Returns an <see cref="T:System.Collections.IList" /> that can be bound to a data source from an object that does not implement an <see cref="T:System.Collections.IList" /> itself.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> that can be bound to a data source from the object.</returns>
		IList GetList();

		/// <summary>Gets a value indicating whether the collection is a collection of <see cref="T:System.Collections.IList" /> objects.</summary>
		/// <returns>true if the collection is a collection of <see cref="T:System.Collections.IList" /> objects; otherwise, false.</returns>
		bool ContainsListCollection { get; }
	}
}
