using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	internal class SendMouseEvents
	{
		private const int m_HitIndexGUI = 0;

		private const int m_HitIndexPhysics3D = 1;

		private const int m_HitIndexPhysics2D = 2;

		private static bool s_MouseUsed = false;

		private static readonly SendMouseEvents.HitInfo[] m_LastHit = new SendMouseEvents.HitInfo[]
		{
			default(SendMouseEvents.HitInfo),
			default(SendMouseEvents.HitInfo),
			default(SendMouseEvents.HitInfo)
		};

		private static readonly SendMouseEvents.HitInfo[] m_MouseDownHit = new SendMouseEvents.HitInfo[]
		{
			default(SendMouseEvents.HitInfo),
			default(SendMouseEvents.HitInfo),
			default(SendMouseEvents.HitInfo)
		};

		private static readonly SendMouseEvents.HitInfo[] m_CurrentHit = new SendMouseEvents.HitInfo[]
		{
			default(SendMouseEvents.HitInfo),
			default(SendMouseEvents.HitInfo),
			default(SendMouseEvents.HitInfo)
		};

		private static Camera[] m_Cameras;

		[RequiredByNativeCode]
		private static void SetMouseMoved()
		{
			SendMouseEvents.s_MouseUsed = true;
		}

		private static void HitTestLegacyGUI(Camera camera, Vector3 mousePosition, ref SendMouseEvents.HitInfo hitInfo)
		{
			GUILayer component = camera.GetComponent<GUILayer>();
			if (component)
			{
				GUIElement guielement = component.HitTest(mousePosition);
				if (guielement)
				{
					hitInfo.target = guielement.gameObject;
					hitInfo.camera = camera;
				}
				else
				{
					hitInfo.target = null;
					hitInfo.camera = null;
				}
			}
		}

		[RequiredByNativeCode]
		private static void DoSendMouseEvents(int skipRTCameras)
		{
			Vector3 mousePosition = Input.mousePosition;
			int allCamerasCount = Camera.allCamerasCount;
			if (SendMouseEvents.m_Cameras == null || SendMouseEvents.m_Cameras.Length != allCamerasCount)
			{
				SendMouseEvents.m_Cameras = new Camera[allCamerasCount];
			}
			Camera.GetAllCameras(SendMouseEvents.m_Cameras);
			for (int i = 0; i < SendMouseEvents.m_CurrentHit.Length; i++)
			{
				SendMouseEvents.m_CurrentHit[i] = default(SendMouseEvents.HitInfo);
			}
			if (!SendMouseEvents.s_MouseUsed)
			{
				foreach (Camera camera in SendMouseEvents.m_Cameras)
				{
					if (!(camera == null) && (skipRTCameras == 0 || !(camera.targetTexture != null)))
					{
						int targetDisplay = camera.targetDisplay;
						Vector3 vector = Display.RelativeMouseAt(mousePosition);
						if (vector != Vector3.zero)
						{
							int num = (int)vector.z;
							if (num != targetDisplay)
							{
								goto IL_361;
							}
							float num2 = (float)Screen.width;
							float num3 = (float)Screen.height;
							if (targetDisplay > 0 && targetDisplay < Display.displays.Length)
							{
								num2 = (float)Display.displays[targetDisplay].systemWidth;
								num3 = (float)Display.displays[targetDisplay].systemHeight;
							}
							Vector2 vector2 = new Vector2(vector.x / num2, vector.y / num3);
							if (vector2.x < 0f || vector2.x > 1f || vector2.y < 0f || vector2.y > 1f)
							{
								goto IL_361;
							}
						}
						else
						{
							vector = mousePosition;
							if (!camera.pixelRect.Contains(vector))
							{
								goto IL_361;
							}
						}
						SendMouseEvents.HitTestLegacyGUI(camera, vector, ref SendMouseEvents.m_CurrentHit[0]);
						if (camera.eventMask != 0)
						{
							Ray ray = camera.ScreenPointToRay(vector);
							float z = ray.direction.z;
							float distance = (!Mathf.Approximately(0f, z)) ? Mathf.Abs((camera.farClipPlane - camera.nearClipPlane) / z) : float.PositiveInfinity;
							GameObject gameObject = camera.RaycastTry(ray, distance, camera.cullingMask & camera.eventMask);
							if (gameObject != null)
							{
								SendMouseEvents.m_CurrentHit[1].target = gameObject;
								SendMouseEvents.m_CurrentHit[1].camera = camera;
							}
							else if (camera.clearFlags == CameraClearFlags.Skybox || camera.clearFlags == CameraClearFlags.Color)
							{
								SendMouseEvents.m_CurrentHit[1].target = null;
								SendMouseEvents.m_CurrentHit[1].camera = null;
							}
							GameObject gameObject2 = camera.RaycastTry2D(ray, distance, camera.cullingMask & camera.eventMask);
							if (gameObject2 != null)
							{
								SendMouseEvents.m_CurrentHit[2].target = gameObject2;
								SendMouseEvents.m_CurrentHit[2].camera = camera;
							}
							else if (camera.clearFlags == CameraClearFlags.Skybox || camera.clearFlags == CameraClearFlags.Color)
							{
								SendMouseEvents.m_CurrentHit[2].target = null;
								SendMouseEvents.m_CurrentHit[2].camera = null;
							}
						}
					}
					IL_361:;
				}
			}
			for (int k = 0; k < SendMouseEvents.m_CurrentHit.Length; k++)
			{
				SendMouseEvents.SendEvents(k, SendMouseEvents.m_CurrentHit[k]);
			}
			SendMouseEvents.s_MouseUsed = false;
		}

		private static void SendEvents(int i, SendMouseEvents.HitInfo hit)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(0);
			bool mouseButton = Input.GetMouseButton(0);
			if (mouseButtonDown)
			{
				if (hit)
				{
					SendMouseEvents.m_MouseDownHit[i] = hit;
					SendMouseEvents.m_MouseDownHit[i].SendMessage("OnMouseDown");
				}
			}
			else if (!mouseButton)
			{
				if (SendMouseEvents.m_MouseDownHit[i])
				{
					if (SendMouseEvents.HitInfo.Compare(hit, SendMouseEvents.m_MouseDownHit[i]))
					{
						SendMouseEvents.m_MouseDownHit[i].SendMessage("OnMouseUpAsButton");
					}
					SendMouseEvents.m_MouseDownHit[i].SendMessage("OnMouseUp");
					SendMouseEvents.m_MouseDownHit[i] = default(SendMouseEvents.HitInfo);
				}
			}
			else if (SendMouseEvents.m_MouseDownHit[i])
			{
				SendMouseEvents.m_MouseDownHit[i].SendMessage("OnMouseDrag");
			}
			if (SendMouseEvents.HitInfo.Compare(hit, SendMouseEvents.m_LastHit[i]))
			{
				if (hit)
				{
					hit.SendMessage("OnMouseOver");
				}
			}
			else
			{
				if (SendMouseEvents.m_LastHit[i])
				{
					SendMouseEvents.m_LastHit[i].SendMessage("OnMouseExit");
				}
				if (hit)
				{
					hit.SendMessage("OnMouseEnter");
					hit.SendMessage("OnMouseOver");
				}
			}
			SendMouseEvents.m_LastHit[i] = hit;
		}

		private struct HitInfo
		{
			public GameObject target;

			public Camera camera;

			public void SendMessage(string name)
			{
				this.target.SendMessage(name, null, SendMessageOptions.DontRequireReceiver);
			}

			public static implicit operator bool(SendMouseEvents.HitInfo exists)
			{
				return exists.target != null && exists.camera != null;
			}

			public static bool Compare(SendMouseEvents.HitInfo lhs, SendMouseEvents.HitInfo rhs)
			{
				return lhs.target == rhs.target && lhs.camera == rhs.camera;
			}
		}
	}
}
