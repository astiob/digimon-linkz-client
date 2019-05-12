using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Diagnostics
{
	/// <summary>Provides an abstract base class to create new debugging and tracing switches.</summary>
	/// <filterpriority>2</filterpriority>
	public abstract class Switch
	{
		private string name;

		private string description;

		private int switchSetting;

		private string value;

		private string defaultSwitchValue;

		private bool initialized;

		private System.Collections.Specialized.StringDictionary attributes = new System.Collections.Specialized.StringDictionary();

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.Switch" /> class.</summary>
		/// <param name="displayName">The name of the switch. </param>
		/// <param name="description">The description for the switch. </param>
		protected Switch(string displayName, string description)
		{
			this.name = displayName;
			this.description = description;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.Switch" /> class, specifying the display name, description, and default value for the switch. </summary>
		/// <param name="displayName">The name of the switch. </param>
		/// <param name="description">The description of the switch. </param>
		/// <param name="defaultSwitchValue">The default value for the switch.</param>
		protected Switch(string displayName, string description, string defaultSwitchValue) : this(displayName, description)
		{
			this.defaultSwitchValue = defaultSwitchValue;
		}

		/// <summary>Gets a description of the switch.</summary>
		/// <returns>The description of the switch. The default value is an empty string ("").</returns>
		/// <filterpriority>2</filterpriority>
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		/// <summary>Gets a name used to identify the switch.</summary>
		/// <returns>The name used to identify the switch. The default value is an empty string ("").</returns>
		/// <filterpriority>2</filterpriority>
		public string DisplayName
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets or sets the current setting for this switch.</summary>
		/// <returns>The current setting for this switch. The default is zero.</returns>
		protected int SwitchSetting
		{
			get
			{
				if (!this.initialized)
				{
					this.initialized = true;
					this.GetConfigFileSetting();
					this.OnSwitchSettingChanged();
				}
				return this.switchSetting;
			}
			set
			{
				if (this.switchSetting != value)
				{
					this.switchSetting = value;
					this.OnSwitchSettingChanged();
				}
				this.initialized = true;
			}
		}

		/// <summary>Gets the custom switch attributes defined in the application configuration file.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.StringDictionary" /> containing the case-insensitive custom attributes for the trace switch.</returns>
		/// <filterpriority>1</filterpriority>
		public System.Collections.Specialized.StringDictionary Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		/// <summary>Gets or sets the value of the switch.</summary>
		/// <returns>A string representing the value of the switch.</returns>
		/// <exception cref="T:System.Configuration.ConfigurationErrorsException">The value is null.-or-The value does not consist solely of an optional negative sign followed by a sequence of digits ranging from 0 to 9.-or-The value represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		protected string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				this.OnValueChanged();
			}
		}

		/// <summary>Gets the custom attributes supported by the switch.</summary>
		/// <returns>A string array that contains the names of the custom attributes supported by the switch, or null if there no custom attributes are supported.</returns>
		protected internal virtual string[] GetSupportedAttributes()
		{
			return null;
		}

		/// <summary>Invoked when the <see cref="P:System.Diagnostics.Switch.Value" /> property is changed.</summary>
		protected virtual void OnValueChanged()
		{
		}

		private void GetConfigFileSetting()
		{
			IDictionary dictionary = null;
			if (dictionary != null && dictionary.Contains(this.name))
			{
				this.switchSetting = (int)dictionary[this.name];
				return;
			}
			if (this.defaultSwitchValue != null)
			{
				this.value = this.defaultSwitchValue;
				this.OnValueChanged();
			}
		}

		/// <summary>Invoked when the <see cref="P:System.Diagnostics.Switch.SwitchSetting" /> property is changed.</summary>
		protected virtual void OnSwitchSettingChanged()
		{
		}
	}
}
