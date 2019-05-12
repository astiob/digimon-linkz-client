using System;
using UnityEngine;

namespace CharacterModelUI
{
	[Serializable]
	public abstract class CharacterModelViewTransition
	{
		[SerializeField]
		private Transform viewRoot;

		[SerializeField]
		private GameObject animationRoot;

		[SerializeField]
		private Vector3 modelViewPosition;

		[SerializeField]
		private int modelViewDepth;

		private ModelViewerOpenAnimationEvent openAnimationEvent;

		private ModelViewerCloseAnimationEvent closeAnimationEvent;

		[SerializeField]
		protected GUI_ModelViewerButton viewButton;

		protected UI_CharacterModelViewer modelViewer;

		protected abstract void OnInitialized();

		protected abstract void OnDeleted();

		protected abstract void OnOpenViewer();

		protected abstract void OnOpendViewer();

		protected abstract void OnCloseViewer();

		protected abstract void OnClosedViewer();

		protected void LoadMonster(string monsterGroupId)
		{
			this.modelViewer.LoadMonsterModel(monsterGroupId, Vector3.zero, 170f);
			this.modelViewer.SetCharacterCameraDistance();
		}

		protected void LoadEgg(string monsterGroupId)
		{
			Vector3 characterPosition = new Vector3(0f, 0.1f, 0f);
			this.modelViewer.LoadEggModel(monsterGroupId, characterPosition, 0f);
		}

		public void Initialize(int baseDepth)
		{
			GameObject original = AssetDataMng.Instance().LoadObject("UI/Common/CharacterModelViewer", null, true) as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			Transform transform = gameObject.transform;
			transform.parent = this.viewRoot;
			transform.localScale = Vector3.one;
			transform.localPosition = this.modelViewPosition;
			transform.localRotation = Quaternion.identity;
			DepthController.SetWidgetDepth_2(transform, this.modelViewDepth + baseDepth);
			this.modelViewer = gameObject.GetComponent<UI_CharacterModelViewer>();
			Animator component = this.animationRoot.GetComponent<Animator>();
			this.openAnimationEvent = component.GetBehaviour<ModelViewerOpenAnimationEvent>();
			this.closeAnimationEvent = component.GetBehaviour<ModelViewerCloseAnimationEvent>();
			this.openAnimationEvent.Initialize(component, new Action(this.OnOpendViewer));
			this.closeAnimationEvent.Initialize(component, new Action(this.OnClosedViewer));
			this.OnInitialized();
		}

		public void Delete()
		{
			this.OnDeleted();
			this.modelViewer.DestroyRenderTexture();
		}

		public void OpenModelViewer()
		{
			this.openAnimationEvent.StartAnimation();
			this.viewButton.SetEnable(false);
			this.OnOpenViewer();
		}

		public void CloseModelViewer()
		{
			this.closeAnimationEvent.StartAnimation();
			this.viewButton.SetEnable(false);
			this.OnCloseViewer();
		}
	}
}
