using System;

namespace System.ComponentModel
{
	/// <summary>Defines identifiers used to indicate the type of filter that a <see cref="T:System.ComponentModel.ToolboxItemFilterAttribute" /> uses.</summary>
	public enum ToolboxItemFilterType
	{
		/// <summary>Indicates that a toolbox item filter string is allowed, but not required.</summary>
		Allow,
		/// <summary>Indicates that custom processing is required to determine whether to use a toolbox item filter string. </summary>
		Custom,
		/// <summary>Indicates that a toolbox item filter string is not allowed. </summary>
		Prevent,
		/// <summary>Indicates that a toolbox item filter string must be present for a toolbox item to be enabled. </summary>
		Require
	}
}
