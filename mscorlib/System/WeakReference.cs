using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>Represents a weak reference, which references an object while still allowing that object to be reclaimed by garbage collection.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class WeakReference : ISerializable
	{
		private bool isLongReference;

		private GCHandle gcHandle;

		protected WeakReference()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.WeakReference" /> class, referencing the specified object.</summary>
		/// <param name="target">The object to track or null. </param>
		public WeakReference(object target) : this(target, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.WeakReference" /> class, referencing the specified object and using the specified resurrection tracking.</summary>
		/// <param name="target">An object to track. </param>
		/// <param name="trackResurrection">Indicates when to stop tracking the object. If true, the object is tracked after finalization; if false, the object is only tracked until finalization. </param>
		public WeakReference(object target, bool trackResurrection)
		{
			this.isLongReference = trackResurrection;
			this.AllocateHandle(target);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.WeakReference" /> class, using deserialized data from the specified serialization and stream objects.</summary>
		/// <param name="info">An object that holds all the data needed to serialize or deserialize the current <see cref="T:System.WeakReference" /> object. </param>
		/// <param name="context">(Reserved) Describes the source and destination of the serialized stream specified by <paramref name="info" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		protected WeakReference(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.isLongReference = info.GetBoolean("TrackResurrection");
			object value = info.GetValue("TrackedObject", typeof(object));
			this.AllocateHandle(value);
		}

		private void AllocateHandle(object target)
		{
			if (this.isLongReference)
			{
				this.gcHandle = GCHandle.Alloc(target, GCHandleType.WeakTrackResurrection);
			}
			else
			{
				this.gcHandle = GCHandle.Alloc(target, GCHandleType.Weak);
			}
		}

		/// <summary>Gets an indication whether the object referenced by the current <see cref="T:System.WeakReference" /> object has been garbage collected.</summary>
		/// <returns>true if the object referenced by the current <see cref="T:System.WeakReference" /> object has not been garbage collected and is still accessible; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsAlive
		{
			get
			{
				return this.Target != null;
			}
		}

		/// <summary>Gets or sets the object (the target) referenced by the current <see cref="T:System.WeakReference" /> object.</summary>
		/// <returns>null if the object referenced by the current <see cref="T:System.WeakReference" /> object has been garbage collected; otherwise, a reference to the object referenced by the current <see cref="T:System.WeakReference" /> object.</returns>
		/// <exception cref="T:System.InvalidOperationException">The reference to the target object is invalid. This exception can be thrown while setting this property if the value is a null reference or if the object has been finalized during the set operation.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual object Target
		{
			get
			{
				return this.gcHandle.Target;
			}
			set
			{
				this.gcHandle.Target = value;
			}
		}

		/// <summary>Gets an indication whether the object referenced by the current <see cref="T:System.WeakReference" /> object is tracked after it is finalized.</summary>
		/// <returns>true if the object the current <see cref="T:System.WeakReference" /> object refers to is tracked after finalization; or false if the object is only tracked until finalization.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool TrackResurrection
		{
			get
			{
				return this.isLongReference;
			}
		}

		/// <summary>Discards the reference to the target represented by the current <see cref="T:System.WeakReference" /> object.</summary>
		~WeakReference()
		{
			this.gcHandle.Free();
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with all the data needed to serialize the current <see cref="T:System.WeakReference" /> object.</summary>
		/// <param name="info">An object that holds all the data needed to serialize or deserialize the current <see cref="T:System.WeakReference" /> object. </param>
		/// <param name="context">(Reserved) The location where serialized data is stored and retrieved. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("TrackResurrection", this.TrackResurrection);
			try
			{
				info.AddValue("TrackedObject", this.Target);
			}
			catch (Exception)
			{
				info.AddValue("TrackedObject", null);
			}
		}
	}
}
