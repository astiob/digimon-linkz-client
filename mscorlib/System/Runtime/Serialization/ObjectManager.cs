using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	/// <summary>Keeps track of objects as they are deserialized.</summary>
	[ComVisible(true)]
	public class ObjectManager
	{
		private ObjectRecord _objectRecordChain;

		private ObjectRecord _lastObjectRecord;

		private ArrayList _deserializedRecords = new ArrayList();

		private ArrayList _onDeserializedCallbackRecords = new ArrayList();

		private Hashtable _objectRecords = new Hashtable();

		private bool _finalFixup;

		private ISurrogateSelector _selector;

		private StreamingContext _context;

		private int _registeredObjectsCount;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.ObjectManager" /> class.</summary>
		/// <param name="selector">The surrogate selector to use. The <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> determines the correct surrogate to use when deserializing objects of a given type. At deserialization time, the surrogate selector creates a new instance of the object from the information transmitted on the stream. </param>
		/// <param name="context">The streaming context. The <see cref="T:System.Runtime.Serialization.StreamingContext" /> is not used by ObjectManager, but is passed as a parameter to any objects implementing <see cref="T:System.Runtime.Serialization.ISerializable" /> or having a <see cref="T:System.Runtime.Serialization.ISerializationSurrogate" />. These objects can take specific actions depending on the source of the information to deserialize. </param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public ObjectManager(ISurrogateSelector selector, StreamingContext context)
		{
			this._selector = selector;
			this._context = context;
		}

		/// <summary>Performs all the recorded fixups.</summary>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A fixup was not successfully completed. </exception>
		public virtual void DoFixups()
		{
			this._finalFixup = true;
			try
			{
				if (this._registeredObjectsCount < this._objectRecords.Count)
				{
					throw new SerializationException("There are some fixups that refer to objects that have not been registered");
				}
				ObjectRecord lastObjectRecord = this._lastObjectRecord;
				bool flag = true;
				ObjectRecord objectRecord2;
				for (ObjectRecord objectRecord = this._objectRecordChain; objectRecord != null; objectRecord = objectRecord2)
				{
					bool flag2 = !objectRecord.IsUnsolvedObjectReference || !flag;
					if (flag2)
					{
						flag2 = objectRecord.DoFixups(true, this, true);
					}
					if (flag2)
					{
						flag2 = objectRecord.LoadData(this, this._selector, this._context);
					}
					if (flag2)
					{
						if (objectRecord.OriginalObject is IDeserializationCallback)
						{
							this._deserializedRecords.Add(objectRecord);
						}
						SerializationCallbacks serializationCallbacks = SerializationCallbacks.GetSerializationCallbacks(objectRecord.OriginalObject.GetType());
						if (serializationCallbacks.HasDeserializedCallbacks)
						{
							this._onDeserializedCallbackRecords.Add(objectRecord);
						}
						objectRecord2 = objectRecord.Next;
					}
					else
					{
						if (objectRecord.ObjectInstance is IObjectReference && !flag)
						{
							if (objectRecord.Status == ObjectRecordStatus.ReferenceSolvingDelayed)
							{
								throw new SerializationException("The object with ID " + objectRecord.ObjectID + " could not be resolved");
							}
							objectRecord.Status = ObjectRecordStatus.ReferenceSolvingDelayed;
						}
						if (objectRecord != this._lastObjectRecord)
						{
							objectRecord2 = objectRecord.Next;
							objectRecord.Next = null;
							this._lastObjectRecord.Next = objectRecord;
							this._lastObjectRecord = objectRecord;
						}
						else
						{
							objectRecord2 = objectRecord;
						}
					}
					if (objectRecord == lastObjectRecord)
					{
						flag = false;
					}
				}
			}
			finally
			{
				this._finalFixup = false;
			}
		}

		internal ObjectRecord GetObjectRecord(long objectID)
		{
			ObjectRecord objectRecord = (ObjectRecord)this._objectRecords[objectID];
			if (objectRecord == null)
			{
				if (this._finalFixup)
				{
					throw new SerializationException("The object with Id " + objectID + " has not been registered");
				}
				objectRecord = new ObjectRecord();
				objectRecord.ObjectID = objectID;
				this._objectRecords[objectID] = objectRecord;
			}
			if (!objectRecord.IsRegistered && this._finalFixup)
			{
				throw new SerializationException("The object with Id " + objectID + " has not been registered");
			}
			return objectRecord;
		}

		/// <summary>Returns the object with the specified object ID.</summary>
		/// <returns>The object with the specified object ID if it has been previously stored or null if no such object has been registered.</returns>
		/// <param name="objectID">The ID of the requested object. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero. </exception>
		public virtual object GetObject(long objectID)
		{
			if (objectID <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectID", "The objectID parameter is less than or equal to zero");
			}
			ObjectRecord objectRecord = (ObjectRecord)this._objectRecords[objectID];
			if (objectRecord == null || !objectRecord.IsRegistered)
			{
				return null;
			}
			return objectRecord.ObjectInstance;
		}

		/// <summary>Raises the deserialization event to any registered object that implements <see cref="T:System.Runtime.Serialization.IDeserializationCallback" />.</summary>
		public virtual void RaiseDeserializationEvent()
		{
			for (int i = this._onDeserializedCallbackRecords.Count - 1; i >= 0; i--)
			{
				ObjectRecord objectRecord = (ObjectRecord)this._onDeserializedCallbackRecords[i];
				this.RaiseOnDeserializedEvent(objectRecord.OriginalObject);
			}
			for (int j = this._deserializedRecords.Count - 1; j >= 0; j--)
			{
				ObjectRecord objectRecord2 = (ObjectRecord)this._deserializedRecords[j];
				IDeserializationCallback deserializationCallback = objectRecord2.OriginalObject as IDeserializationCallback;
				if (deserializationCallback != null)
				{
					deserializationCallback.OnDeserialization(this);
				}
			}
		}

		/// <summary>Invokes the method marked with the <see cref="T:System.Runtime.Serialization.OnDeserializingAttribute" />.</summary>
		/// <param name="obj">The instance of the type that contains the method to be invoked.</param>
		public void RaiseOnDeserializingEvent(object obj)
		{
			SerializationCallbacks serializationCallbacks = SerializationCallbacks.GetSerializationCallbacks(obj.GetType());
			serializationCallbacks.RaiseOnDeserializing(obj, this._context);
		}

		private void RaiseOnDeserializedEvent(object obj)
		{
			SerializationCallbacks serializationCallbacks = SerializationCallbacks.GetSerializationCallbacks(obj.GetType());
			serializationCallbacks.RaiseOnDeserialized(obj, this._context);
		}

		private void AddFixup(BaseFixupRecord record)
		{
			record.ObjectToBeFixed.ChainFixup(record, true);
			record.ObjectRequired.ChainFixup(record, false);
		}

		/// <summary>Records a fixup for one element in an array.</summary>
		/// <param name="arrayToBeFixed">The ID of the array used to record a fixup. </param>
		/// <param name="index">The index within <paramref name="arrayFixup" /> that a fixup is requested for. </param>
		/// <param name="objectRequired">The ID of the object that the current array element will point to after fixup is completed. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="arrayToBeFixed" /> or <paramref name="objectRequired" /> parameter is less than or equal to zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="index" /> parameter is null. </exception>
		public virtual void RecordArrayElementFixup(long arrayToBeFixed, int index, long objectRequired)
		{
			if (arrayToBeFixed <= 0L)
			{
				throw new ArgumentOutOfRangeException("arrayToBeFixed", "The arrayToBeFixed parameter is less than or equal to zero");
			}
			if (objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectRequired", "The objectRequired parameter is less than or equal to zero");
			}
			ArrayFixupRecord record = new ArrayFixupRecord(this.GetObjectRecord(arrayToBeFixed), index, this.GetObjectRecord(objectRequired));
			this.AddFixup(record);
		}

		/// <summary>Records fixups for the specified elements in an array, to be executed later.</summary>
		/// <param name="arrayToBeFixed">The ID of the array used to record a fixup. </param>
		/// <param name="indices">The indexes within the multidimensional array that a fixup is requested for. </param>
		/// <param name="objectRequired">The ID of the object the array elements will point to after fixup is completed. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="arrayToBeFixed" /> or <paramref name="objectRequired" /> parameter is less than or equal to zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="indices" /> parameter is null. </exception>
		public virtual void RecordArrayElementFixup(long arrayToBeFixed, int[] indices, long objectRequired)
		{
			if (arrayToBeFixed <= 0L)
			{
				throw new ArgumentOutOfRangeException("arrayToBeFixed", "The arrayToBeFixed parameter is less than or equal to zero");
			}
			if (objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectRequired", "The objectRequired parameter is less than or equal to zero");
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices");
			}
			MultiArrayFixupRecord record = new MultiArrayFixupRecord(this.GetObjectRecord(arrayToBeFixed), indices, this.GetObjectRecord(objectRequired));
			this.AddFixup(record);
		}

		/// <summary>Records a fixup for an object member, to be executed later.</summary>
		/// <param name="objectToBeFixed">The ID of the object that needs the reference to <paramref name="objectRequired" />. </param>
		/// <param name="memberName">The member name of <paramref name="objectToBeFixed" /> where the fixup will be performed. </param>
		/// <param name="objectRequired">The ID of the object required by <paramref name="objectToBeFixed" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="objectToBeFixed" /> or <paramref name="objectRequired" /> parameter is less than or equal to zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="memberName" /> parameter is null. </exception>
		public virtual void RecordDelayedFixup(long objectToBeFixed, string memberName, long objectRequired)
		{
			if (objectToBeFixed <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectToBeFixed", "The objectToBeFixed parameter is less than or equal to zero");
			}
			if (objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectRequired", "The objectRequired parameter is less than or equal to zero");
			}
			if (memberName == null)
			{
				throw new ArgumentNullException("memberName");
			}
			DelayedFixupRecord record = new DelayedFixupRecord(this.GetObjectRecord(objectToBeFixed), memberName, this.GetObjectRecord(objectRequired));
			this.AddFixup(record);
		}

		/// <summary>Records a fixup for a member of an object, to be executed later.</summary>
		/// <param name="objectToBeFixed">The ID of the object that needs the reference to the <paramref name="objectRequired" /> object. </param>
		/// <param name="member">The member of <paramref name="objectToBeFixed" /> where the fixup will be performed. </param>
		/// <param name="objectRequired">The ID of the object required by <paramref name="objectToBeFixed" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectToBeFixed" /> or <paramref name="objectRequired" /> parameter is less than or equal to zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="member" /> parameter is null. </exception>
		public virtual void RecordFixup(long objectToBeFixed, MemberInfo member, long objectRequired)
		{
			if (objectToBeFixed <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectToBeFixed", "The objectToBeFixed parameter is less than or equal to zero");
			}
			if (objectRequired <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectRequired", "The objectRequired parameter is less than or equal to zero");
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			FixupRecord record = new FixupRecord(this.GetObjectRecord(objectToBeFixed), member, this.GetObjectRecord(objectRequired));
			this.AddFixup(record);
		}

		private void RegisterObjectInternal(object obj, ObjectRecord record)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!record.IsRegistered)
			{
				record.ObjectInstance = obj;
				record.OriginalObject = obj;
				if (obj is IObjectReference)
				{
					record.Status = ObjectRecordStatus.ReferenceUnsolved;
				}
				else
				{
					record.Status = ObjectRecordStatus.ReferenceSolved;
				}
				if (this._selector != null)
				{
					record.Surrogate = this._selector.GetSurrogate(obj.GetType(), this._context, out record.SurrogateSelector);
					if (record.Surrogate != null)
					{
						record.Status = ObjectRecordStatus.ReferenceUnsolved;
					}
				}
				record.DoFixups(true, this, false);
				record.DoFixups(false, this, false);
				this._registeredObjectsCount++;
				if (this._objectRecordChain == null)
				{
					this._objectRecordChain = record;
					this._lastObjectRecord = record;
				}
				else
				{
					this._lastObjectRecord.Next = record;
					this._lastObjectRecord = record;
				}
				return;
			}
			if (record.OriginalObject != obj)
			{
				throw new SerializationException("An object with Id " + record.ObjectID + " has already been registered");
			}
		}

		/// <summary>Registers an object as it is deserialized, associating it with <paramref name="objectID" />.</summary>
		/// <param name="obj">The object to register. </param>
		/// <param name="objectID">The ID of the object to register. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="objectID" /> has already been registered for an object other than <paramref name="obj" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public virtual void RegisterObject(object obj, long objectID)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "The obj parameter is null.");
			}
			if (objectID <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectID", "The objectID parameter is less than or equal to zero");
			}
			this.RegisterObjectInternal(obj, this.GetObjectRecord(objectID));
		}

		/// <summary>Registers an object as it is deserialized, associating it with <paramref name="objectID" />, and recording the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> used with it.</summary>
		/// <param name="obj">The object to register. </param>
		/// <param name="objectID">The ID of the object to register. </param>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> used if <paramref name="obj" /> implements <see cref="T:System.Runtime.Serialization.ISerializable" /> or has a <see cref="T:System.Runtime.Serialization.ISerializationSurrogate" />. <paramref name="info" /> will be completed with any required fixup information and then passed to the required object when that object is completed. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="objectID" /> has already been registered for an object other than <paramref name="obj" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public void RegisterObject(object obj, long objectID, SerializationInfo info)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "The obj parameter is null.");
			}
			if (objectID <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectID", "The objectID parameter is less than or equal to zero");
			}
			ObjectRecord objectRecord = this.GetObjectRecord(objectID);
			objectRecord.Info = info;
			this.RegisterObjectInternal(obj, objectRecord);
		}

		/// <summary>Registers a member of an object as it is deserialized, associating it with <paramref name="objectID" />, and recording the <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
		/// <param name="obj">The object to register. </param>
		/// <param name="objectID">The ID of the object to register. </param>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> used if <paramref name="obj" /> implements <see cref="T:System.Runtime.Serialization.ISerializable" /> or has a <see cref="T:System.Runtime.Serialization.ISerializationSurrogate" />. <paramref name="info" /> will be completed with any required fixup information and then passed to the required object when that object is completed. </param>
		/// <param name="idOfContainingObj">The ID of the object that contains <paramref name="obj" />. This parameter is required only if <paramref name="obj" /> is a value type. </param>
		/// <param name="member">The field in the containing object where <paramref name="obj" /> exists. This parameter has meaning only if <paramref name="obj" /> is a value type. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="objectID" /> has already been registered for an object other than <paramref name="obj" />, or <paramref name="member" /> is not a <see cref="T:System.Reflection.FieldInfo" /> and <paramref name="member" /> is not null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public void RegisterObject(object obj, long objectID, SerializationInfo info, long idOfContainingObj, MemberInfo member)
		{
			this.RegisterObject(obj, objectID, info, idOfContainingObj, member, null);
		}

		/// <summary>Registers a member of an array contained in an object while it is deserialized, associating it with <paramref name="objectID" />, and recording the <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
		/// <param name="obj">The object to register. </param>
		/// <param name="objectID">The ID of the object to register. </param>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> used if <paramref name="obj" /> implements <see cref="T:System.Runtime.Serialization.ISerializable" /> or has a <see cref="T:System.Runtime.Serialization.ISerializationSurrogate" />. <paramref name="info" /> will be completed with any required fixup information and then passed to the required object when that object is completed. </param>
		/// <param name="idOfContainingObj">The ID of the object that contains <paramref name="obj" />. This parameter is required only if <paramref name="obj" /> is a value type. </param>
		/// <param name="member">The field in the containing object where <paramref name="obj" /> exists. This parameter has meaning only if <paramref name="obj" /> is a value type. </param>
		/// <param name="arrayIndex">If <paramref name="obj" /> is a <see cref="T:System.ValueType" /> and a member of an array, <paramref name="arrayIndex" /> contains the index within that array where <paramref name="obj" /> exists. <paramref name="arrayIndex" /> is ignored if <paramref name="obj" /> is not both a <see cref="T:System.ValueType" /> and a member of an array. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="objectID" /> parameter is less than or equal to zero. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="objectID" /> has already been registered for an object other than <paramref name="obj" />, or <paramref name="member" /> is not a <see cref="T:System.Reflection.FieldInfo" /> and <paramref name="member" /> isn't null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public void RegisterObject(object obj, long objectID, SerializationInfo info, long idOfContainingObj, MemberInfo member, int[] arrayIndex)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "The obj parameter is null.");
			}
			if (objectID <= 0L)
			{
				throw new ArgumentOutOfRangeException("objectID", "The objectID parameter is less than or equal to zero");
			}
			ObjectRecord objectRecord = this.GetObjectRecord(objectID);
			objectRecord.Info = info;
			objectRecord.IdOfContainingObj = idOfContainingObj;
			objectRecord.Member = member;
			objectRecord.ArrayIndex = arrayIndex;
			this.RegisterObjectInternal(obj, objectRecord);
		}
	}
}
