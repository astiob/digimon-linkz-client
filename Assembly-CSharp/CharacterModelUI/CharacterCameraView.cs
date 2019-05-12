using DeviceSafeArea;
using Monster;
using System;
using UnityEngine;

namespace CharacterModelUI
{
	public sealed class CharacterCameraView
	{
		private Matrix4x4 orignal = Matrix4x4.identity;

		private Vector3 defaultPos = Vector3.zero;

		private bool isAnimation;

		private float currentTime;

		private float maxTime;

		private float startPosX;

		private float endPosX;

		private PinchInOut pinch;

		public bool enableTouch;

		public CharacterCameraView(MonsterData monsterData) : this(monsterData, (int)SafeArea.GetDeviceScreenSize().x, (int)SafeArea.GetDeviceScreenSize().y)
		{
		}

		public CharacterCameraView(MonsterData monsterData, int width, int height)
		{
			GameObject gameObject = GUIManager.LoadCommonGUI("Render3D/Render3DRT", null);
			this.csRender3DRT = gameObject.GetComponent<CommonRender3DRT>();
			if (!monsterData.userMonster.IsEgg())
			{
				string filePath = MonsterObject.GetFilePath(monsterData.GetMonsterMaster().Group.modelId);
				this.csRender3DRT.LoadChara(filePath, 0f, 10000f, -0.65f, 1.1f, true);
				this.csRender3DRT.SetBillBoardCamera();
			}
			else
			{
				string eggModelId = MonsterObject.GetEggModelId(monsterData.userMonster.monsterEvolutionRouteId);
				string filePath2 = MonsterObject.GetFilePath(eggModelId);
				this.csRender3DRT.LoadEgg(filePath2, 0f, 10000f, 0.1f);
			}
			this.renderTex = this.csRender3DRT.SetRenderTarget(width, height, 16);
			this.defaultPos = Camera.main.ScreenToViewportPoint(new Vector3(-382f, -80f, 0f));
			this.defaultPos.x = -0.6f;
			this.orignal = this.csRender3DRT.projectionMatrix;
			Matrix4x4 lhs = Matrix4x4.TRS(this.defaultPos, Quaternion.identity, Vector3.one);
			this.csRender3DRT.projectionMatrix = lhs * this.orignal;
			this.pinch = new PinchInOut();
		}

		public CommonRender3DRT csRender3DRT { get; private set; }

		public RenderTexture renderTex { get; private set; }

		public void Update(float deltaTime)
		{
			if (null == this.csRender3DRT)
			{
				return;
			}
			if (this.isAnimation)
			{
				this.currentTime += deltaTime;
				if (this.currentTime > this.maxTime)
				{
					this.currentTime = this.maxTime;
					this.isAnimation = false;
				}
				Vector3 pos = Vector3.Lerp(new Vector3(this.startPosX, this.defaultPos.y, 0f), new Vector3(this.endPosX, this.defaultPos.y, 0f), this.currentTime / this.maxTime);
				Matrix4x4 lhs = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
				this.csRender3DRT.projectionMatrix = lhs * this.orignal;
			}
			if (this.enableTouch)
			{
				CharacterParams characterParams = this.csRender3DRT.GetCharacterParams();
				if (characterParams == null || characterParams.transform.parent == null)
				{
					return;
				}
				float value = this.pinch.Value;
				Vector3 localScale = characterParams.transform.localScale;
				if (value == 0f || (value < 0f && localScale.x <= 0.8f) || (value > 0f && localScale.x >= 1.2f))
				{
					return;
				}
				float num = localScale.x + value / 500f;
				if (num < 0.8f)
				{
					num = 0.8f;
				}
				if (num > 1.2f)
				{
					num = 1.2f;
				}
				characterParams.transform.localScale = new Vector3(num, num, num);
			}
		}

		public void MoveToCenter(float animationMaxTime)
		{
			this.MoveTo(this.defaultPos.x, 0f, animationMaxTime);
		}

		public void MoveToLeft(float animationMaxTime)
		{
			this.MoveTo(0f, this.defaultPos.x, animationMaxTime);
		}

		public void MoveTo(float startPosX, float endPosX, float animationMaxTime)
		{
			this.startPosX = startPosX;
			this.endPosX = endPosX;
			this.currentTime = 0f;
			this.maxTime = animationMaxTime;
			this.isAnimation = true;
		}

		public void OnDisplayDrag(Vector2 deltaMove)
		{
			if (this.enableTouch)
			{
				float num = this.csRender3DRT.RotY;
				num -= deltaMove.x / 3f;
				this.csRender3DRT.RotY = num;
			}
		}

		public void Destroy()
		{
			if (null != this.csRender3DRT)
			{
				UnityEngine.Object.DestroyImmediate(this.csRender3DRT.gameObject);
			}
		}
	}
}
