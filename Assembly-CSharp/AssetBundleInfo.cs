using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AssetBundleInfo
{
	public int type;

	public string startName;

	public string endName;

	public string abName;

	public List<string> objNameList;

	public int realCT;

	public string level;

	public uint crc;

	public string size;

	public UnityEngine.Object[] buildObjs;

	public bool IsSame(AssetBundleInfo abi)
	{
		if (this.type != abi.type)
		{
			return false;
		}
		if (this.startName != abi.startName)
		{
			return false;
		}
		if (this.endName != abi.endName)
		{
			return false;
		}
		if (this.objNameList.Count != abi.objNameList.Count)
		{
			return false;
		}
		for (int i = 0; i < this.objNameList.Count; i++)
		{
			if (this.objNameList[i] != abi.objNameList[i])
			{
				return false;
			}
		}
		return this.realCT == abi.realCT;
	}

	public bool IsSame_2(AssetBundleInfo abi)
	{
		if (this.type != abi.type)
		{
			return false;
		}
		if (this.startName != abi.startName)
		{
			return false;
		}
		if (this.endName != abi.endName)
		{
			return false;
		}
		if (this.abName != abi.abName)
		{
			return false;
		}
		if (this.objNameList.Count != abi.objNameList.Count)
		{
			return false;
		}
		for (int i = 0; i < this.objNameList.Count; i++)
		{
			if (this.objNameList[i] != abi.objNameList[i])
			{
				return false;
			}
		}
		return this.realCT == abi.realCT && this.crc == abi.crc;
	}

	public bool ContainsPath(string path)
	{
		for (int i = 0; i < this.objNameList.Count; i++)
		{
			if (this.objNameList[i].Equals(path))
			{
				return true;
			}
		}
		return false;
	}

	public enum AB_TYPE
	{
		TYPE_PREFAB,
		TYPE_TEXTURE,
		TYPE_TXT,
		TYPE_AUDIO,
		TYPE_ASSET,
		TYPE_ANIM,
		TYPE_ANIMATOR,
		TYPE_BYTES
	}
}
