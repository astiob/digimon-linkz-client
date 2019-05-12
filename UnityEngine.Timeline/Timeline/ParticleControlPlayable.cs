using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	public class ParticleControlPlayable : PlayableBehaviour
	{
		private const float kUnsetTime = -1f;

		private float m_LastTime = -1f;

		private float m_LastPsTime = -1f;

		private uint m_RandomSeed = 1u;

		private ParticleSystem m_ParticleSystem;

		public static ScriptPlayable<ParticleControlPlayable> Create(PlayableGraph graph, ParticleSystem component, uint randomSeed)
		{
			ScriptPlayable<ParticleControlPlayable> result;
			if (component == null)
			{
				result = ScriptPlayable<ParticleControlPlayable>.Null;
			}
			else
			{
				ScriptPlayable<ParticleControlPlayable> scriptPlayable = ScriptPlayable<ParticleControlPlayable>.Create(graph, 0);
				scriptPlayable.GetBehaviour().Initialize(component, randomSeed);
				result = scriptPlayable;
			}
			return result;
		}

		public ParticleSystem particleSystem
		{
			get
			{
				return this.m_ParticleSystem;
			}
		}

		public void Initialize(ParticleSystem particleSystem, uint randomSeed)
		{
			this.m_RandomSeed = Math.Max(1u, randomSeed);
			this.m_ParticleSystem = particleSystem;
		}

		public override void PrepareFrame(Playable playable, FrameData data)
		{
			if (!(this.particleSystem == null) && this.particleSystem.gameObject.activeInHierarchy)
			{
				if (this.particleSystem.randomSeed != this.m_RandomSeed)
				{
					this.particleSystem.Stop();
					this.particleSystem.useAutoRandomSeed = false;
					this.particleSystem.randomSeed = this.m_RandomSeed;
				}
				float num = (float)playable.GetTime<Playable>();
				bool flag = Mathf.Approximately(this.m_LastTime, -1f) || !Mathf.Approximately(this.m_LastTime, num);
				if (flag)
				{
					float num2 = Time.fixedDeltaTime * 0.5f;
					float num3 = num;
					float num4 = num3 - this.m_LastTime;
					bool flag2 = num3 < this.m_LastTime || num3 < num2 || Mathf.Approximately(this.m_LastTime, -1f) || num4 > this.particleSystem.main.duration || !Mathf.Approximately(this.m_LastPsTime, this.particleSystem.time);
					if (flag2)
					{
						this.particleSystem.Simulate(0f, true, true);
						this.particleSystem.Simulate(num3, true, false);
					}
					else
					{
						float num5 = num3 % this.particleSystem.main.duration;
						float num6 = num5 - this.particleSystem.time;
						if (num6 < -num2)
						{
							num6 = num5 + this.particleSystem.main.duration - this.particleSystem.time;
						}
						this.particleSystem.Simulate(num6, true, false);
					}
					this.m_LastPsTime = this.particleSystem.time;
					this.m_LastTime = num;
				}
			}
		}

		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			this.m_LastTime = -1f;
		}

		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			this.m_LastTime = -1f;
		}
	}
}
