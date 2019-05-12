using Colosseum.DeckUI;
using System;

namespace PartyEdit
{
	public sealed class PartyEditColosseumButton : GUICollider
	{
		private void OnPushedButton()
		{
			CMD_ColosseumDeck.Create(CMD_ColosseumDeck.Mode.FROM_PARTY_EDIT);
		}
	}
}
