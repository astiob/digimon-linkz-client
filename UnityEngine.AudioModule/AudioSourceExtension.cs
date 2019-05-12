using System;

namespace UnityEngine
{
	internal class AudioSourceExtension : ScriptableObject
	{
		[SerializeField]
		private AudioSource m_audioSource;

		internal int m_ExtensionManagerUpdateIndex = -1;

		public AudioSource audioSource
		{
			get
			{
				return this.m_audioSource;
			}
			set
			{
				this.m_audioSource = value;
			}
		}

		public virtual float ReadExtensionProperty(PropertyName propertyName)
		{
			return 0f;
		}

		public virtual void WriteExtensionProperty(PropertyName propertyName, float propertyValue)
		{
		}

		public virtual void Play()
		{
		}

		public virtual void Stop()
		{
		}

		public virtual void ExtensionUpdate()
		{
		}

		public void OnDestroy()
		{
			this.Stop();
			AudioExtensionManager.RemoveExtensionFromManager(this);
			if (this.audioSource != null)
			{
				if (this.audioSource.spatializerExtension == this)
				{
					this.audioSource.spatializerExtension = null;
				}
				if (this.audioSource.ambisonicExtension == this)
				{
					this.audioSource.ambisonicExtension = null;
				}
			}
		}
	}
}
