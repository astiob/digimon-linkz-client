using System;

namespace UniRx
{
	public abstract class PresenterBase : PresenterBase<Unit>
	{
		protected sealed override void BeforeInitialize(Unit argument)
		{
			this.BeforeInitialize();
		}

		protected abstract void BeforeInitialize();

		protected override void Initialize(Unit argument)
		{
			this.Initialize();
		}

		public void ForceInitialize()
		{
			this.ForceInitialize(Unit.Default);
		}

		protected abstract void Initialize();
	}
}
