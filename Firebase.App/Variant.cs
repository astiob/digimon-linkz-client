using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Firebase
{
	internal class Variant : IDisposable
	{
		private HandleRef swigCPtr;

		protected bool swigCMemOwn;

		internal Variant(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		internal static HandleRef getCPtr(Variant obj)
		{
			return (obj != null) ? obj.swigCPtr : new HandleRef(null, IntPtr.Zero);
		}

		~Variant()
		{
			this.Dispose();
		}

		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						AppUtilPINVOKE.delete_Variant(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public static Variant FromBlob(byte[] blob)
		{
			Variant variant = Variant.EmptyMutableBlob((uint)blob.Length);
			Marshal.Copy(blob, 0, variant.untyped_mutable_blob_data(), blob.Length);
			return variant;
		}

		public byte[] blob_as_bytes()
		{
			byte[] array = new byte[this.blob_size()];
			Marshal.Copy(this.untyped_mutable_blob_data(), array, 0, array.Length);
			return array;
		}

		public static Variant FromObject(object o)
		{
			if (o == null)
			{
				return Variant.Null();
			}
			if (o is string)
			{
				return Variant.FromString((string)o);
			}
			if (o is long)
			{
				return Variant.FromInt64((long)o);
			}
			if (o is double)
			{
				return Variant.FromDouble((double)o);
			}
			if (o is bool)
			{
				return Variant.FromBool((bool)o);
			}
			if (o is byte[])
			{
				return Variant.FromBlob((byte[])o);
			}
			if (o is IList)
			{
				IList list = (IList)o;
				Variant variant = Variant.EmptyVector();
				VariantList variantList = variant.vector();
				IEnumerator enumerator = list.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object o2 = enumerator.Current;
						variantList.Add(Variant.FromObject(o2));
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				return variant;
			}
			if (o is IDictionary)
			{
				IDictionary dictionary = (IDictionary)o;
				Variant variant2 = Variant.EmptyMap();
				VariantVariantMap variantVariantMap = variant2.map();
				IDictionaryEnumerator enumerator2 = dictionary.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj = enumerator2.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						Variant key = Variant.FromObject(dictionaryEntry.Key);
						Variant value = Variant.FromObject(dictionaryEntry.Value);
						variantVariantMap[key] = value;
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				return variant2;
			}
			if (o is byte || o is short || o is int || o is sbyte || o is ushort || o is uint)
			{
				return Variant.FromInt64(Convert.ToInt64(o));
			}
			if (o is float)
			{
				return Variant.FromDouble(Convert.ToDouble(o));
			}
			throw new ArgumentException("Invalid type " + o.GetType() + " for conversion to Variant");
		}

		public static Variant Null()
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_Null(), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Variant EmptyVector()
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_EmptyVector(), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Variant EmptyMap()
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_EmptyMap(), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Variant EmptyMutableBlob(uint sizeBytes)
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_EmptyMutableBlob(sizeBytes), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Variant.Type type()
		{
			Variant.Type result = (Variant.Type)AppUtilPINVOKE.Variant_type(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool is_string()
		{
			bool result = AppUtilPINVOKE.Variant_is_string(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool is_fundamental_type()
		{
			bool result = AppUtilPINVOKE.Variant_is_fundamental_type(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Variant AsString()
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_AsString(this.swigCPtr), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public uint blob_size()
		{
			uint result = AppUtilPINVOKE.Variant_blob_size(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public VariantList vector()
		{
			VariantList result = new VariantList(AppUtilPINVOKE.Variant_vector__SWIG_0(this.swigCPtr), false);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public VariantVariantMap map()
		{
			VariantVariantMap result = new VariantVariantMap(AppUtilPINVOKE.Variant_map__SWIG_0(this.swigCPtr), false);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public long int64_value()
		{
			long result = AppUtilPINVOKE.Variant_int64_value(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public double double_value()
		{
			double result = AppUtilPINVOKE.Variant_double_value(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool bool_value()
		{
			bool result = AppUtilPINVOKE.Variant_bool_value(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public string string_value()
		{
			string result = AppUtilPINVOKE.Variant_string_value(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Variant FromInt64(long value)
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_FromInt64(value), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Variant FromDouble(double value)
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_FromDouble(value), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Variant FromBool(bool value)
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_FromBool(value), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Variant FromString(string value)
		{
			Variant result = new Variant(AppUtilPINVOKE.Variant_FromString(value), true);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public IntPtr untyped_mutable_blob_data()
		{
			IntPtr result = AppUtilPINVOKE.Variant_untyped_mutable_blob_data(this.swigCPtr);
			if (AppUtilPINVOKE.SWIGPendingException.Pending)
			{
				throw AppUtilPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public enum Type
		{
			Null,
			Int64,
			Double,
			Bool,
			StaticString,
			MutableString,
			Vector,
			Map,
			StaticBlob,
			MutableBlob
		}
	}
}
