using CharacterMiniStatusUI;
using System;
using UI.MonsterInfoParts;

namespace Colosseum.DeckUI
{
	public sealed class ColosseumDeckData
	{
		public CMD_ColosseumDeck RootDialog { get; set; }

		public MonsterBasicInfo MonsterBasicInfo { get; set; }

		public ChipBaseSelect MonsterChipSlotInfo { get; set; }

		public MonsterSelectedIcon MonsterSelectedIcon { get; set; }

		public UI_MonsterMiniStatus MiniStatus { get; set; }

		public SortieLimitList SortieLimitList { get; set; }

		public UI_ColosseumDeckButton DeckButton { get; set; }

		public UI_ColosseumDeckList DeckList { get; set; }

		public CMD_ColosseumDeck.Mode Mode { get; set; }
	}
}
