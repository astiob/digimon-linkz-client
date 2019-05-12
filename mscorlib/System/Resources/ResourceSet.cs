using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Resources
{
	/// <summary>Stores all the resources localized for one particular culture, ignoring all other cultures, including any fallback rules.</summary>
	[ComVisible(true)]
	[Serializable]
	public class ResourceSet : IEnumerable, IDisposable
	{
		/// <summary>Indicates the <see cref="T:System.Resources.IResourceReader" /> used to read the resources.</summary>
		[NonSerialized]
		protected IResourceReader Reader;

		/// <summary>The <see cref="T:System.Collections.Hashtable" /> in which the resources are stored.</summary>
		protected Hashtable Table;

		private bool resources_read;

		[NonSerialized]
		private bool disposed;

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.ResourceSet" /> class with default properties.</summary>
		protected ResourceSet()
		{
			this.Table = new Hashtable();
			this.resources_read = true;
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Resources.ResourceSet" /> class using the specified resource reader.</summary>
		/// <param name="reader">The reader that will be used. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="reader" /> parameter is null. </exception>
		public ResourceSet(IResourceReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.Table = new Hashtable();
			this.Reader = reader;
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Resources.ResourceSet" /> class using the system default <see cref="T:System.Resources.ResourceReader" /> that reads resources from the given stream.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> of resources to be read. The stream should refer to an existing resources file. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="stream" /> is not readable. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="stream" /> parameter is null. </exception>
		public ResourceSet(Stream stream)
		{
			this.Table = new Hashtable();
			this.Reader = new ResourceReader(stream);
		}

		internal ResourceSet(UnmanagedMemoryStream stream)
		{
			this.Table = new Hashtable();
			this.Reader = new ResourceReader(stream);
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Resources.ResourceSet" /> class using the system default <see cref="T:System.Resources.ResourceReader" /> that opens and reads resources from the given file.</summary>
		/// <param name="fileName">Resource file to read. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="fileName" /> parameter is null. </exception>
		public ResourceSet(string fileName)
		{
			this.Table = new Hashtable();
			this.Reader = new ResourceReader(fileName);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IEnumerator" /> object to avoid a race condition with Dispose. This member is not intended to be used directly from your code.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the current <see cref="T:System.Resources.ResourceSet" /> object.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Closes and releases any resources used by this <see cref="T:System.Resources.ResourceSet" />.</summary>
		public virtual void Close()
		{
			this.Dispose();
		}

		/// <summary>Disposes of the resources (other than memory) used by the current instance of <see cref="T:System.Resources.ResourceSet" />.</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases resources (other than memory) associated with the current instance, closing internal managed objects if requested.</summary>
		/// <param name="disposing">Indicates whether the objects contained in the current instance should be explicitly closed. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.Reader != null)
			{
				this.Reader.Close();
			}
			this.Reader = null;
			this.Table = null;
			this.disposed = true;
		}

		/// <summary>Returns the preferred resource reader class for this kind of <see cref="T:System.Resources.ResourceSet" />.</summary>
		/// <returns>Returns the <see cref="T:System.Type" /> for the preferred resource reader for this kind of <see cref="T:System.Resources.ResourceSet" />.</returns>
		public virtual Type GetDefaultReader()
		{
			return typeof(ResourceReader);
		}

		/// <summary>Returns the preferred resource writer class for this kind of <see cref="T:System.Resources.ResourceSet" />.</summary>
		/// <returns>Returns the <see cref="T:System.Type" /> for the preferred resource writer for this kind of <see cref="T:System.Resources.ResourceSet" />.</returns>
		public virtual Type GetDefaultWriter()
		{
			return typeof(ResourceWriter);
		}

		/// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> that can iterate through the <see cref="T:System.Resources.ResourceSet" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for this <see cref="T:System.Resources.ResourceSet" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The resource set has been closed or disposed. </exception>
		[ComVisible(false)]
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("ResourceSet is closed.");
			}
			this.ReadResources();
			return this.Table.GetEnumerator();
		}

		private object GetObjectInternal(string name, bool ignoreCase)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.disposed)
			{
				throw new ObjectDisposedException("ResourceSet is closed.");
			}
			this.ReadResources();
			object obj = this.Table[name];
			if (obj != null)
			{
				return obj;
			}
			if (ignoreCase)
			{
				foreach (object obj2 in this.Table)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					string strA = (string)dictionaryEntry.Key;
					if (string.Compare(strA, name, true, CultureInfo.InvariantCulture) == 0)
					{
						return dictionaryEntry.Value;
					}
				}
			}
			return null;
		}

		/// <summary>Searches for a resource object with the specified name.</summary>
		/// <returns>The requested resource.</returns>
		/// <param name="name">Case-sensitive name of the resource to search for. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has been closed or disposed.</exception>
		public virtual object GetObject(string name)
		{
			return this.GetObjectInternal(name, false);
		}

		/// <summary>Searches for a resource object with the specified name in a case-insensitive manner, if requested.</summary>
		/// <returns>The requested resource.</returns>
		/// <param name="name">Name of the resource to search for. </param>
		/// <param name="ignoreCase">Indicates whether the case of the specified name should be ignored. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has been closed or disposed.</exception>
		public virtual object GetObject(string name, bool ignoreCase)
		{
			return this.GetObjectInternal(name, ignoreCase);
		}

		private string GetStringInternal(string name, bool ignoreCase)
		{
			object @object = this.GetObject(name, ignoreCase);
			if (@object == null)
			{
				return null;
			}
			string text = @object as string;
			if (text == null)
			{
				throw new InvalidOperationException(string.Format("Resource '{0}' is not a String. Use GetObject instead.", name));
			}
			return text;
		}

		/// <summary>Searches for a <see cref="T:System.String" /> resource with the specified name.</summary>
		/// <returns>The value of a resource, if the value is a <see cref="T:System.String" />.</returns>
		/// <param name="name">Name of the resource to search for. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has been closed or disposed.</exception>
		public virtual string GetString(string name)
		{
			return this.GetStringInternal(name, false);
		}

		/// <summary>Searches for a <see cref="T:System.String" /> resource with the specified name in a case-insensitive manner, if requested.</summary>
		/// <returns>The value of a resource, if the value is a <see cref="T:System.String" />.</returns>
		/// <param name="name">Name of the resource to search for. </param>
		/// <param name="ignoreCase">Indicates whether the case of the case of the specified name should be ignored. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The object has been closed or disposed.</exception>
		public virtual string GetString(string name, bool ignoreCase)
		{
			return this.GetStringInternal(name, ignoreCase);
		}

		/// <summary>Reads all the resources and stores them in a <see cref="T:System.Collections.Hashtable" /> indicated in the <see cref="F:System.Resources.ResourceSet.Table" /> property.</summary>
		protected virtual void ReadResources()
		{
			if (this.resources_read)
			{
				return;
			}
			if (this.Reader == null)
			{
				throw new ObjectDisposedException("ResourceSet is closed.");
			}
			Hashtable table = this.Table;
			lock (table)
			{
				if (!this.resources_read)
				{
					IDictionaryEnumerator enumerator = this.Reader.GetEnumerator();
					enumerator.Reset();
					while (enumerator.MoveNext())
					{
						this.Table.Add(enumerator.Key, enumerator.Value);
					}
					this.resources_read = true;
				}
			}
		}

		internal UnmanagedMemoryStream GetStream(string name, bool ignoreCase)
		{
			if (this.Reader == null)
			{
				throw new ObjectDisposedException("ResourceSet is closed.");
			}
			IDictionaryEnumerator enumerator = this.Reader.GetEnumerator();
			enumerator.Reset();
			while (enumerator.MoveNext())
			{
				if (string.Compare(name, (string)enumerator.Key, ignoreCase) == 0)
				{
					return ((ResourceReader.ResourceEnumerator)enumerator).ValueAsStream;
				}
			}
			return null;
		}
	}
}
