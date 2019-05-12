using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	internal class ShareResult : ResultBase, IShareResult, IResult
	{
		internal ShareResult(ResultContainer resultContainer) : base(resultContainer)
		{
			if (this.ResultDictionary != null)
			{
				string postId;
				if (this.ResultDictionary.TryGetValue(ShareResult.PostIDKey, out postId))
				{
					this.PostId = postId;
				}
				else if (this.ResultDictionary.TryGetValue("postId", out postId))
				{
					this.PostId = postId;
				}
			}
		}

		public string PostId { get; private set; }

		internal static string PostIDKey
		{
			get
			{
				return (!Constants.IsWeb) ? "id" : "post_id";
			}
		}

		public override string ToString()
		{
			return Utilities.FormatToString(base.ToString(), base.GetType().Name, new Dictionary<string, string>
			{
				{
					"PostId",
					this.PostId
				}
			});
		}
	}
}
