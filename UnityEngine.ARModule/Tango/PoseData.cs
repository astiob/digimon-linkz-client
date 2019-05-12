using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
	[UsedByNativeCode]
	[NativeHeader("ARScriptingClasses.h")]
	[StructLayout(LayoutKind.Explicit, Size = 92)]
	internal struct PoseData
	{
		[FieldOffset(0)]
		public uint version;

		[FieldOffset(8)]
		public double timestamp;

		[FieldOffset(16)]
		public double orientation_x;

		[FieldOffset(24)]
		public double orientation_y;

		[FieldOffset(32)]
		public double orientation_z;

		[FieldOffset(40)]
		public double orientation_w;

		[FieldOffset(48)]
		public double translation_x;

		[FieldOffset(56)]
		public double translation_y;

		[FieldOffset(64)]
		public double translation_z;

		[FieldOffset(72)]
		public PoseStatus statusCode;

		[FieldOffset(76)]
		public CoordinateFramePair frame;

		[FieldOffset(84)]
		public uint confidence;

		[FieldOffset(88)]
		public float accuracy;

		public Quaternion rotation
		{
			get
			{
				return new Quaternion((float)this.orientation_x, (float)this.orientation_y, (float)this.orientation_z, (float)this.orientation_w);
			}
		}

		public Vector3 position
		{
			get
			{
				return new Vector3((float)this.translation_x, (float)this.translation_y, (float)this.translation_z);
			}
		}
	}
}
