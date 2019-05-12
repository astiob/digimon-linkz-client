using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Position, rotation and scale of an object.</para>
	/// </summary>
	public class Transform : Component, IEnumerable
	{
		protected Transform()
		{
		}

		/// <summary>
		///   <para>The position of the transform in world space.</para>
		/// </summary>
		public Vector3 position
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_position(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_position(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_position(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_position(ref Vector3 value);

		/// <summary>
		///   <para>Position of the transform relative to the parent transform.</para>
		/// </summary>
		public Vector3 localPosition
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_localPosition(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_localPosition(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localPosition(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_localPosition(ref Vector3 value);

		/// <summary>
		///   <para>The rotation as Euler angles in degrees.</para>
		/// </summary>
		public Vector3 eulerAngles
		{
			get
			{
				return this.rotation.eulerAngles;
			}
			set
			{
				this.rotation = Quaternion.Euler(value);
			}
		}

		/// <summary>
		///   <para>The rotation as Euler angles in degrees relative to the parent transform's rotation.</para>
		/// </summary>
		public Vector3 localEulerAngles
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_localEulerAngles(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_localEulerAngles(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localEulerAngles(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_localEulerAngles(ref Vector3 value);

		/// <summary>
		///   <para>The red axis of the transform in world space.</para>
		/// </summary>
		public Vector3 right
		{
			get
			{
				return this.rotation * Vector3.right;
			}
			set
			{
				this.rotation = Quaternion.FromToRotation(Vector3.right, value);
			}
		}

		/// <summary>
		///   <para>The green axis of the transform in world space.</para>
		/// </summary>
		public Vector3 up
		{
			get
			{
				return this.rotation * Vector3.up;
			}
			set
			{
				this.rotation = Quaternion.FromToRotation(Vector3.up, value);
			}
		}

		/// <summary>
		///   <para>The blue axis of the transform in world space.</para>
		/// </summary>
		public Vector3 forward
		{
			get
			{
				return this.rotation * Vector3.forward;
			}
			set
			{
				this.rotation = Quaternion.LookRotation(value);
			}
		}

		/// <summary>
		///   <para>The rotation of the transform in world space stored as a Quaternion.</para>
		/// </summary>
		public Quaternion rotation
		{
			get
			{
				Quaternion result;
				this.INTERNAL_get_rotation(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_rotation(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rotation(out Quaternion value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_rotation(ref Quaternion value);

		/// <summary>
		///   <para>The rotation of the transform relative to the parent transform's rotation.</para>
		/// </summary>
		public Quaternion localRotation
		{
			get
			{
				Quaternion result;
				this.INTERNAL_get_localRotation(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_localRotation(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localRotation(out Quaternion value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_localRotation(ref Quaternion value);

		/// <summary>
		///   <para>The scale of the transform relative to the parent.</para>
		/// </summary>
		public Vector3 localScale
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_localScale(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_localScale(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localScale(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_localScale(ref Vector3 value);

		/// <summary>
		///   <para>The parent of the transform.</para>
		/// </summary>
		public Transform parent
		{
			get
			{
				return this.parentInternal;
			}
			set
			{
				if (this is RectTransform)
				{
					Debug.LogWarning("Parent of RectTransform is being set with parent property. Consider using the SetParent method instead, with the worldPositionStays argument set to false. This will retain local orientation and scale rather than world orientation and scale, which can prevent common UI scaling issues.", this);
				}
				this.parentInternal = value;
			}
		}

		internal extern Transform parentInternal { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void SetParent(Transform parent)
		{
			this.SetParent(parent, true);
		}

		/// <summary>
		///   <para>Set the parent of the transform.</para>
		/// </summary>
		/// <param name="parent">The parent Transform to use.</param>
		/// <param name="worldPositionStays">If true, the parent-relative position, scale and rotation is modified such that the object keeps the same world space position, rotation and scale as before.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetParent(Transform parent, bool worldPositionStays);

		/// <summary>
		///   <para>Matrix that transforms a point from world space into local space (Read Only).</para>
		/// </summary>
		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				Matrix4x4 result;
				this.INTERNAL_get_worldToLocalMatrix(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_worldToLocalMatrix(out Matrix4x4 value);

		/// <summary>
		///   <para>Matrix that transforms a point from local space into world space (Read Only).</para>
		/// </summary>
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				Matrix4x4 result;
				this.INTERNAL_get_localToWorldMatrix(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localToWorldMatrix(out Matrix4x4 value);

		/// <summary>
		///   <para>Moves the transform in the direction and distance of translation.</para>
		/// </summary>
		/// <param name="translation"></param>
		/// <param name="relativeTo"></param>
		[ExcludeFromDocs]
		public void Translate(Vector3 translation)
		{
			Space relativeTo = Space.Self;
			this.Translate(translation, relativeTo);
		}

		/// <summary>
		///   <para>Moves the transform in the direction and distance of translation.</para>
		/// </summary>
		/// <param name="translation"></param>
		/// <param name="relativeTo"></param>
		public void Translate(Vector3 translation, [DefaultValue("Space.Self")] Space relativeTo)
		{
			if (relativeTo == Space.World)
			{
				this.position += translation;
			}
			else
			{
				this.position += this.TransformDirection(translation);
			}
		}

		/// <summary>
		///   <para>Moves the transform by x along the x axis, y along the y axis, and z along the z axis.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="relativeTo"></param>
		[ExcludeFromDocs]
		public void Translate(float x, float y, float z)
		{
			Space relativeTo = Space.Self;
			this.Translate(x, y, z, relativeTo);
		}

		/// <summary>
		///   <para>Moves the transform by x along the x axis, y along the y axis, and z along the z axis.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="relativeTo"></param>
		public void Translate(float x, float y, float z, [DefaultValue("Space.Self")] Space relativeTo)
		{
			this.Translate(new Vector3(x, y, z), relativeTo);
		}

		/// <summary>
		///   <para>Moves the transform in the direction and distance of translation.</para>
		/// </summary>
		/// <param name="translation"></param>
		/// <param name="relativeTo"></param>
		public void Translate(Vector3 translation, Transform relativeTo)
		{
			if (relativeTo)
			{
				this.position += relativeTo.TransformDirection(translation);
			}
			else
			{
				this.position += translation;
			}
		}

		/// <summary>
		///   <para>Moves the transform by x along the x axis, y along the y axis, and z along the z axis.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="relativeTo"></param>
		public void Translate(float x, float y, float z, Transform relativeTo)
		{
			this.Translate(new Vector3(x, y, z), relativeTo);
		}

		/// <summary>
		///   <para>Applies a rotation of eulerAngles.z degrees around the z axis, eulerAngles.x degrees around the x axis, and eulerAngles.y degrees around the y axis (in that order).</para>
		/// </summary>
		/// <param name="eulerAngles"></param>
		/// <param name="relativeTo"></param>
		[ExcludeFromDocs]
		public void Rotate(Vector3 eulerAngles)
		{
			Space relativeTo = Space.Self;
			this.Rotate(eulerAngles, relativeTo);
		}

		/// <summary>
		///   <para>Applies a rotation of eulerAngles.z degrees around the z axis, eulerAngles.x degrees around the x axis, and eulerAngles.y degrees around the y axis (in that order).</para>
		/// </summary>
		/// <param name="eulerAngles"></param>
		/// <param name="relativeTo"></param>
		public void Rotate(Vector3 eulerAngles, [DefaultValue("Space.Self")] Space relativeTo)
		{
			Quaternion rhs = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
			if (relativeTo == Space.Self)
			{
				this.localRotation *= rhs;
			}
			else
			{
				this.rotation *= Quaternion.Inverse(this.rotation) * rhs * this.rotation;
			}
		}

		/// <summary>
		///   <para>Applies a rotation of zAngle degrees around the z axis, xAngle degrees around the x axis, and yAngle degrees around the y axis (in that order).</para>
		/// </summary>
		/// <param name="xAngle"></param>
		/// <param name="yAngle"></param>
		/// <param name="zAngle"></param>
		/// <param name="relativeTo"></param>
		[ExcludeFromDocs]
		public void Rotate(float xAngle, float yAngle, float zAngle)
		{
			Space relativeTo = Space.Self;
			this.Rotate(xAngle, yAngle, zAngle, relativeTo);
		}

		/// <summary>
		///   <para>Applies a rotation of zAngle degrees around the z axis, xAngle degrees around the x axis, and yAngle degrees around the y axis (in that order).</para>
		/// </summary>
		/// <param name="xAngle"></param>
		/// <param name="yAngle"></param>
		/// <param name="zAngle"></param>
		/// <param name="relativeTo"></param>
		public void Rotate(float xAngle, float yAngle, float zAngle, [DefaultValue("Space.Self")] Space relativeTo)
		{
			this.Rotate(new Vector3(xAngle, yAngle, zAngle), relativeTo);
		}

		internal void RotateAroundInternal(Vector3 axis, float angle)
		{
			Transform.INTERNAL_CALL_RotateAroundInternal(this, ref axis, angle);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_RotateAroundInternal(Transform self, ref Vector3 axis, float angle);

		/// <summary>
		///   <para>Rotates the transform around axis by angle degrees.</para>
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="angle"></param>
		/// <param name="relativeTo"></param>
		[ExcludeFromDocs]
		public void Rotate(Vector3 axis, float angle)
		{
			Space relativeTo = Space.Self;
			this.Rotate(axis, angle, relativeTo);
		}

		/// <summary>
		///   <para>Rotates the transform around axis by angle degrees.</para>
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="angle"></param>
		/// <param name="relativeTo"></param>
		public void Rotate(Vector3 axis, float angle, [DefaultValue("Space.Self")] Space relativeTo)
		{
			if (relativeTo == Space.Self)
			{
				this.RotateAroundInternal(base.transform.TransformDirection(axis), angle * 0.0174532924f);
			}
			else
			{
				this.RotateAroundInternal(axis, angle * 0.0174532924f);
			}
		}

		/// <summary>
		///   <para>Rotates the transform about axis passing through point in world coordinates by angle degrees.</para>
		/// </summary>
		/// <param name="point"></param>
		/// <param name="axis"></param>
		/// <param name="angle"></param>
		public void RotateAround(Vector3 point, Vector3 axis, float angle)
		{
			Vector3 vector = this.position;
			Quaternion rotation = Quaternion.AngleAxis(angle, axis);
			Vector3 vector2 = vector - point;
			vector2 = rotation * vector2;
			vector = point + vector2;
			this.position = vector;
			this.RotateAroundInternal(axis, angle * 0.0174532924f);
		}

		/// <summary>
		///   <para>Rotates the transform so the forward vector points at target's current position.</para>
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		/// <param name="worldUp">Vector specifying the upward direction.</param>
		[ExcludeFromDocs]
		public void LookAt(Transform target)
		{
			Vector3 up = Vector3.up;
			this.LookAt(target, up);
		}

		/// <summary>
		///   <para>Rotates the transform so the forward vector points at target's current position.</para>
		/// </summary>
		/// <param name="target">Object to point towards.</param>
		/// <param name="worldUp">Vector specifying the upward direction.</param>
		public void LookAt(Transform target, [DefaultValue("Vector3.up")] Vector3 worldUp)
		{
			if (target)
			{
				this.LookAt(target.position, worldUp);
			}
		}

		/// <summary>
		///   <para>Rotates the transform so the forward vector points at worldPosition.</para>
		/// </summary>
		/// <param name="worldPosition">Point to look at.</param>
		/// <param name="worldUp">Vector specifying the upward direction.</param>
		public void LookAt(Vector3 worldPosition, [DefaultValue("Vector3.up")] Vector3 worldUp)
		{
			Transform.INTERNAL_CALL_LookAt(this, ref worldPosition, ref worldUp);
		}

		/// <summary>
		///   <para>Rotates the transform so the forward vector points at worldPosition.</para>
		/// </summary>
		/// <param name="worldPosition">Point to look at.</param>
		/// <param name="worldUp">Vector specifying the upward direction.</param>
		[ExcludeFromDocs]
		public void LookAt(Vector3 worldPosition)
		{
			Vector3 up = Vector3.up;
			Transform.INTERNAL_CALL_LookAt(this, ref worldPosition, ref up);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_LookAt(Transform self, ref Vector3 worldPosition, ref Vector3 worldUp);

		/// <summary>
		///   <para>Transforms direction from local space to world space.</para>
		/// </summary>
		/// <param name="direction"></param>
		public Vector3 TransformDirection(Vector3 direction)
		{
			return Transform.INTERNAL_CALL_TransformDirection(this, ref direction);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_TransformDirection(Transform self, ref Vector3 direction);

		/// <summary>
		///   <para>Transforms direction x, y, z from local space to world space.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3 TransformDirection(float x, float y, float z)
		{
			return this.TransformDirection(new Vector3(x, y, z));
		}

		/// <summary>
		///   <para>Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.</para>
		/// </summary>
		/// <param name="direction"></param>
		public Vector3 InverseTransformDirection(Vector3 direction)
		{
			return Transform.INTERNAL_CALL_InverseTransformDirection(this, ref direction);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_InverseTransformDirection(Transform self, ref Vector3 direction);

		/// <summary>
		///   <para>Transforms the direction x, y, z from world space to local space. The opposite of Transform.TransformDirection.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3 InverseTransformDirection(float x, float y, float z)
		{
			return this.InverseTransformDirection(new Vector3(x, y, z));
		}

		/// <summary>
		///   <para>Transforms vector from local space to world space.</para>
		/// </summary>
		/// <param name="vector"></param>
		public Vector3 TransformVector(Vector3 vector)
		{
			return Transform.INTERNAL_CALL_TransformVector(this, ref vector);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_TransformVector(Transform self, ref Vector3 vector);

		/// <summary>
		///   <para>Transforms vector x, y, z from local space to world space.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3 TransformVector(float x, float y, float z)
		{
			return this.TransformVector(new Vector3(x, y, z));
		}

		/// <summary>
		///   <para>Transforms a vector from world space to local space. The opposite of Transform.TransformVector.</para>
		/// </summary>
		/// <param name="vector"></param>
		public Vector3 InverseTransformVector(Vector3 vector)
		{
			return Transform.INTERNAL_CALL_InverseTransformVector(this, ref vector);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_InverseTransformVector(Transform self, ref Vector3 vector);

		/// <summary>
		///   <para>Transforms the vector x, y, z from world space to local space. The opposite of Transform.TransformVector.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3 InverseTransformVector(float x, float y, float z)
		{
			return this.InverseTransformVector(new Vector3(x, y, z));
		}

		/// <summary>
		///   <para>Transforms position from local space to world space.</para>
		/// </summary>
		/// <param name="position"></param>
		public Vector3 TransformPoint(Vector3 position)
		{
			return Transform.INTERNAL_CALL_TransformPoint(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_TransformPoint(Transform self, ref Vector3 position);

		/// <summary>
		///   <para>Transforms the position x, y, z from local space to world space.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3 TransformPoint(float x, float y, float z)
		{
			return this.TransformPoint(new Vector3(x, y, z));
		}

		/// <summary>
		///   <para>Transforms position from world space to local space.</para>
		/// </summary>
		/// <param name="position"></param>
		public Vector3 InverseTransformPoint(Vector3 position)
		{
			return Transform.INTERNAL_CALL_InverseTransformPoint(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_InverseTransformPoint(Transform self, ref Vector3 position);

		/// <summary>
		///   <para>Transforms the position x, y, z from world space to local space. The opposite of Transform.TransformPoint.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public Vector3 InverseTransformPoint(float x, float y, float z)
		{
			return this.InverseTransformPoint(new Vector3(x, y, z));
		}

		/// <summary>
		///   <para>Returns the topmost transform in the hierarchy.</para>
		/// </summary>
		public extern Transform root { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The number of children the Transform has.</para>
		/// </summary>
		public extern int childCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Unparents all children.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DetachChildren();

		/// <summary>
		///   <para>Move the transform to the start of the local transform list.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAsFirstSibling();

		/// <summary>
		///   <para>Move the transform to the end of the local transform list.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAsLastSibling();

		/// <summary>
		///   <para>Sets the sibling index.</para>
		/// </summary>
		/// <param name="index">Index to set.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetSiblingIndex(int index);

		/// <summary>
		///   <para>Gets the sibling index.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetSiblingIndex();

		/// <summary>
		///   <para>Finds a child by name and returns it.</para>
		/// </summary>
		/// <param name="name">Name of child to be found.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Transform Find(string name);

		/// <summary>
		///   <para>The global scale of the object (Read Only).</para>
		/// </summary>
		public Vector3 lossyScale
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_lossyScale(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_lossyScale(out Vector3 value);

		/// <summary>
		///   <para>Is this transform a child of parent?</para>
		/// </summary>
		/// <param name="parent"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsChildOf(Transform parent);

		/// <summary>
		///   <para>Has the transform changed since the last time the flag was set to 'false'?</para>
		/// </summary>
		public extern bool hasChanged { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Transform FindChild(string name)
		{
			return this.Find(name);
		}

		public IEnumerator GetEnumerator()
		{
			return new Transform.Enumerator(this);
		}

		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="axis"></param>
		/// <param name="angle"></param>
		[Obsolete("use Transform.Rotate instead.")]
		public void RotateAround(Vector3 axis, float angle)
		{
			Transform.INTERNAL_CALL_RotateAround(this, ref axis, angle);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_RotateAround(Transform self, ref Vector3 axis, float angle);

		[Obsolete("use Transform.Rotate instead.")]
		public void RotateAroundLocal(Vector3 axis, float angle)
		{
			Transform.INTERNAL_CALL_RotateAroundLocal(this, ref axis, angle);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_RotateAroundLocal(Transform self, ref Vector3 axis, float angle);

		/// <summary>
		///   <para>Returns a transform child by index.</para>
		/// </summary>
		/// <param name="index">Index of the child transform to return. Must be smaller than Transform.childCount.</param>
		/// <returns>
		///   <para>Transform child by index.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Transform GetChild(int index);

		[WrapperlessIcall]
		[Obsolete("use Transform.childCount instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetChildCount();

		private sealed class Enumerator : IEnumerator
		{
			private Transform outer;

			private int currentIndex = -1;

			internal Enumerator(Transform outer)
			{
				this.outer = outer;
			}

			public object Current
			{
				get
				{
					return this.outer.GetChild(this.currentIndex);
				}
			}

			public bool MoveNext()
			{
				int childCount = this.outer.childCount;
				return ++this.currentIndex < childCount;
			}

			public void Reset()
			{
				this.currentIndex = -1;
			}
		}
	}
}
