using System;
using System.Collections;

namespace System.Xml.Schema
{
	/// <summary>A collection of <see cref="T:System.Xml.Schema.XmlSchemaObject" />s.</summary>
	public class XmlSchemaObjectCollection : CollectionBase
	{
		/// <summary>Initializes a new instance of the XmlSchemaObjectCollection class.</summary>
		public XmlSchemaObjectCollection()
		{
		}

		/// <summary>Initializes a new instance of the XmlSchemaObjectCollection class that takes an <see cref="T:System.Xml.Schema.XmlSchemaObject" />.</summary>
		/// <param name="parent">The <see cref="T:System.Xml.Schema.XmlSchemaObject" />. </param>
		public XmlSchemaObjectCollection(XmlSchemaObject parent)
		{
		}

		/// <summary>Gets the <see cref="T:System.Xml.Schema.XmlSchemaObject" /> at the specified index.</summary>
		/// <returns>The <see cref="T:System.Xml.Schema.XmlSchemaObject" /> at the specified index.</returns>
		/// <param name="index">The index of the <see cref="T:System.Xml.Schema.XmlSchemaObject" />. </param>
		public virtual XmlSchemaObject this[int index]
		{
			get
			{
				return (XmlSchemaObject)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		/// <summary>Adds an <see cref="T:System.Xml.Schema.XmlSchemaObject" /> to the XmlSchemaObjectCollection.</summary>
		/// <returns>The index at which the item has been added.</returns>
		/// <param name="item">The <see cref="T:System.Xml.Schema.XmlSchemaObject" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than Count. </exception>
		/// <exception cref="T:System.InvalidCastException">The <see cref="T:System.Xml.Schema.XmlSchemaObject" /> parameter specified is not of type <see cref="T:System.Xml.Schema.XmlSchemaExternal" /> or its derived types <see cref="T:System.Xml.Schema.XmlSchemaImport" />, <see cref="T:System.Xml.Schema.XmlSchemaInclude" />, and <see cref="T:System.Xml.Schema.XmlSchemaRedefine" />.</exception>
		public int Add(XmlSchemaObject item)
		{
			return base.List.Add(item);
		}

		/// <summary>Indicates if the specified <see cref="T:System.Xml.Schema.XmlSchemaObject" /> is in the XmlSchemaObjectCollection.</summary>
		/// <returns>true if the specified qualified name is in the collection; otherwise, returns false. If null is supplied, false is returned because there is no qualified name with a null name.</returns>
		/// <param name="item">The <see cref="T:System.Xml.Schema.XmlSchemaObject" />. </param>
		public bool Contains(XmlSchemaObject item)
		{
			return base.List.Contains(item);
		}

		/// <summary>Copies all the <see cref="T:System.Xml.Schema.XmlSchemaObject" />s from the collection into the given array, starting at the given index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements copied from the XmlSchemaObjectCollection. The array must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in the array at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is a null reference (Nothing in Visual Basic). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multi-dimensional.- or - <paramref name="index" /> is equal to or greater than the length of <paramref name="array" />.- or - The number of elements in the source <see cref="T:System.Xml.Schema.XmlSchemaObject" /> is greater than the available space from index to the end of the destination array. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Xml.Schema.XmlSchemaObject" /> cannot be cast automatically to the type of the destination array. </exception>
		public void CopyTo(XmlSchemaObject[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		/// <summary>Returns an enumerator for iterating through the XmlSchemaObjects contained in the XmlSchemaObjectCollection.</summary>
		/// <returns>The iterator returns <see cref="T:System.Xml.Schema.XmlSchemaObjectEnumerator" />.</returns>
		public new XmlSchemaObjectEnumerator GetEnumerator()
		{
			return new XmlSchemaObjectEnumerator(base.List);
		}

		/// <summary>Gets the collection index corresponding to the specified <see cref="T:System.Xml.Schema.XmlSchemaObject" />.</summary>
		/// <returns>The index corresponding to the specified <see cref="T:System.Xml.Schema.XmlSchemaObject" />.</returns>
		/// <param name="item">The <see cref="T:System.Xml.Schema.XmlSchemaObject" /> whose index you want to return. </param>
		public int IndexOf(XmlSchemaObject item)
		{
			return base.List.IndexOf(item);
		}

		/// <summary>Inserts an <see cref="T:System.Xml.Schema.XmlSchemaObject" /> to the XmlSchemaObjectCollection.</summary>
		/// <param name="index">The zero-based index at which an item should be inserted. </param>
		/// <param name="item">The <see cref="T:System.Xml.Schema.XmlSchemaObject" /> to insert. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.-or- <paramref name="index" /> is greater than Count. </exception>
		public void Insert(int index, XmlSchemaObject item)
		{
			base.List.Insert(index, item);
		}

		/// <summary>OnClear is invoked before the standard Clear behavior. For more information, see OnClear method for <see cref="T:System.Collections.CollectionBase" />.</summary>
		protected override void OnClear()
		{
		}

		/// <summary>OnInsert is invoked before the standard Insert behavior. For more information, see OnInsert method <see cref="T:System.Collections.CollectionBase" />.</summary>
		/// <param name="index">The index of <see cref="T:System.Xml.Schema.XmlSchemaObject" />. </param>
		/// <param name="item">The item. </param>
		protected override void OnInsert(int index, object item)
		{
		}

		/// <summary>OnRemove is invoked before the standard Remove behavior. For more information, see the OnRemove method for <see cref="T:System.Collections.CollectionBase" />.</summary>
		/// <param name="index">The index of <see cref="T:System.Xml.Schema.XmlSchemaObject" />. </param>
		/// <param name="item">The item. </param>
		protected override void OnRemove(int index, object item)
		{
		}

		/// <summary>OnSet is invoked before the standard Set behavior. For more information, see the OnSet method for <see cref="T:System.Collections.CollectionBase" />.</summary>
		/// <param name="index">The index of <see cref="T:System.Xml.Schema.XmlSchemaObject" />. </param>
		/// <param name="oldValue">The old value. </param>
		/// <param name="newValue">The new value. </param>
		protected override void OnSet(int index, object oldValue, object newValue)
		{
		}

		/// <summary>Removes an <see cref="T:System.Xml.Schema.XmlSchemaObject" /> from the XmlSchemaObjectCollection.</summary>
		/// <param name="item">The <see cref="T:System.Xml.Schema.XmlSchemaObject" /> to remove. </param>
		public void Remove(XmlSchemaObject item)
		{
			base.List.Remove(item);
		}
	}
}
