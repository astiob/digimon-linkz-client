using System;
using System.Collections.Generic;
using TypeSerialize;
using UnityEngine;

[Serializable]
public sealed class AssetBundleFileCategoryList
{
	private const string FILE_CATEGORY_LIST_NAME_PREFIX = "CatetoryList_";

	[SerializeField]
	private List<AssetBundleInfoData> categoryList = new List<AssetBundleInfoData>();

	private int byteSize;

	public AssetBundleFileCategoryList()
	{
	}

	public AssetBundleFileCategoryList(byte[] binary)
	{
		AssetBundleFileCategoryList assetBundleFileCategoryList = null;
		TypeSerializeHelper.BytesToData<AssetBundleFileCategoryList>(binary, out assetBundleFileCategoryList);
		if (assetBundleFileCategoryList != null)
		{
			this.categoryList = assetBundleFileCategoryList.categoryList;
		}
		else
		{
			global::Debug.LogWarning("Can not create AssetBundleFileCategoryList");
		}
	}

	public int GetSize()
	{
		return this.byteSize;
	}

	public void Add(AssetBundleInfoData category, int byteSize)
	{
		this.categoryList.Add(category);
		this.byteSize += byteSize;
	}

	public List<AssetBundleInfoData> GetCategoryList()
	{
		return this.categoryList;
	}

	public void SetCategoryList(List<AssetBundleInfoData> categoryList, int byteSize)
	{
		this.categoryList = categoryList;
		this.byteSize = byteSize;
	}

	public static string GetFileName(int index)
	{
		return "CatetoryList_" + index + ".bytes";
	}
}
