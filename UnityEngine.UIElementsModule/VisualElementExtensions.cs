using System;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEngine.Experimental.UIElements
{
	public static class VisualElementExtensions
	{
		public static Vector2 WorldToLocal(this VisualElement ele, Vector2 p)
		{
			Vector3 vector = ele.worldTransform.inverse.MultiplyPoint3x4(new Vector3(p.x, p.y, 0f));
			return new Vector2(vector.x - ele.layout.position.x, vector.y - ele.layout.position.y);
		}

		public static Vector2 LocalToWorld(this VisualElement ele, Vector2 p)
		{
			Vector3 vector = ele.worldTransform.MultiplyPoint3x4(p + ele.layout.position);
			return new Vector2(vector.x, vector.y);
		}

		public static Rect WorldToLocal(this VisualElement ele, Rect r)
		{
			Matrix4x4 inverse = ele.worldTransform.inverse;
			Vector2 a = inverse.MultiplyPoint3x4(r.position);
			r.position = a - ele.layout.position;
			r.size = inverse.MultiplyVector(r.size);
			return r;
		}

		public static Rect LocalToWorld(this VisualElement ele, Rect r)
		{
			Matrix4x4 worldTransform = ele.worldTransform;
			r.position = worldTransform.MultiplyPoint3x4(ele.layout.position + r.position);
			r.size = worldTransform.MultiplyVector(r.size);
			return r;
		}

		public static Vector2 ChangeCoordinatesTo(this VisualElement src, VisualElement dest, Vector2 point)
		{
			return dest.WorldToLocal(src.LocalToWorld(point));
		}

		public static Rect ChangeCoordinatesTo(this VisualElement src, VisualElement dest, Rect rect)
		{
			return dest.WorldToLocal(src.LocalToWorld(rect));
		}

		public static void StretchToParentSize(this VisualElement elem)
		{
			IStyle style = elem.style;
			style.positionType = PositionType.Absolute;
			style.positionLeft = 0f;
			style.positionTop = 0f;
			style.positionRight = 0f;
			style.positionBottom = 0f;
		}

		public static void AddManipulator(this VisualElement ele, IManipulator manipulator)
		{
			manipulator.target = ele;
		}

		public static void RemoveManipulator(this VisualElement ele, IManipulator manipulator)
		{
			manipulator.target = null;
		}
	}
}
