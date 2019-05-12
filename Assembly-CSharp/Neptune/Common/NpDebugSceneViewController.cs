using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neptune.Common
{
	public class NpDebugSceneViewController : MonoBehaviour
	{
		private static NpDebugSceneViewController s_This;

		private List<NpDebugSceneViewController.NpDebugSceneViewControllerDataBase>[] m_List;

		private int m_ListBuf;

		private int m_PrevListBuf;

		private bool m_IsUpdate;

		public static void DrawSector(Vector3 pos, Vector3 dir, float deg, Color col)
		{
			NpDebugSceneViewController.DrawSector(pos, dir, deg, 4, 1f, 1f, Vector3.up, col);
		}

		public static void DrawSector(Vector3 pos, Vector3 dir, float deg, int divide, Color col)
		{
			NpDebugSceneViewController.DrawSector(pos, dir, deg, divide, 1f, 1f, Vector3.up, col);
		}

		public static void DrawSector(Vector3 pos, Vector3 dir, float deg, int divide, float length, Color col)
		{
			NpDebugSceneViewController.DrawSector(pos, dir, deg, divide, length, 1f, Vector3.up, col);
		}

		public static void DrawSector(Vector3 pos, Vector3 dir, float deg, int divide, float length, float height, Color col)
		{
			NpDebugSceneViewController.DrawSector(pos, dir, deg, divide, length, height, Vector3.up, col);
		}

		public static void DrawSector(Vector3 pos, Vector3 dir, float deg, int divide, float length, float height, Vector3 upVec, Color col)
		{
		}

		public static void DrawSphere(Vector3 center, float radius, Color col)
		{
		}

		public static void DrawBox(Transform trans, Vector3 size, Color col)
		{
			NpDebugSceneViewController.DrawBox(trans.position, size, trans.rotation, col);
		}

		public static void DrawBox(Vector3 center, Vector3 size, Quaternion rotate, Color col)
		{
		}

		public static void DrawCylinder(Vector3 center, float radius, float height, Color col)
		{
			NpDebugSceneViewController.DrawCylinder(center, radius, height, 8, Vector3.up, col);
		}

		public static void DrawCylinder(Vector3 center, float radius, float height, int divide, Color col)
		{
			NpDebugSceneViewController.DrawCylinder(center, radius, height, divide, Vector3.up, col);
		}

		public static void DrawCylinder(Vector3 center, float radius, float height, int divide, Vector3 upVec, Color col)
		{
		}

		public static void DrawArrowRay(Vector3 pos, Vector3 dir, Color col)
		{
		}

		public static void DrawArrow(Vector3 beginPos, Vector3 endPos, Color col)
		{
		}

		public static void DrawPoint(Vector3 pos, Color col)
		{
			NpDebugSceneViewController.DrawPoint(pos, 1f, col);
		}

		public static void DrawPoint(Vector3 pos, float scale, Color col)
		{
		}

		private static NpDebugSceneViewController GetInstance()
		{
			if (NpDebugSceneViewController.s_This == null)
			{
				GameObject gameObject = new GameObject("NpDebugSceneViewController");
				NpDebugSceneViewController.s_This = gameObject.AddComponent<NpDebugSceneViewController>();
			}
			return NpDebugSceneViewController.s_This;
		}

		private void Awake()
		{
			if (NpDebugSceneViewController.s_This == null)
			{
				NpDebugSceneViewController.s_This = this;
				this.m_List = new List<NpDebugSceneViewController.NpDebugSceneViewControllerDataBase>[2];
				this.m_List[0] = new List<NpDebugSceneViewController.NpDebugSceneViewControllerDataBase>();
				this.m_List[1] = new List<NpDebugSceneViewController.NpDebugSceneViewControllerDataBase>();
				this.m_ListBuf = 0;
				this.m_PrevListBuf = 0;
				this.m_IsUpdate = false;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		public void AddData(NpDebugSceneViewController.NpDebugSceneViewControllerDataBase data)
		{
			this.m_List[this.m_ListBuf].Add(data);
		}

		private void LateUpdate()
		{
			this.m_IsUpdate = true;
		}

		private void GizmosFunc()
		{
			if (this.m_List[this.m_PrevListBuf].Count > 0)
			{
				for (int i = 0; i < this.m_List[this.m_PrevListBuf].Count; i++)
				{
					NpDebugSceneViewController.NpDebugSceneViewControllerDataBase npDebugSceneViewControllerDataBase = this.m_List[this.m_PrevListBuf][i];
					switch (npDebugSceneViewControllerDataBase.m_DataType)
					{
					case NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.SECTOR:
						this.DrawSectorFunc((NpDebugSceneViewController.NpDebugSceneViewControllerDataSector)npDebugSceneViewControllerDataBase);
						break;
					case NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.SPHERE:
						this.DrawSphereFunc((NpDebugSceneViewController.NpDebugSceneViewControllerDataSphere)npDebugSceneViewControllerDataBase);
						break;
					case NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.BOX:
						this.DrawBoxFunc((NpDebugSceneViewController.NpDebugSceneViewControllerDataBox)npDebugSceneViewControllerDataBase);
						break;
					case NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.CYLINDER:
						this.DrawSylinderFunc((NpDebugSceneViewController.NpDebugSceneViewControllerDataCylinder)npDebugSceneViewControllerDataBase);
						break;
					case NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.ARROW:
						this.DrawArrowFunc((NpDebugSceneViewController.NpDebugSceneViewControllerDataArrow)npDebugSceneViewControllerDataBase);
						break;
					case NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.POINT:
						this.DrawPointFunc((NpDebugSceneViewController.NpDebugSceneViewControllerDataPoint)npDebugSceneViewControllerDataBase);
						break;
					}
				}
			}
		}

		private void DrawSectorFunc(NpDebugSceneViewController.NpDebugSceneViewControllerDataSector data)
		{
			Gizmos.color = data.m_Color;
			Quaternion rotation = Quaternion.AngleAxis(data.m_Deg, data.m_Up);
			Quaternion rotation2 = Quaternion.AngleAxis(-data.m_Deg, data.m_Up);
			Vector3 b = rotation * data.m_Dir * data.m_Length;
			Vector3 vector = rotation2 * data.m_Dir * data.m_Length;
			Vector3 vector2 = data.m_Pos + data.m_Up * data.m_Height;
			Vector3 pos = data.m_Pos;
			Gizmos.DrawLine(vector2, pos);
			Gizmos.DrawLine(vector2, vector2 + b);
			Gizmos.DrawLine(vector2, vector2 + vector);
			Gizmos.DrawLine(pos, pos + b);
			Gizmos.DrawLine(pos, pos + vector);
			float num = data.m_Deg * 2f;
			Vector3 from = vector2 + vector;
			Vector3 from2 = pos + vector;
			Gizmos.DrawLine(vector2 + vector, pos + vector);
			for (int i = 1; i <= data.m_Divide; i++)
			{
				Quaternion rotation3 = Quaternion.AngleAxis(num / (float)data.m_Divide * (float)i, data.m_Up);
				Vector3 b2 = rotation3 * vector;
				Vector3 vector3 = vector2 + b2;
				Vector3 vector4 = pos + b2;
				Gizmos.DrawLine(from, vector3);
				Gizmos.DrawLine(from2, vector4);
				Gizmos.DrawLine(vector3, vector4);
				from = vector3;
				from2 = vector4;
			}
		}

		private void DrawSphereFunc(NpDebugSceneViewController.NpDebugSceneViewControllerDataSphere data)
		{
			Gizmos.color = data.m_Color;
			Gizmos.DrawSphere(data.m_Center, data.m_Radius);
		}

		private void DrawBoxFunc(NpDebugSceneViewController.NpDebugSceneViewControllerDataBox data)
		{
			Gizmos.color = data.m_Color;
			Gizmos.matrix = Matrix4x4.TRS(data.m_Center, data.m_Rotate, Vector3.one);
			Gizmos.DrawCube(Vector3.zero, data.m_Size);
			Gizmos.matrix = Matrix4x4.identity;
		}

		private void DrawSylinderFunc(NpDebugSceneViewController.NpDebugSceneViewControllerDataCylinder data)
		{
			Gizmos.color = data.m_Color;
			Vector3 vector = data.m_Front * data.m_Radius;
			Vector3 vector2 = data.m_Center + data.m_Up * data.m_Height;
			Vector3 center = data.m_Center;
			Vector3 vector3 = vector2 + vector;
			Vector3 vector4 = center + vector;
			for (int i = 1; i <= data.m_Divide; i++)
			{
				Quaternion rotation = Quaternion.AngleAxis(360f / (float)data.m_Divide * (float)i, data.m_Up);
				Vector3 b = rotation * vector;
				Vector3 vector5 = vector2 + b;
				Vector3 vector6 = center + b;
				Gizmos.DrawLine(vector3, vector5);
				Gizmos.DrawLine(vector4, vector6);
				Gizmos.DrawLine(vector5, vector6);
				Gizmos.DrawLine(vector2, vector3);
				Gizmos.DrawLine(center, vector4);
				vector3 = vector5;
				vector4 = vector6;
			}
		}

		private void DrawArrowFunc(NpDebugSceneViewController.NpDebugSceneViewControllerDataArrow data)
		{
			Gizmos.color = data.m_Color;
			Gizmos.DrawLine(data.m_BeginPos, data.m_EndPos);
			Vector3 vector = data.m_BeginPos - data.m_EndPos;
			float d = vector.magnitude * 0.1f;
			vector.Normalize();
			Vector3 vector2 = Vector3.zero;
			if (vector != Vector3.up)
			{
				vector2 = Vector3.Cross(vector, Vector3.up);
			}
			else
			{
				vector2 = Vector3.Cross(vector, Vector3.forward);
			}
			Quaternion rotation = Quaternion.AngleAxis(45f, vector2);
			Gizmos.DrawLine(data.m_EndPos, data.m_EndPos + rotation * vector * d);
			rotation = Quaternion.AngleAxis(315f, vector2);
			Gizmos.DrawLine(data.m_EndPos, data.m_EndPos + rotation * vector * d);
			Vector3 axis = Vector3.Cross(vector, vector2);
			rotation = Quaternion.AngleAxis(45f, axis);
			Gizmos.DrawLine(data.m_EndPos, data.m_EndPos + rotation * vector * d);
			rotation = Quaternion.AngleAxis(315f, axis);
			Gizmos.DrawLine(data.m_EndPos, data.m_EndPos + rotation * vector * d);
		}

		private void DrawPointFunc(NpDebugSceneViewController.NpDebugSceneViewControllerDataPoint data)
		{
			Gizmos.color = data.m_Color;
			Gizmos.DrawLine(data.m_Pos - Vector3.forward, data.m_Pos + Vector3.forward * data.m_Scale);
			Gizmos.DrawLine(data.m_Pos - Vector3.up, data.m_Pos + Vector3.up * data.m_Scale);
			Gizmos.DrawLine(data.m_Pos - Vector3.right, data.m_Pos + Vector3.right * data.m_Scale);
		}

		private void OnDrawGizmos()
		{
		}

		private void OnDrawGizmosSelected()
		{
		}

		public class NpDebugSceneViewControllerDataBase
		{
			public NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType m_DataType;

			public Color m_Color;

			public NpDebugSceneViewControllerDataBase()
			{
				this.m_DataType = NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.NONE;
				this.m_Color = Color.white;
			}

			public enum eDataType
			{
				NONE,
				SECTOR,
				SPHERE,
				BOX,
				CYLINDER,
				ARROW,
				POINT
			}
		}

		public class NpDebugSceneViewControllerDataSector : NpDebugSceneViewController.NpDebugSceneViewControllerDataBase
		{
			public Vector3 m_Pos;

			public Vector3 m_Dir;

			public float m_Deg;

			public float m_Length;

			public Vector3 m_Up;

			public float m_Height;

			public int m_Divide;

			public NpDebugSceneViewControllerDataSector(Vector3 pos, Vector3 dir, float deg, float length, Vector3 upVec, float height, int divide, Color col)
			{
				this.m_DataType = NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.SECTOR;
				this.m_Color = col;
				this.m_Pos = pos;
				this.m_Dir = dir;
				this.m_Deg = deg;
				this.m_Length = length;
				this.m_Up = upVec;
				this.m_Height = height;
				this.m_Divide = divide;
			}
		}

		public class NpDebugSceneViewControllerDataSphere : NpDebugSceneViewController.NpDebugSceneViewControllerDataBase
		{
			public Vector3 m_Center;

			public float m_Radius;

			public NpDebugSceneViewControllerDataSphere(Vector3 center, float radius, Color col)
			{
				this.m_DataType = NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.SPHERE;
				this.m_Color = col;
				this.m_Center = center;
				this.m_Radius = radius;
			}
		}

		public class NpDebugSceneViewControllerDataBox : NpDebugSceneViewController.NpDebugSceneViewControllerDataBase
		{
			public Vector3 m_Center;

			public Vector3 m_Size;

			public Quaternion m_Rotate;

			public NpDebugSceneViewControllerDataBox(Vector3 center, Vector3 size, Quaternion rotate, Color col)
			{
				this.m_DataType = NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.BOX;
				this.m_Color = col;
				this.m_Center = center;
				this.m_Size = size;
				this.m_Rotate = rotate;
			}
		}

		public class NpDebugSceneViewControllerDataCylinder : NpDebugSceneViewController.NpDebugSceneViewControllerDataBase
		{
			public Vector3 m_Center;

			public float m_Radius;

			public float m_Height;

			public Vector3 m_Up;

			public int m_Divide;

			public Vector3 m_Front;

			public NpDebugSceneViewControllerDataCylinder(Vector3 center, float radius, float height, Vector3 upVec, int divide, Color col, Vector3 frontVec)
			{
				this.m_DataType = NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.CYLINDER;
				this.m_Color = col;
				this.m_Center = center;
				this.m_Radius = radius;
				this.m_Height = height;
				this.m_Up = upVec;
				this.m_Divide = divide;
				this.m_Front = frontVec;
			}
		}

		public class NpDebugSceneViewControllerDataArrow : NpDebugSceneViewController.NpDebugSceneViewControllerDataBase
		{
			public Vector3 m_BeginPos;

			public Vector3 m_EndPos;

			public NpDebugSceneViewControllerDataArrow(Vector3 beginPos, Vector3 endPos, Color col)
			{
				this.m_DataType = NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.ARROW;
				this.m_Color = col;
				this.m_BeginPos = beginPos;
				this.m_EndPos = endPos;
			}
		}

		public class NpDebugSceneViewControllerDataPoint : NpDebugSceneViewController.NpDebugSceneViewControllerDataBase
		{
			public Vector3 m_Pos;

			public float m_Scale;

			public NpDebugSceneViewControllerDataPoint(Vector3 pos, float scale, Color col)
			{
				this.m_DataType = NpDebugSceneViewController.NpDebugSceneViewControllerDataBase.eDataType.POINT;
				this.m_Color = col;
				this.m_Pos = pos;
				this.m_Scale = scale;
			}
		}
	}
}
