using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Defines the set of information that constitutes input to security policy decisions. This class cannot be inherited.</summary>
	[MonoTODO("Serialization format not compatible with .NET")]
	[ComVisible(true)]
	[Serializable]
	public sealed class Evidence : IEnumerable, ICollection
	{
		private bool _locked;

		private ArrayList hostEvidenceList;

		private ArrayList assemblyEvidenceList;

		private int _hashCode;

		/// <summary>Initializes a new empty instance of the <see cref="T:System.Security.Policy.Evidence" /> class.</summary>
		public Evidence()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.Evidence" /> class from a shallow copy of an existing one.</summary>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> instance from which to create the new instance. This instance is not deep copied. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="evidence" /> parameter is not a valid instance of <see cref="T:System.Security.Policy.Evidence" />. </exception>
		public Evidence(Evidence evidence)
		{
			if (evidence != null)
			{
				this.Merge(evidence);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.Evidence" /> class from multiple sets of host and assembly evidence.</summary>
		/// <param name="hostEvidence">The host evidence from which to create the new instance. </param>
		/// <param name="assemblyEvidence">The assembly evidence from which to create the new instance. </param>
		public Evidence(object[] hostEvidence, object[] assemblyEvidence)
		{
			if (hostEvidence != null)
			{
				this.HostEvidenceList.AddRange(hostEvidence);
			}
			if (assemblyEvidence != null)
			{
				this.AssemblyEvidenceList.AddRange(assemblyEvidence);
			}
		}

		/// <summary>Gets the number of evidence objects in the evidence set.</summary>
		/// <returns>The number of evidence objects in the evidence set.</returns>
		public int Count
		{
			get
			{
				int num = 0;
				if (this.hostEvidenceList != null)
				{
					num += this.hostEvidenceList.Count;
				}
				if (this.assemblyEvidenceList != null)
				{
					num += this.assemblyEvidenceList.Count;
				}
				return num;
			}
		}

		/// <summary>Gets a value indicating whether the evidence set is read-only.</summary>
		/// <returns>Always false because read-only evidence sets are not supported.</returns>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the evidence set is thread-safe.</summary>
		/// <returns>Always false because thread-safe evidence sets are not supported.</returns>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets a value indicating whether the evidence is locked.</summary>
		/// <returns>true if the evidence is locked; otherwise, false. The default is false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public bool Locked
		{
			get
			{
				return this._locked;
			}
			set
			{
				this._locked = value;
			}
		}

		/// <summary>Gets the synchronization root.</summary>
		/// <returns>Always this (Me in Visual Basic) because synchronization of evidence sets is not supported.</returns>
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		internal ArrayList HostEvidenceList
		{
			get
			{
				if (this.hostEvidenceList == null)
				{
					this.hostEvidenceList = ArrayList.Synchronized(new ArrayList());
				}
				return this.hostEvidenceList;
			}
		}

		internal ArrayList AssemblyEvidenceList
		{
			get
			{
				if (this.assemblyEvidenceList == null)
				{
					this.assemblyEvidenceList = ArrayList.Synchronized(new ArrayList());
				}
				return this.assemblyEvidenceList;
			}
		}

		/// <summary>Adds the specified assembly evidence to the evidence set.</summary>
		/// <param name="id">Any evidence object. </param>
		public void AddAssembly(object id)
		{
			this.AssemblyEvidenceList.Add(id);
			this._hashCode = 0;
		}

		/// <summary>Adds the specified evidence supplied by the host to the evidence set.</summary>
		/// <param name="id">Any evidence object. </param>
		/// <exception cref="T:System.Security.SecurityException">
		///   <see cref="P:System.Security.Policy.Evidence.Locked" /> is true and the code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlEvidence" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public void AddHost(object id)
		{
			if (this._locked && SecurityManager.SecurityEnabled)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
			}
			this.HostEvidenceList.Add(id);
			this._hashCode = 0;
		}

		/// <summary>Removes the host and assembly evidence from the evidence set.</summary>
		[ComVisible(false)]
		public void Clear()
		{
			if (this.hostEvidenceList != null)
			{
				this.hostEvidenceList.Clear();
			}
			if (this.assemblyEvidenceList != null)
			{
				this.assemblyEvidenceList.Clear();
			}
			this._hashCode = 0;
		}

		/// <summary>Copies evidence objects to an <see cref="T:System.Array" />.</summary>
		/// <param name="array">The target array to which to copy evidence objects. </param>
		/// <param name="index">The zero-based position in the array to which to begin copying evidence objects. </param>
		public void CopyTo(Array array, int index)
		{
			int num = 0;
			if (this.hostEvidenceList != null)
			{
				num = this.hostEvidenceList.Count;
				if (num > 0)
				{
					this.hostEvidenceList.CopyTo(array, index);
				}
			}
			if (this.assemblyEvidenceList != null && this.assemblyEvidenceList.Count > 0)
			{
				this.assemblyEvidenceList.CopyTo(array, index + num);
			}
		}

		/// <summary>Determines whether the specified <see cref="T:System.Security.Policy.Evidence" /> object is equal to the current <see cref="T:System.Security.Policy.Evidence" />.</summary>
		/// <returns>true if the specified <see cref="T:System.Security.Policy.Evidence" /> object is equal to the current <see cref="T:System.Security.Policy.Evidence" />; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Security.Policy.Evidence" /> object to compare with the current <see cref="T:System.Security.Policy.Evidence" />. </param>
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Evidence evidence = obj as Evidence;
			if (evidence == null)
			{
				return false;
			}
			if (this.HostEvidenceList.Count != evidence.HostEvidenceList.Count)
			{
				return false;
			}
			if (this.AssemblyEvidenceList.Count != evidence.AssemblyEvidenceList.Count)
			{
				return false;
			}
			for (int i = 0; i < this.hostEvidenceList.Count; i++)
			{
				bool flag = false;
				int j = 0;
				while (j < evidence.hostEvidenceList.Count)
				{
					if (this.hostEvidenceList[i].Equals(evidence.hostEvidenceList[j]))
					{
						flag = true;
						break;
					}
					i++;
				}
				if (!flag)
				{
					return false;
				}
			}
			for (int k = 0; k < this.assemblyEvidenceList.Count; k++)
			{
				bool flag2 = false;
				int l = 0;
				while (l < evidence.assemblyEvidenceList.Count)
				{
					if (this.assemblyEvidenceList[k].Equals(evidence.assemblyEvidenceList[l]))
					{
						flag2 = true;
						break;
					}
					k++;
				}
				if (!flag2)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Enumerates all evidence in the set, both that provided by the host and that provided by the assembly.</summary>
		/// <returns>An enumerator for evidence added by both the <see cref="M:System.Security.Policy.Evidence.AddHost(System.Object)" /> method and the <see cref="M:System.Security.Policy.Evidence.AddAssembly(System.Object)" /> method.</returns>
		public IEnumerator GetEnumerator()
		{
			IEnumerator hostenum = null;
			if (this.hostEvidenceList != null)
			{
				hostenum = this.hostEvidenceList.GetEnumerator();
			}
			IEnumerator assemblyenum = null;
			if (this.assemblyEvidenceList != null)
			{
				assemblyenum = this.assemblyEvidenceList.GetEnumerator();
			}
			return new Evidence.EvidenceEnumerator(hostenum, assemblyenum);
		}

		/// <summary>Enumerates evidence provided by the assembly.</summary>
		/// <returns>An enumerator for evidence added by the <see cref="M:System.Security.Policy.Evidence.AddAssembly(System.Object)" /> method.</returns>
		public IEnumerator GetAssemblyEnumerator()
		{
			return this.AssemblyEvidenceList.GetEnumerator();
		}

		/// <summary>Gets a hash code for the <see cref="T:System.Security.Policy.Evidence" /> object that is suitable for use in hashing algorithms and data structures such as a hash table.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Security.Policy.Evidence" /> object.</returns>
		[ComVisible(false)]
		public override int GetHashCode()
		{
			if (this._hashCode == 0)
			{
				if (this.hostEvidenceList != null)
				{
					for (int i = 0; i < this.hostEvidenceList.Count; i++)
					{
						this._hashCode ^= this.hostEvidenceList[i].GetHashCode();
					}
				}
				if (this.assemblyEvidenceList != null)
				{
					for (int j = 0; j < this.assemblyEvidenceList.Count; j++)
					{
						this._hashCode ^= this.assemblyEvidenceList[j].GetHashCode();
					}
				}
			}
			return this._hashCode;
		}

		/// <summary>Enumerates evidence supplied by the host.</summary>
		/// <returns>An enumerator for evidence added by the <see cref="M:System.Security.Policy.Evidence.AddHost(System.Object)" /> method.</returns>
		public IEnumerator GetHostEnumerator()
		{
			return this.HostEvidenceList.GetEnumerator();
		}

		/// <summary>Merges the specified evidence set into the current evidence set.</summary>
		/// <param name="evidence">The evidence set to be merged into the current evidence set. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="evidence" /> parameter is not a valid instance of <see cref="T:System.Security.Policy.Evidence" />. </exception>
		/// <exception cref="T:System.Security.SecurityException">
		///   <see cref="P:System.Security.Policy.Evidence.Locked" /> is true, the code that calls this method does not have <see cref="F:System.Security.Permissions.SecurityPermissionFlag.ControlEvidence" />, and the <paramref name="evidence" /> parameter has a host list that is not empty. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public void Merge(Evidence evidence)
		{
			if (evidence != null && evidence.Count > 0)
			{
				if (evidence.hostEvidenceList != null)
				{
					foreach (object id in evidence.hostEvidenceList)
					{
						this.AddHost(id);
					}
				}
				if (evidence.assemblyEvidenceList != null)
				{
					foreach (object id2 in evidence.assemblyEvidenceList)
					{
						this.AddAssembly(id2);
					}
				}
				this._hashCode = 0;
			}
		}

		/// <summary>Removes the evidence for a given type from the host and assembly enumerations.</summary>
		/// <param name="t">The <see cref="T:System.Type" /> of the evidence to be removed. </param>
		[ComVisible(false)]
		public void RemoveType(Type t)
		{
			for (int i = this.hostEvidenceList.Count; i >= 0; i--)
			{
				if (this.hostEvidenceList.GetType() == t)
				{
					this.hostEvidenceList.RemoveAt(i);
					this._hashCode = 0;
				}
			}
			for (int j = this.assemblyEvidenceList.Count; j >= 0; j--)
			{
				if (this.assemblyEvidenceList.GetType() == t)
				{
					this.assemblyEvidenceList.RemoveAt(j);
					this._hashCode = 0;
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsAuthenticodePresent(Assembly a);

		internal static Evidence GetDefaultHostEvidence(Assembly a)
		{
			return new Evidence();
		}

		private class EvidenceEnumerator : IEnumerator
		{
			private IEnumerator currentEnum;

			private IEnumerator hostEnum;

			private IEnumerator assemblyEnum;

			public EvidenceEnumerator(IEnumerator hostenum, IEnumerator assemblyenum)
			{
				this.hostEnum = hostenum;
				this.assemblyEnum = assemblyenum;
				this.currentEnum = this.hostEnum;
			}

			public bool MoveNext()
			{
				if (this.currentEnum == null)
				{
					return false;
				}
				bool flag = this.currentEnum.MoveNext();
				if (!flag && this.hostEnum == this.currentEnum && this.assemblyEnum != null)
				{
					this.currentEnum = this.assemblyEnum;
					flag = this.assemblyEnum.MoveNext();
				}
				return flag;
			}

			public void Reset()
			{
				if (this.hostEnum != null)
				{
					this.hostEnum.Reset();
					this.currentEnum = this.hostEnum;
				}
				else
				{
					this.currentEnum = this.assemblyEnum;
				}
				if (this.assemblyEnum != null)
				{
					this.assemblyEnum.Reset();
				}
			}

			public object Current
			{
				get
				{
					return this.currentEnum.Current;
				}
			}
		}
	}
}
