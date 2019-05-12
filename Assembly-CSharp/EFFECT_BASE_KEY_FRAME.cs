using System;

public class EFFECT_BASE_KEY_FRAME
{
	public EFCB_TYPE move_type_x;

	public EFCB_TYPE move_type_y;

	public EFCB_TYPE move_type_scl_x;

	public EFCB_TYPE move_type_scl_y;

	public EFCB_TYPE move_type_rot_z;

	public EFCB_TYPE move_type_rot_x;

	public EFCB_TYPE move_type_rot_y;

	public EFCB_TYPE move_type_col;

	public float ctr_x;

	public float ctr_y;

	public float scl_x;

	public float scl_y;

	public float rot_z;

	public float rot_x;

	public float rot_y;

	public float col_r;

	public float col_g;

	public float col_b;

	public float col_a;

	public float time;

	public float delay;

	public EFFECT_BASE_KEY_FRAME Clone()
	{
		return (EFFECT_BASE_KEY_FRAME)base.MemberwiseClone();
	}
}
