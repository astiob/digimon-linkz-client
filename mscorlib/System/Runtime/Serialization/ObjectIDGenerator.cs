using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	/// <summary>Generates IDs for objects.</summary>
	[ComVisible(true)]
	[MonoTODO("Serialization format not compatible with.NET")]
	[Serializable]
	public class ObjectIDGenerator
	{
		private Hashtable table;

		private long current;

		private static ObjectIDGenerator.InstanceComparer comparer = new ObjectIDGenerator.InstanceComparer();

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" /> class.</summary>
		public ObjectIDGenerator()
		{
			this.table = new Hashtable(ObjectIDGenerator.comparer, ObjectIDGenerator.comparer);
			this.current = 1L;
		}

		/// <summary>Returns the ID for the specified object, generating a new ID if the specified object has not already been identified by the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" />.</summary>
		/// <returns>The object's ID is used for serialization. <paramref name="firstTime" /> is set to true if this is the first time the object has been identified; otherwise, it is set to false.</returns>
		/// <param name="obj">The object you want an ID for. </param>
		/// <param name="firstTime">true if <paramref name="obj" /> was not previously known to the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" />; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" /> has been asked to keep track of too many objects. </exception>
		public virtual long GetId(object obj, out bool firstTime)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			object obj2 = this.table[obj];
			if (obj2 != null)
			{
				firstTime = false;
				return (long)obj2;
			}
			firstTime = true;
			this.table.Add(obj, this.current);
			long result;
			this.current = (result = this.current) + 1L;
			return result;
		}

		/// <summary>Determines whether an object has already been assigned an ID.</summary>
		/// <returns>The object ID of <paramref name="obj" /> if previously known to the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" />; otherwise, zero.</returns>
		/// <param name="obj">The object you are asking for. </param>
		/// <param name="firstTime">true if <paramref name="obj" /> was not previously known to the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" />; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		public virtual long HasId(object obj, out bool firstTime)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			object obj2 = this.table[obj];
			if (obj2 != null)
			{
				firstTime = false;
				return (long)obj2;
			}
			firstTime = true;
			return 0L;
		}

		internal long NextId
		{
			get
			{
				long result;
				this.current = (result = this.current) + 1L;
				return result;
			}
		}

		private class InstanceComparer : IComparer, IHashCodeProvider
		{
			int IComparer.Compare(object o1, object o2)
			{
				if (o1 is string)
				{
					return (!o1.Equals(o2)) ? 1 : 0;
				}
				return (o1 != o2) ? 1 : 0;
			}

			int IHashCodeProvider.GetHashCode(object o)
			{
				return object.InternalGetHashCode(o);
			}
		}
	}
}
