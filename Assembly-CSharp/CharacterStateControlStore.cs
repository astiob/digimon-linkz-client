using System;

[Serializable]
public class CharacterStateControlStore
{
	public int hp { get; set; }

	public int ap { get; set; }

	public int isSelectSkill { get; set; }

	public int hate { get; set; }

	public int previousHate { get; set; }

	public bool isLeader { get; set; }

	public int skillOrder { get; set; }

	public int myIndex { get; set; }

	public bool isEnemy { get; set; }

	public int[] chipIds { get; set; }

	public bool isEscape { get; set; }

	public int[] skillUseCounts { get; set; }

	public HaveSufferStateStore currentSufferState { get; set; }

	public float randomedSpeed { get; set; }

	public string chipEffectCount { get; set; }

	public string potencyChipIdList { get; set; }
}
