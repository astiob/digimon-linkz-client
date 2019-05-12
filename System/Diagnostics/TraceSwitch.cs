using System;

namespace System.Diagnostics
{
	/// <summary>Provides a multilevel switch to control tracing and debug output without recompiling your code.</summary>
	/// <filterpriority>2</filterpriority>
	[SwitchLevel(typeof(TraceLevel))]
	public class TraceSwitch : Switch
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.TraceSwitch" /> class, using the specified display name and description.</summary>
		/// <param name="displayName">The name to display on a user interface. </param>
		/// <param name="description">The description of the switch. </param>
		public TraceSwitch(string displayName, string description) : base(displayName, description)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.TraceSwitch" /> class, using the specified display name, description, and default value for the switch. </summary>
		/// <param name="displayName">The name to display on a user interface. </param>
		/// <param name="description">The description of the switch. </param>
		/// <param name="defaultSwitchValue">The default value of the switch.</param>
		public TraceSwitch(string displayName, string description, string defaultSwitchValue) : base(displayName, description)
		{
			base.Value = defaultSwitchValue;
		}

		/// <summary>Gets or sets the trace level that determines the messages the switch allows.</summary>
		/// <returns>One of the <see cref="T:System.Diagnostics.TraceLevel" /> values that that specifies the level of messages that are allowed by the switch.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Diagnostics.TraceSwitch.Level" /> is set to a value that is not one of the <see cref="T:System.Diagnostics.TraceLevel" /> values. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public TraceLevel Level
		{
			get
			{
				return (TraceLevel)base.SwitchSetting;
			}
			set
			{
				if (!Enum.IsDefined(typeof(TraceLevel), value))
				{
					throw new ArgumentException("value");
				}
				base.SwitchSetting = (int)value;
			}
		}

		/// <summary>Gets a value indicating whether the switch allows error-handling messages.</summary>
		/// <returns>true if the <see cref="P:System.Diagnostics.TraceSwitch.Level" /> property is set to <see cref="F:System.Diagnostics.TraceLevel.Error" />, <see cref="F:System.Diagnostics.TraceLevel.Warning" />, <see cref="F:System.Diagnostics.TraceLevel.Info" />, or <see cref="F:System.Diagnostics.TraceLevel.Verbose" />; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public bool TraceError
		{
			get
			{
				return base.SwitchSetting >= 1;
			}
		}

		/// <summary>Gets a value indicating whether the switch allows warning messages.</summary>
		/// <returns>true if the <see cref="P:System.Diagnostics.TraceSwitch.Level" /> property is set to <see cref="F:System.Diagnostics.TraceLevel.Warning" />, <see cref="F:System.Diagnostics.TraceLevel.Info" />, or <see cref="F:System.Diagnostics.TraceLevel.Verbose" />; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public bool TraceWarning
		{
			get
			{
				return base.SwitchSetting >= 2;
			}
		}

		/// <summary>Gets a value indicating whether the switch allows informational messages.</summary>
		/// <returns>true if the <see cref="P:System.Diagnostics.TraceSwitch.Level" /> property is set to <see cref="F:System.Diagnostics.TraceLevel.Info" /> or <see cref="F:System.Diagnostics.TraceLevel.Verbose" />; otherwise, false.  </returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public bool TraceInfo
		{
			get
			{
				return base.SwitchSetting >= 3;
			}
		}

		/// <summary>Gets a value indicating whether the switch allows all messages.</summary>
		/// <returns>true if the <see cref="P:System.Diagnostics.TraceSwitch.Level" /> property is set to <see cref="F:System.Diagnostics.TraceLevel.Verbose" />; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public bool TraceVerbose
		{
			get
			{
				return base.SwitchSetting >= 4;
			}
		}

		/// <summary>Updates and corrects the level for this switch.</summary>
		protected override void OnSwitchSettingChanged()
		{
			if (base.SwitchSetting < 0)
			{
				base.SwitchSetting = 0;
			}
			else if (base.SwitchSetting > 4)
			{
				base.SwitchSetting = 4;
			}
		}

		/// <summary>Sets the <see cref="P:System.Diagnostics.Switch.SwitchSetting" /> property to the integer equivalent of the <see cref="P:System.Diagnostics.Switch.Value" /> property.</summary>
		protected override void OnValueChanged()
		{
			base.SwitchSetting = (int)Enum.Parse(typeof(TraceLevel), base.Value, true);
		}
	}
}
