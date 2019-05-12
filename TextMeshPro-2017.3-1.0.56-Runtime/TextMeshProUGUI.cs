using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasRenderer))]
	[AddComponentMenu("UI/TextMeshPro - Text (UI)", 11)]
	public class TextMeshProUGUI : TMP_Text, ILayoutElement
	{
		private bool m_isRebuildingLayout;

		[SerializeField]
		private bool m_hasFontAssetChanged;

		[SerializeField]
		protected TMP_SubMeshUI[] m_subTextObjects = new TMP_SubMeshUI[8];

		private float m_previousLossyScaleY = -1f;

		private Vector3[] m_RectTransformCorners = new Vector3[4];

		private CanvasRenderer m_canvasRenderer;

		private Canvas m_canvas;

		private bool m_isFirstAllocation;

		private int m_max_characters = 8;

		private bool m_isMaskingEnabled;

		[SerializeField]
		private Material m_baseMaterial;

		private bool m_isScrollRegionSet;

		private int m_stencilID;

		[SerializeField]
		private Vector4 m_maskOffset;

		private Matrix4x4 m_EnvMapMatrix = default(Matrix4x4);

		[NonSerialized]
		private bool m_isRegisteredForEvents;

		private int m_recursiveCountA;

		private int loopCountA;

		public override Material materialForRendering
		{
			get
			{
				return TMP_MaterialManager.GetMaterialForRendering(this, this.m_sharedMaterial);
			}
		}

		public override bool autoSizeTextContainer
		{
			get
			{
				return this.m_autoSizeTextContainer;
			}
			set
			{
				if (this.m_autoSizeTextContainer == value)
				{
					return;
				}
				this.m_autoSizeTextContainer = value;
				if (this.m_autoSizeTextContainer)
				{
					CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
					this.SetLayoutDirty();
				}
			}
		}

		public override Mesh mesh
		{
			get
			{
				return this.m_mesh;
			}
		}

		public new CanvasRenderer canvasRenderer
		{
			get
			{
				if (this.m_canvasRenderer == null)
				{
					this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
				}
				return this.m_canvasRenderer;
			}
		}

		public void CalculateLayoutInputHorizontal()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_isCalculateSizeRequired || this.m_rectTransform.hasChanged)
			{
				this.m_preferredWidth = base.GetPreferredWidth();
				this.ComputeMarginSize();
				this.m_isLayoutDirty = true;
			}
		}

		public void CalculateLayoutInputVertical()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.m_isCalculateSizeRequired || this.m_rectTransform.hasChanged)
			{
				this.m_preferredHeight = base.GetPreferredHeight();
				this.ComputeMarginSize();
				this.m_isLayoutDirty = true;
			}
			this.m_isCalculateSizeRequired = false;
		}

		public override void SetVerticesDirty()
		{
			if (this.m_verticesAlreadyDirty || this == null || !this.IsActive() || CanvasUpdateRegistry.IsRebuildingGraphics())
			{
				return;
			}
			this.m_verticesAlreadyDirty = true;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
			if (this.m_OnDirtyVertsCallback != null)
			{
				this.m_OnDirtyVertsCallback();
			}
		}

		public override void SetLayoutDirty()
		{
			this.m_isPreferredWidthDirty = true;
			this.m_isPreferredHeightDirty = true;
			if (this.m_layoutAlreadyDirty || this == null || !this.IsActive())
			{
				return;
			}
			this.m_layoutAlreadyDirty = true;
			LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
			this.m_isLayoutDirty = true;
			if (this.m_OnDirtyLayoutCallback != null)
			{
				this.m_OnDirtyLayoutCallback();
			}
		}

		public override void SetMaterialDirty()
		{
			if (this == null || !this.IsActive() || CanvasUpdateRegistry.IsRebuildingGraphics())
			{
				return;
			}
			this.m_isMaterialDirty = true;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
			if (this.m_OnDirtyMaterialCallback != null)
			{
				this.m_OnDirtyMaterialCallback();
			}
		}

		public override void SetAllDirty()
		{
			this.SetLayoutDirty();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
		}

		public override void Rebuild(CanvasUpdate update)
		{
			if (this == null)
			{
				return;
			}
			if (update == CanvasUpdate.Prelayout)
			{
				if (this.m_autoSizeTextContainer)
				{
					this.m_rectTransform.sizeDelta = base.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
				}
			}
			else if (update == CanvasUpdate.PreRender)
			{
				this.OnPreRenderCanvas();
				this.m_verticesAlreadyDirty = false;
				this.m_layoutAlreadyDirty = false;
				if (!this.m_isMaterialDirty)
				{
					return;
				}
				this.UpdateMaterial();
				this.m_isMaterialDirty = false;
			}
		}

		private void UpdateSubObjectPivot()
		{
			if (this.m_textInfo == null)
			{
				return;
			}
			int num = 1;
			while (num < this.m_subTextObjects.Length && this.m_subTextObjects[num] != null)
			{
				this.m_subTextObjects[num].SetPivotDirty();
				num++;
			}
		}

		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (this.m_ShouldRecalculateStencil)
			{
				this.m_stencilID = TMP_MaterialManager.GetStencilID(base.gameObject);
				this.m_ShouldRecalculateStencil = false;
			}
			if (this.m_stencilID > 0)
			{
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, this.m_stencilID);
				if (this.m_MaskMaterial != null)
				{
					TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				}
				this.m_MaskMaterial = material;
			}
			return material;
		}

		protected override void UpdateMaterial()
		{
			if (this.m_sharedMaterial == null)
			{
				return;
			}
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = this.canvasRenderer;
			}
			this.m_canvasRenderer.materialCount = 1;
			this.m_canvasRenderer.SetMaterial(this.materialForRendering, 0);
		}

		public Vector4 maskOffset
		{
			get
			{
				return this.m_maskOffset;
			}
			set
			{
				this.m_maskOffset = value;
				this.UpdateMask();
				this.m_havePropertiesChanged = true;
			}
		}

		public override void RecalculateClipping()
		{
			base.RecalculateClipping();
		}

		public override void RecalculateMasking()
		{
			this.m_ShouldRecalculateStencil = true;
			this.SetMaterialDirty();
		}

		public override void Cull(Rect clipRect, bool validRect)
		{
			if (this.m_ignoreRectMaskCulling)
			{
				return;
			}
			base.Cull(clipRect, validRect);
		}

		public override void UpdateMeshPadding()
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_havePropertiesChanged = true;
			this.checkPaddingRequired = false;
			if (this.m_textInfo == null)
			{
				return;
			}
			for (int i = 1; i < this.m_textInfo.materialCount; i++)
			{
				this.m_subTextObjects[i].UpdateMeshPadding(this.m_enableExtraPadding, this.m_isUsingBold);
			}
		}

		protected override void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 1; i < materialCount; i++)
			{
				this.m_subTextObjects[i].CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			}
		}

		protected override void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 1; i < materialCount; i++)
			{
				this.m_subTextObjects[i].CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			}
		}

		public override void ForceMeshUpdate()
		{
			this.m_havePropertiesChanged = true;
			this.OnPreRenderCanvas();
		}

		public override void ForceMeshUpdate(bool ignoreInactive)
		{
			this.m_havePropertiesChanged = true;
			this.m_ignoreActiveState = true;
			this.OnPreRenderCanvas();
		}

		public override TMP_TextInfo GetTextInfo(string text)
		{
			base.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			this.m_renderMode = TextRenderFlags.DontRender;
			this.ComputeMarginSize();
			if (this.m_canvas == null)
			{
				this.m_canvas = base.canvas;
			}
			this.GenerateTextMesh();
			this.m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		public override void ClearMesh()
		{
			this.m_canvasRenderer.SetMesh(null);
			int num = 1;
			while (num < this.m_subTextObjects.Length && this.m_subTextObjects[num] != null)
			{
				this.m_subTextObjects[num].canvasRenderer.SetMesh(null);
				num++;
			}
		}

		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
			if (index == 0)
			{
				this.m_canvasRenderer.SetMesh(mesh);
			}
			else
			{
				this.m_subTextObjects[index].canvasRenderer.SetMesh(mesh);
			}
		}

		public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = this.m_mesh;
				}
				else
				{
					mesh = this.m_subTextObjects[i].mesh;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Vertices) == TMP_VertexDataUpdateFlags.Vertices)
				{
					mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv0) == TMP_VertexDataUpdateFlags.Uv0)
				{
					mesh.uv = this.m_textInfo.meshInfo[i].uvs0;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv2) == TMP_VertexDataUpdateFlags.Uv2)
				{
					mesh.uv2 = this.m_textInfo.meshInfo[i].uvs2;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Colors32) == TMP_VertexDataUpdateFlags.Colors32)
				{
					mesh.colors32 = this.m_textInfo.meshInfo[i].colors32;
				}
				mesh.RecalculateBounds();
				if (i == 0)
				{
					this.m_canvasRenderer.SetMesh(mesh);
				}
				else
				{
					this.m_subTextObjects[i].canvasRenderer.SetMesh(mesh);
				}
			}
		}

		public override void UpdateVertexData()
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = this.m_mesh;
				}
				else
				{
					this.m_textInfo.meshInfo[i].ClearUnusedVertices();
					mesh = this.m_subTextObjects[i].mesh;
				}
				mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				mesh.uv = this.m_textInfo.meshInfo[i].uvs0;
				mesh.uv2 = this.m_textInfo.meshInfo[i].uvs2;
				mesh.colors32 = this.m_textInfo.meshInfo[i].colors32;
				mesh.RecalculateBounds();
				if (i == 0)
				{
					this.m_canvasRenderer.SetMesh(mesh);
				}
				else
				{
					this.m_subTextObjects[i].canvasRenderer.SetMesh(mesh);
				}
			}
		}

		public void UpdateFontAsset()
		{
			this.LoadFontAsset();
		}

		protected override void Awake()
		{
			this.m_canvas = base.canvas;
			this.m_isOrthographic = true;
			this.m_rectTransform = base.gameObject.GetComponent<RectTransform>();
			if (this.m_rectTransform == null)
			{
				this.m_rectTransform = base.gameObject.AddComponent<RectTransform>();
			}
			this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = base.gameObject.AddComponent<CanvasRenderer>();
			}
			if (this.m_mesh == null)
			{
				this.m_mesh = new Mesh();
				this.m_mesh.hideFlags = HideFlags.HideAndDontSave;
			}
			base.LoadDefaultSettings();
			this.LoadFontAsset();
			TMP_StyleSheet.LoadDefaultStyleSheet();
			if (this.m_char_buffer == null)
			{
				this.m_char_buffer = new int[this.m_max_characters];
			}
			this.m_cached_TextElement = new TMP_Glyph();
			this.m_isFirstAllocation = true;
			if (this.m_textInfo == null)
			{
				this.m_textInfo = new TMP_TextInfo(this);
			}
			if (this.m_fontAsset == null)
			{
				Debug.LogWarning("Please assign a Font Asset to this " + base.transform.name + " gameobject.", this);
				return;
			}
			TMP_SubMeshUI[] componentsInChildren = base.GetComponentsInChildren<TMP_SubMeshUI>();
			if (componentsInChildren.Length > 0)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.m_subTextObjects[i + 1] = componentsInChildren[i];
				}
			}
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.m_isAwake = true;
		}

		protected override void OnEnable()
		{
			if (!this.m_isRegisteredForEvents)
			{
				this.m_isRegisteredForEvents = true;
			}
			this.m_canvas = this.GetCanvas();
			this.SetActiveSubMeshes(true);
			GraphicRegistry.RegisterGraphicForCanvas(this.m_canvas, this);
			this.ComputeMarginSize();
			this.m_verticesAlreadyDirty = false;
			this.m_layoutAlreadyDirty = false;
			this.m_ShouldRecalculateStencil = true;
			this.m_isInputParsingRequired = true;
			this.SetAllDirty();
			this.RecalculateClipping();
		}

		protected override void OnDisable()
		{
			if (this.m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				this.m_MaskMaterial = null;
			}
			GraphicRegistry.UnregisterGraphicForCanvas(this.m_canvas, this);
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_canvasRenderer != null)
			{
				this.m_canvasRenderer.Clear();
			}
			this.SetActiveSubMeshes(false);
			LayoutRebuilder.MarkLayoutForRebuild(this.m_rectTransform);
			this.RecalculateClipping();
		}

		protected override void OnDestroy()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(this.m_canvas, this);
			if (this.m_mesh != null)
			{
				Object.DestroyImmediate(this.m_mesh);
			}
			if (this.m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				this.m_MaskMaterial = null;
			}
			this.m_isRegisteredForEvents = false;
		}

		protected override void LoadFontAsset()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (this.m_fontAsset == null)
			{
				if (TMP_Settings.defaultFontAsset != null)
				{
					this.m_fontAsset = TMP_Settings.defaultFontAsset;
				}
				else
				{
					this.m_fontAsset = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
				}
				if (this.m_fontAsset == null)
				{
					Debug.LogWarning("The LiberationSans SDF Font Asset was not found. There is no Font Asset assigned to " + base.gameObject.name + ".", this);
					return;
				}
				if (this.m_fontAsset.characterDictionary == null)
				{
					Debug.Log("Dictionary is Null!");
				}
				this.m_sharedMaterial = this.m_fontAsset.material;
			}
			else
			{
				if (this.m_fontAsset.characterDictionary == null)
				{
					this.m_fontAsset.ReadFontDefinition();
				}
				if (this.m_sharedMaterial == null && this.m_baseMaterial != null)
				{
					this.m_sharedMaterial = this.m_baseMaterial;
					this.m_baseMaterial = null;
				}
				if (this.m_sharedMaterial == null || this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex) == null || this.m_fontAsset.atlas.GetInstanceID() != this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
				{
					if (this.m_fontAsset.material == null)
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"The Font Atlas Texture of the Font Asset ",
							this.m_fontAsset.name,
							" assigned to ",
							base.gameObject.name,
							" is missing."
						}), this);
					}
					else
					{
						this.m_sharedMaterial = this.m_fontAsset.material;
					}
				}
			}
			base.GetSpecialCharacters(this.m_fontAsset);
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		private Canvas GetCanvas()
		{
			Canvas result = null;
			List<Canvas> list = TMP_ListPool<Canvas>.Get();
			base.gameObject.GetComponentsInParent<Canvas>(false, list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].isActiveAndEnabled)
					{
						result = list[i];
						break;
					}
				}
			}
			TMP_ListPool<Canvas>.Release(list);
			return result;
		}

		private void UpdateEnvMapMatrix()
		{
			if (!this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap) || this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == null)
			{
				return;
			}
			Vector3 euler = this.m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation);
			this.m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one);
			this.m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, this.m_EnvMapMatrix);
		}

		private void EnableMasking()
		{
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
				this.m_canvasRenderer.SetMaterial(this.m_fontMaterial, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			if (this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				this.UpdateMask();
			}
			this.m_isMaskingEnabled = true;
		}

		private void DisableMasking()
		{
			if (this.m_fontMaterial != null)
			{
				if (this.m_stencilID > 0)
				{
					this.m_sharedMaterial = this.m_MaskMaterial;
				}
				this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
				Object.DestroyImmediate(this.m_fontMaterial);
			}
			this.m_isMaskingEnabled = false;
		}

		private void UpdateMask()
		{
			if (this.m_rectTransform != null)
			{
				if (!ShaderUtilities.isInitialized)
				{
					ShaderUtilities.GetShaderPropertyIDs();
				}
				this.m_isScrollRegionSet = true;
				float num = Mathf.Min(Mathf.Min(this.m_margin.x, this.m_margin.z), this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessX));
				float num2 = Mathf.Min(Mathf.Min(this.m_margin.y, this.m_margin.w), this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessY));
				num = ((num <= 0f) ? 0f : num);
				num2 = ((num2 <= 0f) ? 0f : num2);
				float z = (this.m_rectTransform.rect.width - Mathf.Max(this.m_margin.x, 0f) - Mathf.Max(this.m_margin.z, 0f)) / 2f + num;
				float w = (this.m_rectTransform.rect.height - Mathf.Max(this.m_margin.y, 0f) - Mathf.Max(this.m_margin.w, 0f)) / 2f + num2;
				Vector2 vector = this.m_rectTransform.localPosition + new Vector3((0.5f - this.m_rectTransform.pivot.x) * this.m_rectTransform.rect.width + (Mathf.Max(this.m_margin.x, 0f) - Mathf.Max(this.m_margin.z, 0f)) / 2f, (0.5f - this.m_rectTransform.pivot.y) * this.m_rectTransform.rect.height + (-Mathf.Max(this.m_margin.y, 0f) + Mathf.Max(this.m_margin.w, 0f)) / 2f);
				Vector4 value = new Vector4(vector.x, vector.y, z, w);
				this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, value);
			}
		}

		protected override Material GetMaterial(Material mat)
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (this.m_fontMaterial == null || this.m_fontMaterial.GetInstanceID() != mat.GetInstanceID())
			{
				this.m_fontMaterial = this.CreateMaterialInstance(mat);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_ShouldRecalculateStencil = true;
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			return this.m_sharedMaterial;
		}

		protected override Material[] GetMaterials(Material[] mats)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontMaterials == null)
			{
				this.m_fontMaterials = new Material[materialCount];
			}
			else if (this.m_fontMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					this.m_fontMaterials[i] = base.fontMaterial;
				}
				else
				{
					this.m_fontMaterials[i] = this.m_subTextObjects[i].material;
				}
			}
			this.m_fontSharedMaterials = this.m_fontMaterials;
			return this.m_fontMaterials;
		}

		protected override void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		protected override Material[] GetSharedMaterials()
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
			{
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					this.m_fontSharedMaterials[i] = this.m_sharedMaterial;
				}
				else
				{
					this.m_fontSharedMaterials[i] = this.m_subTextObjects[i].sharedMaterial;
				}
			}
			return this.m_fontSharedMaterials;
		}

		protected override void SetSharedMaterials(Material[] materials)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
			{
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					if (!(materials[i].GetTexture(ShaderUtilities.ID_MainTex) == null) && materials[i].GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() == this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
					{
						this.m_sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
						this.m_padding = this.GetPaddingForMaterial(this.m_sharedMaterial);
					}
				}
				else if (!(materials[i].GetTexture(ShaderUtilities.ID_MainTex) == null) && materials[i].GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() == this.m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
				{
					if (this.m_subTextObjects[i].isDefaultMaterial)
					{
						this.m_subTextObjects[i].sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
					}
				}
			}
		}

		protected override void SetOutlineThickness(float thickness)
		{
			if (this.m_fontMaterial != null && this.m_sharedMaterial.GetInstanceID() != this.m_fontMaterial.GetInstanceID())
			{
				this.m_sharedMaterial = this.m_fontMaterial;
				this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			else if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
				this.m_sharedMaterial = this.m_fontMaterial;
				this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			thickness = Mathf.Clamp01(thickness);
			this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			this.m_padding = this.GetPaddingForMaterial();
		}

		protected override void SetFaceColor(Color32 color)
		{
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_sharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, color);
		}

		protected override void SetOutlineColor(Color32 color)
		{
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_sharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, color);
		}

		protected override void SetShaderDepth()
		{
			if (this.m_canvas == null || this.m_sharedMaterial == null)
			{
				return;
			}
			if (this.m_canvas.renderMode == RenderMode.ScreenSpaceOverlay || this.m_isOverlay)
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
			}
			else
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
			}
		}

		protected override void SetCulling()
		{
			if (this.m_isCullingEnabled)
			{
				Material materialForRendering = this.materialForRendering;
				if (materialForRendering != null)
				{
					materialForRendering.SetFloat("_CullMode", 2f);
				}
				int num = 1;
				while (num < this.m_subTextObjects.Length && this.m_subTextObjects[num] != null)
				{
					materialForRendering = this.m_subTextObjects[num].materialForRendering;
					if (materialForRendering != null)
					{
						materialForRendering.SetFloat(ShaderUtilities.ShaderTag_CullMode, 2f);
					}
					num++;
				}
			}
			else
			{
				Material materialForRendering2 = this.materialForRendering;
				if (materialForRendering2 != null)
				{
					materialForRendering2.SetFloat("_CullMode", 0f);
				}
				int num2 = 1;
				while (num2 < this.m_subTextObjects.Length && this.m_subTextObjects[num2] != null)
				{
					materialForRendering2 = this.m_subTextObjects[num2].materialForRendering;
					if (materialForRendering2 != null)
					{
						materialForRendering2.SetFloat(ShaderUtilities.ShaderTag_CullMode, 0f);
					}
					num2++;
				}
			}
		}

		private void SetPerspectiveCorrection()
		{
			if (this.m_isOrthographic)
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0f);
			}
			else
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875f);
			}
		}

		protected override float GetPaddingForMaterial(Material mat)
		{
			this.m_padding = ShaderUtilities.GetPadding(mat, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = mat.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		protected override float GetPaddingForMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		private void SetMeshArrays(int size)
		{
			this.m_textInfo.meshInfo[0].ResizeMeshInfo(size);
			this.m_canvasRenderer.SetMesh(this.m_textInfo.meshInfo[0].mesh);
		}

		protected override int SetArraySizes(int[] chars)
		{
			int num = 0;
			int num2 = 0;
			this.m_totalCharacterCount = 0;
			this.m_isUsingBold = false;
			this.m_isParsingText = false;
			this.tag_NoParsing = false;
			this.m_style = this.m_fontStyle;
			this.m_fontWeightInternal = (((this.m_style & FontStyles.Bold) != FontStyles.Bold) ? this.m_fontWeight : 700);
			this.m_fontWeightStack.SetDefault(this.m_fontWeightInternal);
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(this.m_currentMaterialIndex, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			this.m_materialReferenceIndexLookup.Clear();
			MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
			if (this.m_textInfo == null)
			{
				this.m_textInfo = new TMP_TextInfo();
			}
			this.m_textElementType = TMP_TextElementType.Character;
			if (this.m_linkedTextComponent != null)
			{
				this.m_linkedTextComponent.text = string.Empty;
				this.m_linkedTextComponent.ForceMeshUpdate();
			}
			int num3 = 0;
			while (num3 < chars.Length && chars[num3] != 0)
			{
				if (this.m_textInfo.characterInfo == null || this.m_totalCharacterCount >= this.m_textInfo.characterInfo.Length)
				{
					TMP_TextInfo.Resize<TMP_CharacterInfo>(ref this.m_textInfo.characterInfo, this.m_totalCharacterCount + 1, true);
				}
				int num4 = chars[num3];
				if (!this.m_isRichText || num4 != 60)
				{
					goto IL_2C7;
				}
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!base.ValidateHtmlTag(chars, num3 + 1, out num))
				{
					goto IL_2C7;
				}
				num3 = num;
				if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
				{
					this.m_isUsingBold = true;
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					MaterialReference[] materialReferences = this.m_materialReferences;
					int currentMaterialIndex2 = this.m_currentMaterialIndex;
					materialReferences[currentMaterialIndex2].referenceCount = materialReferences[currentMaterialIndex2].referenceCount + 1;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)(57344 + this.m_spriteIndex);
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteIndex = this.m_spriteIndex;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteAsset = this.m_currentSpriteAsset;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = this.m_textElementType;
					this.m_textElementType = TMP_TextElementType.Character;
					this.m_currentMaterialIndex = currentMaterialIndex;
					num2++;
					this.m_totalCharacterCount++;
				}
				IL_9F7:
				num3++;
				continue;
				IL_2C7:
				bool flag = false;
				bool isUsingAlternateTypeface = false;
				TMP_FontAsset currentFontAsset = this.m_currentFontAsset;
				Material currentMaterial = this.m_currentMaterial;
				int currentMaterialIndex3 = this.m_currentMaterialIndex;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num4))
						{
							num4 = (int)char.ToUpper((char)num4);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num4))
						{
							num4 = (int)char.ToLower((char)num4);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num4))
					{
						num4 = (int)char.ToUpper((char)num4);
					}
				}
				TMP_FontAsset tmp_FontAsset = base.GetFontAssetForWeight(this.m_fontWeightInternal);
				if (tmp_FontAsset != null)
				{
					flag = true;
					isUsingAlternateTypeface = true;
					this.m_currentFontAsset = tmp_FontAsset;
				}
				TMP_Glyph tmp_Glyph;
				tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num4, out tmp_Glyph);
				if (tmp_Glyph == null)
				{
					TMP_SpriteAsset tmp_SpriteAsset = base.spriteAsset;
					if (tmp_SpriteAsset != null)
					{
						int num5 = -1;
						tmp_SpriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(tmp_SpriteAsset, num4, out num5);
						if (num5 != -1)
						{
							this.m_textElementType = TMP_TextElementType.Sprite;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = this.m_textElementType;
							this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(tmp_SpriteAsset.material, tmp_SpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
							MaterialReference[] materialReferences2 = this.m_materialReferences;
							int currentMaterialIndex4 = this.m_currentMaterialIndex;
							materialReferences2[currentMaterialIndex4].referenceCount = materialReferences2[currentMaterialIndex4].referenceCount + 1;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num4;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteIndex = num5;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteAsset = tmp_SpriteAsset;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
							this.m_textElementType = TMP_TextElementType.Character;
							this.m_currentMaterialIndex = currentMaterialIndex3;
							num2++;
							this.m_totalCharacterCount++;
							goto IL_9F7;
						}
					}
				}
				if (tmp_Glyph == null && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
				{
					tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num4, out tmp_Glyph);
				}
				if (tmp_Glyph == null && TMP_Settings.defaultFontAsset != null)
				{
					tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num4, out tmp_Glyph);
				}
				if (tmp_Glyph == null)
				{
					TMP_SpriteAsset tmp_SpriteAsset2 = TMP_Settings.defaultSpriteAsset;
					if (tmp_SpriteAsset2 != null)
					{
						int num6 = -1;
						tmp_SpriteAsset2 = TMP_SpriteAsset.SearchFallbackForSprite(tmp_SpriteAsset2, num4, out num6);
						if (num6 != -1)
						{
							this.m_textElementType = TMP_TextElementType.Sprite;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = this.m_textElementType;
							this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(tmp_SpriteAsset2.material, tmp_SpriteAsset2, this.m_materialReferences, this.m_materialReferenceIndexLookup);
							MaterialReference[] materialReferences3 = this.m_materialReferences;
							int currentMaterialIndex5 = this.m_currentMaterialIndex;
							materialReferences3[currentMaterialIndex5].referenceCount = materialReferences3[currentMaterialIndex5].referenceCount + 1;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num4;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteIndex = num6;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteAsset = tmp_SpriteAsset2;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
							this.m_textElementType = TMP_TextElementType.Character;
							this.m_currentMaterialIndex = currentMaterialIndex3;
							num2++;
							this.m_totalCharacterCount++;
							goto IL_9F7;
						}
					}
				}
				if (tmp_Glyph == null)
				{
					int num7 = num4;
					num4 = (chars[num3] = ((TMP_Settings.missingGlyphCharacter != 0) ? TMP_Settings.missingGlyphCharacter : 9633));
					tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num4, out tmp_Glyph);
					if (tmp_Glyph == null && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
					{
						tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num4, out tmp_Glyph);
					}
					if (tmp_Glyph == null && TMP_Settings.defaultFontAsset != null)
					{
						tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num4, out tmp_Glyph);
					}
					if (tmp_Glyph == null)
					{
						num4 = (chars[num3] = 32);
						tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num4, out tmp_Glyph);
						if (!TMP_Settings.warningsDisabled)
						{
							Debug.LogWarning("Character with ASCII value of " + num7 + " was not found in the Font Asset Glyph Table. It was replaced by a space.", this);
						}
					}
				}
				if (tmp_FontAsset != null && tmp_FontAsset.GetInstanceID() != this.m_currentFontAsset.GetInstanceID())
				{
					flag = true;
					isUsingAlternateTypeface = false;
					this.m_currentFontAsset = tmp_FontAsset;
				}
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = TMP_TextElementType.Character;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].textElement = tmp_Glyph;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].isUsingAlternateTypeface = isUsingAlternateTypeface;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num4;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
				if (flag)
				{
					if (TMP_Settings.matchMaterialPreset)
					{
						this.m_currentMaterial = TMP_MaterialManager.GetFallbackMaterial(this.m_currentMaterial, this.m_currentFontAsset.material);
					}
					else
					{
						this.m_currentMaterial = this.m_currentFontAsset.material;
					}
					this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
				}
				if (!char.IsWhiteSpace((char)num4) && num4 != 8203)
				{
					if (this.m_materialReferences[this.m_currentMaterialIndex].referenceCount < 16383)
					{
						MaterialReference[] materialReferences4 = this.m_materialReferences;
						int currentMaterialIndex6 = this.m_currentMaterialIndex;
						materialReferences4[currentMaterialIndex6].referenceCount = materialReferences4[currentMaterialIndex6].referenceCount + 1;
					}
					else
					{
						this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(new Material(this.m_currentMaterial), this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
						MaterialReference[] materialReferences5 = this.m_materialReferences;
						int currentMaterialIndex7 = this.m_currentMaterialIndex;
						materialReferences5[currentMaterialIndex7].referenceCount = materialReferences5[currentMaterialIndex7].referenceCount + 1;
					}
				}
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].material = this.m_currentMaterial;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
				this.m_materialReferences[this.m_currentMaterialIndex].isFallbackMaterial = flag;
				if (flag)
				{
					this.m_materialReferences[this.m_currentMaterialIndex].fallbackMaterial = currentMaterial;
					this.m_currentFontAsset = currentFontAsset;
					this.m_currentMaterial = currentMaterial;
					this.m_currentMaterialIndex = currentMaterialIndex3;
				}
				this.m_totalCharacterCount++;
				goto IL_9F7;
			}
			if (this.m_isCalculatingPreferredValues)
			{
				this.m_isCalculatingPreferredValues = false;
				this.m_isInputParsingRequired = true;
				return this.m_totalCharacterCount;
			}
			this.m_textInfo.spriteCount = num2;
			int num8 = this.m_textInfo.materialCount = this.m_materialReferenceIndexLookup.Count;
			if (num8 > this.m_textInfo.meshInfo.Length)
			{
				TMP_TextInfo.Resize<TMP_MeshInfo>(ref this.m_textInfo.meshInfo, num8, false);
			}
			if (num8 > this.m_subTextObjects.Length)
			{
				TMP_TextInfo.Resize<TMP_SubMeshUI>(ref this.m_subTextObjects, Mathf.NextPowerOfTwo(num8 + 1));
			}
			if (this.m_textInfo.characterInfo.Length - this.m_totalCharacterCount > 256)
			{
				TMP_TextInfo.Resize<TMP_CharacterInfo>(ref this.m_textInfo.characterInfo, Mathf.Max(this.m_totalCharacterCount + 1, 256), true);
			}
			for (int i = 0; i < num8; i++)
			{
				if (i > 0)
				{
					if (this.m_subTextObjects[i] == null)
					{
						this.m_subTextObjects[i] = TMP_SubMeshUI.AddSubTextObject(this, this.m_materialReferences[i]);
						this.m_textInfo.meshInfo[i].vertices = null;
					}
					if (this.m_rectTransform.pivot != this.m_subTextObjects[i].rectTransform.pivot)
					{
						this.m_subTextObjects[i].rectTransform.pivot = this.m_rectTransform.pivot;
					}
					if (this.m_subTextObjects[i].sharedMaterial == null || this.m_subTextObjects[i].sharedMaterial.GetInstanceID() != this.m_materialReferences[i].material.GetInstanceID())
					{
						bool isDefaultMaterial = this.m_materialReferences[i].isDefaultMaterial;
						this.m_subTextObjects[i].isDefaultMaterial = isDefaultMaterial;
						if (!isDefaultMaterial || this.m_subTextObjects[i].sharedMaterial == null || this.m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() != this.m_materialReferences[i].material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
						{
							this.m_subTextObjects[i].sharedMaterial = this.m_materialReferences[i].material;
							this.m_subTextObjects[i].fontAsset = this.m_materialReferences[i].fontAsset;
							this.m_subTextObjects[i].spriteAsset = this.m_materialReferences[i].spriteAsset;
						}
					}
					if (this.m_materialReferences[i].isFallbackMaterial)
					{
						this.m_subTextObjects[i].fallbackMaterial = this.m_materialReferences[i].material;
						this.m_subTextObjects[i].fallbackSourceMaterial = this.m_materialReferences[i].fallbackMaterial;
					}
				}
				int referenceCount = this.m_materialReferences[i].referenceCount;
				if (this.m_textInfo.meshInfo[i].vertices == null || this.m_textInfo.meshInfo[i].vertices.Length < referenceCount * 4)
				{
					if (this.m_textInfo.meshInfo[i].vertices == null)
					{
						if (i == 0)
						{
							this.m_textInfo.meshInfo[i] = new TMP_MeshInfo(this.m_mesh, referenceCount + 1);
						}
						else
						{
							this.m_textInfo.meshInfo[i] = new TMP_MeshInfo(this.m_subTextObjects[i].mesh, referenceCount + 1);
						}
					}
					else
					{
						this.m_textInfo.meshInfo[i].ResizeMeshInfo((referenceCount <= 1024) ? Mathf.NextPowerOfTwo(referenceCount) : (referenceCount + 256));
					}
				}
				else if (this.m_textInfo.meshInfo[i].vertices.Length - referenceCount * 4 > 1024)
				{
					this.m_textInfo.meshInfo[i].ResizeMeshInfo((referenceCount <= 1024) ? Mathf.Max(Mathf.NextPowerOfTwo(referenceCount), 256) : (referenceCount + 256));
				}
			}
			int num9 = num8;
			while (num9 < this.m_subTextObjects.Length && this.m_subTextObjects[num9] != null)
			{
				if (num9 < this.m_textInfo.meshInfo.Length)
				{
					this.m_subTextObjects[num9].canvasRenderer.SetMesh(null);
				}
				num9++;
			}
			return this.m_totalCharacterCount;
		}

		protected override void ComputeMarginSize()
		{
			if (base.rectTransform != null)
			{
				this.m_marginWidth = this.m_rectTransform.rect.width - this.m_margin.x - this.m_margin.z;
				this.m_marginHeight = this.m_rectTransform.rect.height - this.m_margin.y - this.m_margin.w;
				this.m_RectTransformCorners = this.GetTextContainerLocalCorners();
			}
		}

		protected override void OnDidApplyAnimationProperties()
		{
			this.m_havePropertiesChanged = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			this.m_canvas = base.canvas;
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_canvas = base.canvas;
			this.ComputeMarginSize();
			this.m_havePropertiesChanged = true;
		}

		protected override void OnRectTransformDimensionsChange()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.ComputeMarginSize();
			this.UpdateSubObjectPivot();
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		private void LateUpdate()
		{
			if (this.m_rectTransform.hasChanged)
			{
				float y = this.m_rectTransform.lossyScale.y;
				if (!this.m_havePropertiesChanged && y != this.m_previousLossyScaleY && this.m_text != string.Empty && this.m_text != null)
				{
					this.UpdateSDFScale(y);
					this.m_previousLossyScaleY = y;
				}
				this.m_rectTransform.hasChanged = false;
			}
			if (this.m_isUsingLegacyAnimationComponent)
			{
				this.m_havePropertiesChanged = true;
				this.OnPreRenderCanvas();
			}
		}

		private void OnPreRenderCanvas()
		{
			if (!this.m_isAwake || (!this.m_ignoreActiveState && !this.IsActive()))
			{
				return;
			}
			if (this.m_canvas == null)
			{
				this.m_canvas = base.canvas;
				if (this.m_canvas == null)
				{
					return;
				}
			}
			this.loopCountA = 0;
			if (this.m_havePropertiesChanged || this.m_isLayoutDirty)
			{
				if (this.checkPaddingRequired)
				{
					this.UpdateMeshPadding();
				}
				if (this.m_isInputParsingRequired || this.m_isTextTruncated)
				{
					base.ParseInputText();
				}
				if (this.m_enableAutoSizing)
				{
					this.m_fontSize = Mathf.Clamp(this.m_fontSizeBase, this.m_fontSizeMin, this.m_fontSizeMax);
				}
				this.m_maxFontSize = this.m_fontSizeMax;
				this.m_minFontSize = this.m_fontSizeMin;
				this.m_lineSpacingDelta = 0f;
				this.m_charWidthAdjDelta = 0f;
				this.m_isCharacterWrappingEnabled = false;
				this.m_isTextTruncated = false;
				this.m_havePropertiesChanged = false;
				this.m_isLayoutDirty = false;
				this.m_ignoreActiveState = false;
				this.GenerateTextMesh();
			}
		}

		protected override void GenerateTextMesh()
		{
			if (this.m_fontAsset == null || this.m_fontAsset.characterDictionary == null)
			{
				Debug.LogWarning("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + base.GetInstanceID());
				return;
			}
			if (this.m_textInfo != null)
			{
				this.m_textInfo.Clear();
			}
			if (this.m_char_buffer == null || this.m_char_buffer.Length == 0 || this.m_char_buffer[0] == 0)
			{
				this.ClearMesh();
				this.m_preferredWidth = 0f;
				this.m_preferredHeight = 0f;
				TMPro_EventManager.ON_TEXT_CHANGED(this);
				return;
			}
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(this.m_currentMaterialIndex, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			this.m_currentSpriteAsset = this.m_spriteAsset;
			if (this.m_spriteAnimator != null)
			{
				this.m_spriteAnimator.StopAllAnimations();
			}
			int totalCharacterCount = this.m_totalCharacterCount;
			float num = this.m_fontScale = this.m_fontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
			float num2 = num;
			this.m_fontScaleMultiplier = 1f;
			this.m_currentFontSize = this.m_fontSize;
			this.m_sizeStack.SetDefault(this.m_currentFontSize);
			this.m_style = this.m_fontStyle;
			this.m_fontWeightInternal = (((this.m_style & FontStyles.Bold) != FontStyles.Bold) ? this.m_fontWeight : 700);
			this.m_fontWeightStack.SetDefault(this.m_fontWeightInternal);
			this.m_fontStyleStack.Clear();
			this.m_lineJustification = this.m_textAlignment;
			this.m_lineJustificationStack.SetDefault(this.m_lineJustification);
			float num3 = 0f;
			float num4 = 1f;
			this.m_baselineOffset = 0f;
			this.m_baselineOffsetStack.Clear();
			bool flag = false;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			bool flag2 = false;
			Vector3 zero3 = Vector3.zero;
			Vector3 zero4 = Vector3.zero;
			bool flag3 = false;
			Vector3 start = Vector3.zero;
			Vector3 vector = Vector3.zero;
			this.m_fontColor32 = this.m_fontColor;
			this.m_htmlColor = this.m_fontColor32;
			this.m_underlineColor = this.m_htmlColor;
			this.m_strikethroughColor = this.m_htmlColor;
			this.m_colorStack.SetDefault(this.m_htmlColor);
			this.m_underlineColorStack.SetDefault(this.m_htmlColor);
			this.m_strikethroughColorStack.SetDefault(this.m_htmlColor);
			this.m_highlightColorStack.SetDefault(this.m_htmlColor);
			this.m_colorGradientPreset = null;
			this.m_colorGradientStack.SetDefault(null);
			this.m_actionStack.Clear();
			this.m_isFXMatrixSet = false;
			this.m_lineOffset = 0f;
			this.m_lineHeight = -32767f;
			float num5 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
			this.m_cSpacing = 0f;
			this.m_monoSpacing = 0f;
			this.m_xAdvance = 0f;
			this.tag_LineIndent = 0f;
			this.tag_Indent = 0f;
			this.m_indentStack.SetDefault(0f);
			this.tag_NoParsing = false;
			this.m_characterCount = 0;
			this.m_firstCharacterOfLine = 0;
			this.m_lastCharacterOfLine = 0;
			this.m_firstVisibleCharacterOfLine = 0;
			this.m_lastVisibleCharacterOfLine = 0;
			this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
			this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
			this.m_lineNumber = 0;
			this.m_lineVisibleCharacterCount = 0;
			bool flag4 = true;
			this.m_firstOverflowCharacterIndex = -1;
			this.m_pageNumber = 0;
			int num6 = Mathf.Clamp(this.m_pageToDisplay - 1, 0, this.m_textInfo.pageInfo.Length - 1);
			int num7 = 0;
			int num8 = 0;
			Vector4 margin = this.m_margin;
			float marginWidth = this.m_marginWidth;
			float marginHeight = this.m_marginHeight;
			this.m_marginLeft = 0f;
			this.m_marginRight = 0f;
			this.m_width = -1f;
			float num9 = marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight;
			this.m_meshExtents.min = TMP_Text.k_LargePositiveVector2;
			this.m_meshExtents.max = TMP_Text.k_LargeNegativeVector2;
			this.m_textInfo.ClearLineInfo();
			this.m_maxCapHeight = 0f;
			this.m_maxAscender = 0f;
			this.m_maxDescender = 0f;
			float num10 = 0f;
			float num11 = 0f;
			bool flag5 = false;
			this.m_isNewPage = false;
			bool flag6 = true;
			this.m_isNonBreakingSpace = false;
			bool flag7 = false;
			bool flag8 = false;
			int num12 = 0;
			base.SaveWordWrappingState(ref this.m_SavedWordWrapState, -1, -1);
			base.SaveWordWrappingState(ref this.m_SavedLineState, -1, -1);
			this.loopCountA++;
			int num13 = 0;
			int num14 = 0;
			while (num14 < this.m_char_buffer.Length && this.m_char_buffer[num14] != 0)
			{
				int num15 = this.m_char_buffer[num14];
				this.m_textElementType = this.m_textInfo.characterInfo[this.m_characterCount].elementType;
				this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
				this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!this.m_isRichText || num15 != 60)
				{
					goto IL_5BA;
				}
				this.m_isParsingText = true;
				this.m_textElementType = TMP_TextElementType.Character;
				if (!base.ValidateHtmlTag(this.m_char_buffer, num14 + 1, out num13))
				{
					goto IL_5BA;
				}
				num14 = num13;
				if (this.m_textElementType != TMP_TextElementType.Character)
				{
					goto IL_5BA;
				}
				IL_3303:
				num14++;
				continue;
				IL_5BA:
				this.m_isParsingText = false;
				bool isUsingAlternateTypeface = this.m_textInfo.characterInfo[this.m_characterCount].isUsingAlternateTypeface;
				if (this.m_characterCount < this.m_firstVisibleCharacter)
				{
					this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
					this.m_textInfo.characterInfo[this.m_characterCount].character = '​';
					this.m_characterCount++;
					goto IL_3303;
				}
				float num16 = 1f;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num15))
						{
							num15 = (int)char.ToUpper((char)num15);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num15))
						{
							num15 = (int)char.ToLower((char)num15);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num15))
					{
						num16 = 0.8f;
						num15 = (int)char.ToUpper((char)num15);
					}
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					this.m_currentSpriteAsset = this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset;
					this.m_spriteIndex = this.m_textInfo.characterInfo[this.m_characterCount].spriteIndex;
					TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
					if (tmp_Sprite == null)
					{
						goto IL_3303;
					}
					if (num15 == 60)
					{
						num15 = 57344 + this.m_spriteIndex;
					}
					else
					{
						this.m_spriteColor = TMP_Text.s_colorWhite;
					}
					this.m_currentFontAsset = this.m_fontAsset;
					float num17 = this.m_currentFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
					num2 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num17;
					this.m_cached_TextElement = tmp_Sprite;
					this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
					this.m_textInfo.characterInfo[this.m_characterCount].scale = num17;
					this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset = this.m_currentSpriteAsset;
					this.m_textInfo.characterInfo[this.m_characterCount].fontAsset = this.m_currentFontAsset;
					this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex = this.m_currentMaterialIndex;
					this.m_currentMaterialIndex = currentMaterialIndex;
					num3 = 0f;
				}
				else if (this.m_textElementType == TMP_TextElementType.Character)
				{
					this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
					if (this.m_cached_TextElement == null)
					{
						goto IL_3303;
					}
					this.m_currentFontAsset = this.m_textInfo.characterInfo[this.m_characterCount].fontAsset;
					this.m_currentMaterial = this.m_textInfo.characterInfo[this.m_characterCount].material;
					this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
					this.m_fontScale = this.m_currentFontSize * num16 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
					num2 = this.m_fontScale * this.m_fontScaleMultiplier * this.m_cached_TextElement.scale;
					this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
					this.m_textInfo.characterInfo[this.m_characterCount].scale = num2;
					num3 = ((this.m_currentMaterialIndex != 0) ? this.m_subTextObjects[this.m_currentMaterialIndex].padding : this.m_padding);
				}
				float num18 = num2;
				if (num15 == 173)
				{
					num2 = 0f;
				}
				this.m_textInfo.characterInfo[this.m_characterCount].character = (char)num15;
				this.m_textInfo.characterInfo[this.m_characterCount].pointSize = this.m_currentFontSize;
				this.m_textInfo.characterInfo[this.m_characterCount].color = this.m_htmlColor;
				this.m_textInfo.characterInfo[this.m_characterCount].underlineColor = this.m_underlineColor;
				this.m_textInfo.characterInfo[this.m_characterCount].strikethroughColor = this.m_strikethroughColor;
				this.m_textInfo.characterInfo[this.m_characterCount].highlightColor = this.m_highlightColor;
				this.m_textInfo.characterInfo[this.m_characterCount].style = this.m_style;
				this.m_textInfo.characterInfo[this.m_characterCount].index = num14;
				GlyphValueRecord a = default(GlyphValueRecord);
				if (this.m_enableKerning)
				{
					KerningPair kerningPair = null;
					if (this.m_characterCount < totalCharacterCount - 1)
					{
						uint character = (uint)this.m_textInfo.characterInfo[this.m_characterCount + 1].character;
						KerningPairKey kerningPairKey = new KerningPairKey((uint)num15, character);
						this.m_currentFontAsset.kerningDictionary.TryGetValue((int)kerningPairKey.key, out kerningPair);
						if (kerningPair != null)
						{
							a = kerningPair.firstGlyphAdjustments;
						}
					}
					if (this.m_characterCount >= 1)
					{
						uint character2 = (uint)this.m_textInfo.characterInfo[this.m_characterCount - 1].character;
						KerningPairKey kerningPairKey2 = new KerningPairKey(character2, (uint)num15);
						this.m_currentFontAsset.kerningDictionary.TryGetValue((int)kerningPairKey2.key, out kerningPair);
						if (kerningPair != null)
						{
							a += kerningPair.secondGlyphAdjustments;
						}
					}
				}
				if (this.m_isRightToLeft)
				{
					this.m_xAdvance -= ((this.m_cached_TextElement.xAdvance * num4 + this.m_characterSpacing + this.m_wordSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
					if (char.IsWhiteSpace((char)num15) || num15 == 8203)
					{
						this.m_xAdvance -= this.m_wordSpacing * num2;
					}
				}
				float num19 = 0f;
				if (this.m_monoSpacing != 0f)
				{
					num19 = (this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num2) * (1f - this.m_charWidthAdjDelta);
					this.m_xAdvance += num19;
				}
				float num20;
				if (this.m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface && ((this.m_style & FontStyles.Bold) == FontStyles.Bold || (this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold))
				{
					if (this.m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
					{
						float @float = this.m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
						num20 = this.m_currentFontAsset.boldStyle / 4f * @float * this.m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
						if (num20 + num3 > @float)
						{
							num3 = @float - num20;
						}
					}
					else
					{
						num20 = 0f;
					}
					num4 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
				}
				else
				{
					if (this.m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
					{
						float float2 = this.m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
						num20 = this.m_currentFontAsset.normalStyle / 4f * float2 * this.m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
						if (num20 + num3 > float2)
						{
							num3 = float2 - num20;
						}
					}
					else
					{
						num20 = 0f;
					}
					num4 = 1f;
				}
				float baseline = this.m_currentFontAsset.fontInfo.Baseline;
				Vector3 vector2;
				vector2.x = this.m_xAdvance + (this.m_cached_TextElement.xOffset - num3 - num20 + a.xPlacement) * num2 * (1f - this.m_charWidthAdjDelta);
				vector2.y = (baseline + this.m_cached_TextElement.yOffset + num3 + a.yPlacement) * num2 - this.m_lineOffset + this.m_baselineOffset;
				vector2.z = 0f;
				Vector3 vector3;
				vector3.x = vector2.x;
				vector3.y = vector2.y - (this.m_cached_TextElement.height + num3 * 2f) * num2;
				vector3.z = 0f;
				Vector3 vector4;
				vector4.x = vector3.x + (this.m_cached_TextElement.width + num3 * 2f + num20 * 2f) * num2 * (1f - this.m_charWidthAdjDelta);
				vector4.y = vector2.y;
				vector4.z = 0f;
				Vector3 vector5;
				vector5.x = vector4.x;
				vector5.y = vector3.y;
				vector5.z = 0f;
				if (this.m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface && ((this.m_style & FontStyles.Italic) == FontStyles.Italic || (this.m_fontStyle & FontStyles.Italic) == FontStyles.Italic))
				{
					float num21 = (float)this.m_currentFontAsset.italicStyle * 0.01f;
					Vector3 b = new Vector3(num21 * ((this.m_cached_TextElement.yOffset + num3 + num20) * num2), 0f, 0f);
					Vector3 b2 = new Vector3(num21 * ((this.m_cached_TextElement.yOffset - this.m_cached_TextElement.height - num3 - num20) * num2), 0f, 0f);
					vector2 += b;
					vector3 += b2;
					vector4 += b;
					vector5 += b2;
				}
				if (this.m_isFXMatrixSet)
				{
					if (this.m_FXMatrix.m00 != 1f)
					{
					}
					Vector3 b3 = (vector4 + vector3) / 2f;
					vector2 = this.m_FXMatrix.MultiplyPoint3x4(vector2 - b3) + b3;
					vector3 = this.m_FXMatrix.MultiplyPoint3x4(vector3 - b3) + b3;
					vector4 = this.m_FXMatrix.MultiplyPoint3x4(vector4 - b3) + b3;
					vector5 = this.m_FXMatrix.MultiplyPoint3x4(vector5 - b3) + b3;
				}
				this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft = vector3;
				this.m_textInfo.characterInfo[this.m_characterCount].topLeft = vector2;
				this.m_textInfo.characterInfo[this.m_characterCount].topRight = vector4;
				this.m_textInfo.characterInfo[this.m_characterCount].bottomRight = vector5;
				this.m_textInfo.characterInfo[this.m_characterCount].origin = this.m_xAdvance;
				this.m_textInfo.characterInfo[this.m_characterCount].baseLine = 0f - this.m_lineOffset + this.m_baselineOffset;
				this.m_textInfo.characterInfo[this.m_characterCount].aspectRatio = (vector4.x - vector3.x) / (vector2.y - vector3.y);
				float num22 = this.m_currentFontAsset.fontInfo.Ascender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_textInfo.characterInfo[this.m_characterCount].scale : (num2 / num16)) + this.m_baselineOffset;
				this.m_textInfo.characterInfo[this.m_characterCount].ascender = num22 - this.m_lineOffset;
				this.m_maxLineAscender = ((num22 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num22);
				float num23 = this.m_currentFontAsset.fontInfo.Descender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_textInfo.characterInfo[this.m_characterCount].scale : (num2 / num16)) + this.m_baselineOffset;
				float num24 = this.m_textInfo.characterInfo[this.m_characterCount].descender = num23 - this.m_lineOffset;
				this.m_maxLineDescender = ((num23 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num23);
				if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript || (this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num25 = (num22 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num22 = this.m_maxLineAscender;
					this.m_maxLineAscender = ((num25 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num25);
					float num26 = (num23 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num23 = this.m_maxLineDescender;
					this.m_maxLineDescender = ((num26 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num26);
				}
				if (this.m_lineNumber == 0 || this.m_isNewPage)
				{
					this.m_maxAscender = ((this.m_maxAscender <= num22) ? num22 : this.m_maxAscender);
					this.m_maxCapHeight = Mathf.Max(this.m_maxCapHeight, this.m_currentFontAsset.fontInfo.CapHeight * num2 / num16);
				}
				if (this.m_lineOffset == 0f)
				{
					num10 = ((num10 <= num22) ? num22 : num10);
				}
				this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
				if (num15 == 9 || (!char.IsWhiteSpace((char)num15) && num15 != 8203) || this.m_textElementType == TMP_TextElementType.Sprite)
				{
					this.m_textInfo.characterInfo[this.m_characterCount].isVisible = true;
					num9 = ((this.m_width == -1f) ? (marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight) : Mathf.Min(marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width));
					this.m_textInfo.lineInfo[this.m_lineNumber].marginLeft = this.m_marginLeft;
					bool flag9 = (this.m_lineJustification & (TextAlignmentOptions)16) == (TextAlignmentOptions)16 || (this.m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8;
					float num27 = Mathf.Abs(this.m_xAdvance) + (this.m_isRightToLeft ? 0f : this.m_cached_TextElement.xAdvance) * (1f - this.m_charWidthAdjDelta) * ((num15 == 173) ? num18 : num2);
					if (num27 > num9 * ((!flag9) ? 1f : 1.05f))
					{
						num8 = this.m_characterCount - 1;
						if (base.enableWordWrapping && this.m_characterCount != this.m_firstCharacterOfLine)
						{
							if (num12 == this.m_SavedWordWrapState.previous_WordBreak || flag6)
							{
								if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
								{
									if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
									{
										this.loopCountA = 0;
										this.m_charWidthAdjDelta += 0.01f;
										this.GenerateTextMesh();
										return;
									}
									this.m_maxFontSize = this.m_fontSize;
									this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
									this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
									if (this.loopCountA > 20)
									{
										return;
									}
									this.GenerateTextMesh();
									return;
								}
								else if (!this.m_isCharacterWrappingEnabled)
								{
									if (!flag7)
									{
										flag7 = true;
									}
									else
									{
										this.m_isCharacterWrappingEnabled = true;
									}
								}
								else
								{
									flag8 = true;
								}
							}
							num14 = base.RestoreWordWrappingState(ref this.m_SavedWordWrapState);
							num12 = num14;
							if (this.m_char_buffer[num14] == 173)
							{
								this.m_isTextTruncated = true;
								this.m_char_buffer[num14] = 45;
								this.GenerateTextMesh();
								return;
							}
							if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f && !this.m_isNewPage)
							{
								float num28 = this.m_maxLineAscender - this.m_startOfLineAscender;
								this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num28);
								this.m_lineOffset += num28;
								this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
								this.m_SavedWordWrapState.previousLineAscender = this.m_maxLineAscender;
							}
							this.m_isNewPage = false;
							float num29 = this.m_maxLineAscender - this.m_lineOffset;
							float num30 = this.m_maxLineDescender - this.m_lineOffset;
							this.m_maxDescender = ((this.m_maxDescender >= num30) ? num30 : this.m_maxDescender);
							if (!flag5)
							{
								num11 = this.m_maxDescender;
							}
							if (this.m_useMaxVisibleDescender && (this.m_characterCount >= this.m_maxVisibleCharacters || this.m_lineNumber >= this.m_maxVisibleLines))
							{
								flag5 = true;
							}
							this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
							this.m_textInfo.lineInfo[this.m_lineNumber].firstVisibleCharacterIndex = (this.m_firstVisibleCharacterOfLine = ((this.m_firstCharacterOfLine <= this.m_firstVisibleCharacterOfLine) ? this.m_firstVisibleCharacterOfLine : this.m_firstCharacterOfLine));
							this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = (this.m_lastCharacterOfLine = ((this.m_characterCount - 1 <= 0) ? 0 : (this.m_characterCount - 1)));
							this.m_textInfo.lineInfo[this.m_lineNumber].lastVisibleCharacterIndex = (this.m_lastVisibleCharacterOfLine = ((this.m_lastVisibleCharacterOfLine >= this.m_firstVisibleCharacterOfLine) ? this.m_lastVisibleCharacterOfLine : this.m_firstVisibleCharacterOfLine));
							this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
							this.m_textInfo.lineInfo[this.m_lineNumber].visibleCharacterCount = this.m_lineVisibleCharacterCount;
							this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num30);
							this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num29);
							this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x;
							this.m_textInfo.lineInfo[this.m_lineNumber].width = num9;
							this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 - this.m_cSpacing;
							this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
							this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num29;
							this.m_textInfo.lineInfo[this.m_lineNumber].descender = num30;
							this.m_textInfo.lineInfo[this.m_lineNumber].lineHeight = num29 - num30 + num5 * num;
							this.m_firstCharacterOfLine = this.m_characterCount;
							this.m_lineVisibleCharacterCount = 0;
							base.SaveWordWrappingState(ref this.m_SavedLineState, num14, this.m_characterCount - 1);
							this.m_lineNumber++;
							flag4 = true;
							if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
							{
								base.ResizeLineExtents(this.m_lineNumber);
							}
							if (this.m_lineHeight == -32767f)
							{
								float num31 = this.m_textInfo.characterInfo[this.m_characterCount].ascender - this.m_textInfo.characterInfo[this.m_characterCount].baseLine;
								float num32 = 0f - this.m_maxLineDescender + num31 + (num5 + this.m_lineSpacing + this.m_lineSpacingDelta) * num;
								this.m_lineOffset += num32;
								this.m_startOfLineAscender = num31;
							}
							else
							{
								this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num;
							}
							this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
							this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
							this.m_xAdvance = this.tag_Indent;
							goto IL_3303;
						}
						else if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
						{
							if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
							{
								this.loopCountA = 0;
								this.m_charWidthAdjDelta += 0.01f;
								this.GenerateTextMesh();
								return;
							}
							this.m_maxFontSize = this.m_fontSize;
							this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
							this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
							if (this.loopCountA > 20)
							{
								return;
							}
							this.GenerateTextMesh();
							return;
						}
						else
						{
							switch (this.m_overflowMode)
							{
							case TextOverflowModes.Overflow:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								break;
							case TextOverflowModes.Ellipsis:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								this.m_isTextTruncated = true;
								if (this.m_characterCount >= 1)
								{
									this.m_char_buffer[num14 - 1] = 8230;
									this.m_char_buffer[num14] = 0;
									if (this.m_cached_Ellipsis_GlyphInfo != null)
									{
										this.m_textInfo.characterInfo[num8].character = '…';
										this.m_textInfo.characterInfo[num8].textElement = this.m_cached_Ellipsis_GlyphInfo;
										this.m_textInfo.characterInfo[num8].fontAsset = this.m_materialReferences[0].fontAsset;
										this.m_textInfo.characterInfo[num8].material = this.m_materialReferences[0].material;
										this.m_textInfo.characterInfo[num8].materialReferenceIndex = 0;
									}
									else
									{
										Debug.LogWarning("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + this.m_fontAsset.name + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.", this);
									}
									this.m_totalCharacterCount = num8 + 1;
									this.GenerateTextMesh();
									return;
								}
								this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
								break;
							case TextOverflowModes.Masking:
								if (!this.m_isMaskingEnabled)
								{
									this.EnableMasking();
								}
								break;
							case TextOverflowModes.Truncate:
								if (this.m_isMaskingEnabled)
								{
									this.DisableMasking();
								}
								this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
								break;
							case TextOverflowModes.ScrollRect:
								if (!this.m_isMaskingEnabled)
								{
									this.EnableMasking();
								}
								break;
							}
						}
					}
					if (num15 != 9)
					{
						Color32 vertexColor;
						if (this.m_overrideHtmlColors)
						{
							vertexColor = this.m_fontColor32;
						}
						else
						{
							vertexColor = this.m_htmlColor;
						}
						if (this.m_textElementType == TMP_TextElementType.Character)
						{
							this.SaveGlyphVertexInfo(num3, num20, vertexColor);
						}
						else if (this.m_textElementType == TMP_TextElementType.Sprite)
						{
							this.SaveSpriteVertexInfo(vertexColor);
						}
					}
					else
					{
						this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
						this.m_lastVisibleCharacterOfLine = this.m_characterCount;
						TMP_LineInfo[] lineInfo = this.m_textInfo.lineInfo;
						int lineNumber = this.m_lineNumber;
						lineInfo[lineNumber].spaceCount = lineInfo[lineNumber].spaceCount + 1;
						this.m_textInfo.spaceCount++;
					}
					if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible && num15 != 173)
					{
						if (flag4)
						{
							flag4 = false;
							this.m_firstVisibleCharacterOfLine = this.m_characterCount;
						}
						this.m_lineVisibleCharacterCount++;
						this.m_lastVisibleCharacterOfLine = this.m_characterCount;
					}
				}
				else if ((num15 == 10 || char.IsSeparator((char)num15)) && num15 != 173 && num15 != 8203 && num15 != 8288)
				{
					TMP_LineInfo[] lineInfo2 = this.m_textInfo.lineInfo;
					int lineNumber2 = this.m_lineNumber;
					lineInfo2[lineNumber2].spaceCount = lineInfo2[lineNumber2].spaceCount + 1;
					this.m_textInfo.spaceCount++;
				}
				if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f && !this.m_isNewPage)
				{
					float num33 = this.m_maxLineAscender - this.m_startOfLineAscender;
					this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num33);
					num24 -= num33;
					this.m_lineOffset += num33;
					this.m_startOfLineAscender += num33;
					this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
					this.m_SavedWordWrapState.previousLineAscender = this.m_startOfLineAscender;
				}
				this.m_textInfo.characterInfo[this.m_characterCount].lineNumber = this.m_lineNumber;
				this.m_textInfo.characterInfo[this.m_characterCount].pageNumber = this.m_pageNumber;
				if ((num15 != 10 && num15 != 13 && num15 != 8230) || this.m_textInfo.lineInfo[this.m_lineNumber].characterCount == 1)
				{
					this.m_textInfo.lineInfo[this.m_lineNumber].alignment = this.m_lineJustification;
				}
				if (this.m_maxAscender - num24 > marginHeight + 0.0001f)
				{
					if (this.m_enableAutoSizing && this.m_lineSpacingDelta > this.m_lineSpacingMax && this.m_lineNumber > 0)
					{
						this.loopCountA = 0;
						this.m_lineSpacingDelta -= 1f;
						this.GenerateTextMesh();
						return;
					}
					if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
					{
						this.m_maxFontSize = this.m_fontSize;
						this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
						this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
						if (this.loopCountA > 20)
						{
							return;
						}
						this.GenerateTextMesh();
						return;
					}
					else
					{
						if (this.m_firstOverflowCharacterIndex == -1)
						{
							this.m_firstOverflowCharacterIndex = this.m_characterCount;
						}
						switch (this.m_overflowMode)
						{
						case TextOverflowModes.Overflow:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							break;
						case TextOverflowModes.Ellipsis:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (this.m_lineNumber > 0)
							{
								this.m_char_buffer[this.m_textInfo.characterInfo[num8].index] = 8230;
								this.m_char_buffer[this.m_textInfo.characterInfo[num8].index + 1] = 0;
								if (this.m_cached_Ellipsis_GlyphInfo != null)
								{
									this.m_textInfo.characterInfo[num8].character = '…';
									this.m_textInfo.characterInfo[num8].textElement = this.m_cached_Ellipsis_GlyphInfo;
									this.m_textInfo.characterInfo[num8].fontAsset = this.m_materialReferences[0].fontAsset;
									this.m_textInfo.characterInfo[num8].material = this.m_materialReferences[0].material;
									this.m_textInfo.characterInfo[num8].materialReferenceIndex = 0;
								}
								else
								{
									Debug.LogWarning("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + this.m_fontAsset.name + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.", this);
								}
								this.m_totalCharacterCount = num8 + 1;
								this.GenerateTextMesh();
								this.m_isTextTruncated = true;
								return;
							}
							this.ClearMesh();
							return;
						case TextOverflowModes.Masking:
							if (!this.m_isMaskingEnabled)
							{
								this.EnableMasking();
							}
							break;
						case TextOverflowModes.Truncate:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (this.m_lineNumber > 0)
							{
								this.m_char_buffer[this.m_textInfo.characterInfo[num8].index + 1] = 0;
								this.m_totalCharacterCount = num8 + 1;
								this.GenerateTextMesh();
								this.m_isTextTruncated = true;
								return;
							}
							this.ClearMesh();
							return;
						case TextOverflowModes.ScrollRect:
							if (!this.m_isMaskingEnabled)
							{
								this.EnableMasking();
							}
							break;
						case TextOverflowModes.Page:
							if (this.m_isMaskingEnabled)
							{
								this.DisableMasking();
							}
							if (num15 != 13 && num15 != 10)
							{
								if (num14 == 0)
								{
									this.ClearMesh();
									return;
								}
								if (num7 == num14)
								{
									this.m_char_buffer[num14] = 0;
									this.m_isTextTruncated = true;
								}
								num7 = num14;
								num14 = base.RestoreWordWrappingState(ref this.m_SavedLineState);
								this.m_isNewPage = true;
								this.m_xAdvance = this.tag_Indent;
								this.m_lineOffset = 0f;
								this.m_maxAscender = 0f;
								num10 = 0f;
								this.m_lineNumber++;
								this.m_pageNumber++;
								goto IL_3303;
							}
							break;
						case TextOverflowModes.Linked:
							if (this.m_linkedTextComponent != null)
							{
								this.m_linkedTextComponent.text = base.text;
								this.m_linkedTextComponent.firstVisibleCharacter = this.m_characterCount;
								this.m_linkedTextComponent.ForceMeshUpdate();
							}
							if (this.m_lineNumber > 0)
							{
								this.m_char_buffer[num14] = 0;
								this.m_totalCharacterCount = this.m_characterCount;
								this.GenerateTextMesh();
								this.m_isTextTruncated = true;
								return;
							}
							this.ClearMesh();
							return;
						}
					}
				}
				if (num15 == 9)
				{
					float num34 = this.m_currentFontAsset.fontInfo.TabWidth * num2;
					float num35 = Mathf.Ceil(this.m_xAdvance / num34) * num34;
					this.m_xAdvance = ((num35 <= this.m_xAdvance) ? (this.m_xAdvance + num34) : num35);
				}
				else if (this.m_monoSpacing != 0f)
				{
					this.m_xAdvance += (this.m_monoSpacing - num19 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
					if (char.IsWhiteSpace((char)num15) || num15 == 8203)
					{
						this.m_xAdvance += this.m_wordSpacing * num2;
					}
				}
				else if (!this.m_isRightToLeft)
				{
					float num36 = 1f;
					if (this.m_isFXMatrixSet)
					{
						num36 = this.m_FXMatrix.m00;
					}
					this.m_xAdvance += ((this.m_cached_TextElement.xAdvance * num36 * num4 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset + a.xAdvance) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
					if (char.IsWhiteSpace((char)num15) || num15 == 8203)
					{
						this.m_xAdvance += this.m_wordSpacing * num2;
					}
				}
				else
				{
					this.m_xAdvance -= a.xAdvance * num2;
				}
				this.m_textInfo.characterInfo[this.m_characterCount].xAdvance = this.m_xAdvance;
				if (num15 == 13)
				{
					this.m_xAdvance = this.tag_Indent;
				}
				if (num15 == 10 || this.m_characterCount == totalCharacterCount - 1)
				{
					if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f && !this.m_isNewPage)
					{
						float num37 = this.m_maxLineAscender - this.m_startOfLineAscender;
						this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num37);
						num24 -= num37;
						this.m_lineOffset += num37;
					}
					this.m_isNewPage = false;
					float num38 = this.m_maxLineAscender - this.m_lineOffset;
					float num39 = this.m_maxLineDescender - this.m_lineOffset;
					this.m_maxDescender = ((this.m_maxDescender >= num39) ? num39 : this.m_maxDescender);
					if (!flag5)
					{
						num11 = this.m_maxDescender;
					}
					if (this.m_useMaxVisibleDescender && (this.m_characterCount >= this.m_maxVisibleCharacters || this.m_lineNumber >= this.m_maxVisibleLines))
					{
						flag5 = true;
					}
					this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
					this.m_textInfo.lineInfo[this.m_lineNumber].firstVisibleCharacterIndex = (this.m_firstVisibleCharacterOfLine = ((this.m_firstCharacterOfLine <= this.m_firstVisibleCharacterOfLine) ? this.m_firstVisibleCharacterOfLine : this.m_firstCharacterOfLine));
					this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = (this.m_lastCharacterOfLine = this.m_characterCount);
					this.m_textInfo.lineInfo[this.m_lineNumber].lastVisibleCharacterIndex = (this.m_lastVisibleCharacterOfLine = ((this.m_lastVisibleCharacterOfLine >= this.m_firstVisibleCharacterOfLine) ? this.m_lastVisibleCharacterOfLine : this.m_firstVisibleCharacterOfLine));
					this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
					this.m_textInfo.lineInfo[this.m_lineNumber].visibleCharacterCount = this.m_lineVisibleCharacterCount;
					this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num39);
					this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num38);
					this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x - num3 * num2;
					this.m_textInfo.lineInfo[this.m_lineNumber].width = num9;
					if (this.m_textInfo.lineInfo[this.m_lineNumber].characterCount == 1)
					{
						this.m_textInfo.lineInfo[this.m_lineNumber].alignment = this.m_lineJustification;
					}
					if (this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].isVisible)
					{
						this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 - this.m_cSpacing;
					}
					else
					{
						this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 - this.m_cSpacing;
					}
					this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
					this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num38;
					this.m_textInfo.lineInfo[this.m_lineNumber].descender = num39;
					this.m_textInfo.lineInfo[this.m_lineNumber].lineHeight = num38 - num39 + num5 * num;
					this.m_firstCharacterOfLine = this.m_characterCount + 1;
					this.m_lineVisibleCharacterCount = 0;
					if (num15 == 10)
					{
						base.SaveWordWrappingState(ref this.m_SavedLineState, num14, this.m_characterCount);
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num14, this.m_characterCount);
						this.m_lineNumber++;
						flag4 = true;
						flag7 = false;
						if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
						{
							base.ResizeLineExtents(this.m_lineNumber);
						}
						if (this.m_lineHeight == -32767f)
						{
							float num32 = 0f - this.m_maxLineDescender + num22 + (num5 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num;
							this.m_lineOffset += num32;
						}
						else
						{
							this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num;
						}
						this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
						this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
						this.m_startOfLineAscender = num22;
						this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
						num8 = this.m_characterCount - 1;
						this.m_characterCount++;
						goto IL_3303;
					}
				}
				if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible)
				{
					this.m_meshExtents.min.x = Mathf.Min(this.m_meshExtents.min.x, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.x);
					this.m_meshExtents.min.y = Mathf.Min(this.m_meshExtents.min.y, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.y);
					this.m_meshExtents.max.x = Mathf.Max(this.m_meshExtents.max.x, this.m_textInfo.characterInfo[this.m_characterCount].topRight.x);
					this.m_meshExtents.max.y = Mathf.Max(this.m_meshExtents.max.y, this.m_textInfo.characterInfo[this.m_characterCount].topRight.y);
				}
				if (this.m_overflowMode == TextOverflowModes.Page && num15 != 13 && num15 != 10)
				{
					if (this.m_pageNumber + 1 > this.m_textInfo.pageInfo.Length)
					{
						TMP_TextInfo.Resize<TMP_PageInfo>(ref this.m_textInfo.pageInfo, this.m_pageNumber + 1, true);
					}
					this.m_textInfo.pageInfo[this.m_pageNumber].ascender = num10;
					this.m_textInfo.pageInfo[this.m_pageNumber].descender = ((num23 >= this.m_textInfo.pageInfo[this.m_pageNumber].descender) ? this.m_textInfo.pageInfo[this.m_pageNumber].descender : num23);
					if (this.m_pageNumber == 0 && this.m_characterCount == 0)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
					}
					else if (this.m_characterCount > 0 && this.m_pageNumber != this.m_textInfo.characterInfo[this.m_characterCount - 1].pageNumber)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber - 1].lastCharacterIndex = this.m_characterCount - 1;
						this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
					}
					else if (this.m_characterCount == totalCharacterCount - 1)
					{
						this.m_textInfo.pageInfo[this.m_pageNumber].lastCharacterIndex = this.m_characterCount;
					}
				}
				if (this.m_enableWordWrapping || this.m_overflowMode == TextOverflowModes.Truncate || this.m_overflowMode == TextOverflowModes.Ellipsis)
				{
					if ((char.IsWhiteSpace((char)num15) || num15 == 8203 || num15 == 45 || num15 == 173) && (!this.m_isNonBreakingSpace || flag7) && num15 != 160 && num15 != 8209 && num15 != 8239 && num15 != 8288)
					{
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num14, this.m_characterCount);
						this.m_isCharacterWrappingEnabled = false;
						flag6 = false;
					}
					else if (((num15 > 4352 && num15 < 4607) || (num15 > 11904 && num15 < 40959) || (num15 > 43360 && num15 < 43391) || (num15 > 44032 && num15 < 55295) || (num15 > 63744 && num15 < 64255) || (num15 > 65072 && num15 < 65103) || (num15 > 65280 && num15 < 65519)) && !this.m_isNonBreakingSpace)
					{
						if (flag6 || flag8 || (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num15) && this.m_characterCount < totalCharacterCount - 1 && !TMP_Settings.linebreakingRules.followingCharacters.ContainsKey((int)this.m_textInfo.characterInfo[this.m_characterCount + 1].character)))
						{
							base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num14, this.m_characterCount);
							this.m_isCharacterWrappingEnabled = false;
							flag6 = false;
						}
					}
					else if (flag6 || this.m_isCharacterWrappingEnabled || flag8)
					{
						base.SaveWordWrappingState(ref this.m_SavedWordWrapState, num14, this.m_characterCount);
					}
				}
				this.m_characterCount++;
				goto IL_3303;
			}
			float num40 = this.m_maxFontSize - this.m_minFontSize;
			if (!this.m_isCharacterWrappingEnabled && this.m_enableAutoSizing && num40 > 0.051f && this.m_fontSize < this.m_fontSizeMax)
			{
				this.m_minFontSize = this.m_fontSize;
				this.m_fontSize += Mathf.Max((this.m_maxFontSize - this.m_fontSize) / 2f, 0.05f);
				this.m_fontSize = (float)((int)(Mathf.Min(this.m_fontSize, this.m_fontSizeMax) * 20f + 0.5f)) / 20f;
				if (this.loopCountA > 20)
				{
					return;
				}
				this.GenerateTextMesh();
				return;
			}
			else
			{
				this.m_isCharacterWrappingEnabled = false;
				if (this.m_characterCount == 0)
				{
					this.ClearMesh();
					TMPro_EventManager.ON_TEXT_CHANGED(this);
					return;
				}
				int num41 = this.m_materialReferences[0].referenceCount * 4;
				this.m_textInfo.meshInfo[0].Clear(false);
				Vector3 a2 = Vector3.zero;
				Vector3[] rectTransformCorners = this.m_RectTransformCorners;
				TextAlignmentOptions textAlignment = this.m_textAlignment;
				switch (textAlignment)
				{
				case TextAlignmentOptions.TopLeft:
				case TextAlignmentOptions.Top:
				case TextAlignmentOptions.TopRight:
				case TextAlignmentOptions.TopJustified:
					break;
				default:
					switch (textAlignment)
					{
					case TextAlignmentOptions.Left:
					case TextAlignmentOptions.Center:
					case TextAlignmentOptions.Right:
					case TextAlignmentOptions.Justified:
						break;
					default:
						switch (textAlignment)
						{
						case TextAlignmentOptions.BottomLeft:
						case TextAlignmentOptions.Bottom:
						case TextAlignmentOptions.BottomRight:
						case TextAlignmentOptions.BottomJustified:
							break;
						default:
							switch (textAlignment)
							{
							case TextAlignmentOptions.BaselineLeft:
							case TextAlignmentOptions.Baseline:
							case TextAlignmentOptions.BaselineRight:
							case TextAlignmentOptions.BaselineJustified:
								break;
							default:
								switch (textAlignment)
								{
								case TextAlignmentOptions.MidlineLeft:
								case TextAlignmentOptions.Midline:
								case TextAlignmentOptions.MidlineRight:
								case TextAlignmentOptions.MidlineJustified:
									break;
								default:
									switch (textAlignment)
									{
									case TextAlignmentOptions.CaplineLeft:
									case TextAlignmentOptions.Capline:
									case TextAlignmentOptions.CaplineRight:
									case TextAlignmentOptions.CaplineJustified:
										break;
									default:
										if (textAlignment == TextAlignmentOptions.TopFlush || textAlignment == TextAlignmentOptions.TopGeoAligned)
										{
											goto IL_35E3;
										}
										if (textAlignment == TextAlignmentOptions.Flush || textAlignment == TextAlignmentOptions.CenterGeoAligned)
										{
											goto IL_367C;
										}
										if (textAlignment == TextAlignmentOptions.BottomFlush || textAlignment == TextAlignmentOptions.BottomGeoAligned)
										{
											goto IL_3784;
										}
										if (textAlignment == TextAlignmentOptions.BaselineFlush || textAlignment == TextAlignmentOptions.BaselineGeoAligned)
										{
											goto IL_3819;
										}
										if (textAlignment == TextAlignmentOptions.MidlineFlush || textAlignment == TextAlignmentOptions.MidlineGeoAligned)
										{
											goto IL_3864;
										}
										if (textAlignment != TextAlignmentOptions.CaplineFlush && textAlignment != TextAlignmentOptions.CaplineGeoAligned)
										{
											goto IL_394F;
										}
										break;
									}
									a2 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_maxCapHeight - margin.y - margin.w) / 2f, 0f);
									goto IL_394F;
								}
								IL_3864:
								a2 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_meshExtents.max.y + margin.y + this.m_meshExtents.min.y - margin.w) / 2f, 0f);
								goto IL_394F;
							}
							IL_3819:
							a2 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f, 0f);
							goto IL_394F;
						}
						IL_3784:
						if (this.m_overflowMode != TextOverflowModes.Page)
						{
							a2 = rectTransformCorners[0] + new Vector3(margin.x, 0f - num11 + margin.w, 0f);
						}
						else
						{
							a2 = rectTransformCorners[0] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num6].descender + margin.w, 0f);
						}
						goto IL_394F;
					}
					IL_367C:
					if (this.m_overflowMode != TextOverflowModes.Page)
					{
						a2 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_maxAscender + margin.y + num11 - margin.w) / 2f, 0f);
					}
					else
					{
						a2 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_textInfo.pageInfo[num6].ascender + margin.y + this.m_textInfo.pageInfo[num6].descender - margin.w) / 2f, 0f);
					}
					goto IL_394F;
				}
				IL_35E3:
				if (this.m_overflowMode != TextOverflowModes.Page)
				{
					a2 = rectTransformCorners[1] + new Vector3(margin.x, 0f - this.m_maxAscender - margin.y, 0f);
				}
				else
				{
					a2 = rectTransformCorners[1] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num6].ascender - margin.y, 0f);
				}
				IL_394F:
				Vector3 vector6 = Vector3.zero;
				Vector3 b4 = Vector3.zero;
				int index_X = 0;
				int index_X2 = 0;
				int num42 = 0;
				int lineCount = 0;
				int num43 = 0;
				bool flag10 = false;
				bool flag11 = false;
				int num44 = 0;
				bool flag12 = !(this.m_canvas.worldCamera == null);
				float num45 = this.m_previousLossyScaleY = base.transform.lossyScale.y;
				RenderMode renderMode = this.m_canvas.renderMode;
				float scaleFactor = this.m_canvas.scaleFactor;
				Color32 color = Color.white;
				Color32 underlineColor = Color.white;
				Color32 highlightColor = new Color32(byte.MaxValue, byte.MaxValue, 0, 64);
				float num46 = 0f;
				float num47 = 0f;
				float num48 = 0f;
				float num49 = TMP_Text.k_LargePositiveFloat;
				int num50 = 0;
				float num51 = 0f;
				float num52 = 0f;
				float b5 = 0f;
				TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
				int i = 0;
				while (i < this.m_characterCount)
				{
					TMP_FontAsset fontAsset = characterInfo[i].fontAsset;
					char character3 = characterInfo[i].character;
					int lineNumber3 = characterInfo[i].lineNumber;
					TMP_LineInfo tmp_LineInfo = this.m_textInfo.lineInfo[lineNumber3];
					lineCount = lineNumber3 + 1;
					TextAlignmentOptions alignment = tmp_LineInfo.alignment;
					switch (alignment)
					{
					case TextAlignmentOptions.TopLeft:
						goto IL_3C47;
					case TextAlignmentOptions.Top:
						goto IL_3C92;
					default:
						switch (alignment)
						{
						case TextAlignmentOptions.Left:
							goto IL_3C47;
						case TextAlignmentOptions.Center:
							goto IL_3C92;
						default:
							switch (alignment)
							{
							case TextAlignmentOptions.BottomLeft:
								goto IL_3C47;
							case TextAlignmentOptions.Bottom:
								goto IL_3C92;
							default:
								switch (alignment)
								{
								case TextAlignmentOptions.BaselineLeft:
									goto IL_3C47;
								case TextAlignmentOptions.Baseline:
									goto IL_3C92;
								default:
									switch (alignment)
									{
									case TextAlignmentOptions.MidlineLeft:
										goto IL_3C47;
									case TextAlignmentOptions.Midline:
										goto IL_3C92;
									default:
										switch (alignment)
										{
										case TextAlignmentOptions.CaplineLeft:
											goto IL_3C47;
										case TextAlignmentOptions.Capline:
											goto IL_3C92;
										default:
											if (alignment == TextAlignmentOptions.TopFlush)
											{
												goto IL_3D7D;
											}
											if (alignment != TextAlignmentOptions.TopGeoAligned)
											{
												if (alignment == TextAlignmentOptions.Flush)
												{
													goto IL_3D7D;
												}
												if (alignment != TextAlignmentOptions.CenterGeoAligned)
												{
													if (alignment == TextAlignmentOptions.BottomFlush)
													{
														goto IL_3D7D;
													}
													if (alignment != TextAlignmentOptions.BottomGeoAligned)
													{
														if (alignment == TextAlignmentOptions.BaselineFlush)
														{
															goto IL_3D7D;
														}
														if (alignment != TextAlignmentOptions.BaselineGeoAligned)
														{
															if (alignment == TextAlignmentOptions.MidlineFlush)
															{
																goto IL_3D7D;
															}
															if (alignment != TextAlignmentOptions.MidlineGeoAligned)
															{
																if (alignment == TextAlignmentOptions.CaplineFlush)
																{
																	goto IL_3D7D;
																}
																if (alignment != TextAlignmentOptions.CaplineGeoAligned)
																{
																	break;
																}
															}
														}
													}
												}
											}
											vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width / 2f - (tmp_LineInfo.lineExtents.min.x + tmp_LineInfo.lineExtents.max.x) / 2f, 0f, 0f);
											break;
										case TextAlignmentOptions.CaplineRight:
											goto IL_3D20;
										case TextAlignmentOptions.CaplineJustified:
											goto IL_3D7D;
										}
										break;
									case TextAlignmentOptions.MidlineRight:
										goto IL_3D20;
									case TextAlignmentOptions.MidlineJustified:
										goto IL_3D7D;
									}
									break;
								case TextAlignmentOptions.BaselineRight:
									goto IL_3D20;
								case TextAlignmentOptions.BaselineJustified:
									goto IL_3D7D;
								}
								break;
							case TextAlignmentOptions.BottomRight:
								goto IL_3D20;
							case TextAlignmentOptions.BottomJustified:
								goto IL_3D7D;
							}
							break;
						case TextAlignmentOptions.Right:
							goto IL_3D20;
						case TextAlignmentOptions.Justified:
							goto IL_3D7D;
						}
						break;
					case TextAlignmentOptions.TopRight:
						goto IL_3D20;
					case TextAlignmentOptions.TopJustified:
						goto IL_3D7D;
					}
					IL_4038:
					b4 = a2 + vector6;
					bool isVisible = characterInfo[i].isVisible;
					if (isVisible)
					{
						TMP_TextElementType elementType = characterInfo[i].elementType;
						if (elementType != TMP_TextElementType.Character)
						{
							if (elementType != TMP_TextElementType.Sprite)
							{
							}
						}
						else
						{
							Extents lineExtents = tmp_LineInfo.lineExtents;
							float num53 = this.m_uvLineOffset * (float)lineNumber3 % 1f;
							switch (this.m_horizontalMapping)
							{
							case TextureMappingOptions.Character:
								characterInfo[i].vertex_BL.uv2.x = 0f;
								characterInfo[i].vertex_TL.uv2.x = 0f;
								characterInfo[i].vertex_TR.uv2.x = 1f;
								characterInfo[i].vertex_BR.uv2.x = 1f;
								break;
							case TextureMappingOptions.Line:
								if (this.m_textAlignment != TextAlignmentOptions.Justified)
								{
									characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num53;
									characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num53;
									characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num53;
									characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num53;
								}
								else
								{
									characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num53;
									characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num53;
									characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num53;
									characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num53;
								}
								break;
							case TextureMappingOptions.Paragraph:
								characterInfo[i].vertex_BL.uv2.x = (characterInfo[i].vertex_BL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num53;
								characterInfo[i].vertex_TL.uv2.x = (characterInfo[i].vertex_TL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num53;
								characterInfo[i].vertex_TR.uv2.x = (characterInfo[i].vertex_TR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num53;
								characterInfo[i].vertex_BR.uv2.x = (characterInfo[i].vertex_BR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num53;
								break;
							case TextureMappingOptions.MatchAspect:
							{
								switch (this.m_verticalMapping)
								{
								case TextureMappingOptions.Character:
									characterInfo[i].vertex_BL.uv2.y = 0f;
									characterInfo[i].vertex_TL.uv2.y = 1f;
									characterInfo[i].vertex_TR.uv2.y = 0f;
									characterInfo[i].vertex_BR.uv2.y = 1f;
									break;
								case TextureMappingOptions.Line:
									characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num53;
									characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num53;
									characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
									characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
									break;
								case TextureMappingOptions.Paragraph:
									characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num53;
									characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num53;
									characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
									characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
									break;
								case TextureMappingOptions.MatchAspect:
									Debug.Log("ERROR: Cannot Match both Vertical & Horizontal.");
									break;
								}
								float num54 = (1f - (characterInfo[i].vertex_BL.uv2.y + characterInfo[i].vertex_TL.uv2.y) * characterInfo[i].aspectRatio) / 2f;
								characterInfo[i].vertex_BL.uv2.x = characterInfo[i].vertex_BL.uv2.y * characterInfo[i].aspectRatio + num54 + num53;
								characterInfo[i].vertex_TL.uv2.x = characterInfo[i].vertex_BL.uv2.x;
								characterInfo[i].vertex_TR.uv2.x = characterInfo[i].vertex_TL.uv2.y * characterInfo[i].aspectRatio + num54 + num53;
								characterInfo[i].vertex_BR.uv2.x = characterInfo[i].vertex_TR.uv2.x;
								break;
							}
							}
							switch (this.m_verticalMapping)
							{
							case TextureMappingOptions.Character:
								characterInfo[i].vertex_BL.uv2.y = 0f;
								characterInfo[i].vertex_TL.uv2.y = 1f;
								characterInfo[i].vertex_TR.uv2.y = 1f;
								characterInfo[i].vertex_BR.uv2.y = 0f;
								break;
							case TextureMappingOptions.Line:
								characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - tmp_LineInfo.descender) / (tmp_LineInfo.ascender - tmp_LineInfo.descender);
								characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - tmp_LineInfo.descender) / (tmp_LineInfo.ascender - tmp_LineInfo.descender);
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								break;
							case TextureMappingOptions.Paragraph:
								characterInfo[i].vertex_BL.uv2.y = (characterInfo[i].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y);
								characterInfo[i].vertex_TL.uv2.y = (characterInfo[i].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y);
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								break;
							case TextureMappingOptions.MatchAspect:
							{
								float num55 = (1f - (characterInfo[i].vertex_BL.uv2.x + characterInfo[i].vertex_TR.uv2.x) / characterInfo[i].aspectRatio) / 2f;
								characterInfo[i].vertex_BL.uv2.y = num55 + characterInfo[i].vertex_BL.uv2.x / characterInfo[i].aspectRatio;
								characterInfo[i].vertex_TL.uv2.y = num55 + characterInfo[i].vertex_TR.uv2.x / characterInfo[i].aspectRatio;
								characterInfo[i].vertex_BR.uv2.y = characterInfo[i].vertex_BL.uv2.y;
								characterInfo[i].vertex_TR.uv2.y = characterInfo[i].vertex_TL.uv2.y;
								break;
							}
							}
							num46 = characterInfo[i].scale * (1f - this.m_charWidthAdjDelta);
							if (!characterInfo[i].isUsingAlternateTypeface && (characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
							{
								num46 *= -1f;
							}
							if (renderMode != RenderMode.ScreenSpaceOverlay)
							{
								if (renderMode != RenderMode.ScreenSpaceCamera)
								{
									if (renderMode == RenderMode.WorldSpace)
									{
										num46 *= num45;
									}
								}
								else
								{
									num46 *= ((!flag12) ? 1f : num45);
								}
							}
							else
							{
								num46 *= num45 / scaleFactor;
							}
							float num56 = characterInfo[i].vertex_BL.uv2.x;
							float num57 = characterInfo[i].vertex_BL.uv2.y;
							float num58 = characterInfo[i].vertex_TR.uv2.x;
							float num59 = characterInfo[i].vertex_TR.uv2.y;
							float num60 = (float)((int)num56);
							float num61 = (float)((int)num57);
							num56 -= num60;
							num58 -= num60;
							num57 -= num61;
							num59 -= num61;
							characterInfo[i].vertex_BL.uv2.x = base.PackUV(num56, num57);
							characterInfo[i].vertex_BL.uv2.y = num46;
							characterInfo[i].vertex_TL.uv2.x = base.PackUV(num56, num59);
							characterInfo[i].vertex_TL.uv2.y = num46;
							characterInfo[i].vertex_TR.uv2.x = base.PackUV(num58, num59);
							characterInfo[i].vertex_TR.uv2.y = num46;
							characterInfo[i].vertex_BR.uv2.x = base.PackUV(num58, num57);
							characterInfo[i].vertex_BR.uv2.y = num46;
						}
						if (i < this.m_maxVisibleCharacters && num42 < this.m_maxVisibleWords && lineNumber3 < this.m_maxVisibleLines && this.m_overflowMode != TextOverflowModes.Page)
						{
							TMP_CharacterInfo[] array = characterInfo;
							int num62 = i;
							array[num62].vertex_BL.position = array[num62].vertex_BL.position + b4;
							TMP_CharacterInfo[] array2 = characterInfo;
							int num63 = i;
							array2[num63].vertex_TL.position = array2[num63].vertex_TL.position + b4;
							TMP_CharacterInfo[] array3 = characterInfo;
							int num64 = i;
							array3[num64].vertex_TR.position = array3[num64].vertex_TR.position + b4;
							TMP_CharacterInfo[] array4 = characterInfo;
							int num65 = i;
							array4[num65].vertex_BR.position = array4[num65].vertex_BR.position + b4;
						}
						else if (i < this.m_maxVisibleCharacters && num42 < this.m_maxVisibleWords && lineNumber3 < this.m_maxVisibleLines && this.m_overflowMode == TextOverflowModes.Page && characterInfo[i].pageNumber == num6)
						{
							TMP_CharacterInfo[] array5 = characterInfo;
							int num66 = i;
							array5[num66].vertex_BL.position = array5[num66].vertex_BL.position + b4;
							TMP_CharacterInfo[] array6 = characterInfo;
							int num67 = i;
							array6[num67].vertex_TL.position = array6[num67].vertex_TL.position + b4;
							TMP_CharacterInfo[] array7 = characterInfo;
							int num68 = i;
							array7[num68].vertex_TR.position = array7[num68].vertex_TR.position + b4;
							TMP_CharacterInfo[] array8 = characterInfo;
							int num69 = i;
							array8[num69].vertex_BR.position = array8[num69].vertex_BR.position + b4;
						}
						else
						{
							characterInfo[i].vertex_BL.position = Vector3.zero;
							characterInfo[i].vertex_TL.position = Vector3.zero;
							characterInfo[i].vertex_TR.position = Vector3.zero;
							characterInfo[i].vertex_BR.position = Vector3.zero;
							characterInfo[i].isVisible = false;
						}
						if (elementType == TMP_TextElementType.Character)
						{
							this.FillCharacterVertexBuffers(i, index_X);
						}
						else if (elementType == TMP_TextElementType.Sprite)
						{
							this.FillSpriteVertexBuffers(i, index_X2);
						}
					}
					TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
					int num70 = i;
					characterInfo2[num70].bottomLeft = characterInfo2[num70].bottomLeft + b4;
					TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
					int num71 = i;
					characterInfo3[num71].topLeft = characterInfo3[num71].topLeft + b4;
					TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
					int num72 = i;
					characterInfo4[num72].topRight = characterInfo4[num72].topRight + b4;
					TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
					int num73 = i;
					characterInfo5[num73].bottomRight = characterInfo5[num73].bottomRight + b4;
					TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
					int num74 = i;
					characterInfo6[num74].origin = characterInfo6[num74].origin + b4.x;
					TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
					int num75 = i;
					characterInfo7[num75].xAdvance = characterInfo7[num75].xAdvance + b4.x;
					TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
					int num76 = i;
					characterInfo8[num76].ascender = characterInfo8[num76].ascender + b4.y;
					TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
					int num77 = i;
					characterInfo9[num77].descender = characterInfo9[num77].descender + b4.y;
					TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
					int num78 = i;
					characterInfo10[num78].baseLine = characterInfo10[num78].baseLine + b4.y;
					if (isVisible)
					{
					}
					if (lineNumber3 != num43 || i == this.m_characterCount - 1)
					{
						if (lineNumber3 != num43)
						{
							TMP_LineInfo[] lineInfo3 = this.m_textInfo.lineInfo;
							int num79 = num43;
							lineInfo3[num79].baseline = lineInfo3[num79].baseline + b4.y;
							TMP_LineInfo[] lineInfo4 = this.m_textInfo.lineInfo;
							int num80 = num43;
							lineInfo4[num80].ascender = lineInfo4[num80].ascender + b4.y;
							TMP_LineInfo[] lineInfo5 = this.m_textInfo.lineInfo;
							int num81 = num43;
							lineInfo5[num81].descender = lineInfo5[num81].descender + b4.y;
							this.m_textInfo.lineInfo[num43].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num43].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[num43].descender);
							this.m_textInfo.lineInfo[num43].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num43].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[num43].ascender);
						}
						if (i == this.m_characterCount - 1)
						{
							TMP_LineInfo[] lineInfo6 = this.m_textInfo.lineInfo;
							int num82 = lineNumber3;
							lineInfo6[num82].baseline = lineInfo6[num82].baseline + b4.y;
							TMP_LineInfo[] lineInfo7 = this.m_textInfo.lineInfo;
							int num83 = lineNumber3;
							lineInfo7[num83].ascender = lineInfo7[num83].ascender + b4.y;
							TMP_LineInfo[] lineInfo8 = this.m_textInfo.lineInfo;
							int num84 = lineNumber3;
							lineInfo8[num84].descender = lineInfo8[num84].descender + b4.y;
							this.m_textInfo.lineInfo[lineNumber3].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber3].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[lineNumber3].descender);
							this.m_textInfo.lineInfo[lineNumber3].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber3].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[lineNumber3].ascender);
						}
					}
					if (char.IsLetterOrDigit(character3) || character3 == '-' || character3 == '­' || character3 == '‐' || character3 == '‑')
					{
						if (!flag11)
						{
							flag11 = true;
							num44 = i;
						}
						if (flag11 && i == this.m_characterCount - 1)
						{
							int num85 = this.m_textInfo.wordInfo.Length;
							int wordCount = this.m_textInfo.wordCount;
							if (this.m_textInfo.wordCount + 1 > num85)
							{
								TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num85 + 1);
							}
							int num86 = i;
							this.m_textInfo.wordInfo[wordCount].firstCharacterIndex = num44;
							this.m_textInfo.wordInfo[wordCount].lastCharacterIndex = num86;
							this.m_textInfo.wordInfo[wordCount].characterCount = num86 - num44 + 1;
							this.m_textInfo.wordInfo[wordCount].textComponent = this;
							num42++;
							this.m_textInfo.wordCount++;
							TMP_LineInfo[] lineInfo9 = this.m_textInfo.lineInfo;
							int num87 = lineNumber3;
							lineInfo9[num87].wordCount = lineInfo9[num87].wordCount + 1;
						}
					}
					else if (flag11 || (i == 0 && (!char.IsPunctuation(character3) || char.IsWhiteSpace(character3) || character3 == '​' || i == this.m_characterCount - 1)))
					{
						if (i <= 0 || i >= characterInfo.Length - 1 || i >= this.m_characterCount || (character3 != '\'' && character3 != '’') || !char.IsLetterOrDigit(characterInfo[i - 1].character) || !char.IsLetterOrDigit(characterInfo[i + 1].character))
						{
							int num86 = (i != this.m_characterCount - 1 || !char.IsLetterOrDigit(character3)) ? (i - 1) : i;
							flag11 = false;
							int num88 = this.m_textInfo.wordInfo.Length;
							int wordCount2 = this.m_textInfo.wordCount;
							if (this.m_textInfo.wordCount + 1 > num88)
							{
								TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num88 + 1);
							}
							this.m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num44;
							this.m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num86;
							this.m_textInfo.wordInfo[wordCount2].characterCount = num86 - num44 + 1;
							this.m_textInfo.wordInfo[wordCount2].textComponent = this;
							num42++;
							this.m_textInfo.wordCount++;
							TMP_LineInfo[] lineInfo10 = this.m_textInfo.lineInfo;
							int num89 = lineNumber3;
							lineInfo10[num89].wordCount = lineInfo10[num89].wordCount + 1;
						}
					}
					bool flag13 = (this.m_textInfo.characterInfo[i].style & FontStyles.Underline) == FontStyles.Underline;
					if (flag13)
					{
						bool flag14 = true;
						int pageNumber = this.m_textInfo.characterInfo[i].pageNumber;
						if (i > this.m_maxVisibleCharacters || lineNumber3 > this.m_maxVisibleLines || (this.m_overflowMode == TextOverflowModes.Page && pageNumber + 1 != this.m_pageToDisplay))
						{
							flag14 = false;
						}
						if (!char.IsWhiteSpace(character3) && character3 != '​')
						{
							num48 = Mathf.Max(num48, this.m_textInfo.characterInfo[i].scale);
							num49 = Mathf.Min((pageNumber != num50) ? TMP_Text.k_LargePositiveFloat : num49, this.m_textInfo.characterInfo[i].baseLine + base.font.fontInfo.Underline * num48);
							num50 = pageNumber;
						}
						if (!flag && flag14 && i <= tmp_LineInfo.lastVisibleCharacterIndex && character3 != '\n' && character3 != '\r')
						{
							if (i != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character3))
							{
								flag = true;
								num47 = this.m_textInfo.characterInfo[i].scale;
								if (num48 == 0f)
								{
									num48 = num47;
								}
								zero = new Vector3(this.m_textInfo.characterInfo[i].bottomLeft.x, num49, 0f);
								color = this.m_textInfo.characterInfo[i].underlineColor;
							}
						}
						if (flag && this.m_characterCount == 1)
						{
							flag = false;
							zero2 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, num49, 0f);
							float scale = this.m_textInfo.characterInfo[i].scale;
							this.DrawUnderlineMesh(zero, zero2, ref num41, num47, scale, num48, num46, color);
							num48 = 0f;
							num49 = TMP_Text.k_LargePositiveFloat;
						}
						else if (flag && (i == tmp_LineInfo.lastCharacterIndex || i >= tmp_LineInfo.lastVisibleCharacterIndex))
						{
							float scale;
							if (char.IsWhiteSpace(character3) || character3 == '​')
							{
								int lastVisibleCharacterIndex = tmp_LineInfo.lastVisibleCharacterIndex;
								zero2 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num49, 0f);
								scale = this.m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
							}
							else
							{
								zero2 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, num49, 0f);
								scale = this.m_textInfo.characterInfo[i].scale;
							}
							flag = false;
							this.DrawUnderlineMesh(zero, zero2, ref num41, num47, scale, num48, num46, color);
							num48 = 0f;
							num49 = TMP_Text.k_LargePositiveFloat;
						}
						else if (flag && !flag14)
						{
							flag = false;
							zero2 = new Vector3(this.m_textInfo.characterInfo[i - 1].topRight.x, num49, 0f);
							float scale = this.m_textInfo.characterInfo[i - 1].scale;
							this.DrawUnderlineMesh(zero, zero2, ref num41, num47, scale, num48, num46, color);
							num48 = 0f;
							num49 = TMP_Text.k_LargePositiveFloat;
						}
						else if (flag && i < this.m_characterCount - 1 && !color.Compare(this.m_textInfo.characterInfo[i + 1].underlineColor))
						{
							flag = false;
							zero2 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, num49, 0f);
							float scale = this.m_textInfo.characterInfo[i].scale;
							this.DrawUnderlineMesh(zero, zero2, ref num41, num47, scale, num48, num46, color);
							num48 = 0f;
							num49 = TMP_Text.k_LargePositiveFloat;
						}
					}
					else if (flag)
					{
						flag = false;
						zero2 = new Vector3(this.m_textInfo.characterInfo[i - 1].topRight.x, num49, 0f);
						float scale = this.m_textInfo.characterInfo[i - 1].scale;
						this.DrawUnderlineMesh(zero, zero2, ref num41, num47, scale, num48, num46, color);
						num48 = 0f;
						num49 = TMP_Text.k_LargePositiveFloat;
					}
					bool flag15 = (this.m_textInfo.characterInfo[i].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
					float strikethrough = fontAsset.fontInfo.strikethrough;
					if (flag15)
					{
						bool flag16 = true;
						if (i > this.m_maxVisibleCharacters || lineNumber3 > this.m_maxVisibleLines || (this.m_overflowMode == TextOverflowModes.Page && this.m_textInfo.characterInfo[i].pageNumber + 1 != this.m_pageToDisplay))
						{
							flag16 = false;
						}
						if (!flag2 && flag16 && i <= tmp_LineInfo.lastVisibleCharacterIndex && character3 != '\n' && character3 != '\r')
						{
							if (i != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character3))
							{
								flag2 = true;
								num51 = this.m_textInfo.characterInfo[i].pointSize;
								num52 = this.m_textInfo.characterInfo[i].scale;
								zero3 = new Vector3(this.m_textInfo.characterInfo[i].bottomLeft.x, this.m_textInfo.characterInfo[i].baseLine + strikethrough * num52, 0f);
								underlineColor = this.m_textInfo.characterInfo[i].strikethroughColor;
								b5 = this.m_textInfo.characterInfo[i].baseLine;
							}
						}
						if (flag2 && this.m_characterCount == 1)
						{
							flag2 = false;
							zero4 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + strikethrough * num52, 0f);
							this.DrawUnderlineMesh(zero3, zero4, ref num41, num52, num52, num52, num46, underlineColor);
						}
						else if (flag2 && i == tmp_LineInfo.lastCharacterIndex)
						{
							if (char.IsWhiteSpace(character3) || character3 == '​')
							{
								int lastVisibleCharacterIndex2 = tmp_LineInfo.lastVisibleCharacterIndex;
								zero4 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + strikethrough * num52, 0f);
							}
							else
							{
								zero4 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + strikethrough * num52, 0f);
							}
							flag2 = false;
							this.DrawUnderlineMesh(zero3, zero4, ref num41, num52, num52, num52, num46, underlineColor);
						}
						else if (flag2 && i < this.m_characterCount && (this.m_textInfo.characterInfo[i + 1].pointSize != num51 || !TMP_Math.Approximately(this.m_textInfo.characterInfo[i + 1].baseLine + b4.y, b5)))
						{
							flag2 = false;
							int lastVisibleCharacterIndex3 = tmp_LineInfo.lastVisibleCharacterIndex;
							if (i > lastVisibleCharacterIndex3)
							{
								zero4 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + strikethrough * num52, 0f);
							}
							else
							{
								zero4 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + strikethrough * num52, 0f);
							}
							this.DrawUnderlineMesh(zero3, zero4, ref num41, num52, num52, num52, num46, underlineColor);
						}
						else if (flag2 && i < this.m_characterCount && fontAsset.GetInstanceID() != characterInfo[i + 1].fontAsset.GetInstanceID())
						{
							flag2 = false;
							zero4 = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].baseLine + strikethrough * num52, 0f);
							this.DrawUnderlineMesh(zero3, zero4, ref num41, num52, num52, num52, num46, underlineColor);
						}
						else if (flag2 && !flag16)
						{
							flag2 = false;
							zero4 = new Vector3(this.m_textInfo.characterInfo[i - 1].topRight.x, this.m_textInfo.characterInfo[i - 1].baseLine + strikethrough * num52, 0f);
							this.DrawUnderlineMesh(zero3, zero4, ref num41, num52, num52, num52, num46, underlineColor);
						}
					}
					else if (flag2)
					{
						flag2 = false;
						zero4 = new Vector3(this.m_textInfo.characterInfo[i - 1].topRight.x, this.m_textInfo.characterInfo[i - 1].baseLine + strikethrough * num52, 0f);
						this.DrawUnderlineMesh(zero3, zero4, ref num41, num52, num52, num52, num46, underlineColor);
					}
					bool flag17 = (this.m_textInfo.characterInfo[i].style & FontStyles.Highlight) == FontStyles.Highlight;
					if (flag17)
					{
						bool flag18 = true;
						int pageNumber2 = this.m_textInfo.characterInfo[i].pageNumber;
						if (i > this.m_maxVisibleCharacters || lineNumber3 > this.m_maxVisibleLines || (this.m_overflowMode == TextOverflowModes.Page && pageNumber2 + 1 != this.m_pageToDisplay))
						{
							flag18 = false;
						}
						if (!flag3 && flag18 && i <= tmp_LineInfo.lastVisibleCharacterIndex && character3 != '\n' && character3 != '\r')
						{
							if (i != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character3))
							{
								flag3 = true;
								start = TMP_Text.k_LargePositiveVector2;
								vector = TMP_Text.k_LargeNegativeVector2;
								highlightColor = this.m_textInfo.characterInfo[i].highlightColor;
							}
						}
						if (flag3)
						{
							Color32 highlightColor2 = this.m_textInfo.characterInfo[i].highlightColor;
							bool flag19 = false;
							if (!highlightColor.Compare(highlightColor2))
							{
								vector.x = (vector.x + this.m_textInfo.characterInfo[i].bottomLeft.x) / 2f;
								start.y = Mathf.Min(start.y, this.m_textInfo.characterInfo[i].descender);
								vector.y = Mathf.Max(vector.y, this.m_textInfo.characterInfo[i].ascender);
								this.DrawTextHighlight(start, vector, ref num41, highlightColor);
								flag3 = true;
								start = vector;
								vector = new Vector3(this.m_textInfo.characterInfo[i].topRight.x, this.m_textInfo.characterInfo[i].descender, 0f);
								highlightColor = this.m_textInfo.characterInfo[i].highlightColor;
								flag19 = true;
							}
							if (!flag19)
							{
								start.x = Mathf.Min(start.x, this.m_textInfo.characterInfo[i].bottomLeft.x);
								start.y = Mathf.Min(start.y, this.m_textInfo.characterInfo[i].descender);
								vector.x = Mathf.Max(vector.x, this.m_textInfo.characterInfo[i].topRight.x);
								vector.y = Mathf.Max(vector.y, this.m_textInfo.characterInfo[i].ascender);
							}
						}
						if (flag3 && this.m_characterCount == 1)
						{
							flag3 = false;
							this.DrawTextHighlight(start, vector, ref num41, highlightColor);
						}
						else if (flag3 && (i == tmp_LineInfo.lastCharacterIndex || i >= tmp_LineInfo.lastVisibleCharacterIndex))
						{
							flag3 = false;
							this.DrawTextHighlight(start, vector, ref num41, highlightColor);
						}
						else if (flag3 && !flag18)
						{
							flag3 = false;
							this.DrawTextHighlight(start, vector, ref num41, highlightColor);
						}
					}
					else if (flag3)
					{
						flag3 = false;
						this.DrawTextHighlight(start, vector, ref num41, highlightColor);
					}
					num43 = lineNumber3;
					i++;
					continue;
					IL_3C47:
					if (!this.m_isRightToLeft)
					{
						vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
					}
					else
					{
						vector6 = new Vector3(0f - tmp_LineInfo.maxAdvance, 0f, 0f);
					}
					goto IL_4038;
					IL_3C92:
					vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width / 2f - tmp_LineInfo.maxAdvance / 2f, 0f, 0f);
					goto IL_4038;
					IL_3D20:
					if (!this.m_isRightToLeft)
					{
						vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width - tmp_LineInfo.maxAdvance, 0f, 0f);
					}
					else
					{
						vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
					}
					goto IL_4038;
					IL_3D7D:
					if (character3 == '­' || character3 == '​' || character3 == '⁠')
					{
						goto IL_4038;
					}
					char character4 = characterInfo[tmp_LineInfo.lastCharacterIndex].character;
					bool flag20 = (alignment & (TextAlignmentOptions)16) == (TextAlignmentOptions)16;
					if ((!char.IsControl(character4) && lineNumber3 < this.m_lineNumber) || flag20 || tmp_LineInfo.maxAdvance > tmp_LineInfo.width)
					{
						if (lineNumber3 != num43 || i == 0 || i == this.m_firstVisibleCharacter)
						{
							if (!this.m_isRightToLeft)
							{
								vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
							}
							else
							{
								vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
							}
							flag10 = char.IsSeparator(character3);
						}
						else
						{
							float num90 = this.m_isRightToLeft ? (tmp_LineInfo.width + tmp_LineInfo.maxAdvance) : (tmp_LineInfo.width - tmp_LineInfo.maxAdvance);
							int num91 = tmp_LineInfo.visibleCharacterCount - 1;
							int num92 = (!characterInfo[tmp_LineInfo.lastCharacterIndex].isVisible) ? (tmp_LineInfo.spaceCount - 1) : tmp_LineInfo.spaceCount;
							if (flag10)
							{
								num92--;
								num91++;
							}
							float num93 = (num92 <= 0) ? 1f : this.m_wordWrappingRatios;
							if (num92 < 1)
							{
								num92 = 1;
							}
							if (character3 == '\t' || char.IsSeparator(character3))
							{
								if (!this.m_isRightToLeft)
								{
									vector6 += new Vector3(num90 * (1f - num93) / (float)num92, 0f, 0f);
								}
								else
								{
									vector6 -= new Vector3(num90 * (1f - num93) / (float)num92, 0f, 0f);
								}
							}
							else if (!this.m_isRightToLeft)
							{
								vector6 += new Vector3(num90 * num93 / (float)num91, 0f, 0f);
							}
							else
							{
								vector6 -= new Vector3(num90 * num93 / (float)num91, 0f, 0f);
							}
						}
					}
					else if (!this.m_isRightToLeft)
					{
						vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
					}
					else
					{
						vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
					}
					goto IL_4038;
				}
				this.m_textInfo.characterCount = this.m_characterCount;
				this.m_textInfo.spriteCount = this.m_spriteCount;
				this.m_textInfo.lineCount = lineCount;
				this.m_textInfo.wordCount = ((num42 == 0 || this.m_characterCount <= 0) ? 1 : num42);
				this.m_textInfo.pageCount = this.m_pageNumber + 1;
				if (this.m_renderMode == TextRenderFlags.Render)
				{
					if (this.m_canvas.additionalShaderChannels != (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent))
					{
						this.m_canvas.additionalShaderChannels |= (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent);
					}
					if (this.m_geometrySortingOrder != VertexSortingOrder.Normal)
					{
						this.m_textInfo.meshInfo[0].SortGeometry(VertexSortingOrder.Reverse);
					}
					this.m_mesh.MarkDynamic();
					this.m_mesh.vertices = this.m_textInfo.meshInfo[0].vertices;
					this.m_mesh.uv = this.m_textInfo.meshInfo[0].uvs0;
					this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
					this.m_mesh.colors32 = this.m_textInfo.meshInfo[0].colors32;
					this.m_mesh.RecalculateBounds();
					this.m_canvasRenderer.SetMesh(this.m_mesh);
					Color color2 = this.m_canvasRenderer.GetColor();
					for (int j = 1; j < this.m_textInfo.materialCount; j++)
					{
						this.m_textInfo.meshInfo[j].ClearUnusedVertices();
						if (!(this.m_subTextObjects[j] == null))
						{
							if (this.m_geometrySortingOrder != VertexSortingOrder.Normal)
							{
								this.m_textInfo.meshInfo[j].SortGeometry(VertexSortingOrder.Reverse);
							}
							this.m_subTextObjects[j].mesh.vertices = this.m_textInfo.meshInfo[j].vertices;
							this.m_subTextObjects[j].mesh.uv = this.m_textInfo.meshInfo[j].uvs0;
							this.m_subTextObjects[j].mesh.uv2 = this.m_textInfo.meshInfo[j].uvs2;
							this.m_subTextObjects[j].mesh.colors32 = this.m_textInfo.meshInfo[j].colors32;
							this.m_subTextObjects[j].mesh.RecalculateBounds();
							this.m_subTextObjects[j].canvasRenderer.SetMesh(this.m_subTextObjects[j].mesh);
							this.m_subTextObjects[j].canvasRenderer.SetColor(color2);
						}
					}
				}
				TMPro_EventManager.ON_TEXT_CHANGED(this);
				return;
			}
		}

		protected override Vector3[] GetTextContainerLocalCorners()
		{
			if (this.m_rectTransform == null)
			{
				this.m_rectTransform = base.rectTransform;
			}
			this.m_rectTransform.GetLocalCorners(this.m_RectTransformCorners);
			return this.m_RectTransformCorners;
		}

		protected override void SetActiveSubMeshes(bool state)
		{
			int num = 1;
			while (num < this.m_subTextObjects.Length && this.m_subTextObjects[num] != null)
			{
				if (this.m_subTextObjects[num].enabled != state)
				{
					this.m_subTextObjects[num].enabled = state;
				}
				num++;
			}
		}

		protected override Bounds GetCompoundBounds()
		{
			Bounds bounds = this.m_mesh.bounds;
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			int num = 1;
			while (num < this.m_subTextObjects.Length && this.m_subTextObjects[num] != null)
			{
				Bounds bounds2 = this.m_subTextObjects[num].mesh.bounds;
				min.x = ((min.x >= bounds2.min.x) ? bounds2.min.x : min.x);
				min.y = ((min.y >= bounds2.min.y) ? bounds2.min.y : min.y);
				max.x = ((max.x <= bounds2.max.x) ? bounds2.max.x : max.x);
				max.y = ((max.y <= bounds2.max.y) ? bounds2.max.y : max.y);
				num++;
			}
			Vector3 center = (min + max) / 2f;
			Vector2 v = max - min;
			return new Bounds(center, v);
		}

		private void UpdateSDFScale(float lossyScale)
		{
			if (this.m_canvas == null)
			{
				this.m_canvas = this.GetCanvas();
				if (this.m_canvas == null)
				{
					return;
				}
			}
			lossyScale = ((lossyScale != 0f) ? lossyScale : 1f);
			float scaleFactor = this.m_canvas.scaleFactor;
			float num;
			if (this.m_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				num = lossyScale / scaleFactor;
			}
			else if (this.m_canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				num = ((!(this.m_canvas.worldCamera != null)) ? 1f : lossyScale);
			}
			else
			{
				num = lossyScale;
			}
			for (int i = 0; i < this.m_textInfo.characterCount; i++)
			{
				if (this.m_textInfo.characterInfo[i].isVisible && this.m_textInfo.characterInfo[i].elementType == TMP_TextElementType.Character)
				{
					float num2 = num * this.m_textInfo.characterInfo[i].scale * (1f - this.m_charWidthAdjDelta);
					if (!this.m_textInfo.characterInfo[i].isUsingAlternateTypeface && (this.m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num2 *= -1f;
					}
					int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = this.m_textInfo.characterInfo[i].vertexIndex;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num2;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num2;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num2;
					this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num2;
				}
			}
			for (int j = 0; j < this.m_textInfo.materialCount; j++)
			{
				if (j == 0)
				{
					this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
					this.m_canvasRenderer.SetMesh(this.m_mesh);
				}
				else
				{
					this.m_subTextObjects[j].mesh.uv2 = this.m_textInfo.meshInfo[j].uvs2;
					this.m_subTextObjects[j].canvasRenderer.SetMesh(this.m_subTextObjects[j].mesh);
				}
			}
		}

		protected override void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
			Vector3 b = new Vector3(0f, offset, 0f);
			for (int i = startIndex; i <= endIndex; i++)
			{
				TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
				int num = i;
				characterInfo[num].bottomLeft = characterInfo[num].bottomLeft - b;
				TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
				int num2 = i;
				characterInfo2[num2].topLeft = characterInfo2[num2].topLeft - b;
				TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
				int num3 = i;
				characterInfo3[num3].topRight = characterInfo3[num3].topRight - b;
				TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
				int num4 = i;
				characterInfo4[num4].bottomRight = characterInfo4[num4].bottomRight - b;
				TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
				int num5 = i;
				characterInfo5[num5].ascender = characterInfo5[num5].ascender - b.y;
				TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
				int num6 = i;
				characterInfo6[num6].baseLine = characterInfo6[num6].baseLine - b.y;
				TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
				int num7 = i;
				characterInfo7[num7].descender = characterInfo7[num7].descender - b.y;
				if (this.m_textInfo.characterInfo[i].isVisible)
				{
					TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
					int num8 = i;
					characterInfo8[num8].vertex_BL.position = characterInfo8[num8].vertex_BL.position - b;
					TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
					int num9 = i;
					characterInfo9[num9].vertex_TL.position = characterInfo9[num9].vertex_TL.position - b;
					TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
					int num10 = i;
					characterInfo10[num10].vertex_TR.position = characterInfo10[num10].vertex_TR.position - b;
					TMP_CharacterInfo[] characterInfo11 = this.m_textInfo.characterInfo;
					int num11 = i;
					characterInfo11[num11].vertex_BR.position = characterInfo11[num11].vertex_BR.position - b;
				}
			}
		}
	}
}
