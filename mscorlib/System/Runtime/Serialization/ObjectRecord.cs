using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	internal class ObjectRecord
	{
		public ObjectRecordStatus Status;

		public object OriginalObject;

		public object ObjectInstance;

		public long ObjectID;

		public SerializationInfo Info;

		public long IdOfContainingObj;

		public ISerializationSurrogate Surrogate;

		public ISurrogateSelector SurrogateSelector;

		public MemberInfo Member;

		public int[] ArrayIndex;

		public BaseFixupRecord FixupChainAsContainer;

		public BaseFixupRecord FixupChainAsRequired;

		public ObjectRecord Next;

		public void SetMemberValue(ObjectManager manager, MemberInfo member, object value)
		{
			if (member is FieldInfo)
			{
				((FieldInfo)member).SetValue(this.ObjectInstance, value);
			}
			else
			{
				if (!(member is PropertyInfo))
				{
					throw new SerializationException("Cannot perform fixup");
				}
				((PropertyInfo)member).SetValue(this.ObjectInstance, value, null);
			}
			if (this.Member != null)
			{
				ObjectRecord objectRecord = manager.GetObjectRecord(this.IdOfContainingObj);
				if (objectRecord.IsRegistered)
				{
					objectRecord.SetMemberValue(manager, this.Member, this.ObjectInstance);
				}
			}
			else if (this.ArrayIndex != null)
			{
				ObjectRecord objectRecord2 = manager.GetObjectRecord(this.IdOfContainingObj);
				if (objectRecord2.IsRegistered)
				{
					objectRecord2.SetArrayValue(manager, this.ObjectInstance, this.ArrayIndex);
				}
			}
		}

		public void SetArrayValue(ObjectManager manager, object value, int[] indices)
		{
			((Array)this.ObjectInstance).SetValue(value, indices);
		}

		public void SetMemberValue(ObjectManager manager, string memberName, object value)
		{
			if (this.Info == null)
			{
				throw new SerializationException("Cannot perform fixup");
			}
			this.Info.AddValue(memberName, value, value.GetType());
		}

		public bool IsInstanceReady
		{
			get
			{
				return this.IsRegistered && !this.IsUnsolvedObjectReference && (!this.ObjectInstance.GetType().IsValueType || (!this.HasPendingFixups && this.Info == null));
			}
		}

		public bool IsUnsolvedObjectReference
		{
			get
			{
				return this.Status != ObjectRecordStatus.ReferenceSolved;
			}
		}

		public bool IsRegistered
		{
			get
			{
				return this.Status != ObjectRecordStatus.Unregistered;
			}
		}

		public bool DoFixups(bool asContainer, ObjectManager manager, bool strict)
		{
			BaseFixupRecord prevFixup = null;
			BaseFixupRecord baseFixupRecord = (!asContainer) ? this.FixupChainAsRequired : this.FixupChainAsContainer;
			bool result = true;
			while (baseFixupRecord != null)
			{
				if (baseFixupRecord.DoFixup(manager, strict))
				{
					this.UnchainFixup(baseFixupRecord, prevFixup, asContainer);
					if (asContainer)
					{
						baseFixupRecord.ObjectRequired.RemoveFixup(baseFixupRecord, false);
					}
					else
					{
						baseFixupRecord.ObjectToBeFixed.RemoveFixup(baseFixupRecord, true);
					}
				}
				else
				{
					prevFixup = baseFixupRecord;
					result = false;
				}
				baseFixupRecord = ((!asContainer) ? baseFixupRecord.NextSameRequired : baseFixupRecord.NextSameContainer);
			}
			return result;
		}

		public void RemoveFixup(BaseFixupRecord fixupToRemove, bool asContainer)
		{
			BaseFixupRecord prevFixup = null;
			for (BaseFixupRecord baseFixupRecord = (!asContainer) ? this.FixupChainAsRequired : this.FixupChainAsContainer; baseFixupRecord != null; baseFixupRecord = ((!asContainer) ? baseFixupRecord.NextSameRequired : baseFixupRecord.NextSameContainer))
			{
				if (baseFixupRecord == fixupToRemove)
				{
					this.UnchainFixup(baseFixupRecord, prevFixup, asContainer);
					return;
				}
				prevFixup = baseFixupRecord;
			}
		}

		private void UnchainFixup(BaseFixupRecord fixup, BaseFixupRecord prevFixup, bool asContainer)
		{
			if (prevFixup == null)
			{
				if (asContainer)
				{
					this.FixupChainAsContainer = fixup.NextSameContainer;
				}
				else
				{
					this.FixupChainAsRequired = fixup.NextSameRequired;
				}
			}
			else if (asContainer)
			{
				prevFixup.NextSameContainer = fixup.NextSameContainer;
			}
			else
			{
				prevFixup.NextSameRequired = fixup.NextSameRequired;
			}
		}

		public void ChainFixup(BaseFixupRecord fixup, bool asContainer)
		{
			if (asContainer)
			{
				fixup.NextSameContainer = this.FixupChainAsContainer;
				this.FixupChainAsContainer = fixup;
			}
			else
			{
				fixup.NextSameRequired = this.FixupChainAsRequired;
				this.FixupChainAsRequired = fixup;
			}
		}

		public bool LoadData(ObjectManager manager, ISurrogateSelector selector, StreamingContext context)
		{
			if (this.Info != null)
			{
				if (this.Surrogate != null)
				{
					object obj = this.Surrogate.SetObjectData(this.ObjectInstance, this.Info, context, this.SurrogateSelector);
					if (obj != null)
					{
						this.ObjectInstance = obj;
					}
					this.Status = ObjectRecordStatus.ReferenceSolved;
				}
				else
				{
					if (!(this.ObjectInstance is ISerializable))
					{
						throw new SerializationException("No surrogate selector was found for type " + this.ObjectInstance.GetType().FullName);
					}
					object[] parameters = new object[]
					{
						this.Info,
						context
					};
					ConstructorInfo constructor = this.ObjectInstance.GetType().GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(SerializationInfo),
						typeof(StreamingContext)
					}, null);
					if (constructor == null)
					{
						throw new SerializationException("The constructor to deserialize an object of type " + this.ObjectInstance.GetType().FullName + " was not found.");
					}
					constructor.Invoke(this.ObjectInstance, parameters);
				}
				this.Info = null;
			}
			if (this.ObjectInstance is IObjectReference && this.Status != ObjectRecordStatus.ReferenceSolved)
			{
				try
				{
					this.ObjectInstance = ((IObjectReference)this.ObjectInstance).GetRealObject(context);
					int num = 100;
					while (this.ObjectInstance is IObjectReference && num > 0)
					{
						object realObject = ((IObjectReference)this.ObjectInstance).GetRealObject(context);
						if (realObject == this.ObjectInstance)
						{
							break;
						}
						this.ObjectInstance = realObject;
						num--;
					}
					if (num == 0)
					{
						throw new SerializationException("The implementation of the IObjectReference interface returns too many nested references to other objects that implement IObjectReference.");
					}
					this.Status = ObjectRecordStatus.ReferenceSolved;
				}
				catch (NullReferenceException)
				{
					return false;
				}
			}
			if (this.Member != null)
			{
				ObjectRecord objectRecord = manager.GetObjectRecord(this.IdOfContainingObj);
				objectRecord.SetMemberValue(manager, this.Member, this.ObjectInstance);
			}
			else if (this.ArrayIndex != null)
			{
				ObjectRecord objectRecord2 = manager.GetObjectRecord(this.IdOfContainingObj);
				objectRecord2.SetArrayValue(manager, this.ObjectInstance, this.ArrayIndex);
			}
			return true;
		}

		public bool HasPendingFixups
		{
			get
			{
				return this.FixupChainAsContainer != null;
			}
		}
	}
}
