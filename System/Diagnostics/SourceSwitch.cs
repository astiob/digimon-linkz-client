using System;

namespace System.Diagnostics
{
	/// <summary>Provides a multilevel switch to control tracing and debug output without recompiling your code.</summary>
	/// <filterpriority>2</filterpriority>
	public class SourceSwitch : Switch
	{
		private const string description = "Source switch.";

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.SourceSwitch" /> class, specifying the name of the source.</summary>
		/// <param name="name">The name of the source.</param>
		public SourceSwitch(string displayName) : this(displayName, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.SourceSwitch" /> class, specifying the display name and the default value for the source switch.</summary>
		/// <param name="displayName">The name of the source switch. </param>
		/// <param name="defaultSwitchValue">The default value for the switch. </param>
		public SourceSwitch(string displayName, string defaultSwitchValue) : base(displayName, "Source switch.", defaultSwitchValue)
		{
		}

		/// <summary>Gets or sets the level of the switch.</summary>
		/// <returns>One of the <see cref="T:System.Diagnostics.SourceLevels" /> values that represents the event level of the switch.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public SourceLevels Level
		{
			get
			{
				return (SourceLevels)base.SwitchSetting;
			}
			set
			{
				base.SwitchSetting = (int)value;
			}
		}

		/// <summary>Determines if trace listeners should be called, based on the trace event type.</summary>
		/// <returns>True if the trace listeners should be called; otherwise, false.</returns>
		/// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values.</param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public bool ShouldTrace(TraceEventType eventType)
		{
			switch (eventType)
			{
			case TraceEventType.Critical:
				return (this.Level & SourceLevels.Critical) != SourceLevels.Off;
			case TraceEventType.Error:
				return (this.Level & SourceLevels.Error) != SourceLevels.Off;
			default:
				if (eventType != TraceEventType.Verbose)
				{
					if (eventType != TraceEventType.Start && eventType != TraceEventType.Stop && eventType != TraceEventType.Suspend && eventType != TraceEventType.Resume && eventType != TraceEventType.Transfer)
					{
					}
					return (this.Level & SourceLevels.ActivityTracing) != SourceLevels.Off;
				}
				return (this.Level & SourceLevels.Verbose) != SourceLevels.Off;
			case TraceEventType.Warning:
				return (this.Level & SourceLevels.Warning) != SourceLevels.Off;
			case TraceEventType.Information:
				return (this.Level & SourceLevels.Information) != SourceLevels.Off;
			}
		}

		/// <summary>Invoked when the value of the <see cref="P:System.Diagnostics.Switch.Value" /> property changes.</summary>
		/// <exception cref="T:System.ArgumentException">The new value of <see cref="P:System.Diagnostics.Switch.Value" /> is not one of the <see cref="T:System.Diagnostics.SourceLevels" /> values.</exception>
		protected override void OnValueChanged()
		{
			base.SwitchSetting = (int)Enum.Parse(typeof(SourceLevels), base.Value, true);
		}
	}
}
