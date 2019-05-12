using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniRx
{
	public static class UnityUIComponentExtensions
	{
		public static IDisposable SubscribeToText(this IObservable<string> source, Text text)
		{
			return source.SubscribeWithState(text, delegate(string x, Text t)
			{
				t.text = x;
			});
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, Text text)
		{
			return source.SubscribeWithState(text, delegate(T x, Text t)
			{
				t.text = x.ToString();
			});
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, Text text, Func<T, string> selector)
		{
			return source.SubscribeWithState2(text, selector, delegate(T x, Text t, Func<T, string> s)
			{
				t.text = s(x);
			});
		}

		public static IDisposable SubscribeToInteractable(this IObservable<bool> source, Selectable selectable)
		{
			return source.SubscribeWithState(selectable, delegate(bool x, Selectable s)
			{
				s.interactable = x;
			});
		}

		public static IObservable<Unit> OnClickAsObservable(this Button button)
		{
			return button.onClick.AsObservable();
		}

		public static IObservable<bool> OnValueChangedAsObservable(this Toggle toggle)
		{
			return Observable.CreateWithState<bool, Toggle>(toggle, delegate(Toggle t, IObserver<bool> observer)
			{
				observer.OnNext(t.isOn);
				return t.onValueChanged.AsObservable<bool>().Subscribe(observer);
			});
		}

		public static IObservable<float> OnValueChangedAsObservable(this Scrollbar scrollbar)
		{
			return Observable.CreateWithState<float, Scrollbar>(scrollbar, delegate(Scrollbar s, IObserver<float> observer)
			{
				observer.OnNext(s.value);
				return s.onValueChanged.AsObservable<float>().Subscribe(observer);
			});
		}

		public static IObservable<Vector2> OnValueChangedAsObservable(this ScrollRect scrollRect)
		{
			return Observable.CreateWithState<Vector2, ScrollRect>(scrollRect, delegate(ScrollRect s, IObserver<Vector2> observer)
			{
				observer.OnNext(s.normalizedPosition);
				return s.onValueChanged.AsObservable<Vector2>().Subscribe(observer);
			});
		}

		public static IObservable<float> OnValueChangedAsObservable(this Slider slider)
		{
			return Observable.CreateWithState<float, Slider>(slider, delegate(Slider s, IObserver<float> observer)
			{
				observer.OnNext(s.value);
				return s.onValueChanged.AsObservable<float>().Subscribe(observer);
			});
		}

		public static IObservable<string> OnEndEditAsObservable(this InputField inputField)
		{
			return inputField.onEndEdit.AsObservable<string>();
		}

		[Obsolete("onValueChange has been renamed to onValueChanged")]
		public static IObservable<string> OnValueChangeAsObservable(this InputField inputField)
		{
			return Observable.CreateWithState<string, InputField>(inputField, delegate(InputField i, IObserver<string> observer)
			{
				observer.OnNext(i.text);
				return i.onValueChanged.AsObservable<string>().Subscribe(observer);
			});
		}

		public static IObservable<string> OnValueChangedAsObservable(this InputField inputField)
		{
			return Observable.CreateWithState<string, InputField>(inputField, delegate(InputField i, IObserver<string> observer)
			{
				observer.OnNext(i.text);
				return i.onValueChanged.AsObservable<string>().Subscribe(observer);
			});
		}
	}
}
