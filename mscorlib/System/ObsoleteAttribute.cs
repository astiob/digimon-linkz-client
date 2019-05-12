using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Marks the program elements that are no longer in use. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	[Serializable]
	public sealed class ObsoleteAttribute : Attribute
	{
		private string _message;

		private bool _error;

		/// <summary>Initializes a new instance of the <see cref="T:System.ObsoleteAttribute" /> class with default properties.</summary>
		public ObsoleteAttribute()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ObsoleteAttribute" /> class with a specified workaround message.</summary>
		/// <param name="message">The text string that describes alternative workarounds. </param>
		public ObsoleteAttribute(string message)
		{
			this._message = message;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ObsoleteAttribute" /> class with a workaround message and a Boolean value indicating whether the obsolete element usage is considered an error.</summary>
		/// <param name="message">The text string that describes alternative workarounds. </param>
		/// <param name="error">The Boolean value that indicates whether the obsolete element usage is considered an error. </param>
		public ObsoleteAttribute(string message, bool error)
		{
			this._message = message;
			this._error = error;
		}

		/// <summary>Gets the workaround message, including a description of the alternative program elements.</summary>
		/// <returns>The workaround text string.</returns>
		/// <filterpriority>2</filterpriority>
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		/// <summary>Gets a Boolean value indicating whether the compiler will treat usage of the obsolete program element as an error.</summary>
		/// <returns>true if the obsolete element usage is considered an error; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsError
		{
			get
			{
				return this._error;
			}
		}
	}
}
