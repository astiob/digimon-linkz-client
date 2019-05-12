using System;
using UnityEngine;

namespace UniRx
{
	public abstract class PresenterBase<T> : MonoBehaviour, IPresenter
	{
		protected static readonly IPresenter[] EmptyChildren = new IPresenter[0];

		private int childrenCount;

		private int currentCalledCount;

		private bool isAwaken;

		private bool isInitialized;

		private bool isStartedCapturePhase;

		private Subject<Unit> initializeSubject;

		private IPresenter[] children;

		private IPresenter parent;

		private T argument = default(T);

		public IPresenter Parent
		{
			get
			{
				return this.parent;
			}
		}

		public IObservable<Unit> InitializeAsObservable()
		{
			if (this.isInitialized)
			{
				return Observable.Return(Unit.Default);
			}
			Subject<Unit> result;
			if ((result = this.initializeSubject) == null)
			{
				result = (this.initializeSubject = new Subject<Unit>());
			}
			return result;
		}

		public void PropagateArgument(T argument)
		{
			this.argument = argument;
		}

		protected abstract IPresenter[] Children { get; }

		protected abstract void BeforeInitialize(T argument);

		protected abstract void Initialize(T argument);

		public virtual void ForceInitialize(T argument)
		{
			this.Awake();
			this.PropagateArgument(argument);
			this.Start();
		}

		void IPresenter.ForceInitialize(object argument)
		{
			this.ForceInitialize((T)((object)argument));
		}

		void IPresenter.Awake()
		{
			if (this.isAwaken)
			{
				return;
			}
			this.isAwaken = true;
			this.children = this.Children;
			this.childrenCount = this.children.Length;
			for (int i = 0; i < this.children.Length; i++)
			{
				IPresenter presenter = this.children[i];
				presenter.RegisterParent(this);
				presenter.Awake();
			}
			this.OnAwake();
		}

		protected void Awake()
		{
			((IPresenter)this).Awake();
		}

		protected virtual void OnAwake()
		{
		}

		protected void Start()
		{
			if (this.isStartedCapturePhase)
			{
				return;
			}
			IPresenter presenter = this.parent;
			if (presenter == null)
			{
				presenter = this;
			}
			else
			{
				while (presenter.Parent != null)
				{
					presenter = presenter.Parent;
				}
			}
			presenter.StartCapturePhase();
		}

		void IPresenter.StartCapturePhase()
		{
			this.isStartedCapturePhase = true;
			this.BeforeInitialize(this.argument);
			for (int i = 0; i < this.children.Length; i++)
			{
				IPresenter presenter = this.children[i];
				presenter.StartCapturePhase();
			}
			if (this.children.Length == 0)
			{
				this.Initialize(this.argument);
				this.isInitialized = true;
				if (this.initializeSubject != null)
				{
					this.initializeSubject.OnNext(Unit.Default);
					this.initializeSubject.OnCompleted();
				}
				if (this.parent != null)
				{
					this.parent.InitializeCore();
				}
			}
		}

		void IPresenter.RegisterParent(IPresenter parent)
		{
			if (this.parent != null)
			{
				throw new InvalidOperationException("PresenterBase can't register multiple parent. Name:" + base.name);
			}
			this.parent = parent;
		}

		void IPresenter.InitializeCore()
		{
			this.currentCalledCount++;
			if (this.childrenCount == this.currentCalledCount)
			{
				this.Initialize(this.argument);
				this.isInitialized = true;
				if (this.initializeSubject != null)
				{
					this.initializeSubject.OnNext(Unit.Default);
					this.initializeSubject.OnCompleted();
				}
				if (this.parent != null)
				{
					this.parent.InitializeCore();
				}
			}
		}

		GameObject IPresenter.get_gameObject()
		{
			return base.gameObject;
		}
	}
}
