using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
	public abstract class BaseRaycaster : UIBehaviour
	{
		public abstract void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList);

		public abstract Camera eventCamera { get; }

		[Obsolete("Please use sortOrderPriority and renderOrderPriority", false)]
		public virtual int priority
		{
			get
			{
				return 0;
			}
		}

		public virtual int sortOrderPriority
		{
			get
			{
				return int.MinValue;
			}
		}

		public virtual int renderOrderPriority
		{
			get
			{
				return int.MinValue;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Name: ",
				base.gameObject,
				"\neventCamera: ",
				this.eventCamera,
				"\nsortOrderPriority: ",
				this.sortOrderPriority,
				"\nrenderOrderPriority: ",
				this.renderOrderPriority
			});
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			RaycasterManager.AddRaycaster(this);
		}

		protected override void OnDisable()
		{
			RaycasterManager.RemoveRaycasters(this);
			base.OnDisable();
		}
	}
}
