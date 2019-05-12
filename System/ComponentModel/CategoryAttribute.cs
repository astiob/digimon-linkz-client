using System;

namespace System.ComponentModel
{
	/// <summary>Specifies the name of the category in which to group the property or event when displayed in a <see cref="T:System.Windows.Forms.PropertyGrid" /> control set to Categorized mode.</summary>
	[AttributeUsage(AttributeTargets.All)]
	public class CategoryAttribute : Attribute
	{
		private string category;

		private bool IsLocalized;

		private static volatile CategoryAttribute action;

		private static volatile CategoryAttribute appearance;

		private static volatile CategoryAttribute behaviour;

		private static volatile CategoryAttribute data;

		private static volatile CategoryAttribute def;

		private static volatile CategoryAttribute design;

		private static volatile CategoryAttribute drag_drop;

		private static volatile CategoryAttribute focus;

		private static volatile CategoryAttribute format;

		private static volatile CategoryAttribute key;

		private static volatile CategoryAttribute layout;

		private static volatile CategoryAttribute mouse;

		private static volatile CategoryAttribute window_style;

		private static volatile CategoryAttribute async;

		private static object lockobj = new object();

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.CategoryAttribute" /> class using the category name Default.</summary>
		public CategoryAttribute()
		{
			this.category = "Misc";
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.CategoryAttribute" /> class using the specified category name.</summary>
		/// <param name="category">The name of the category. </param>
		public CategoryAttribute(string category)
		{
			this.category = category;
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Action category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the action category.</returns>
		public static CategoryAttribute Action
		{
			get
			{
				if (CategoryAttribute.action != null)
				{
					return CategoryAttribute.action;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.action == null)
					{
						CategoryAttribute.action = new CategoryAttribute("Action");
					}
				}
				return CategoryAttribute.action;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Appearance category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the appearance category.</returns>
		public static CategoryAttribute Appearance
		{
			get
			{
				if (CategoryAttribute.appearance != null)
				{
					return CategoryAttribute.appearance;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.appearance == null)
					{
						CategoryAttribute.appearance = new CategoryAttribute("Appearance");
					}
				}
				return CategoryAttribute.appearance;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Asynchronous category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the asynchronous category.</returns>
		public static CategoryAttribute Asynchronous
		{
			get
			{
				if (CategoryAttribute.behaviour != null)
				{
					return CategoryAttribute.behaviour;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.async == null)
					{
						CategoryAttribute.async = new CategoryAttribute("Asynchronous");
					}
				}
				return CategoryAttribute.async;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Behavior category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the behavior category.</returns>
		public static CategoryAttribute Behavior
		{
			get
			{
				if (CategoryAttribute.behaviour != null)
				{
					return CategoryAttribute.behaviour;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.behaviour == null)
					{
						CategoryAttribute.behaviour = new CategoryAttribute("Behavior");
					}
				}
				return CategoryAttribute.behaviour;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Data category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the data category.</returns>
		public static CategoryAttribute Data
		{
			get
			{
				if (CategoryAttribute.data != null)
				{
					return CategoryAttribute.data;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.data == null)
					{
						CategoryAttribute.data = new CategoryAttribute("Data");
					}
				}
				return CategoryAttribute.data;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Default category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the default category.</returns>
		public static CategoryAttribute Default
		{
			get
			{
				if (CategoryAttribute.def != null)
				{
					return CategoryAttribute.def;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.def == null)
					{
						CategoryAttribute.def = new CategoryAttribute("Default");
					}
				}
				return CategoryAttribute.def;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Design category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the design category.</returns>
		public static CategoryAttribute Design
		{
			get
			{
				if (CategoryAttribute.design != null)
				{
					return CategoryAttribute.design;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.design == null)
					{
						CategoryAttribute.design = new CategoryAttribute("Design");
					}
				}
				return CategoryAttribute.design;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the DragDrop category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the drag-and-drop category.</returns>
		public static CategoryAttribute DragDrop
		{
			get
			{
				if (CategoryAttribute.drag_drop != null)
				{
					return CategoryAttribute.drag_drop;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.drag_drop == null)
					{
						CategoryAttribute.drag_drop = new CategoryAttribute("DragDrop");
					}
				}
				return CategoryAttribute.drag_drop;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Focus category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the focus category.</returns>
		public static CategoryAttribute Focus
		{
			get
			{
				if (CategoryAttribute.focus != null)
				{
					return CategoryAttribute.focus;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.focus == null)
					{
						CategoryAttribute.focus = new CategoryAttribute("Focus");
					}
				}
				return CategoryAttribute.focus;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Format category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the format category.</returns>
		public static CategoryAttribute Format
		{
			get
			{
				if (CategoryAttribute.format != null)
				{
					return CategoryAttribute.format;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.format == null)
					{
						CategoryAttribute.format = new CategoryAttribute("Format");
					}
				}
				return CategoryAttribute.format;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Key category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the key category.</returns>
		public static CategoryAttribute Key
		{
			get
			{
				if (CategoryAttribute.key != null)
				{
					return CategoryAttribute.key;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.key == null)
					{
						CategoryAttribute.key = new CategoryAttribute("Key");
					}
				}
				return CategoryAttribute.key;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Layout category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the layout category.</returns>
		public static CategoryAttribute Layout
		{
			get
			{
				if (CategoryAttribute.layout != null)
				{
					return CategoryAttribute.layout;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.layout == null)
					{
						CategoryAttribute.layout = new CategoryAttribute("Layout");
					}
				}
				return CategoryAttribute.layout;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the Mouse category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the mouse category.</returns>
		public static CategoryAttribute Mouse
		{
			get
			{
				if (CategoryAttribute.mouse != null)
				{
					return CategoryAttribute.mouse;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.mouse == null)
					{
						CategoryAttribute.mouse = new CategoryAttribute("Mouse");
					}
				}
				return CategoryAttribute.mouse;
			}
		}

		/// <summary>Gets a <see cref="T:System.ComponentModel.CategoryAttribute" /> representing the WindowStyle category.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.CategoryAttribute" /> for the window style category.</returns>
		public static CategoryAttribute WindowStyle
		{
			get
			{
				if (CategoryAttribute.window_style != null)
				{
					return CategoryAttribute.window_style;
				}
				object obj = CategoryAttribute.lockobj;
				lock (obj)
				{
					if (CategoryAttribute.window_style == null)
					{
						CategoryAttribute.window_style = new CategoryAttribute("WindowStyle");
					}
				}
				return CategoryAttribute.window_style;
			}
		}

		/// <summary>Looks up the localized name of the specified category.</summary>
		/// <returns>The localized name of the category, or null if a localized name does not exist.</returns>
		/// <param name="value">The identifer for the category to look up. </param>
		protected virtual string GetLocalizedString(string value)
		{
			return Locale.GetText(value);
		}

		/// <summary>Gets the name of the category for the property or event that this attribute is applied to.</summary>
		/// <returns>The name of the category for the property or event that this attribute is applied to.</returns>
		public string Category
		{
			get
			{
				if (!this.IsLocalized)
				{
					this.IsLocalized = true;
					string localizedString = this.GetLocalizedString(this.category);
					if (localizedString != null)
					{
						this.category = localizedString;
					}
				}
				return this.category;
			}
		}

		/// <summary>Returns whether the value of the given object is equal to the current <see cref="T:System.ComponentModel.CategoryAttribute" />..</summary>
		/// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
		/// <param name="obj">The object to test the value equality of. </param>
		public override bool Equals(object obj)
		{
			return obj is CategoryAttribute && (obj == this || ((CategoryAttribute)obj).Category == this.category);
		}

		/// <summary>Returns the hash code for this attribute.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			return this.category.GetHashCode();
		}

		/// <summary>Determines if this attribute is the default.</summary>
		/// <returns>true if the attribute is the default value for this attribute class; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override bool IsDefaultAttribute()
		{
			return this.category == CategoryAttribute.Default.Category;
		}
	}
}
