using System;
using System.Collections.Generic;

namespace UnityEngine.UIComponentSkinnerInternal
{
	[Serializable]
	public sealed class UIComponentSkinnerObject
	{
		[SerializeField]
		private UIComponentSkinnerObjectParts[] _uiCompornentSkinnerObjectParts = new UIComponentSkinnerObjectParts[]
		{
			new UIComponentSkinnerObjectParts()
		};

		public UIComponentSkinnerObject()
		{
		}

		public UIComponentSkinnerObject(UIComponentSkinnerObject baseObject)
		{
			List<UIComponentSkinnerObjectParts> list = new List<UIComponentSkinnerObjectParts>();
			foreach (UIComponentSkinnerObjectParts uicomponentSkinnerObjectParts in baseObject._uiCompornentSkinnerObjectParts)
			{
				list.Add(uicomponentSkinnerObjectParts.Clone());
			}
			this._uiCompornentSkinnerObjectParts = list.ToArray();
		}

		public UIComponentSkinnerObjectParts[] uiCompornentSkinnerObjectParts
		{
			get
			{
				return this._uiCompornentSkinnerObjectParts;
			}
		}

		public void ApplySkins()
		{
			foreach (UIComponentSkinnerObjectParts uicomponentSkinnerObjectParts in this._uiCompornentSkinnerObjectParts)
			{
				uicomponentSkinnerObjectParts.ApplySkin();
			}
		}
	}
}
