using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[NotKeyable]
	[Serializable]
	public class ControlPlayableAsset : PlayableAsset, IPropertyPreview, ITimelineClipAsset
	{
		private static readonly int k_MaxRandInt = 10000;

		[SerializeField]
		public ExposedReference<GameObject> sourceGameObject;

		[SerializeField]
		public GameObject prefabGameObject;

		[SerializeField]
		public bool updateParticle = true;

		[SerializeField]
		public uint particleRandomSeed;

		[SerializeField]
		public bool updateDirector = true;

		[SerializeField]
		public bool updateITimeControl = true;

		[SerializeField]
		public bool searchHierarchy = true;

		[SerializeField]
		public bool active = true;

		[SerializeField]
		public ActivationControlPlayable.PostPlaybackState postPlayback = ActivationControlPlayable.PostPlaybackState.Revert;

		private PlayableAsset m_ControlDirectorAsset;

		private double m_Duration = PlayableBinding.DefaultDuration;

		private bool m_SupportLoop;

		private static HashSet<PlayableDirector> s_ProcessedDirectors = new HashSet<PlayableDirector>();

		public void OnEnable()
		{
			if (this.particleRandomSeed == 0u)
			{
				this.particleRandomSeed = (uint)Random.Range(1, ControlPlayableAsset.k_MaxRandInt);
			}
		}

		public override double duration
		{
			get
			{
				return this.m_Duration;
			}
		}

		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ((!this.m_SupportLoop) ? ClipCaps.None : ClipCaps.Looping);
			}
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			List<Playable> list = new List<Playable>();
			GameObject gameObject = this.sourceGameObject.Resolve(graph.GetResolver());
			if (this.prefabGameObject != null)
			{
				Transform parentTransform = (!(gameObject != null)) ? null : gameObject.transform;
				ScriptPlayable<PrefabControlPlayable> playable = PrefabControlPlayable.Create(graph, this.prefabGameObject, parentTransform);
				gameObject = playable.GetBehaviour().prefabInstance;
				list.Add(playable);
			}
			this.m_Duration = PlayableBinding.DefaultDuration;
			this.m_SupportLoop = false;
			Playable result;
			if (gameObject == null)
			{
				result = Playable.Create(graph, 0);
			}
			else
			{
				IList<PlayableDirector> component = this.GetComponent<PlayableDirector>(gameObject);
				IList<ParticleSystem> component2 = this.GetComponent<ParticleSystem>(gameObject);
				this.UpdateDurationAndLoopFlag(component, component2);
				PlayableDirector component3 = go.GetComponent<PlayableDirector>();
				if (component3 != null)
				{
					this.m_ControlDirectorAsset = component3.playableAsset;
				}
				if (go == gameObject && this.prefabGameObject == null)
				{
					Debug.LogWarning("Control Playable (" + base.name + ") is referencing the same PlayableDirector component than the one in which it is playing.");
					this.active = false;
					if (!this.searchHierarchy)
					{
						this.updateDirector = false;
					}
				}
				if (this.active)
				{
					this.CreateActivationPlayable(gameObject, graph, list);
				}
				if (this.updateDirector)
				{
					this.SearchHierarchyAndConnectDirector(component, graph, list);
				}
				if (this.updateParticle)
				{
					this.SearchHiearchyAndConnectParticleSystem(component2, graph, list);
				}
				if (this.updateITimeControl)
				{
					ControlPlayableAsset.SearchHierarchyAndConnectControlableScripts(ControlPlayableAsset.GetControlableScripts(gameObject), graph, list);
				}
				Playable playable2 = ControlPlayableAsset.ConnectPlayablesToMixer(graph, list);
				result = playable2;
			}
			return result;
		}

		private static Playable ConnectPlayablesToMixer(PlayableGraph graph, List<Playable> playables)
		{
			Playable playable = Playable.Create(graph, playables.Count);
			for (int num = 0; num != playables.Count; num++)
			{
				ControlPlayableAsset.ConnectMixerAndPlayable(graph, playable, playables[num], num);
			}
			playable.SetPropagateSetTime(true);
			return playable;
		}

		private void CreateActivationPlayable(GameObject root, PlayableGraph graph, List<Playable> outplayables)
		{
			ScriptPlayable<ActivationControlPlayable> playable = ActivationControlPlayable.Create(graph, root, this.postPlayback);
			if (playable.IsValid<ScriptPlayable<ActivationControlPlayable>>())
			{
				outplayables.Add(playable);
			}
		}

		private void SearchHiearchyAndConnectParticleSystem(IEnumerable<ParticleSystem> particleSystems, PlayableGraph graph, List<Playable> outplayables)
		{
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				if (particleSystem != null)
				{
					outplayables.Add(ParticleControlPlayable.Create(graph, particleSystem, this.particleRandomSeed));
				}
			}
		}

		private void SearchHierarchyAndConnectDirector(IEnumerable<PlayableDirector> directors, PlayableGraph graph, List<Playable> outplayables)
		{
			foreach (PlayableDirector playableDirector in directors)
			{
				if (playableDirector != null)
				{
					if (playableDirector.playableAsset != this.m_ControlDirectorAsset)
					{
						outplayables.Add(DirectorControlPlayable.Create(graph, playableDirector));
					}
				}
			}
		}

		private static void SearchHierarchyAndConnectControlableScripts(IEnumerable<MonoBehaviour> controlableScripts, PlayableGraph graph, List<Playable> outplayables)
		{
			foreach (MonoBehaviour monoBehaviour in controlableScripts)
			{
				outplayables.Add(TimeControlPlayable.Create(graph, (ITimeControl)monoBehaviour));
			}
		}

		private static void ConnectMixerAndPlayable(PlayableGraph graph, Playable mixer, Playable playable, int portIndex)
		{
			graph.Connect<Playable, Playable>(playable, 0, mixer, portIndex);
			mixer.SetInputWeight(playable, 1f);
		}

		internal IList<T> GetComponent<T>(GameObject gameObject)
		{
			List<T> list = new List<T>();
			if (gameObject != null)
			{
				if (this.searchHierarchy)
				{
					T[] componentsInChildren = gameObject.GetComponentsInChildren<T>(true);
					foreach (T item in componentsInChildren)
					{
						list.Add(item);
					}
				}
				else
				{
					T component = gameObject.GetComponent<T>();
					if (component != null)
					{
						list.Add(component);
					}
				}
			}
			return list;
		}

		private static IEnumerable<MonoBehaviour> GetControlableScripts(GameObject root)
		{
			if (root == null)
			{
				yield break;
			}
			foreach (MonoBehaviour script in root.GetComponentsInChildren<MonoBehaviour>())
			{
				if (script is ITimeControl)
				{
					yield return script;
				}
			}
			yield break;
		}

		private void UpdateDurationAndLoopFlag(IList<PlayableDirector> directors, IList<ParticleSystem> particleSystems)
		{
			if (directors.Count == 1 && particleSystems.Count == 0)
			{
				PlayableDirector playableDirector = directors[0];
				if (playableDirector.playableAsset != null)
				{
					this.m_Duration = playableDirector.playableAsset.duration;
					this.m_SupportLoop = (playableDirector.extrapolationMode == DirectorWrapMode.Loop);
				}
			}
			else if (particleSystems.Count == 1 && directors.Count == 0)
			{
				ParticleSystem particleSystem = particleSystems[0];
				this.m_Duration = (double)particleSystem.main.duration;
				this.m_SupportLoop = particleSystem.main.loop;
			}
		}

		public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			if (!(director == null))
			{
				if (!ControlPlayableAsset.s_ProcessedDirectors.Contains(director))
				{
					ControlPlayableAsset.s_ProcessedDirectors.Add(director);
					GameObject gameObject = this.sourceGameObject.Resolve(director);
					if (gameObject != null)
					{
						if (this.updateParticle)
						{
							foreach (ParticleSystem particleSystem in this.GetComponent<ParticleSystem>(gameObject))
							{
								driver.AddFromName<ParticleSystem>(particleSystem.gameObject, "randomSeed");
								driver.AddFromName<ParticleSystem>(particleSystem.gameObject, "autoRandomSeed");
							}
						}
						if (this.active)
						{
							driver.AddFromName(gameObject, "m_IsActive");
						}
						if (this.updateITimeControl)
						{
							foreach (MonoBehaviour monoBehaviour in ControlPlayableAsset.GetControlableScripts(gameObject))
							{
								IPropertyPreview propertyPreview = monoBehaviour as IPropertyPreview;
								if (propertyPreview != null)
								{
									propertyPreview.GatherProperties(director, driver);
								}
								else
								{
									driver.AddFromComponent(monoBehaviour.gameObject, monoBehaviour);
								}
							}
						}
						if (this.updateDirector)
						{
							foreach (PlayableDirector playableDirector in this.GetComponent<PlayableDirector>(gameObject))
							{
								if (!(playableDirector == null))
								{
									TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;
									if (!(timelineAsset == null))
									{
										timelineAsset.GatherProperties(playableDirector, driver);
									}
								}
							}
						}
					}
					ControlPlayableAsset.s_ProcessedDirectors.Remove(director);
				}
			}
		}
	}
}
