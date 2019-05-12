using Master;
using System;
using UnityEngine;

public class EmotionButtonFront : MonoBehaviour
{
	[Header("エモーションのホルダー")]
	[SerializeField]
	private GameObject emotionFolder;

	[Header("エモーションを開くボタン")]
	[SerializeField]
	private UIButton openEmotionButton;

	[SerializeField]
	[Header("スタンプのラベル")]
	private UILabel stampLabel;

	[Header("エモーションを開くコライダー")]
	[SerializeField]
	private Collider openEmotionCollider;

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
