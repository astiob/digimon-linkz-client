using System;
using System.Reflection;

namespace JsonFx
{
	public sealed class About
	{
		public static readonly About Fx = new About(typeof(About).Assembly);

		public readonly Version Version;

		public readonly string FullName;

		public readonly string Name;

		public readonly string Configuration;

		public readonly string Copyright;

		public readonly string Title;

		public readonly string Description;

		public readonly string Company;

		public About(Assembly assembly)
		{
			AssemblyName name = assembly.GetName();
			this.FullName = assembly.FullName;
			this.Version = name.Version;
			this.Name = name.Name;
			AssemblyCopyrightAttribute assemblyCopyrightAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
			this.Copyright = ((assemblyCopyrightAttribute == null) ? string.Empty : assemblyCopyrightAttribute.Copyright);
			AssemblyDescriptionAttribute assemblyDescriptionAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute)) as AssemblyDescriptionAttribute;
			this.Description = ((assemblyDescriptionAttribute == null) ? string.Empty : assemblyDescriptionAttribute.Description);
			AssemblyTitleAttribute assemblyTitleAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
			this.Title = ((assemblyTitleAttribute == null) ? string.Empty : assemblyTitleAttribute.Title);
			AssemblyCompanyAttribute assemblyCompanyAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute)) as AssemblyCompanyAttribute;
			this.Company = ((assemblyCompanyAttribute == null) ? string.Empty : assemblyCompanyAttribute.Company);
			AssemblyConfigurationAttribute assemblyConfigurationAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyConfigurationAttribute)) as AssemblyConfigurationAttribute;
			this.Configuration = ((assemblyConfigurationAttribute == null) ? string.Empty : assemblyConfigurationAttribute.Configuration);
		}
	}
}
