using System;
using UnityEngine;

public class FarmDragCreate : MonoBehaviour
{
	private FarmEditFacilityButton callFacilityButton;

	public FarmEditFacilityButton SetCallFacilityButton
	{
		set
		{
			this.callFacilityButton = value;
		}
	}

	private void OnDragOver(GameObject draggedObject)
	{
		if (this.callFacilityButton != null)
		{
			this.callFacilityButton.DragCreate();
		}
	}
}
