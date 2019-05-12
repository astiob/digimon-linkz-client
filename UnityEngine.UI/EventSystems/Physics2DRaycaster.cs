using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	[AddComponentMenu("Event/Physics 2D Raycaster")]
	[RequireComponent(typeof(Camera))]
	public class Physics2DRaycaster : PhysicsRaycaster
	{
		private RaycastHit2D[] m_Hits;

		protected Physics2DRaycaster()
		{
		}

		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (!(this.eventCamera == null))
			{
				Ray r;
				float f;
				base.ComputeRayAndDistance(eventData, out r, out f);
				int num;
				if (base.maxRayIntersections == 0)
				{
					if (ReflectionMethodsCache.Singleton.getRayIntersectionAll == null)
					{
						return;
					}
					this.m_Hits = ReflectionMethodsCache.Singleton.getRayIntersectionAll(r, f, base.finalEventMask);
					num = this.m_Hits.Length;
				}
				else
				{
					if (ReflectionMethodsCache.Singleton.getRayIntersectionAllNonAlloc == null)
					{
						return;
					}
					if (this.m_LastMaxRayIntersections != this.m_MaxRayIntersections)
					{
						this.m_Hits = new RaycastHit2D[base.maxRayIntersections];
						this.m_LastMaxRayIntersections = this.m_MaxRayIntersections;
					}
					num = ReflectionMethodsCache.Singleton.getRayIntersectionAllNonAlloc(r, this.m_Hits, f, base.finalEventMask);
				}
				if (num != 0)
				{
					int i = 0;
					int num2 = num;
					while (i < num2)
					{
						SpriteRenderer component = this.m_Hits[i].collider.gameObject.GetComponent<SpriteRenderer>();
						RaycastResult item = new RaycastResult
						{
							gameObject = this.m_Hits[i].collider.gameObject,
							module = this,
							distance = Vector3.Distance(this.eventCamera.transform.position, this.m_Hits[i].point),
							worldPosition = this.m_Hits[i].point,
							worldNormal = this.m_Hits[i].normal,
							screenPosition = eventData.position,
							index = (float)resultAppendList.Count,
							sortingLayer = ((!(component != null)) ? 0 : component.sortingLayerID),
							sortingOrder = ((!(component != null)) ? 0 : component.sortingOrder)
						};
						resultAppendList.Add(item);
						i++;
					}
				}
			}
		}
	}
}
