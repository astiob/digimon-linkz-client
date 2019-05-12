using System;
using UnityEngine;

public class EffectKeyFrame
{
	public static EFFECT_BASE_KEY_FRAME[] GetTagSlideKeys(float stx, float edx, float _delay = 0f)
	{
		return new EFFECT_BASE_KEY_FRAME[]
		{
			new EFFECT_BASE_KEY_FRAME
			{
				move_type_x = EFCB_TYPE.EFCB_SIN_01,
				move_type_y = EFCB_TYPE.EFCB_MOVE_OFF,
				move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_col = EFCB_TYPE.EFCB_LINEAR,
				ctr_x = stx,
				ctr_y = 0f,
				scl_x = 1f,
				scl_y = 1f,
				rot_z = 0f,
				rot_x = 0f,
				rot_y = 0f,
				col_r = 1f,
				col_g = 1f,
				col_b = 1f,
				col_a = 0f,
				time = 0f,
				delay = _delay
			},
			new EFFECT_BASE_KEY_FRAME
			{
				move_type_x = EFCB_TYPE.EFCB_SIN_01,
				move_type_y = EFCB_TYPE.EFCB_MOVE_OFF,
				move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_col = EFCB_TYPE.EFCB_LINEAR,
				ctr_x = edx,
				ctr_y = 0f,
				scl_x = 1f,
				scl_y = 1f,
				rot_z = 0f,
				rot_x = 0f,
				rot_y = 0f,
				col_r = 1f,
				col_g = 1f,
				col_b = 1f,
				col_a = 1f,
				time = 0.8f,
				delay = 0f
			}
		};
	}

	public static EFFECT_BASE_KEY_FRAME[] GetTagSlideKeys2(Vector3 start, Vector3 end)
	{
		return new EFFECT_BASE_KEY_FRAME[]
		{
			new EFFECT_BASE_KEY_FRAME
			{
				move_type_x = EFCB_TYPE.EFCB_SIN_01,
				move_type_y = EFCB_TYPE.EFCB_SIN_01,
				move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_col = EFCB_TYPE.EFCB_LINEAR,
				ctr_x = start.x,
				ctr_y = start.y,
				scl_x = 1f,
				scl_y = 1f,
				rot_z = 0f,
				rot_x = 0f,
				rot_y = 0f,
				col_r = 1f,
				col_g = 1f,
				col_b = 1f,
				col_a = 1f,
				time = 0f,
				delay = 0f
			},
			new EFFECT_BASE_KEY_FRAME
			{
				move_type_x = EFCB_TYPE.EFCB_SIN_01,
				move_type_y = EFCB_TYPE.EFCB_SIN_01,
				move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_col = EFCB_TYPE.EFCB_LINEAR,
				ctr_x = end.x,
				ctr_y = end.y,
				scl_x = 1f,
				scl_y = 1f,
				rot_z = 0f,
				rot_x = 0f,
				rot_y = 0f,
				col_r = 1f,
				col_g = 1f,
				col_b = 1f,
				col_a = 1f,
				time = 0.6f,
				delay = 0f
			}
		};
	}

	public static EFFECT_BASE_KEY_FRAME[] GetStar00Keys(float ctx, float cty)
	{
		EFFECT_BASE_KEY_FRAME[] array = new EFFECT_BASE_KEY_FRAME[2];
		float num = 300f * UnityEngine.Random.value + 10f;
		float x = num / 3f;
		Vector3 point = new Vector3(num, 0f, 0f);
		Vector3 point2 = new Vector3(x, 0f, 0f);
		Quaternion rotation = Quaternion.Euler(0f, 0f, 360f * UnityEngine.Random.value);
		point = rotation * point;
		point2 = rotation * point2;
		array[0] = new EFFECT_BASE_KEY_FRAME
		{
			move_type_x = EFCB_TYPE.EFCB_LINEAR,
			move_type_y = EFCB_TYPE.EFCB_LINEAR,
			move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
			move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
			move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
			move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
			move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
			move_type_col = EFCB_TYPE.EFCB_LINEAR,
			ctr_x = point2.x,
			ctr_y = point2.y,
			scl_x = 1f,
			scl_y = 1f,
			rot_z = 0f,
			rot_x = 0f,
			rot_y = 0f,
			col_r = 1f,
			col_g = 1f,
			col_b = 1f,
			col_a = 1f,
			time = 0f,
			delay = 0f
		};
		array[1] = new EFFECT_BASE_KEY_FRAME
		{
			move_type_x = EFCB_TYPE.EFCB_LINEAR,
			move_type_y = EFCB_TYPE.EFCB_LINEAR,
			move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
			move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
			move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
			move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
			move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
			move_type_col = EFCB_TYPE.EFCB_LINEAR,
			ctr_x = point.x,
			ctr_y = point.y,
			scl_x = 1f,
			scl_y = 1f,
			rot_z = 0f,
			rot_x = 0f,
			rot_y = 0f,
			col_r = 1f,
			col_g = 1f,
			col_b = 1f,
			col_a = 0f,
			time = 2.4f,
			delay = 0f
		};
		return array;
	}

	public static EFFECT_BASE_KEY_FRAME[] GetFadeKeys(float foutTime_, float waitTime_, float finTime_, float alphaMax_)
	{
		return new EFFECT_BASE_KEY_FRAME[]
		{
			new EFFECT_BASE_KEY_FRAME
			{
				move_type_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_col = EFCB_TYPE.EFCB_LINEAR,
				ctr_x = 0f,
				ctr_y = 0f,
				scl_x = 1f,
				scl_y = 1f,
				rot_z = 0f,
				rot_x = 0f,
				rot_y = 0f,
				col_r = 1f,
				col_g = 1f,
				col_b = 1f,
				col_a = 0f,
				time = 0f,
				delay = 0f
			},
			new EFFECT_BASE_KEY_FRAME
			{
				move_type_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_col = EFCB_TYPE.EFCB_LINEAR,
				ctr_x = 0f,
				ctr_y = 0f,
				scl_x = 1f,
				scl_y = 1f,
				rot_z = 0f,
				rot_x = 0f,
				rot_y = 0f,
				col_r = 1f,
				col_g = 1f,
				col_b = 1f,
				col_a = alphaMax_,
				time = foutTime_,
				delay = 0f
			},
			new EFFECT_BASE_KEY_FRAME
			{
				move_type_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_col = EFCB_TYPE.EFCB_LINEAR,
				ctr_x = 0f,
				ctr_y = 0f,
				scl_x = 1f,
				scl_y = 1f,
				rot_z = 0f,
				rot_x = 0f,
				rot_y = 0f,
				col_r = 1f,
				col_g = 1f,
				col_b = 1f,
				col_a = alphaMax_,
				time = foutTime_ + waitTime_,
				delay = 0f
			},
			new EFFECT_BASE_KEY_FRAME
			{
				move_type_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_scl_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_z = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_x = EFCB_TYPE.EFCB_LINEAR,
				move_type_rot_y = EFCB_TYPE.EFCB_LINEAR,
				move_type_col = EFCB_TYPE.EFCB_LINEAR,
				ctr_x = 0f,
				ctr_y = 0f,
				scl_x = 1f,
				scl_y = 1f,
				rot_z = 0f,
				rot_x = 0f,
				rot_y = 0f,
				col_r = 1f,
				col_g = 1f,
				col_b = 1f,
				col_a = 0f,
				time = foutTime_ + waitTime_ + finTime_,
				delay = 0f
			}
		};
	}
}
