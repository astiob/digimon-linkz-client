using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.CSSLayout
{
	[NativeHeader("External/CSSLayout/CSSLayout/Native.bindings.h")]
	internal static class Native
	{
		private const string DllName = "CSSLayout";

		private static Dictionary<IntPtr, WeakReference> s_MeasureFunctions = new Dictionary<IntPtr, WeakReference>();

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CSSNodeNew();

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeInit(IntPtr cssNode);

		public static void CSSNodeFree(IntPtr cssNode)
		{
			if (!(cssNode == IntPtr.Zero))
			{
				Native.CSSNodeSetMeasureFunc(cssNode, null);
				Native.CSSNodeFreeInternal(cssNode);
			}
		}

		[NativeMethod(Name = "CSSNodeFree", IsFreeFunction = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CSSNodeFreeInternal(IntPtr cssNode);

		public static void CSSNodeReset(IntPtr cssNode)
		{
			Native.CSSNodeSetMeasureFunc(cssNode, null);
			Native.CSSNodeResetInternal(cssNode);
		}

		[NativeMethod(Name = "CSSNodeReset", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CSSNodeResetInternal(IntPtr cssNode);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int CSSNodeGetInstanceCount();

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSLayoutSetExperimentalFeatureEnabled(CSSExperimentalFeature feature, bool enabled);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CSSLayoutIsExperimentalFeatureEnabled(CSSExperimentalFeature feature);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeInsertChild(IntPtr node, IntPtr child, uint index);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeRemoveChild(IntPtr node, IntPtr child);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CSSNodeGetChild(IntPtr node, uint index);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint CSSNodeChildCount(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeCalculateLayout(IntPtr node, float availableWidth, float availableHeight, CSSDirection parentDirection);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeMarkDirty(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CSSNodeIsDirty(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodePrint(IntPtr node, CSSPrintOptions options);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CSSValueIsUndefined(float value);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeCopyStyle(IntPtr dstNode, IntPtr srcNode);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeSetContext(IntPtr node, IntPtr context);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CSSNodeGetContext(IntPtr node);

		public static void CSSNodeSetMeasureFunc(IntPtr node, CSSMeasureFunc measureFunc)
		{
			if (measureFunc != null)
			{
				Native.s_MeasureFunctions[node] = new WeakReference(measureFunc);
				CSSLayoutCallbacks.RegisterWrapper(node);
			}
			else if (Native.s_MeasureFunctions.ContainsKey(node))
			{
				Native.s_MeasureFunctions.Remove(node);
				CSSLayoutCallbacks.UnegisterWrapper(node);
			}
		}

		public static CSSMeasureFunc CSSNodeGetMeasureFunc(IntPtr node)
		{
			WeakReference weakReference = null;
			CSSMeasureFunc result;
			if (Native.s_MeasureFunctions.TryGetValue(node, out weakReference) && weakReference.IsAlive)
			{
				result = (weakReference.Target as CSSMeasureFunc);
			}
			else
			{
				result = null;
			}
			return result;
		}

		[RequiredByNativeCode]
		public unsafe static void CSSNodeMeasureInvoke(IntPtr node, float width, CSSMeasureMode widthMode, float height, CSSMeasureMode heightMode, IntPtr returnValueAddress)
		{
			CSSMeasureFunc cssmeasureFunc = Native.CSSNodeGetMeasureFunc(node);
			if (cssmeasureFunc != null)
			{
				*(CSSSize*)((void*)returnValueAddress) = cssmeasureFunc(node, width, widthMode, height, heightMode);
			}
		}

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeSetHasNewLayout(IntPtr node, bool hasNewLayout);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CSSNodeGetHasNewLayout(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetDirection(IntPtr node, CSSDirection direction);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSDirection CSSNodeStyleGetDirection(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetFlexDirection(IntPtr node, CSSFlexDirection flexDirection);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSFlexDirection CSSNodeStyleGetFlexDirection(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetJustifyContent(IntPtr node, CSSJustify justifyContent);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSJustify CSSNodeStyleGetJustifyContent(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetAlignContent(IntPtr node, CSSAlign alignContent);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSAlign CSSNodeStyleGetAlignContent(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetAlignItems(IntPtr node, CSSAlign alignItems);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSAlign CSSNodeStyleGetAlignItems(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetAlignSelf(IntPtr node, CSSAlign alignSelf);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSAlign CSSNodeStyleGetAlignSelf(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetPositionType(IntPtr node, CSSPositionType positionType);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSPositionType CSSNodeStyleGetPositionType(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetFlexWrap(IntPtr node, CSSWrap flexWrap);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSWrap CSSNodeStyleGetFlexWrap(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetOverflow(IntPtr node, CSSOverflow flexWrap);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSOverflow CSSNodeStyleGetOverflow(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetFlex(IntPtr node, float flex);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetFlexGrow(IntPtr node, float flexGrow);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetFlexGrow(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetFlexShrink(IntPtr node, float flexShrink);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetFlexShrink(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetFlexBasis(IntPtr node, float flexBasis);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetFlexBasis(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetWidth(IntPtr node, float width);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetWidth(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetHeight(IntPtr node, float height);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetHeight(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetMinWidth(IntPtr node, float minWidth);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetMinWidth(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetMinHeight(IntPtr node, float minHeight);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetMinHeight(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetMaxWidth(IntPtr node, float maxWidth);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetMaxWidth(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetMaxHeight(IntPtr node, float maxHeight);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetMaxHeight(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetAspectRatio(IntPtr node, float aspectRatio);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetAspectRatio(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetPosition(IntPtr node, CSSEdge edge, float position);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetPosition(IntPtr node, CSSEdge edge);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetMargin(IntPtr node, CSSEdge edge, float margin);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetMargin(IntPtr node, CSSEdge edge);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetPadding(IntPtr node, CSSEdge edge, float padding);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetPadding(IntPtr node, CSSEdge edge);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CSSNodeStyleSetBorder(IntPtr node, CSSEdge edge, float border);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeStyleGetBorder(IntPtr node, CSSEdge edge);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeLayoutGetLeft(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeLayoutGetTop(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeLayoutGetRight(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeLayoutGetBottom(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeLayoutGetWidth(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float CSSNodeLayoutGetHeight(IntPtr node);

		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CSSDirection CSSNodeLayoutGetDirection(IntPtr node);
	}
}
