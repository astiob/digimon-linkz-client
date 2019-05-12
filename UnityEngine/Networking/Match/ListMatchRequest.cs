using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
	public class ListMatchRequest : Request
	{
		public int pageSize { get; set; }

		public int pageNum { get; set; }

		public string nameFilter { get; set; }

		public bool includePasswordMatches { get; set; }

		public int eloScore { get; set; }

		public Dictionary<string, long> matchAttributeFilterLessThan { get; set; }

		public Dictionary<string, long> matchAttributeFilterEqualTo { get; set; }

		public Dictionary<string, long> matchAttributeFilterGreaterThan { get; set; }

		public override string ToString()
		{
			return UnityString.Format("[{0}]-pageSize:{1},pageNum:{2},nameFilter:{3},matchAttributeFilterLessThan.Count:{4}, matchAttributeFilterGreaterThan.Count:{5}", new object[]
			{
				base.ToString(),
				this.pageSize,
				this.pageNum,
				this.nameFilter,
				(this.matchAttributeFilterLessThan != null) ? this.matchAttributeFilterLessThan.Count : 0,
				(this.matchAttributeFilterGreaterThan != null) ? this.matchAttributeFilterGreaterThan.Count : 0
			});
		}

		public override bool IsValid()
		{
			int num = (this.matchAttributeFilterLessThan != null) ? this.matchAttributeFilterLessThan.Count : 0;
			num += ((this.matchAttributeFilterEqualTo != null) ? this.matchAttributeFilterEqualTo.Count : 0);
			num += ((this.matchAttributeFilterGreaterThan != null) ? this.matchAttributeFilterGreaterThan.Count : 0);
			return base.IsValid() && (this.pageSize >= 1 || this.pageSize <= 1000) && num <= 10;
		}
	}
}
