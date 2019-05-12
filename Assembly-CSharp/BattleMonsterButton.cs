using System;
using UnityEngine;

public class BattleMonsterButton : MonoBehaviour
{
	[SerializeField]
	public UIButton button;

	[Header("アイコン画像")]
	[SerializeField]
	public UI2DSprite monsterIcon;

	[SerializeField]
	public HoldPressButton playerMonsterDescriptionSwitch;

	[SerializeField]
	public UIScreenPosition currentSelection;

	[SerializeField]
	public UIComponentSkinner buttonMode;

	[SerializeField]
	private UIComponentSkinner leaderDisplay;

	[SerializeField]
	private UIComponentSkinner evolutionStep;

	[SerializeField]
	private UIComponentSkinner arousal;

	[SerializeField]
	public UIComponentSkinner playerNumber;

	[SerializeField]
	public UIComponentSkinner playerName;

	[SerializeField]
	public UITextReplacer playerNameText;

	public void ApplyMonsterButtonIcon(Sprite image, CharacterStateControl characterStatus, bool isLeader)
	{
		this.monsterIcon.sprite2D = image;
		this.evolutionStep.SetSkins(BattleUIControlBasic.GetEvolutionStepSetSkinner(characterStatus.evolutionStep));
		this.arousal.SetSkins(characterStatus.arousal);
		if (isLeader)
		{
			this.leaderDisplay.SetSkins(0);
		}
		else
		{
			this.leaderDisplay.SetSkins(1);
		}
	}
}
