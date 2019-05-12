using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene.UI
{
	[RequireComponent(typeof(Camera))]
	public sealed class ButtonTouchObserver : MonoBehaviour
	{
		[SerializeField]
		private List<Collider> colliderList;

		private Camera camera2D;

		private void Awake()
		{
			this.camera2D = base.GetComponent<Camera>();
		}

		private Collider FindCollider(Collider collider)
		{
			Collider result = null;
			for (int i = 0; i < this.colliderList.Count; i++)
			{
				if (this.colliderList[i] == collider)
				{
					result = this.colliderList[i];
					break;
				}
			}
			return result;
		}

		private void Update()
		{
			if (Input.GetMouseButtonUp(0) || Input.touchCount > 0)
			{
				Ray ray = this.camera2D.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] array = Physics.RaycastAll(ray);
				for (int i = 0; i < array.Length; i++)
				{
					Collider collider = this.FindCollider(array[i].collider);
					if (null != collider)
					{
						IButtonTouchEvent component = collider.GetComponent<IButtonTouchEvent>();
						if (component != null)
						{
							component.TouchButton(this);
							break;
						}
					}
				}
			}
		}

		public void RemoveCollider(Collider c)
		{
			this.colliderList.Remove(c);
		}
	}
}
