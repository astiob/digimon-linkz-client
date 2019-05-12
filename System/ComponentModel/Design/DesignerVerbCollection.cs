using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Represents a collection of <see cref="T:System.ComponentModel.Design.DesignerVerb" /> objects.</summary>
	[ComVisible(true)]
	public class DesignerVerbCollection : CollectionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerVerbCollection" /> class.</summary>
		public DesignerVerbCollection()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.DesignerVerbCollection" /> class using the specified array of <see cref="T:System.ComponentModel.Design.DesignerVerb" /> objects.</summary>
		/// <param name="value">A <see cref="T:System.ComponentModel.Design.DesignerVerb" /> array that indicates the verbs to contain within the collection. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null.</exception>
		public DesignerVerbCollection(DesignerVerb[] value)
		{
			base.InnerList.AddRange(value);
		}

		/// <summary>Gets or sets the <see cref="T:System.ComponentModel.Design.DesignerVerb" /> at the specified index.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.Design.DesignerVerb" /> at each valid index in the collection.</returns>
		/// <param name="index">The index at which to get or set the <see cref="T:System.ComponentModel.Design.DesignerVerb" />. </param>
		public DesignerVerb this[int index]
		{
			get
			{
				return (DesignerVerb)base.InnerList[index];
			}
			set
			{
				base.InnerList[index] = value;
			}
		}

		/// <summary>Adds the specified <see cref="T:System.ComponentModel.Design.DesignerVerb" /> to the collection.</summary>
		/// <returns>The index in the collection at which the verb was added.</returns>
		/// <param name="value">The <see cref="T:System.ComponentModel.Design.DesignerVerb" /> to add to the collection. </param>
		public int Add(DesignerVerb value)
		{
			return base.InnerList.Add(value);
		}

		/// <summary>Adds the specified set of designer verbs to the collection.</summary>
		/// <param name="value">An array of <see cref="T:System.ComponentModel.Design.DesignerVerb" /> objects to add to the collection. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null.</exception>
		public void AddRange(DesignerVerb[] value)
		{
			base.InnerList.AddRange(value);
		}

		/// <summary>Adds the specified collection of designer verbs to the collection.</summary>
		/// <param name="value">A <see cref="T:System.ComponentModel.Design.DesignerVerbCollection" /> to add to the collection. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null.</exception>
		public void AddRange(DesignerVerbCollection value)
		{
			base.InnerList.AddRange(value);
		}

		/// <summary>Gets a value indicating whether the specified <see cref="T:System.ComponentModel.Design.DesignerVerb" /> exists in the collection.</summary>
		/// <returns>true if the specified object exists in the collection; otherwise, false.</returns>
		/// <param name="value">The <see cref="T:System.ComponentModel.Design.DesignerVerb" /> to search for in the collection. </param>
		public bool Contains(DesignerVerb value)
		{
			return base.InnerList.Contains(value);
		}

		/// <summary>Copies the collection members to the specified <see cref="T:System.ComponentModel.Design.DesignerVerb" /> array beginning at the specified destination index.</summary>
		/// <param name="array">The array to copy collection members to. </param>
		/// <param name="index">The destination index to begin copying to. </param>
		public void CopyTo(DesignerVerb[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		/// <summary>Gets the index of the specified <see cref="T:System.ComponentModel.Design.DesignerVerb" />.</summary>
		/// <returns>The index of the specified object if it is found in the list; otherwise, -1.</returns>
		/// <param name="value">The <see cref="T:System.ComponentModel.Design.DesignerVerb" /> whose index to get in the collection. </param>
		public int IndexOf(DesignerVerb value)
		{
			return base.InnerList.IndexOf(value);
		}

		/// <summary>Inserts the specified <see cref="T:System.ComponentModel.Design.DesignerVerb" /> at the specified index.</summary>
		/// <param name="index">The index in the collection at which to insert the verb. </param>
		/// <param name="value">The <see cref="T:System.ComponentModel.Design.DesignerVerb" /> to insert in the collection. </param>
		public void Insert(int index, DesignerVerb value)
		{
			base.InnerList.Insert(index, value);
		}

		/// <summary>Raises the Clear event.</summary>
		protected override void OnClear()
		{
		}

		/// <summary>Raises the Insert event.</summary>
		/// <param name="index">The index at which to insert an item. </param>
		/// <param name="value">The object to insert. </param>
		protected override void OnInsert(int index, object value)
		{
		}

		/// <summary>Raises the Remove event.</summary>
		/// <param name="index">The index at which to remove the item. </param>
		/// <param name="value">The object to remove. </param>
		protected override void OnRemove(int index, object value)
		{
		}

		/// <summary>Raises the Set event.</summary>
		/// <param name="index">The index at which to set the item. </param>
		/// <param name="oldValue">The old object. </param>
		/// <param name="newValue">The new object. </param>
		protected override void OnSet(int index, object oldValue, object newValue)
		{
		}

		/// <summary>Raises the Validate event.</summary>
		/// <param name="value">The object to validate. </param>
		protected override void OnValidate(object value)
		{
		}

		/// <summary>Removes the specified <see cref="T:System.ComponentModel.Design.DesignerVerb" /> from the collection.</summary>
		/// <param name="value">The <see cref="T:System.ComponentModel.Design.DesignerVerb" /> to remove from the collection. </param>
		public void Remove(DesignerVerb value)
		{
			base.InnerList.Remove(value);
		}
	}
}
