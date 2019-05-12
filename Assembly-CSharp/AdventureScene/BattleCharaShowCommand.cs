using System;
using System.Linq;

namespace AdventureScene
{
	public sealed class BattleCharaShowCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_battle_chara_show";

		private string groupType = string.Empty;

		private string rangeType = string.Empty;

		private string showFlag = string.Empty;

		public BattleCharaShowCommand()
		{
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_battle_chara_show";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.groupType = commandParams[1];
				this.rangeType = commandParams[2];
				this.showFlag = commandParams[3];
				base.SetWaitScriptEngine(true);
				result = true;
			}
			catch
			{
				base.OnErrorGetParameter();
			}
			return result;
		}

		public override bool RunScriptCommand()
		{
			BattleStateManager current = BattleStateManager.current;
			bool result;
			if (current != null)
			{
				result = true;
				CharacterStateControl[] array = current.battleStateData.GetTotalCharacters();
				array = array.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
				if (!(this.groupType == "all"))
				{
					if (this.groupType == "player")
					{
						array = array.Where((CharacterStateControl item) => !item.isEnemy).ToArray<CharacterStateControl>();
					}
					else if (this.groupType == "enemy")
					{
						array = array.Where((CharacterStateControl item) => item.isEnemy).ToArray<CharacterStateControl>();
					}
				}
				if (!(this.rangeType == "all"))
				{
					if (this.rangeType == "center")
					{
						array = array.Where((CharacterStateControl item) => item.myIndex == 1).ToArray<CharacterStateControl>();
					}
					else if (this.rangeType == "left")
					{
						array = array.Where((CharacterStateControl item) => item.myIndex == ((!item.isEnemy) ? 0 : 2)).ToArray<CharacterStateControl>();
					}
					else if (this.rangeType == "right")
					{
						array = array.Where((CharacterStateControl item) => item.myIndex == ((!item.isEnemy) ? 2 : 0)).ToArray<CharacterStateControl>();
					}
				}
				bool flag = "on" == this.showFlag;
				if (flag)
				{
					current.threeDAction.ShowAllCharactersAction(array);
					current.threeDAction.PlayIdleAnimationActiveCharacterAction(array);
				}
				else
				{
					current.threeDAction.HideAllCharactersAction(array);
				}
				base.ResumeScriptEngine();
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
