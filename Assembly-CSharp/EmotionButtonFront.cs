using Master;
using System;
using UnityEngine;

public class EmotionButtonFront : MonoBehaviour
{
	[SerializeField]
	[Header("エモーションを開くボタン")]
	private UIButton openEmotionButton;

	[SerializeField]
	[Header("スタンプのラベル")]
	private UILabel stampLabel;

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
	}

	public void Initialize(EmotionSenderMulti emotionSenderMulti, Action<int> callback)
	{
		emotionSenderMulti.Initialize(this.openEmotionButton, callback);
	}
}
