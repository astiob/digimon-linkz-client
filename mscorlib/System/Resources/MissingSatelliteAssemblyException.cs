using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Resources
{
	/// <summary>The exception that is thrown when the satellite assembly for the resources of the neutral culture is missing.</summary>
	[ComVisible(true)]
	[Serializable]
	public class MissingSatelliteAssemblyException : SystemException
	{
		private string culture;

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.MissingSatelliteAssemblyException" /> class with default properties.</summary>
		public MissingSatelliteAssemblyException() : base(Locale.GetText("The satellite assembly was not found for the required culture."))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.MissingSatelliteAssemblyException" /> class with the specified error message. </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public MissingSatelliteAssemblyException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.MissingSatelliteAssemblyException" /> class with a specified error message and the name of a neutral culture. </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="cultureName">The name of the neutral culture.</param>
		public MissingSatelliteAssemblyException(string message, string cultureName) : base(message)
		{
			this.culture = cultureName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.MissingSatelliteAssemblyException" /> class from serialized data. </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination of the exception.</param>
		protected MissingSatelliteAssemblyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.MissingSatelliteAssemblyException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception. </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
		public MissingSatelliteAssemblyException(string message, Exception inner) : base(message, inner)
		{
		}

		/// <summary>Gets the name of a neutral culture. </summary>
		/// <returns>A <see cref="T:System.String" /> object with the name of the neutral culture.</returns>
		public string CultureName
		{
			get
			{
				return this.culture;
			}
		}
	}
}
