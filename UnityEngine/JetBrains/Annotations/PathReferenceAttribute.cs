using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class PathReferenceAttribute : Attribute
	{
		public PathReferenceAttribute()
		{
		}

		public PathReferenceAttribute([PathReference] string basePath)
		{
			this.BasePath = basePath;
		}

		[NotNull]
		public string BasePath { get; private set; }
	}
}
