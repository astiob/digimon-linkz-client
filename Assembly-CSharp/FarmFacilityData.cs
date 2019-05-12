using System;
using UnityEngine;

public class FarmFacilityData : ScriptableObject
{
	public FarmFacilityData.FacilityID[] initBuild;

	public enum FacilityID
	{
		MEAT_FARM = 1,
		STOREHOUSE,
		RESTAURANT,
		TRAINING_HOUSE,
		LABORATORY,
		DIGI_GARDEN,
		DIGI_HOUSE,
		DIGIMON_GYM,
		REFRIGERATOR,
		WOOD,
		FENCE,
		STONE_PAVEMENT,
		RAILWAY_LINE_H,
		RAILWAY_LINE_V,
		RAILROAD_CROSSING,
		TRAIN,
		VENDING_MACHINE,
		TELEPHONE_BOX,
		STONE,
		GRASS,
		RECRUITMENT_BOARD,
		EXCHANGE,
		EVOLUTION,
		SIGN_BOARD,
		CHIP_FACTORY
	}
}
