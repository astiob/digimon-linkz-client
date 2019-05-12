using Master;
using System;
using UnityEngine;

public class EmotionButtonFront : MonoBehaviour
{
	[Header("エモーションを開くボタン")]
	[SerializeField]
	private UIButton openEmotionButton;

	[SerializeField]
	[Header("スタンプのラベル")]
	private UILabel stampLabel;

	[SerializeField]
	[Header("スタンプの本体")]
	private UITexture[] stampTextureList;

	[SerializeField]
	[Header("スタンプの画像名")]
	private string[] stampNameList;

	private void Awake()
	{
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		if (this.stampLabel != null)
		{
			this.stampLabel.text = StringMaster.GetString("BattleUI-37");
		}
		for (int i = 1; i <= this.stampTextureList.Length; i++)
		{
			this.stampTextureList[i - 1].mainTexture = (AssetDataMng.Instance().LoadObject("StampIcons/JP/" + this.stampNameList[i - 1], null, true) as Texture2D);
		}
	}

	public void Initialize(EmotionSenderMulti emotionSenderMulti, Action<int> callback)
	{
		emotionSenderMulti.Initialize(this.openEmotionButton, callback);
	}
}
