using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>Represents a multicast delegate; that is, a delegate that can have more than one element in its invocation list.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public abstract class MulticastDelegate : Delegate
	{
		private MulticastDelegate prev;

		private MulticastDelegate kpm_next;

		/// <summary>Initializes a new instance of the <see cref="T:System.MulticastDelegate" /> class.</summary>
		/// <param name="target">The object on which <paramref name="method" /> is defined. </param>
		/// <param name="method">The name of the method for which a delegate is created. </param>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		protected MulticastDelegate(object target, string method) : base(target, method)
		{
			this.prev = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MulticastDelegate" /> class.</summary>
		/// <param name="target">The type of object on which <paramref name="method" /> is defined. </param>
		/// <param name="method">The name of the static method for which a delegate is created. </param>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		protected MulticastDelegate(Type target, string method) : base(target, method)
		{
			this.prev = null;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with all the data needed to serialize this instance.</summary>
		/// <param name="info">An object that holds all the data needed to serialize or deserialize this instance. </param>
		/// <param name="context">(Reserved) The location where serialized data is stored and retrieved. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A serialization error occurred.</exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		/// <summary>Processes the full invocation list.</summary>
		/// <returns>An array of type <see cref="T:System.Object" /> that contains the return value of the encapsulated method.</returns>
		/// <param name="args">The arguments to pass to the encapsulated method. </param>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.-or- There is an attempt to invoke a method to which the caller does not have access (that is, a private method). </exception>
		/// <exception cref="T:System.ArgumentException">The number, order, or type of parameters is invalid. </exception>
		/// <exception cref="T:System.Reflection.TargetException">An encapsulated method is not static, and the target object is null.-or- There is an attempt to invoke a method on an object or class that does not support the method. </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">One of the encapsulated methods throws an exception. </exception>
		protected sealed override object DynamicInvokeImpl(object[] args)
		{
			if (this.prev != null)
			{
				this.prev.DynamicInvokeImpl(args);
			}
			return base.DynamicInvokeImpl(args);
		}

		/// <summary>Determines whether this multicast delegate and the specified object are equal.</summary>
		/// <returns>true if <paramref name="obj" /> and this instance have the same invocation lists; otherwise, false.</returns>
		/// <param name="obj">The object to compare with this instance. </param>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <filterpriority>2</filterpriority>
		public sealed override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			MulticastDelegate multicastDelegate = obj as MulticastDelegate;
			if (multicastDelegate == null)
			{
				return false;
			}
			if (this.prev == null)
			{
				return multicastDelegate.prev == null;
			}
			return this.prev.Equals(multicastDelegate.prev);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <filterpriority>2</filterpriority>
		public sealed override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Returns the invocation list of this multicast delegate, in invocation order.</summary>
		/// <returns>An array of delegates whose invocation lists collectively match the invocation list of this instance.</returns>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <filterpriority>2</filterpriority>
		public sealed override Delegate[] GetInvocationList()
		{
			MulticastDelegate multicastDelegate = (MulticastDelegate)this.Clone();
			multicastDelegate.kpm_next = null;
			while (multicastDelegate.prev != null)
			{
				multicastDelegate.prev.kpm_next = multicastDelegate;
				multicastDelegate = multicastDelegate.prev;
			}
			if (multicastDelegate.kpm_next == null)
			{
				MulticastDelegate multicastDelegate2 = (MulticastDelegate)multicastDelegate.Clone();
				multicastDelegate2.prev = null;
				multicastDelegate2.kpm_next = null;
				return new Delegate[]
				{
					multicastDelegate2
				};
			}
			ArrayList arrayList = new ArrayList();
			while (multicastDelegate != null)
			{
				MulticastDelegate multicastDelegate3 = (MulticastDelegate)multicastDelegate.Clone();
				multicastDelegate3.prev = null;
				multicastDelegate3.kpm_next = null;
				arrayList.Add(multicastDelegate3);
				multicastDelegate = multicastDelegate.kpm_next;
			}
			return (Delegate[])arrayList.ToArray(typeof(Delegate));
		}

		/// <summary>Combines this <see cref="T:System.Delegate" /> with the specified <see cref="T:System.Delegate" /> to form a new delegate.</summary>
		/// <returns>A <see cref="T:System.Delegate" /> that is the new root of the <see cref="T:System.MulticastDelegate" /> invocation list.</returns>
		/// <param name="follow">The delegate to combine with this delegate. </param>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		protected sealed override Delegate CombineImpl(Delegate follow)
		{
			if (base.GetType() != follow.GetType())
			{
				throw new ArgumentException(Locale.GetText("Incompatible Delegate Types."));
			}
			MulticastDelegate multicastDelegate = (MulticastDelegate)follow.Clone();
			multicastDelegate.SetMulticastInvoke();
			MulticastDelegate multicastDelegate2 = multicastDelegate;
			for (MulticastDelegate multicastDelegate3 = ((MulticastDelegate)follow).prev; multicastDelegate3 != null; multicastDelegate3 = multicastDelegate3.prev)
			{
				multicastDelegate2.prev = (MulticastDelegate)multicastDelegate3.Clone();
				multicastDelegate2 = multicastDelegate2.prev;
			}
			multicastDelegate2.prev = (MulticastDelegate)this.Clone();
			multicastDelegate2 = multicastDelegate2.prev;
			for (MulticastDelegate multicastDelegate3 = this.prev; multicastDelegate3 != null; multicastDelegate3 = multicastDelegate3.prev)
			{
				multicastDelegate2.prev = (MulticastDelegate)multicastDelegate3.Clone();
				multicastDelegate2 = multicastDelegate2.prev;
			}
			return multicastDelegate;
		}

		private bool BaseEquals(MulticastDelegate value)
		{
			return base.Equals(value);
		}

		private static MulticastDelegate KPM(MulticastDelegate needle, MulticastDelegate haystack, out MulticastDelegate tail)
		{
			MulticastDelegate multicastDelegate = needle;
			MulticastDelegate multicastDelegate2 = needle.kpm_next = null;
			for (;;)
			{
				while (multicastDelegate2 != null && !multicastDelegate2.BaseEquals(multicastDelegate))
				{
					multicastDelegate2 = multicastDelegate2.kpm_next;
				}
				multicastDelegate = multicastDelegate.prev;
				if (multicastDelegate == null)
				{
					break;
				}
				multicastDelegate2 = ((multicastDelegate2 != null) ? multicastDelegate2.prev : needle);
				if (multicastDelegate.BaseEquals(multicastDelegate2))
				{
					multicastDelegate.kpm_next = multicastDelegate2.kpm_next;
				}
				else
				{
					multicastDelegate.kpm_next = multicastDelegate2;
				}
			}
			MulticastDelegate multicastDelegate3 = haystack;
			multicastDelegate2 = needle;
			multicastDelegate = haystack;
			for (;;)
			{
				while (multicastDelegate2 != null && !multicastDelegate2.BaseEquals(multicastDelegate))
				{
					multicastDelegate2 = multicastDelegate2.kpm_next;
					multicastDelegate3 = multicastDelegate3.prev;
				}
				multicastDelegate2 = ((multicastDelegate2 != null) ? multicastDelegate2.prev : needle);
				if (multicastDelegate2 == null)
				{
					break;
				}
				multicastDelegate = multicastDelegate.prev;
				if (multicastDelegate == null)
				{
					goto Block_8;
				}
			}
			tail = multicastDelegate.prev;
			return multicastDelegate3;
			Block_8:
			tail = null;
			return null;
		}

		/// <summary>Removes an element from the invocation list of this <see cref="T:System.MulticastDelegate" /> that is equal to the specified delegate.</summary>
		/// <returns>If <paramref name="value" /> is found in the invocation list for this instance, then a new <see cref="T:System.Delegate" /> without <paramref name="value" /> in its invocation list; otherwise, this instance with its original invocation list.</returns>
		/// <param name="value">The delegate to search for in the invocation list. </param>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		protected sealed override Delegate RemoveImpl(Delegate value)
		{
			if (value == null)
			{
				return this;
			}
			MulticastDelegate multicastDelegate2;
			MulticastDelegate multicastDelegate = MulticastDelegate.KPM((MulticastDelegate)value, this, out multicastDelegate2);
			if (multicastDelegate == null)
			{
				return this;
			}
			MulticastDelegate multicastDelegate3 = null;
			MulticastDelegate result = null;
			for (MulticastDelegate multicastDelegate4 = this; multicastDelegate4 != multicastDelegate; multicastDelegate4 = multicastDelegate4.prev)
			{
				MulticastDelegate multicastDelegate5 = (MulticastDelegate)multicastDelegate4.Clone();
				if (multicastDelegate3 != null)
				{
					multicastDelegate3.prev = multicastDelegate5;
				}
				else
				{
					result = multicastDelegate5;
				}
				multicastDelegate3 = multicastDelegate5;
			}
			for (MulticastDelegate multicastDelegate4 = multicastDelegate2; multicastDelegate4 != null; multicastDelegate4 = multicastDelegate4.prev)
			{
				MulticastDelegate multicastDelegate6 = (MulticastDelegate)multicastDelegate4.Clone();
				if (multicastDelegate3 != null)
				{
					multicastDelegate3.prev = multicastDelegate6;
				}
				else
				{
					result = multicastDelegate6;
				}
				multicastDelegate3 = multicastDelegate6;
			}
			if (multicastDelegate3 != null)
			{
				multicastDelegate3.prev = null;
			}
			return result;
		}

		/// <summary>Determines whether two <see cref="T:System.MulticastDelegate" /> objects are equal.</summary>
		/// <returns>true if <paramref name="d1" /> and <paramref name="d2" /> have the same invocation lists; otherwise, false.</returns>
		/// <param name="d1">The left operand. </param>
		/// <param name="d2">The right operand. </param>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <filterpriority>3</filterpriority>
		public static bool operator ==(MulticastDelegate d1, MulticastDelegate d2)
		{
			if (d1 == null)
			{
				return d2 == null;
			}
			return d1.Equals(d2);
		}

		/// <summary>Determines whether two <see cref="T:System.MulticastDelegate" /> objects are not equal.</summary>
		/// <returns>true if <paramref name="d1" /> and <paramref name="d2" /> do not have the same invocation lists; otherwise, false.</returns>
		/// <param name="d1">The left operand. </param>
		/// <param name="d2">The right operand. </param>
		/// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
		/// <filterpriority>3</filterpriority>
		public static bool operator !=(MulticastDelegate d1, MulticastDelegate d2)
		{
			if (d1 == null)
			{
				return d2 != null;
			}
			return !d1.Equals(d2);
		}
	}
}
