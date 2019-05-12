using System;
using UnityEngine;

public class BattleMonsterButton : MonoBehaviour
{
	[SerializeField]
	public UIButton button;

	[SerializeField]
	[Header("アイコン画像")]
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
		this.ApplyLeaderIcon(isLeader);
	}

	public void ApplyLeaderIcon(bool isLeader)
	{
		this.leaderDisplay.SetSkins((!isLeader) ? 1 : 0);
	}
}
