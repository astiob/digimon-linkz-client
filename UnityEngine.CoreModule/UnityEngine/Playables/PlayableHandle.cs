using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[UsedByNativeCode]
	public struct PlayableHandle
	{
		internal IntPtr m_Handle;

		internal int m_Version;

		internal T GetObject<T>() where T : class, IPlayableBehaviour
		{
			T result;
			if (!this.IsValid())
			{
				result = (T)((object)null);
			}
			else
			{
				object scriptInstance = this.GetScriptInstance();
				if (scriptInstance == null)
				{
					result = (T)((object)null);
				}
				else
				{
					result = (T)((object)scriptInstance);
				}
			}
			return result;
		}

		internal bool IsPlayableOfType<T>()
		{
			return this.GetPlayableType() == typeof(T);
		}

		public static PlayableHandle Null
		{
			get
			{
				return new PlayableHandle
				{
					m_Version = 10
				};
			}
		}

		internal Playable GetInput(int inputPort)
		{
			return new Playable(this.GetInputHandle(inputPort));
		}

		internal Playable GetOutput(int outputPort)
		{
			return new Playable(this.GetOutputHandle(outputPort));
		}

		internal bool SetInputWeight(int inputIndex, float weight)
		{
			bool result;
			if (this.CheckInputBounds(inputIndex))
			{
				this.SetInputWeightFromIndex(inputIndex, weight);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal float GetInputWeight(int inputIndex)
		{
			float result;
			if (this.CheckInputBounds(inputIndex))
			{
				result = this.GetInputWeightFromIndex(inputIndex);
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		internal void Destroy()
		{
			this.GetGraph().DestroyPlayable<Playable>(new Playable(this));
		}

		public static bool operator ==(PlayableHandle x, PlayableHandle y)
		{
			return PlayableHandle.CompareVersion(x, y);
		}

		public static bool operator !=(PlayableHandle x, PlayableHandle y)
		{
			return !PlayableHandle.CompareVersion(x, y);
		}

		public override bool Equals(object p)
		{
			return p is PlayableHandle && PlayableHandle.CompareVersion(this, (PlayableHandle)p);
		}

		public override int GetHashCode()
		{
			return this.m_Handle.GetHashCode() ^ this.m_Version.GetHashCode();
		}

		internal static bool CompareVersion(PlayableHandle lhs, PlayableHandle rhs)
		{
			return lhs.m_Handle == rhs.m_Handle && lhs.m_Version == rhs.m_Version;
		}

		internal bool CheckInputBounds(int inputIndex)
		{
			return this.CheckInputBounds(inputIndex, false);
		}

		internal bool CheckInputBounds(int inputIndex, bool acceptAny)
		{
			bool result;
			if (inputIndex == -1 && acceptAny)
			{
				result = true;
			}
			else
			{
				if (inputIndex < 0)
				{
					throw new IndexOutOfRangeException("Index must be greater than 0");
				}
				if (this.GetInputCount() <= inputIndex)
				{
					throw new IndexOutOfRangeException(string.Concat(new object[]
					{
						"inputIndex ",
						inputIndex,
						" is greater than the number of available inputs (",
						this.GetInputCount(),
						")."
					}));
				}
				result = true;
			}
			return result;
		}

		internal bool IsValid()
		{
			return PlayableHandle.IsValid_Injected(ref this);
		}

		internal Type GetPlayableType()
		{
			return PlayableHandle.GetPlayableType_Injected(ref this);
		}

		internal void SetScriptInstance(object scriptInstance)
		{
			PlayableHandle.SetScriptInstance_Injected(ref this, scriptInstance);
		}

		internal bool CanChangeInputs()
		{
			return PlayableHandle.CanChangeInputs_Injected(ref this);
		}

		internal bool CanSetWeights()
		{
			return PlayableHandle.CanSetWeights_Injected(ref this);
		}

		internal bool CanDestroy()
		{
			return PlayableHandle.CanDestroy_Injected(ref this);
		}

		internal PlayState GetPlayState()
		{
			return PlayableHandle.GetPlayState_Injected(ref this);
		}

		internal void Play()
		{
			PlayableHandle.Play_Injected(ref this);
		}

		internal void Pause()
		{
			PlayableHandle.Pause_Injected(ref this);
		}

		internal double GetSpeed()
		{
			return PlayableHandle.GetSpeed_Injected(ref this);
		}

		internal void SetSpeed(double value)
		{
			PlayableHandle.SetSpeed_Injected(ref this, value);
		}

		internal double GetTime()
		{
			return PlayableHandle.GetTime_Injected(ref this);
		}

		internal void SetTime(double value)
		{
			PlayableHandle.SetTime_Injected(ref this, value);
		}

		internal bool IsDone()
		{
			return PlayableHandle.IsDone_Injected(ref this);
		}

		internal void SetDone(bool value)
		{
			PlayableHandle.SetDone_Injected(ref this, value);
		}

		internal double GetDuration()
		{
			return PlayableHandle.GetDuration_Injected(ref this);
		}

		internal void SetDuration(double value)
		{
			PlayableHandle.SetDuration_Injected(ref this, value);
		}

		internal bool GetPropagateSetTime()
		{
			return PlayableHandle.GetPropagateSetTime_Injected(ref this);
		}

		internal void SetPropagateSetTime(bool value)
		{
			PlayableHandle.SetPropagateSetTime_Injected(ref this, value);
		}

		internal PlayableGraph GetGraph()
		{
			PlayableGraph result;
			PlayableHandle.GetGraph_Injected(ref this, out result);
			return result;
		}

		internal int GetInputCount()
		{
			return PlayableHandle.GetInputCount_Injected(ref this);
		}

		internal void SetInputCount(int value)
		{
			PlayableHandle.SetInputCount_Injected(ref this, value);
		}

		internal int GetOutputCount()
		{
			return PlayableHandle.GetOutputCount_Injected(ref this);
		}

		internal void SetOutputCount(int value)
		{
			PlayableHandle.SetOutputCount_Injected(ref this, value);
		}

		internal void SetInputWeight(PlayableHandle input, float weight)
		{
			PlayableHandle.SetInputWeight_Injected(ref this, ref input, weight);
		}

		internal void SetDelay(double delay)
		{
			PlayableHandle.SetDelay_Injected(ref this, delay);
		}

		internal double GetDelay()
		{
			return PlayableHandle.GetDelay_Injected(ref this);
		}

		internal bool IsDelayed()
		{
			return PlayableHandle.IsDelayed_Injected(ref this);
		}

		internal double GetPreviousTime()
		{
			return PlayableHandle.GetPreviousTime_Injected(ref this);
		}

		private object GetScriptInstance()
		{
			return PlayableHandle.GetScriptInstance_Injected(ref this);
		}

		private PlayableHandle GetInputHandle(int index)
		{
			PlayableHandle result;
			PlayableHandle.GetInputHandle_Injected(ref this, index, out result);
			return result;
		}

		private PlayableHandle GetOutputHandle(int index)
		{
			PlayableHandle result;
			PlayableHandle.GetOutputHandle_Injected(ref this, index, out result);
			return result;
		}

		private void SetInputWeightFromIndex(int index, float weight)
		{
			PlayableHandle.SetInputWeightFromIndex_Injected(ref this, index, weight);
		}

		private float GetInputWeightFromIndex(int index)
		{
			return PlayableHandle.GetInputWeightFromIndex_Injected(ref this, index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValid_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type GetPlayableType_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetScriptInstance_Injected(ref PlayableHandle _unity_self, object scriptInstance);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanChangeInputs_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanSetWeights_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanDestroy_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PlayState GetPlayState_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Play_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Pause_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetSpeed_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSpeed_Injected(ref PlayableHandle _unity_self, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetTime_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTime_Injected(ref PlayableHandle _unity_self, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDone_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDone_Injected(ref PlayableHandle _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetDuration_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDuration_Injected(ref PlayableHandle _unity_self, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetPropagateSetTime_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPropagateSetTime_Injected(ref PlayableHandle _unity_self, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGraph_Injected(ref PlayableHandle _unity_self, out PlayableGraph ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetInputCount_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetInputCount_Injected(ref PlayableHandle _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetOutputCount_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetOutputCount_Injected(ref PlayableHandle _unity_self, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetInputWeight_Injected(ref PlayableHandle _unity_self, ref PlayableHandle input, float weight);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDelay_Injected(ref PlayableHandle _unity_self, double delay);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetDelay_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDelayed_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetPreviousTime_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object GetScriptInstance_Injected(ref PlayableHandle _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetInputHandle_Injected(ref PlayableHandle _unity_self, int index, out PlayableHandle ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetOutputHandle_Injected(ref PlayableHandle _unity_self, int index, out PlayableHandle ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetInputWeightFromIndex_Injected(ref PlayableHandle _unity_self, int index, float weight);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetInputWeightFromIndex_Injected(ref PlayableHandle _unity_self, int index);
	}
}
