using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Used to set the default loader optimization policy for the main method of an executable application.</summary>
	/// <filterpriority>1</filterpriority>
	[AttributeUsage(AttributeTargets.Method)]
	[ComVisible(true)]
	public sealed class LoaderOptimizationAttribute : Attribute
	{
		private LoaderOptimization lo;

		/// <summary>Initializes a new instance of the <see cref="T:System.LoaderOptimizationAttribute" /> class to the specified value.</summary>
		/// <param name="value">A value equivalent to a <see cref="T:System.LoaderOptimization" /> constant. </param>
		public LoaderOptimizationAttribute(byte value)
		{
			this.lo = (LoaderOptimization)value;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.LoaderOptimizationAttribute" /> class to the specified value.</summary>
		/// <param name="value">A <see cref="T:System.LoaderOptimization" /> constant. </param>
		public LoaderOptimizationAttribute(LoaderOptimization value)
		{
			this.lo = value;
		}

		/// <summary>Gets the current <see cref="T:System.LoaderOptimization" /> value for this instance.</summary>
		/// <returns>A <see cref="T:System.LoaderOptimization" /> constant.</returns>
		/// <filterpriority>2</filterpriority>
		public LoaderOptimization Value
		{
			get
			{
				return this.lo;
			}
		}
	}
}
