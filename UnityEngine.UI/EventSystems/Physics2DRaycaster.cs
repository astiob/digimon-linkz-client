using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Event/Physics 2D Raycaster")]
	public class Physics2DRaycaster : PhysicsRaycaster
	{
		protected Physics2DRaycaster()
		{
		}

		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (this.eventCamera == null)
			{
				return;
			}
			Ray ray = this.eventCamera.ScreenPointToRay(eventData.position);
			float distance = this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane;
			RaycastHit2D[] rayIntersectionAll = Physics2D.GetRayIntersectionAll(ray, distance, base.finalEventMask);
			if (rayIntersectionAll.Length != 0)
			{
				int i = 0;
				int num = rayIntersectionAll.Length;
				while (i < num)
				{
					SpriteRenderer component = rayIntersectionAll[i].collider.gameObject.GetComponent<SpriteRenderer>();
					RaycastResult item = new RaycastResult
					{
						gameObject = rayIntersectionAll[i].collider.gameObject,
						module = this,
						distance = Vector3.Distance(this.eventCamera.transform.position, rayIntersectionAll[i].transform.position),
						worldPosition = rayIntersectionAll[i].point,
						worldNormal = rayIntersectionAll[i].normal,
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
