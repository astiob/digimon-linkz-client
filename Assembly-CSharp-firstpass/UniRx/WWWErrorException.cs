using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace UniRx
{
	public class WWWErrorException : Exception
	{
		public WWWErrorException(WWW www, string text)
		{
			this.WWW = www;
			this.RawErrorMessage = www.error;
			this.ResponseHeaders = www.responseHeaders;
			this.HasResponse = false;
			this.Text = text;
			string[] array = this.RawErrorMessage.Split(new char[]
			{
				' ',
				':'
			});
			int statusCode;
			if (array.Length != 0 && int.TryParse(array[0], out statusCode))
			{
				this.HasResponse = true;
				this.StatusCode = (HttpStatusCode)statusCode;
			}
		}

		public string RawErrorMessage { get; private set; }

		public bool HasResponse { get; private set; }

		public string Text { get; private set; }

		public HttpStatusCode StatusCode { get; private set; }

		public Dictionary<string, string> ResponseHeaders { get; private set; }

		public WWW WWW { get; private set; }

		public override string ToString()
		{
			string text = this.Text;
			if (string.IsNullOrEmpty(text))
			{
				return this.RawErrorMessage;
			}
			return this.RawErrorMessage + " " + text;
		}
	}
}
