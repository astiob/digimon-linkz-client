using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	public interface IPropertyPreview
	{
		void GatherProperties(PlayableDirector director, IPropertyCollector driver);
	}
}
