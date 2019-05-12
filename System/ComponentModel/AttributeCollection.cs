using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	/// <summary>Represents a collection of attributes.</summary>
	[ComVisible(true)]
	public class AttributeCollection : ICollection, IEnumerable
	{
		private ArrayList attrList = new ArrayList();

		/// <summary>Specifies an empty collection that you can use, rather than creating a new one. This field is read-only.</summary>
		public static readonly AttributeCollection Empty = new AttributeCollection(null);

		internal AttributeCollection(ArrayList attributes)
		{
			if (attributes != null)
			{
				this.attrList = attributes;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.AttributeCollection" /> class.</summary>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that provides the attributes for this collection. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="attributes" /> is null.</exception>
		public AttributeCollection(params Attribute[] attributes)
		{
			if (attributes != null)
			{
				for (int i = 0; i < attributes.Length; i++)
				{
					this.attrList.Add(attributes[i]);
				}
			}
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />. </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Gets a value indicating whether access to the collection is synchronized (thread-safe).</summary>
		/// <returns>true if access to the collection is synchronized (thread-safe); otherwise, false.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.attrList.IsSynchronized;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return this.attrList.SyncRoot;
			}
		}

		/// <summary>Gets the number of elements contained in the collection.</summary>
		/// <returns>The number of elements contained in the collection.</returns>
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		/// <summary>Creates a new <see cref="T:System.ComponentModel.AttributeCollection" /> from an existing <see cref="T:System.ComponentModel.AttributeCollection" />.</summary>
		/// <returns>A new <see cref="T:System.ComponentModel.AttributeCollection" /> that is a copy of <paramref name="existing" />.</returns>
		/// <param name="existing">An <see cref="T:System.ComponentModel.AttributeCollection" /> from which to create the copy.</param>
		/// <param name="newAttributes">An array of type <see cref="T:System.Attribute" /> that provides the attributes for this collection. Can be null.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="existing" /> is null.</exception>
		public static AttributeCollection FromExisting(AttributeCollection existing, params Attribute[] newAttributes)
		{
			if (existing == null)
			{
				throw new ArgumentNullException("existing");
			}
			AttributeCollection attributeCollection = new AttributeCollection(new Attribute[0]);
			attributeCollection.attrList.AddRange(existing.attrList);
			if (newAttributes != null)
			{
				attributeCollection.attrList.AddRange(newAttributes);
			}
			return attributeCollection;
		}

		/// <summary>Determines whether this collection of attributes has the specified attribute.</summary>
		/// <returns>true if the collection contains the attribute or is the default attribute for the type of attribute; otherwise, false.</returns>
		/// <param name="attribute">An <see cref="T:System.Attribute" /> to find in the collection. </param>
		public bool Contains(Attribute attr)
		{
			Attribute attribute = this[attr.GetType()];
			return attribute != null && attr.Equals(attribute);
		}

		/// <summary>Determines whether this attribute collection contains all the specified attributes in the attribute array.</summary>
		/// <returns>true if the collection contains all the attributes; otherwise, false.</returns>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> to find in the collection. </param>
		public bool Contains(Attribute[] attributes)
		{
			if (attributes == null)
			{
				return true;
			}
			foreach (Attribute attr in attributes)
			{
				if (!this.Contains(attr))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Copies the collection to an array, starting at the specified index.</summary>
		/// <param name="array">The <see cref="T:System.Array" /> to copy the collection to. </param>
		/// <param name="index">The index to start from. </param>
		public void CopyTo(Array array, int index)
		{
			this.attrList.CopyTo(array, index);
		}

		/// <summary>Gets an enumerator for this collection.</summary>
		/// <returns>An enumerator of type <see cref="T:System.Collections.IEnumerator" />.</returns>
		public IEnumerator GetEnumerator()
		{
			return this.attrList.GetEnumerator();
		}

		/// <summary>Determines whether a specified attribute is the same as an attribute in the collection.</summary>
		/// <returns>true if the attribute is contained within the collection and has the same value as the attribute in the collection; otherwise, false.</returns>
		/// <param name="attribute">An instance of <see cref="T:System.Attribute" /> to compare with the attributes in this collection. </param>
		public bool Matches(Attribute attr)
		{
			foreach (object obj in this.attrList)
			{
				Attribute attribute = (Attribute)obj;
				if (attribute.Match(attr))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Determines whether the attributes in the specified array are the same as the attributes in the collection.</summary>
		/// <returns>true if all the attributes in the array are contained in the collection and have the same values as the attributes in the collection; otherwise, false.</returns>
		/// <param name="attributes">An array of <see cref="T:System.CodeDom.MemberAttributes" /> to compare with the attributes in this collection. </param>
		public bool Matches(Attribute[] attributes)
		{
			foreach (Attribute attr in attributes)
			{
				if (!this.Matches(attr))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Returns the default <see cref="T:System.Attribute" /> of a given <see cref="T:System.Type" />.</summary>
		/// <returns>An <see cref="T:System.Attribute" />.</returns>
		/// <param name="attributeType">The <see cref="T:System.Type" /> of the attribute to retrieve. </param>
		protected Attribute GetDefaultAttribute(Type attributeType)
		{
			Attribute attribute = null;
			BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public;
			FieldInfo field = attributeType.GetField("Default", bindingAttr);
			if (field == null)
			{
				ConstructorInfo constructor = attributeType.GetConstructor(Type.EmptyTypes);
				if (constructor != null)
				{
					attribute = (constructor.Invoke(null) as Attribute);
				}
				if (attribute != null && !attribute.IsDefaultAttribute())
				{
					attribute = null;
				}
			}
			else
			{
				attribute = (Attribute)field.GetValue(null);
			}
			return attribute;
		}

		/// <summary>Gets the number of attributes.</summary>
		/// <returns>The number of attributes.</returns>
		public int Count
		{
			get
			{
				return (this.attrList == null) ? 0 : this.attrList.Count;
			}
		}

		/// <summary>Gets the attribute with the specified type.</summary>
		/// <returns>The <see cref="T:System.Attribute" /> with the specified type or, if the attribute does not exist, the default value for the attribute type.</returns>
		/// <param name="attributeType">The <see cref="T:System.Type" /> of the <see cref="T:System.Attribute" /> to get from the collection. </param>
		public virtual Attribute this[Type type]
		{
			get
			{
				Attribute attribute = null;
				if (this.attrList != null)
				{
					foreach (object obj in this.attrList)
					{
						Attribute attribute2 = (Attribute)obj;
						if (type.IsAssignableFrom(attribute2.GetType()))
						{
							attribute = attribute2;
							break;
						}
					}
				}
				if (attribute == null)
				{
					attribute = this.GetDefaultAttribute(type);
				}
				return attribute;
			}
		}

		/// <summary>Gets the attribute with the specified index number.</summary>
		/// <returns>The <see cref="T:System.Attribute" /> with the specified index number.</returns>
		/// <param name="index">The zero-based index of <see cref="T:System.ComponentModel.AttributeCollection" />. </param>
		public virtual Attribute this[int index]
		{
			get
			{
				return (Attribute)this.attrList[index];
			}
		}
	}
}
