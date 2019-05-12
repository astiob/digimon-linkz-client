using System;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal delegate void HandlesApplicatorFunction<T>(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<T> property);
}
