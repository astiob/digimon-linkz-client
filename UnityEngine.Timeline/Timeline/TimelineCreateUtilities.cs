using System;
using System.Collections.Generic;

namespace UnityEngine.Timeline
{
	internal static class TimelineCreateUtilities
	{
		public static string GenerateUniqueActorName(List<ScriptableObject> tracks, string prefix)
		{
			string result;
			if (!tracks.Exists((ScriptableObject x) => x != null && x.name == prefix))
			{
				result = prefix;
			}
			else
			{
				int num = 1;
				string newName = prefix + num;
				while (tracks.Exists((ScriptableObject x) => x != null && x.name == newName))
				{
					num++;
					newName = prefix + num;
				}
				result = newName;
			}
			return result;
		}

		public static void SaveAssetIntoObject(Object childAsset, Object masterAsset)
		{
			if ((masterAsset.hideFlags & HideFlags.DontSave) != HideFlags.None)
			{
				childAsset.hideFlags |= HideFlags.DontSave;
			}
			else
			{
				childAsset.hideFlags |= HideFlags.HideInHierarchy;
			}
		}

		internal static bool ValidateParentTrack(TrackAsset parent, Type childType)
		{
			bool result;
			if (childType == null || !typeof(TrackAsset).IsAssignableFrom(childType))
			{
				result = false;
			}
			else if (parent == null)
			{
				result = true;
			}
			else
			{
				SupportsChildTracksAttribute supportsChildTracksAttribute = Attribute.GetCustomAttribute(parent.GetType(), typeof(SupportsChildTracksAttribute)) as SupportsChildTracksAttribute;
				if (supportsChildTracksAttribute == null)
				{
					result = false;
				}
				else if (supportsChildTracksAttribute.childType == null)
				{
					result = true;
				}
				else if (childType == supportsChildTracksAttribute.childType)
				{
					int num = 0;
					TrackAsset trackAsset = parent;
					while (trackAsset != null && trackAsset.isSubTrack)
					{
						num++;
						trackAsset = (trackAsset.parent as TrackAsset);
					}
					result = (num < supportsChildTracksAttribute.levels);
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}
}
