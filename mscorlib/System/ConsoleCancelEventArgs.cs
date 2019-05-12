using System;

namespace System
{
	/// <summary>Provides data for the <see cref="E:System.Console.CancelKeyPress" /> event. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	public sealed class ConsoleCancelEventArgs : EventArgs
	{
		private bool cancel;

		private ConsoleSpecialKey specialKey;

		internal ConsoleCancelEventArgs(ConsoleSpecialKey key)
		{
			this.specialKey = key;
		}

		/// <summary>Gets or sets a value indicating whether simultaneously pressing the <see cref="F:System.ConsoleModifiers.Control" /> modifier key and <see cref="F:System.ConsoleKey.C" /> console key (CTRL+C) terminates the current process.</summary>
		/// <returns>true if the current process should resume when the event handler concludes; false if the current process should terminate.</returns>
		/// <exception cref="T:System.InvalidOperationException">true was specified in a set operation and the event was caused by simultaneously pressing the <see cref="F:System.ConsoleModifiers.Control" /> modifier key and the BREAK console key (CTRL+BREAK).</exception>
		/// <filterpriority>2</filterpriority>
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		/// <summary>Gets the combination of modifier and console keys that interrupted the current process.</summary>
		/// <returns>One of the <see cref="T:System.ConsoleSpecialKey" /> values. There is no default value.</returns>
		/// <filterpriority>1</filterpriority>
		public ConsoleSpecialKey SpecialKey
		{
			get
			{
				return this.specialKey;
			}
		}
	}
}
