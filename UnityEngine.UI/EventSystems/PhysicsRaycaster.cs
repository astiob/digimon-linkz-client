using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	[AddComponentMenu("Event/Physics Raycaster")]
	[RequireComponent(typeof(Camera))]
	public class PhysicsRaycaster : BaseRaycaster
	{
		protected const int kNoEventMaskSet = -1;

		protected Camera m_EventCamera;

		[SerializeField]
		protected LayerMask m_EventMask = -1;

		[SerializeField]
		protected int m_MaxRayIntersections = 0;

		protected int m_LastMaxRayIntersections = 0;

		private RaycastHit[] m_Hits;

		protected PhysicsRaycaster()
		{
		}

		public override Camera eventCamera
		{
			get
			{
				if (this.m_EventCamera == null)
				{
					this.m_EventCamera = base.GetComponent<Camera>();
				}
				return this.m_EventCamera ?? Camera.main;
			}
		}

		public virtual int depth
		{
			get
			{
				return (!(this.eventCamera != null)) ? 16777215 : ((int)this.eventCamera.depth);
			}
		}

		public int finalEventMask
		{
			get
			{
				return (!(this.eventCamera != null)) ? -1 : (this.eventCamera.cullingMask & this.m_EventMask);
			}
		}

		public LayerMask eventMask
		{
			get
			{
				return this.m_EventMask;
			}
			set
			{
				this.m_EventMask = value;
			}
		}

		public int maxRayIntersections
		{
			get
			{
				return this.m_MaxRayIntersections;
			}
			set
			{
				this.m_MaxRayIntersections = value;
			}
		}

		protected void ComputeRayAndDistance(PointerEventData eventData, out Ray ray, out float distanceToClipPlane)
		{
			ray = this.eventCamera.ScreenPointToRay(eventData.position);
			float z = ray.direction.z;
			distanceToClipPlane = ((!Mathf.Approximately(0f, z)) ? Mathf.Abs((this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane) / z) : float.PositiveInfinity);
		}

		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (!(this.eventCamera == null) && this.eventCamera.pixelRect.Contains(eventData.position))
			{
				Ray r;
				float f;
				this.ComputeRayAndDistance(eventData, out r, out f);
				int num;
				if (this.m_MaxRayIntersections == 0)
				{
					if (ReflectionMethodsCache.Singleton.raycast3DAll == null)
					{
						return;
					}
					this.m_Hits = ReflectionMethodsCache.Singleton.raycast3DAll(r, f, this.finalEventMask);
					num = this.m_Hits.Length;
				}
				else
				{
					if (ReflectionMethodsCache.Singleton.getRaycastNonAlloc == null)
					{
						return;
					}
					if (this.m_LastMaxRayIntersections != this.m_MaxRayIntersections)
					{
						this.m_Hits = new RaycastHit[this.m_MaxRayIntersections];
						this.m_LastMaxRayIntersections = this.m_MaxRayIntersections;
					}
					num = ReflectionMethodsCache.Singleton.getRaycastNonAlloc(r, this.m_Hits, f, this.finalEventMask);
				}
				if (num > 1)
				{
					Array.Sort<RaycastHit>(this.m_Hits, (RaycastHit r1, RaycastHit r2) => r1.distance.CompareTo(r2.distance));
				}
				if (num != 0)
				{
					int i = 0;
					int num2 = num;
					while (i < num2)
					{
						RaycastResult item = new RaycastResult
						{
							gameObject = this.m_Hits[i].collider.gameObject,
							module = this,
							distance = this.m_Hits[i].distance,
							worldPosition = this.m_Hits[i].point,
							worldNormal = this.m_Hits[i].normal,
							screenPosition = eventData.position,
							index = (float)resultAppendList.Count,
							sortingLayer = 0,
							sortingOrder = 0
						};
						resultAppendList.Add(item);
						i++;
					}
				}
			}
		}
	}
}
