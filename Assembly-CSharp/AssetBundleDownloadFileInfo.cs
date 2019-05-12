using System;
using TypeSerialize;

[Serializable]
public sealed class AssetBundleDownloadFileInfo
{
	public const string ASSET_BUNDLE_DOWNLOAD_FILE_INFO_NAME = "AB_info.bytes";

	public AssetBundleFileInfoList fileInfoList;

	public string[] categoryNameList;

	public AssetBundleDownloadFileInfo()
	{
	}

	public AssetBundleDownloadFileInfo(byte[] binary)
	{
		AssetBundleDownloadFileInfo assetBundleDownloadFileInfo = null;
		TypeSerializeHelper.BytesToData<AssetBundleDownloadFileInfo>(binary, out assetBundleDownloadFileInfo);
		if (assetBundleDownloadFileInfo != null)
		{
			this.fileInfoList = assetBundleDownloadFileInfo.fileInfoList;
			this.categoryNameList = assetBundleDownloadFileInfo.categoryNameList;
		}
		else
		{
			Debug.LogWarning("Can not create AssetBundleDownloadFileInfo");
		}
	}
}
