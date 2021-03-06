﻿using System;
using System.IO;

namespace JsonFx.Json
{
	public class JsonDataReader : IDataReader
	{
		public const string JsonMimeType = "application/json";

		public const string JsonFileExtension = ".json";

		private readonly JsonReaderSettings Settings;

		public JsonDataReader(JsonReaderSettings settings)
		{
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			this.Settings = settings;
		}

		public string ContentType
		{
			get
			{
				return "application/json";
			}
		}

		public object Deserialize(TextReader input, Type type)
		{
			return new JsonReader(input, this.Settings).Deserialize(type);
		}

		public static JsonReaderSettings CreateSettings(bool allowNullValueTypes)
		{
			return new JsonReaderSettings
			{
				AllowNullValueTypes = allowNullValueTypes
			};
		}
	}
}
