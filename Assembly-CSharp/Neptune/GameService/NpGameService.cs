using System;

namespace Neptune.GameService
{
	public class NpGameService : NpSingleton<NpGameService>
	{
		private int mResolveRetryMax = 5;

		private bool mEnableLog;

		private Action mSignInSucceededCb;

		private Action<string> mSignInFailedCb;

		private Action mSaveSuccessCb;

		private Action<string> mSaveFailedCb;

		private Action<string> mLoadSuccessCb;

		private Action<string> mLoadFailedCb;

		private Action<string> mConflictCb;

		private Action mUnlockSuccessCb;

		private Action<string> mUnlockFailedCb;

		private Action mIncrementSuccessCb;

		private Action<string> mIncrementFailedCb;

		public int ResolveRetryMax
		{
			get
			{
				return NpGameServiceAndroid.GetResolveRetryMax();
			}
			set
			{
				NpGameServiceAndroid.SetResolveRetryMax(value);
				this.mResolveRetryMax = value;
			}
		}

		public void EnableDebugLog(bool isDebug)
		{
			this.mEnableLog = isDebug;
			NpGameServiceAndroid.EnableDebugLog(isDebug);
		}

		public void SignedIn(NpGameServiceAndroid.CLIENT_TYPE clientsToUse, Action successCb, Action<string> failedCb)
		{
			this.mSignInSucceededCb = successCb;
			this.mSignInFailedCb = failedCb;
			NpGameServiceAndroid.SignedIn(clientsToUse);
		}

		public bool IsSignedIn()
		{
			return NpGameServiceAndroid.IsSignedIn();
		}

		public void SignOut()
		{
			NpGameServiceAndroid.SignOut();
		}

		public void DataSave(string key, string value, Action successCb, Action<string> failedCb)
		{
			this.mSaveSuccessCb = successCb;
			this.mSaveFailedCb = failedCb;
			NpGameServiceAndroid.DataSave(key, value);
		}

		public void DataLoad(string key, Action<string> successCb, Action<string> failedCb, Action<string> conflictCb)
		{
			this.mLoadSuccessCb = successCb;
			this.mLoadFailedCb = failedCb;
			this.mConflictCb = conflictCb;
			NpGameServiceAndroid.DataLoad(key);
		}

		public void Synchronism()
		{
		}

		public void UnlockAchievements(string achievementId, Action successCb, Action<string> failedCb)
		{
			this.mUnlockSuccessCb = successCb;
			this.mUnlockFailedCb = failedCb;
			NpGameServiceAndroid.UnlockAchievements(achievementId);
		}

		public void IncrementAchievements(string achievementId, int value, Action successCb, Action<string> failedCb)
		{
			this.mIncrementSuccessCb = successCb;
			this.mIncrementFailedCb = failedCb;
			NpGameServiceAndroid.IncrementAchievements(achievementId, value);
		}

		public void ShowAchievements()
		{
			NpGameServiceAndroid.ShowAchievements();
		}

		private void onSignInSucceeded()
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onSignInSucceeded");
			if (this.mSignInSucceededCb != null)
			{
				this.mSignInSucceededCb();
			}
		}

		private void onSignInFailed(string errCode)
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onSignInFailed : errCode = " + errCode);
			if (this.mSignInFailedCb != null)
			{
				this.mSignInFailedCb(errCode);
			}
		}

		private void onSaveSucceeded()
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onSaveSucceeded");
			if (this.mSaveSuccessCb != null)
			{
				this.mSaveSuccessCb();
			}
		}

		private void onSaveFailed(string errCode)
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onSaveFailed:errCode = " + errCode);
			if (this.mSaveFailedCb != null)
			{
				this.mSaveFailedCb(errCode);
			}
		}

		private void onLoadSucceeded(string loadData)
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onLoadSucceeded : loadData = " + loadData);
			if (this.mLoadSuccessCb != null)
			{
				this.mLoadSuccessCb(loadData);
			}
		}

		private void onLoadFailed(string errCode)
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onLoadFailed:errCode = " + errCode);
			if (this.mLoadFailedCb != null)
			{
				this.mLoadFailedCb(errCode);
			}
		}

		private void onConflictCb(string keys)
		{
			NpLogUtil.DebugLog(this.mEnableLog, "OnConflictCb");
			if (this.mConflictCb != null)
			{
				this.mConflictCb(keys);
			}
		}

		private void onUnlockSucceeded()
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onUnlockSucceeded");
			if (this.mUnlockSuccessCb != null)
			{
				this.mUnlockSuccessCb();
			}
		}

		private void onUnlockFailed(string errCode)
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onUnlockFailed : errCode = " + errCode);
			if (this.mUnlockFailedCb != null)
			{
				this.mUnlockFailedCb(errCode);
			}
		}

		private void onIncrementSucceeded()
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onIncrementSucceeded");
			if (this.mIncrementSuccessCb != null)
			{
				this.mIncrementSuccessCb();
			}
		}

		private void onIncrementFailed(string errCode)
		{
			NpLogUtil.DebugLog(this.mEnableLog, "onIncrementFailed : errCode = " + errCode);
			if (this.mIncrementFailedCb != null)
			{
				this.mIncrementFailedCb(errCode);
			}
		}
	}
}
