using CharacterModelUI;
using System;
using UnityEngine;

namespace MonsterPicturebook
{
	[Serializable]
	public sealed class PicturebookModelViewTransition : CharacterModelViewTransition
	{
		[SerializeField]
		private Vector2 cameraViewOffset;

		private PicturebookModelViewTransition.ViewerState viewerState;

		protected override void OnInitialized()
		{
			this.viewButton.activeCollider = false;
			this.modelViewer.Initialize(new Vector3(0f, 5000f, 0f));
			this.modelViewer.SetCameraViewOffset(this.cameraViewOffset.y, this.cameraViewOffset.x);
			this.modelViewer.EnableTouchEvent(false);
		}

		protected override void OnDeleted()
		{
			this.viewButton.activeCollider = false;
			this.modelViewer.EnableTouchEvent(false);
			this.modelViewer.DeleteCharacterModel();
		}

		protected override void OnOpenViewer()
		{
		}

		protected override void OnOpendViewer()
		{
			this.viewButton.SetCloseButton();
			this.viewButton.SetEnable(true);
		}

		protected override void OnCloseViewer()
		{
		}

		protected override void OnClosedViewer()
		{
			this.viewButton.SetOpenButton();
			this.viewButton.SetEnable(true);
		}

		public void LoadModel(string monsterGroupId)
		{
			base.LoadMonster(monsterGroupId);
			this.modelViewer.EnableTouchEvent(true);
			this.viewButton.activeCollider = true;
		}

		public void OnPushedButtonViewButton()
		{
			if (this.viewerState == PicturebookModelViewTransition.ViewerState.CLOSE)
			{
				this.viewerState = PicturebookModelViewTransition.ViewerState.OPEN;
				base.OpenModelViewer();
			}
			else
			{
				this.viewerState = PicturebookModelViewTransition.ViewerState.CLOSE;
				base.CloseModelViewer();
			}
		}

		private enum ViewerState
		{
			CLOSE,
			OPEN
		}
	}
}
