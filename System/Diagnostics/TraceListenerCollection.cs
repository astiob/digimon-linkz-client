using System;
using System.Collections;

namespace System.Diagnostics
{
	/// <summary>Provides a thread-safe list of <see cref="T:System.Diagnostics.TraceListener" /> objects.</summary>
	/// <filterpriority>2</filterpriority>
	public class TraceListenerCollection : ICollection, IEnumerable, IList
	{
		private ArrayList listeners = ArrayList.Synchronized(new ArrayList(1));

		internal TraceListenerCollection() : this(true)
		{
		}

		internal TraceListenerCollection(bool addDefault)
		{
			if (addDefault)
			{
				this.Add(new DefaultTraceListener());
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Diagnostics.TraceListener" /> at the specified index in the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</summary>
		/// <returns>The <see cref="T:System.Diagnostics.TraceListener" /> at the specified index.</returns>
		/// <param name="index">The zero-based index of the <paramref name="value" /> to get.</param>
		object IList.this[int index]
		{
			get
			{
				return this.listeners[index];
			}
			set
			{
				TraceListener traceListener = (TraceListener)value;
				this.InitializeListener(traceListener);
				this[index] = traceListener;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Diagnostics.TraceListenerCollection" /> is synchronized (thread safe).</summary>
		/// <returns>Always true.</returns>
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.listeners.IsSynchronized;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</summary>
		/// <returns>The current <see cref="T:System.Diagnostics.TraceListenerCollection" /> object.</returns>
		object ICollection.SyncRoot
		{
			get
			{
				return this.listeners.SyncRoot;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Diagnostics.TraceListenerCollection" /> has a fixed size.</summary>
		/// <returns>Always false.</returns>
		bool IList.IsFixedSize
		{
			get
			{
				return this.listeners.IsFixedSize;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Diagnostics.TraceListenerCollection" /> is read-only</summary>
		/// <returns>Always false.</returns>
		bool IList.IsReadOnly
		{
			get
			{
				return this.listeners.IsReadOnly;
			}
		}

		/// <summary>Copies a section of the current <see cref="T:System.Diagnostics.TraceListenerCollection" /> to the specified array of <see cref="T:System.Diagnostics.TraceListener" /> objects. </summary>
		/// <param name="array">The one-dimensional array of <see cref="T:System.Diagnostics.TraceListener" /> objects that is the destination of the elements copied from the <see cref="T:System.Diagnostics.TraceListenerCollection" />. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			this.listeners.CopyTo(array, index);
		}

		/// <summary>Adds a trace listener to the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</summary>
		/// <returns>The position into which the new trace listener was inserted.</returns>
		/// <param name="value">The object to add to the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is null. -or-<paramref name="value" /> is not a <see cref="T:System.Diagnostics.TraceListener" />.</exception>
		int IList.Add(object value)
		{
			if (value is TraceListener)
			{
				return this.Add((TraceListener)value);
			}
			throw new NotSupportedException(Locale.GetText("You can only add TraceListener objects to the collection"));
		}

		/// <summary>Determines whether the <see cref="T:System.Diagnostics.TraceListenerCollection" /> contains a specific object.</summary>
		/// <returns>true if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Diagnostics.TraceListenerCollection" />; otherwise, false.</returns>
		/// <param name="value">The object to locate in the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</param>
		bool IList.Contains(object value)
		{
			return value is TraceListener && this.listeners.Contains(value);
		}

		/// <summary>Determines the index of a specific object in the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</summary>
		/// <returns>The index of <paramref name="value" /> if found in the <see cref="T:System.Diagnostics.TraceListenerCollection" />; otherwise, -1.</returns>
		/// <param name="value">The object to locate in the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</param>
		int IList.IndexOf(object value)
		{
			if (value is TraceListener)
			{
				return this.listeners.IndexOf(value);
			}
			return -1;
		}

		/// <summary>Inserts a <see cref="T:System.Diagnostics.TraceListener" /> object at the specified position in the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The <see cref="T:System.Diagnostics.TraceListener" /> object to insert into the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.Diagnostics.TraceListener" /> object.</exception>
		void IList.Insert(int index, object value)
		{
			if (value is TraceListener)
			{
				this.Insert(index, (TraceListener)value);
				return;
			}
			throw new NotSupportedException(Locale.GetText("You can only insert TraceListener objects into the collection"));
		}

		/// <summary>Removes an object from the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</summary>
		/// <param name="value">The object to remove from the <see cref="T:System.Diagnostics.TraceListenerCollection" />.</param>
		void IList.Remove(object value)
		{
			if (value is TraceListener)
			{
				this.listeners.Remove(value);
			}
		}

		/// <summary>Gets the number of listeners in the list.</summary>
		/// <returns>The number of listeners in the list.</returns>
		/// <filterpriority>2</filterpriority>
		public int Count
		{
			get
			{
				return this.listeners.Count;
			}
		}

		/// <summary>Gets the first <see cref="T:System.Diagnostics.TraceListener" /> in the list with the specified name.</summary>
		/// <returns>The first <see cref="T:System.Diagnostics.TraceListener" /> in the list with the given <see cref="P:System.Diagnostics.TraceListener.Name" />. This item returns null if no <see cref="T:System.Diagnostics.TraceListener" /> with the given name can be found.</returns>
		/// <param name="name">The name of the <see cref="T:System.Diagnostics.TraceListener" /> to get from the list. </param>
		/// <filterpriority>2</filterpriority>
		public TraceListener this[string name]
		{
			get
			{
				object syncRoot = this.listeners.SyncRoot;
				lock (syncRoot)
				{
					foreach (object obj in this.listeners)
					{
						TraceListener traceListener = (TraceListener)obj;
						if (traceListener.Name == name)
						{
							return traceListener;
						}
					}
				}
				return null;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Diagnostics.TraceListener" /> at the specified index.</summary>
		/// <returns>A <see cref="T:System.Diagnostics.TraceListener" /> with the specified index.</returns>
		/// <param name="i">The zero-based index of the <see cref="T:System.Diagnostics.TraceListener" /> to get from the list. </param>
		/// <exception cref="T:System.ArgumentNullException">The value is null.</exception>
		/// <filterpriority>2</filterpriority>
		public TraceListener this[int index]
		{
			get
			{
				return (TraceListener)this.listeners[index];
			}
			set
			{
				this.InitializeListener(value);
				this.listeners[index] = value;
			}
		}

		/// <summary>Adds a <see cref="T:System.Diagnostics.TraceListener" /> to the list.</summary>
		/// <returns>The position at which the new listener was inserted.</returns>
		/// <param name="listener">A <see cref="T:System.Diagnostics.TraceListener" /> to add to the list. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public int Add(TraceListener listener)
		{
			this.InitializeListener(listener);
			return this.listeners.Add(listener);
		}

		internal void Add(TraceListener listener, TraceImplSettings settings)
		{
			listener.IndentLevel = settings.IndentLevel;
			listener.IndentSize = settings.IndentSize;
			this.listeners.Add(listener);
		}

		private void InitializeListener(TraceListener listener)
		{
			listener.IndentLevel = TraceImpl.IndentLevel;
			listener.IndentSize = TraceImpl.IndentSize;
		}

		private void InitializeRange(IList listeners)
		{
			int count = listeners.Count;
			for (int num = 0; num != count; num++)
			{
				this.InitializeListener((TraceListener)listeners[num]);
			}
		}

		/// <summary>Adds an array of <see cref="T:System.Diagnostics.TraceListener" /> objects to the list.</summary>
		/// <param name="value">An array of <see cref="T:System.Diagnostics.TraceListener" /> objects to add to the list. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public void AddRange(TraceListener[] value)
		{
			this.InitializeRange(value);
			this.listeners.AddRange(value);
		}

		/// <summary>Adds the contents of another <see cref="T:System.Diagnostics.TraceListenerCollection" /> to the list.</summary>
		/// <param name="value">Another <see cref="T:System.Diagnostics.TraceListenerCollection" /> whose contents are added to the list. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is null.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public void AddRange(TraceListenerCollection value)
		{
			this.InitializeRange(value);
			this.listeners.AddRange(value.listeners);
		}

		/// <summary>Clears all the listeners from the list.</summary>
		/// <filterpriority>2</filterpriority>
		public void Clear()
		{
			this.listeners.Clear();
		}

		/// <summary>Checks whether the list contains the specified listener.</summary>
		/// <returns>true if the listener is in the list; otherwise, false.</returns>
		/// <param name="listener">A <see cref="T:System.Diagnostics.TraceListener" /> to find in the list. </param>
		/// <filterpriority>2</filterpriority>
		public bool Contains(TraceListener listener)
		{
			return this.listeners.Contains(listener);
		}

		/// <summary>Copies a section of the current <see cref="T:System.Diagnostics.TraceListenerCollection" /> list to the specified array at the specified index.</summary>
		/// <param name="listeners">An array of type <see cref="T:System.Array" /> to copy the elements into. </param>
		/// <param name="index">The starting index number in the current list to copy from. </param>
		/// <filterpriority>2</filterpriority>
		public void CopyTo(TraceListener[] listeners, int index)
		{
			listeners.CopyTo(listeners, index);
		}

		/// <summary>Gets an enumerator for this list.</summary>
		/// <returns>An enumerator of type <see cref="T:System.Collections.IEnumerator" />.</returns>
		/// <filterpriority>2</filterpriority>
		public IEnumerator GetEnumerator()
		{
			return this.listeners.GetEnumerator();
		}

		/// <summary>Gets the index of the specified listener.</summary>
		/// <returns>The index of the listener, if it can be found in the list; otherwise, -1.</returns>
		/// <param name="listener">A <see cref="T:System.Diagnostics.TraceListener" /> to find in the list. </param>
		/// <filterpriority>2</filterpriority>
		public int IndexOf(TraceListener listener)
		{
			return this.listeners.IndexOf(listener);
		}

		/// <summary>Inserts the listener at the specified index.</summary>
		/// <param name="index">The position in the list to insert the new <see cref="T:System.Diagnostics.TraceListener" />. </param>
		/// <param name="listener">A <see cref="T:System.Diagnostics.TraceListener" /> to insert in the list. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> is not a valid index in the list. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="listener" /> is null.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public void Insert(int index, TraceListener listener)
		{
			this.InitializeListener(listener);
			this.listeners.Insert(index, listener);
		}

		/// <summary>Removes from the collection the first <see cref="T:System.Diagnostics.TraceListener" /> with the specified name.</summary>
		/// <param name="name">The name of the <see cref="T:System.Diagnostics.TraceListener" /> to remove from the list. </param>
		/// <filterpriority>2</filterpriority>
		public void Remove(string name)
		{
			TraceListener traceListener = null;
			object syncRoot = this.listeners.SyncRoot;
			lock (syncRoot)
			{
				foreach (object obj in this.listeners)
				{
					TraceListener traceListener2 = (TraceListener)obj;
					if (traceListener2.Name == name)
					{
						traceListener = traceListener2;
						break;
					}
				}
				if (traceListener == null)
				{
					throw new ArgumentException(Locale.GetText("TraceListener " + name + " was not in the collection"));
				}
				this.listeners.Remove(traceListener);
			}
		}

		/// <summary>Removes from the collection the specified <see cref="T:System.Diagnostics.TraceListener" />.</summary>
		/// <param name="listener">A <see cref="T:System.Diagnostics.TraceListener" /> to remove from the list. </param>
		/// <filterpriority>2</filterpriority>
		public void Remove(TraceListener listener)
		{
			this.listeners.Remove(listener);
		}

		/// <summary>Removes from the collection the <see cref="T:System.Diagnostics.TraceListener" /> at the specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Diagnostics.TraceListener" /> to remove from the list. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> is not a valid index in the list. </exception>
		/// <filterpriority>2</filterpriority>
		public void RemoveAt(int index)
		{
			this.listeners.RemoveAt(index);
		}
	}
}
