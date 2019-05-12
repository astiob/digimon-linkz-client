using System;
using System.Collections.Generic;

public class PvPBattleEnd : TCPData<PvPBattleEnd>
{
	public int battle_result;

	public int wave_count;

	public List<int> use_inheritance_skill;
}
