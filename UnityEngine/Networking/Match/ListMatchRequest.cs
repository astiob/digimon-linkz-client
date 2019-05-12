using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
	/// <summary>
	///   <para>JSON object to request a list of UNET matches. This list is page based with a 1 index.</para>
	/// </summary>
	public class ListMatchRequest : Request
	{
		/// <summary>
		///   <para>Number of results per page to be returned.</para>
		/// </summary>
		public int pageSize { get; set; }

		/// <summary>
		///   <para>1 based page number requested.</para>
		/// </summary>
		public int pageNum { get; set; }

		/// <summary>
		///   <para>Name filter to apply to the match list.</para>
		/// </summary>
		public string nameFilter { get; set; }

		/// <summary>
		///   <para>Only return matches that have a password if this is true, only return matches without a password if this is false.</para>
		/// </summary>
		public bool includePasswordMatches { get; set; }

		/// <summary>
		///   <para>The optional game defined Elo score for the client making the request. The Elo score is averaged against all clients in a match and that value is used to produce better search results when listing available matches.
		/// If the Elo is provided the result set will be ordered according to the magnitude of the absoloute value of the difference of the a client searching for a match and the network average for all clients in each match. If the Elo score is not provided (and therefore 0 for all matches) the Elo score will not affect the search results.
		/// Each game can calculate this value as they wish according to whatever scale is best for that game.</para>
		/// </summary>
		public int eloScore { get; set; }

		/// <summary>
		///   <para>List of match attributes to filter against. This will filter down to matches that both have a name that contains the entire text string provided and the value specified in the filter is less than the attribute value for the matching name.
		/// No additional wildcards are allowed in the name. A maximum of 10 filters can be specified between all 3 filter lists.</para>
		/// </summary>
		public Dictionary<string, long> matchAttributeFilterLessThan { get; set; }

		/// <summary>
		///   <para>List of match attributes to filter against. This will filter down to matches that both have a name that contains the entire text string provided and the value specified in the filter is equal to the attribute value for the matching name.
		/// No additional wildcards are allowed in the name. A maximum of 10 filters can be specified between all 3 filter lists.</para>
		/// </summary>
		public Dictionary<string, long> matchAttributeFilterEqualTo { get; set; }

		/// <summary>
		///   <para>List of match attributes to filter against. This will filter down to matches that both have a name that contains the entire text string provided and the value specified in the filter is greater than the attribute value for the matching name.
		/// No additional wildcards are allowed in the name. A maximum of 10 filters can be specified between all 3 filter lists.</para>
		/// </summary>
		public Dictionary<string, long> matchAttributeFilterGreaterThan { get; set; }

		/// <summary>
		///   <para>Provides string description of current class data.</para>
		/// </summary>
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

		/// <summary>
		///   <para>Accessor to verify if the contained data is a valid request with respect to initialized variables and accepted parameters.</para>
		/// </summary>
		public override bool IsValid()
		{
			int num = (this.matchAttributeFilterLessThan != null) ? this.matchAttributeFilterLessThan.Count : 0;
			num += ((this.matchAttributeFilterEqualTo != null) ? this.matchAttributeFilterEqualTo.Count : 0);
			num += ((this.matchAttributeFilterGreaterThan != null) ? this.matchAttributeFilterGreaterThan.Count : 0);
			return base.IsValid() && (this.pageSize >= 1 || this.pageSize <= 1000) && num <= 10;
		}
	}
}
