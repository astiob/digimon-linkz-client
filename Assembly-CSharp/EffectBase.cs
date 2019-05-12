using System;
using UnityEngine;

public class EffectBase
{
	private float EFFECT_BASE_FRAME_PER_SEC = 30f;

	private int EFFECT_BASE_KEY_FRAME_MAX = 64;

	private int keyFrameNum_;

	private EFFECT_BASE_KEY_FRAME[] keyFrame_;

	private bool start_;

	private bool loopEna_;

	private int loopCt_;

	private int curLoopCt_;

	private int startKeyFrameIdx_;

	private int curKeyFrameIdx_;

	private int curKey_FrameCt;

	private int curKey_FrameMax;

	private int curKey_DelayCt;

	private int curKey_DelayMax;

	private EFFECT_BASE_KEY_FRAME curFrameValue_;

	public EffectBase()
	{
		this.keyFrameNum_ = 0;
		this.keyFrame_ = new EFFECT_BASE_KEY_FRAME[this.EFFECT_BASE_KEY_FRAME_MAX];
		this.start_ = false;
		this.loopEna_ = true;
		this.curLoopCt_ = 0;
		this.startKeyFrameIdx_ = 0;
	}

	~EffectBase()
	{
	}

	public void efInit()
	{
		this.curKeyFrameIdx_ = this.startKeyFrameIdx_;
		this.curKey_FrameCt = 0;
		this.curKey_DelayCt = 0;
		this.curKey_FrameMax = (int)this.getCurKey_FrameMax();
		this.curKey_DelayMax = (int)this.getCurKey_DelayMax();
		this.curFrameValue_ = this.keyFrame_[this.curKeyFrameIdx_].Clone();
	}

	public void efChangeStartIdx(int idx)
	{
		this.startKeyFrameIdx_ = idx;
	}

	public void efAddKeyFrame(EFFECT_BASE_KEY_FRAME KF)
	{
		this.keyFrame_[this.keyFrameNum_] = KF.Clone();
		this.keyFrameNum_++;
	}

	public void efAddKeyFrameTbl(EFFECT_BASE_KEY_FRAME[] KFS)
	{
		for (int i = 0; i < KFS.Length; i++)
		{
			this.keyFrame_[this.keyFrameNum_] = KFS[i].Clone();
			this.keyFrameNum_++;
		}
	}

	public void efSetKeyFrameTbl(EFFECT_BASE_KEY_FRAME[] KFS)
	{
		this.keyFrameNum_ = 0;
		for (int i = 0; i < KFS.Length; i++)
		{
			this.keyFrame_[this.keyFrameNum_] = KFS[i].Clone();
			this.keyFrameNum_++;
		}
	}

	public void efSetLoopCt(int ct)
	{
		this.loopCt_ = ct;
	}

	public void efStart()
	{
		this.start_ = true;
		this.loopEna_ = true;
		this.curLoopCt_ = 0;
	}

	public void efStop()
	{
		this.start_ = false;
	}

	public void efStopLoop()
	{
		this.loopEna_ = false;
	}

	public bool efIsLoopEna()
	{
		return this.loopEna_;
	}

	public bool efIsEnd()
	{
		return !this.start_;
	}

	public void efUpdate()
	{
		this.updateFrame();
		this.updateValue();
	}

	public void efTransform(Transform tm)
	{
		Vector3 vector = tm.localPosition;
		if (this.curFrameValue_.move_type_x != EFCB_TYPE.EFCB_MOVE_OFF)
		{
			vector.x = this.curFrameValue_.ctr_x;
		}
		if (this.curFrameValue_.move_type_y != EFCB_TYPE.EFCB_MOVE_OFF)
		{
			vector.y = this.curFrameValue_.ctr_y;
		}
		tm.localPosition = vector;
		vector = tm.localScale;
		if (this.curFrameValue_.move_type_scl_x != EFCB_TYPE.EFCB_MOVE_OFF)
		{
			vector.x = this.curFrameValue_.scl_x;
		}
		if (this.curFrameValue_.move_type_scl_y != EFCB_TYPE.EFCB_MOVE_OFF)
		{
			vector.y = this.curFrameValue_.scl_y;
		}
		tm.localScale = vector;
		Quaternion localRotation = Quaternion.Euler(this.curFrameValue_.rot_x, this.curFrameValue_.rot_y, this.curFrameValue_.rot_z);
		tm.localRotation = localRotation;
	}

	public int efGetCurLoopCt()
	{
		return this.curLoopCt_;
	}

	public int efGetCurKeyFrameIdx()
	{
		return this.curKeyFrameIdx_;
	}

	public EFFECT_BASE_KEY_FRAME getCurFrameValue()
	{
		return this.curFrameValue_;
	}

	public float EFC_NORMALIZE_TM(float ft, float fstt, float fend)
	{
		if (ft < fstt)
		{
			return 0f;
		}
		if (ft > fend)
		{
			return 1f;
		}
		return (ft - fstt) / (fend - fstt);
	}

	private void updateFrame()
	{
		if (this.start_)
		{
			if (this.curKey_FrameCt == this.curKey_FrameMax)
			{
				this.curKeyFrameIdx_++;
				if (this.curKeyFrameIdx_ == this.keyFrameNum_ - 1)
				{
					if (this.loopEna_)
					{
						this.curLoopCt_++;
						if (this.loopCt_ == 0)
						{
							this.efInit();
						}
						else if (this.curLoopCt_ == this.loopCt_)
						{
							this.curLoopCt_ = 0;
							this.efInit();
							this.efStop();
						}
						else
						{
							this.efInit();
						}
					}
					else
					{
						this.efInit();
						this.efStop();
					}
				}
				else
				{
					this.curKey_FrameCt = 0;
					this.curKey_FrameMax = (int)this.getCurKey_FrameMax();
					this.curKey_DelayCt = 0;
					this.curKey_DelayMax = (int)this.getCurKey_DelayMax();
					this.curFrameValue_ = this.keyFrame_[this.curKeyFrameIdx_].Clone();
				}
			}
			else if (this.curLoopCt_ > 0 || this.curKey_DelayCt >= this.curKey_DelayMax)
			{
				this.curKey_FrameCt++;
			}
			else
			{
				this.curKey_DelayCt++;
			}
		}
	}

	private void updateValue()
	{
		EFFECT_BASE_KEY_FRAME effect_BASE_KEY_FRAME = this.keyFrame_[this.curKeyFrameIdx_];
		EFFECT_BASE_KEY_FRAME effect_BASE_KEY_FRAME2 = this.keyFrame_[this.curKeyFrameIdx_ + 1];
		EFFECT_BASE_KEY_FRAME effect_BASE_KEY_FRAME3 = this.curFrameValue_;
		float curKey_NormalizedTime = this.getCurKey_NormalizedTime();
		float num = 0f;
		switch (effect_BASE_KEY_FRAME.move_type_x)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.ctr_x - effect_BASE_KEY_FRAME.ctr_x);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.ctr_x - effect_BASE_KEY_FRAME.ctr_x);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.ctr_x - effect_BASE_KEY_FRAME.ctr_x);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.ctr_x - effect_BASE_KEY_FRAME.ctr_x);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.ctr_x - effect_BASE_KEY_FRAME.ctr_x);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.ctr_x - effect_BASE_KEY_FRAME.ctr_x);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.ctr_x - effect_BASE_KEY_FRAME.ctr_x);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.ctr_x - effect_BASE_KEY_FRAME.ctr_x);
			break;
		}
		effect_BASE_KEY_FRAME3.ctr_x = effect_BASE_KEY_FRAME.ctr_x + num;
		switch (effect_BASE_KEY_FRAME.move_type_y)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.ctr_y - effect_BASE_KEY_FRAME.ctr_y);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.ctr_y - effect_BASE_KEY_FRAME.ctr_y);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.ctr_y - effect_BASE_KEY_FRAME.ctr_y);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.ctr_y - effect_BASE_KEY_FRAME.ctr_y);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.ctr_y - effect_BASE_KEY_FRAME.ctr_y);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.ctr_y - effect_BASE_KEY_FRAME.ctr_y);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.ctr_y - effect_BASE_KEY_FRAME.ctr_y);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.ctr_y - effect_BASE_KEY_FRAME.ctr_y);
			break;
		}
		effect_BASE_KEY_FRAME3.ctr_y = effect_BASE_KEY_FRAME.ctr_y + num;
		switch (effect_BASE_KEY_FRAME.move_type_scl_x)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.scl_x - effect_BASE_KEY_FRAME.scl_x);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.scl_x - effect_BASE_KEY_FRAME.scl_x);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.scl_x - effect_BASE_KEY_FRAME.scl_x);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.scl_x - effect_BASE_KEY_FRAME.scl_x);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.scl_x - effect_BASE_KEY_FRAME.scl_x);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.scl_x - effect_BASE_KEY_FRAME.scl_x);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.scl_x - effect_BASE_KEY_FRAME.scl_x);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.scl_x - effect_BASE_KEY_FRAME.scl_x);
			break;
		}
		effect_BASE_KEY_FRAME3.scl_x = effect_BASE_KEY_FRAME.scl_x + num;
		switch (effect_BASE_KEY_FRAME.move_type_scl_y)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.scl_y - effect_BASE_KEY_FRAME.scl_y);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.scl_y - effect_BASE_KEY_FRAME.scl_y);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.scl_y - effect_BASE_KEY_FRAME.scl_y);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.scl_y - effect_BASE_KEY_FRAME.scl_y);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.scl_y - effect_BASE_KEY_FRAME.scl_y);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.scl_y - effect_BASE_KEY_FRAME.scl_y);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.scl_y - effect_BASE_KEY_FRAME.scl_y);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.scl_y - effect_BASE_KEY_FRAME.scl_y);
			break;
		}
		effect_BASE_KEY_FRAME3.scl_y = effect_BASE_KEY_FRAME.scl_y + num;
		switch (effect_BASE_KEY_FRAME.move_type_rot_z)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_z - effect_BASE_KEY_FRAME.rot_z);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.rot_z - effect_BASE_KEY_FRAME.rot_z);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.rot_z - effect_BASE_KEY_FRAME.rot_z);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_z - effect_BASE_KEY_FRAME.rot_z);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_z - effect_BASE_KEY_FRAME.rot_z);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.rot_z - effect_BASE_KEY_FRAME.rot_z);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.rot_z - effect_BASE_KEY_FRAME.rot_z);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.rot_z - effect_BASE_KEY_FRAME.rot_z);
			break;
		}
		effect_BASE_KEY_FRAME3.rot_z = effect_BASE_KEY_FRAME.rot_z + num;
		switch (effect_BASE_KEY_FRAME.move_type_rot_x)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_x - effect_BASE_KEY_FRAME.rot_x);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.rot_x - effect_BASE_KEY_FRAME.rot_x);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.rot_x - effect_BASE_KEY_FRAME.rot_x);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_x - effect_BASE_KEY_FRAME.rot_x);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_x - effect_BASE_KEY_FRAME.rot_x);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.rot_x - effect_BASE_KEY_FRAME.rot_x);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.rot_x - effect_BASE_KEY_FRAME.rot_x);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.rot_x - effect_BASE_KEY_FRAME.rot_x);
			break;
		}
		effect_BASE_KEY_FRAME3.rot_x = effect_BASE_KEY_FRAME.rot_x + num;
		switch (effect_BASE_KEY_FRAME.move_type_rot_y)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_y - effect_BASE_KEY_FRAME.rot_y);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.rot_y - effect_BASE_KEY_FRAME.rot_y);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.rot_y - effect_BASE_KEY_FRAME.rot_y);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_y - effect_BASE_KEY_FRAME.rot_y);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.rot_y - effect_BASE_KEY_FRAME.rot_y);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.rot_y - effect_BASE_KEY_FRAME.rot_y);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.rot_y - effect_BASE_KEY_FRAME.rot_y);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.rot_y - effect_BASE_KEY_FRAME.rot_y);
			break;
		}
		effect_BASE_KEY_FRAME3.rot_y = effect_BASE_KEY_FRAME.rot_y + num;
		switch (effect_BASE_KEY_FRAME.move_type_col)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_r - effect_BASE_KEY_FRAME.col_r);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.col_r - effect_BASE_KEY_FRAME.col_r);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.col_r - effect_BASE_KEY_FRAME.col_r);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_r - effect_BASE_KEY_FRAME.col_r);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_r - effect_BASE_KEY_FRAME.col_r);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.col_r - effect_BASE_KEY_FRAME.col_r);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.col_r - effect_BASE_KEY_FRAME.col_r);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.col_r - effect_BASE_KEY_FRAME.col_r);
			break;
		}
		effect_BASE_KEY_FRAME3.col_r = effect_BASE_KEY_FRAME.col_r + num;
		switch (effect_BASE_KEY_FRAME.move_type_col)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_g - effect_BASE_KEY_FRAME.col_g);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.col_g - effect_BASE_KEY_FRAME.col_g);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.col_g - effect_BASE_KEY_FRAME.col_g);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_g - effect_BASE_KEY_FRAME.col_g);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_g - effect_BASE_KEY_FRAME.col_g);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.col_g - effect_BASE_KEY_FRAME.col_g);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.col_g - effect_BASE_KEY_FRAME.col_g);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.col_g - effect_BASE_KEY_FRAME.col_g);
			break;
		}
		effect_BASE_KEY_FRAME3.col_g = effect_BASE_KEY_FRAME.col_g + num;
		switch (effect_BASE_KEY_FRAME.move_type_col)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_b - effect_BASE_KEY_FRAME.col_b);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.col_b - effect_BASE_KEY_FRAME.col_b);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.col_b - effect_BASE_KEY_FRAME.col_b);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_b - effect_BASE_KEY_FRAME.col_b);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_b - effect_BASE_KEY_FRAME.col_b);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.col_b - effect_BASE_KEY_FRAME.col_b);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.col_b - effect_BASE_KEY_FRAME.col_b);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.col_b - effect_BASE_KEY_FRAME.col_b);
			break;
		}
		effect_BASE_KEY_FRAME3.col_b = effect_BASE_KEY_FRAME.col_b + num;
		switch (effect_BASE_KEY_FRAME.move_type_col)
		{
		case EFCB_TYPE.EFCB_LINEAR:
			num = this.EFC_LNR_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_a - effect_BASE_KEY_FRAME.col_a);
			break;
		case EFCB_TYPE.EFCB_FALL_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.col_a - effect_BASE_KEY_FRAME.col_a);
			break;
		case EFCB_TYPE.EFCB_FALL_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 2) * (effect_BASE_KEY_FRAME2.col_a - effect_BASE_KEY_FRAME.col_a);
			break;
		case EFCB_TYPE.EFCB_SIN_01:
			num = this.EFC_SIN_01(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_a - effect_BASE_KEY_FRAME.col_a);
			break;
		case EFCB_TYPE.EFCB_SIN_10:
			num = this.EFC_SIN_10(curKey_NormalizedTime) * (effect_BASE_KEY_FRAME2.col_a - effect_BASE_KEY_FRAME.col_a);
			break;
		case EFCB_TYPE.EFCB_SPRING_01:
			num = this.EFC_FALL_01(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.col_a - effect_BASE_KEY_FRAME.col_a);
			break;
		case EFCB_TYPE.EFCB_SPRING_10:
			num = this.EFC_FALL_10(curKey_NormalizedTime, 3) * (effect_BASE_KEY_FRAME2.col_a - effect_BASE_KEY_FRAME.col_a);
			break;
		case EFCB_TYPE.EFCB_BOMB_01:
			num = this.EFC_BOMB_01(curKey_NormalizedTime, 0.2f) * (effect_BASE_KEY_FRAME2.col_a - effect_BASE_KEY_FRAME.col_a);
			break;
		}
		effect_BASE_KEY_FRAME3.col_a = effect_BASE_KEY_FRAME.col_a + num;
	}

	private float getCurKey_NormalizedTime()
	{
		return (float)this.curKey_FrameCt / (float)this.curKey_FrameMax;
	}

	private float getCurKey_FrameMax()
	{
		float num = (this.keyFrame_[this.curKeyFrameIdx_ + 1].time - this.keyFrame_[this.curKeyFrameIdx_].time) * this.EFFECT_BASE_FRAME_PER_SEC;
		if (num < 1f)
		{
			num = 1f;
		}
		return num;
	}

	private float getCurKey_DelayMax()
	{
		return this.keyFrame_[this.curKeyFrameIdx_].delay * this.EFFECT_BASE_FRAME_PER_SEC;
	}

	private float EFC_SIN_01(float ft)
	{
		return Mathf.Sin(1.57079637f * ft);
	}

	private float EFC_SIN_10(float ft)
	{
		return 1f - Mathf.Sin(1.57079637f * (ft + 1f));
	}

	private float EFC_LNR_01(float ft)
	{
		return ft;
	}

	private float EFC_FALL_01(float ft, int fN = 2)
	{
		float num = ft;
		for (int i = 0; i < fN - 1; i++)
		{
			num *= ft;
		}
		return num;
	}

	private float EFC_FALL_10(float ft, int fN = 2)
	{
		float num = this.EFC_FALL_01(1f - ft, fN);
		return -num + 1f;
	}

	private float EFC_BOMB_01(float ft, float fr = 0.2f)
	{
		return 1f;
	}

	private float EFC_MODIFY_0to1(float fv, float fs, float fl)
	{
		float num = fv * (1f - (fs + (1f - fl)));
		return num + fs;
	}
}
