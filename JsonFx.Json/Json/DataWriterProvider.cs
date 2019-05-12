using System;
using System.Collections.Generic;
using System.IO;

namespace JsonFx.Json
{
	public class DataWriterProvider : IDataWriterProvider
	{
		private readonly IDataWriter DefaultWriter;

		private readonly IDictionary<string, IDataWriter> WritersByExt = new Dictionary<string, IDataWriter>(StringComparer.OrdinalIgnoreCase);

		private readonly IDictionary<string, IDataWriter> WritersByMime = new Dictionary<string, IDataWriter>(StringComparer.OrdinalIgnoreCase);

		public DataWriterProvider(IEnumerable<IDataWriter> writers)
		{
			if (writers != null)
			{
				foreach (IDataWriter dataWriter in writers)
				{
					if (this.DefaultWriter == null)
					{
						this.DefaultWriter = dataWriter;
					}
					if (!string.IsNullOrEmpty(dataWriter.ContentType))
					{
						this.WritersByMime[dataWriter.ContentType] = dataWriter;
					}
					if (!string.IsNullOrEmpty(dataWriter.ContentType))
					{
						string key = DataWriterProvider.NormalizeExtension(dataWriter.FileExtension);
						this.WritersByExt[key] = dataWriter;
					}
				}
			}
		}

		public IDataWriter DefaultDataWriter
		{
			get
			{
				return this.DefaultWriter;
			}
		}

		public IDataWriter Find(string extension)
		{
			extension = DataWriterProvider.NormalizeExtension(extension);
			if (this.WritersByExt.ContainsKey(extension))
			{
				return this.WritersByExt[extension];
			}
			return null;
		}

		public IDataWriter Find(string acceptHeader, string contentTypeHeader)
		{
			foreach (string key in DataWriterProvider.ParseHeaders(acceptHeader, contentTypeHeader))
			{
				if (this.WritersByMime.ContainsKey(key))
				{
					return this.WritersByMime[key];
				}
			}
			return null;
		}

		public static IEnumerable<string> ParseHeaders(string accept, string contentType)
		{
			string mime;
			foreach (string type in DataWriterProvider.SplitTrim(accept, ','))
			{
				mime = DataWriterProvider.ParseMediaType(type);
				if (!string.IsNullOrEmpty(mime))
				{
					yield return mime;
				}
			}
			mime = DataWriterProvider.ParseMediaType(contentType);
			if (!string.IsNullOrEmpty(mime))
			{
				yield return mime;
			}
			yield break;
		}

		public static string ParseMediaType(string type)
		{
			using (IEnumerator<string> enumerator = DataWriterProvider.SplitTrim(type, ';').GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return string.Empty;
		}

		private static IEnumerable<string> SplitTrim(string source, char ch)
		{
			if (string.IsNullOrEmpty(source))
			{
				yield break;
			}
			int length = source.Length;
			int prev = 0;
			int next = 0;
			while (prev < length && next >= 0)
			{
				next = source.IndexOf(ch, prev);
				if (next < 0)
				{
					next = length;
				}
				string part = source.Substring(prev, next - prev).Trim();
				if (part.Length > 0)
				{
					yield return part;
				}
				prev = next + 1;
			}
			yield break;
		}

		private static string NormalizeExtension(string extension)
		{
			if (string.IsNullOrEmpty(extension))
			{
				return string.Empty;
			}
			return Path.GetExtension(extension);
		}
	}
}
