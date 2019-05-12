using System;
using System.Runtime.InteropServices;

namespace System
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct Variant
	{
		[FieldOffset(0)]
		public short vt;

		[FieldOffset(2)]
		public ushort wReserved1;

		[FieldOffset(4)]
		public ushort wReserved2;

		[FieldOffset(6)]
		public ushort wReserved3;

		[FieldOffset(8)]
		public long llVal;

		[FieldOffset(8)]
		public int lVal;

		[FieldOffset(8)]
		public byte bVal;

		[FieldOffset(8)]
		public short iVal;

		[FieldOffset(8)]
		public float fltVal;

		[FieldOffset(8)]
		public double dblVal;

		[FieldOffset(8)]
		public short boolVal;

		[FieldOffset(8)]
		public IntPtr bstrVal;

		[FieldOffset(8)]
		public sbyte cVal;

		[FieldOffset(8)]
		public ushort uiVal;

		[FieldOffset(8)]
		public uint ulVal;

		[FieldOffset(8)]
		public ulong ullVal;

		[FieldOffset(8)]
		public int intVal;

		[FieldOffset(8)]
		public uint uintVal;

		[FieldOffset(8)]
		public IntPtr pdispVal;

		[FieldOffset(8)]
		public BRECORD bRecord;

		public void SetValue(object obj)
		{
			this.vt = 0;
			if (obj == null)
			{
				return;
			}
			Type type = obj.GetType();
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			if (type == typeof(sbyte))
			{
				this.vt = 16;
				this.cVal = (sbyte)obj;
			}
			else if (type == typeof(byte))
			{
				this.vt = 17;
				this.bVal = (byte)obj;
			}
			else if (type == typeof(short))
			{
				this.vt = 2;
				this.iVal = (short)obj;
			}
			else if (type == typeof(ushort))
			{
				this.vt = 18;
				this.uiVal = (ushort)obj;
			}
			else if (type == typeof(int))
			{
				this.vt = 3;
				this.lVal = (int)obj;
			}
			else if (type == typeof(uint))
			{
				this.vt = 19;
				this.ulVal = (uint)obj;
			}
			else if (type == typeof(long))
			{
				this.vt = 20;
				this.llVal = (long)obj;
			}
			else if (type == typeof(ulong))
			{
				this.vt = 21;
				this.ullVal = (ulong)obj;
			}
			else if (type == typeof(float))
			{
				this.vt = 4;
				this.fltVal = (float)obj;
			}
			else if (type == typeof(double))
			{
				this.vt = 5;
				this.dblVal = (double)obj;
			}
			else if (type == typeof(string))
			{
				this.vt = 8;
				this.bstrVal = Marshal.StringToBSTR((string)obj);
			}
			else if (type == typeof(bool))
			{
				this.vt = 11;
				this.lVal = ((!(bool)obj) ? 0 : -1);
			}
			else if (type == typeof(BStrWrapper))
			{
				this.vt = 8;
				this.bstrVal = Marshal.StringToBSTR(((BStrWrapper)obj).WrappedObject);
			}
			else if (type == typeof(UnknownWrapper))
			{
				this.vt = 13;
				this.pdispVal = Marshal.GetIUnknownForObject(((UnknownWrapper)obj).WrappedObject);
			}
			else if (type == typeof(DispatchWrapper))
			{
				this.vt = 9;
				this.pdispVal = Marshal.GetIDispatchForObject(((DispatchWrapper)obj).WrappedObject);
			}
			else
			{
				try
				{
					this.pdispVal = Marshal.GetIDispatchForObject(obj);
					this.vt = 9;
					return;
				}
				catch
				{
				}
				try
				{
					this.vt = 13;
					this.pdispVal = Marshal.GetIUnknownForObject(obj);
				}
				catch (Exception inner)
				{
					throw new NotImplementedException(string.Format("Variant couldn't handle object of type {0}", obj.GetType()), inner);
				}
			}
		}

		public object GetValue()
		{
			object result = null;
			switch (this.vt)
			{
			case 2:
				result = this.iVal;
				break;
			case 3:
				result = this.lVal;
				break;
			case 4:
				result = this.fltVal;
				break;
			case 5:
				result = this.dblVal;
				break;
			case 8:
				result = Marshal.PtrToStringBSTR(this.bstrVal);
				break;
			case 9:
			case 13:
				if (this.pdispVal != IntPtr.Zero)
				{
					result = Marshal.GetObjectForIUnknown(this.pdispVal);
				}
				break;
			case 11:
				result = (this.boolVal != 0);
				break;
			case 16:
				result = this.cVal;
				break;
			case 17:
				result = this.bVal;
				break;
			case 18:
				result = this.uiVal;
				break;
			case 19:
				result = this.ulVal;
				break;
			case 20:
				result = this.llVal;
				break;
			case 21:
				result = this.ullVal;
				break;
			}
			return result;
		}

		public void Clear()
		{
			if (this.vt == 8)
			{
				Marshal.FreeBSTR(this.bstrVal);
			}
			else if ((this.vt == 9 || this.vt == 13) && this.pdispVal != IntPtr.Zero)
			{
				Marshal.Release(this.pdispVal);
			}
		}
	}
}
