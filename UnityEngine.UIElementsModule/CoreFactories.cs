using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.UIElements
{
	internal static class CoreFactories
	{
		[CompilerGenerated]
		private static Func<IUxmlAttributes, CreationContext, VisualElement> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<IUxmlAttributes, CreationContext, VisualElement> <>f__mg$cache1;

		[CompilerGenerated]
		private static Func<IUxmlAttributes, CreationContext, VisualElement> <>f__mg$cache2;

		[CompilerGenerated]
		private static Func<IUxmlAttributes, CreationContext, VisualElement> <>f__mg$cache3;

		[CompilerGenerated]
		private static Func<IUxmlAttributes, CreationContext, VisualElement> <>f__mg$cache4;

		[CompilerGenerated]
		private static Func<IUxmlAttributes, CreationContext, VisualElement> <>f__mg$cache5;

		[CompilerGenerated]
		private static Func<IUxmlAttributes, CreationContext, VisualElement> <>f__mg$cache6;

		[CompilerGenerated]
		private static Func<IUxmlAttributes, CreationContext, VisualElement> <>f__mg$cache7;

		internal static void RegisterAll()
		{
			if (CoreFactories.<>f__mg$cache0 == null)
			{
				CoreFactories.<>f__mg$cache0 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateButton);
			}
			Factories.RegisterFactory<Button>(CoreFactories.<>f__mg$cache0);
			if (CoreFactories.<>f__mg$cache1 == null)
			{
				CoreFactories.<>f__mg$cache1 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateIMGUIContainer);
			}
			Factories.RegisterFactory<IMGUIContainer>(CoreFactories.<>f__mg$cache1);
			Factories.RegisterFactory<Image>((IUxmlAttributes _, CreationContext __) => new Image());
			Factories.RegisterFactory<Label>((IUxmlAttributes _, CreationContext __) => new Label());
			if (CoreFactories.<>f__mg$cache2 == null)
			{
				CoreFactories.<>f__mg$cache2 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateRepeatButton);
			}
			Factories.RegisterFactory<RepeatButton>(CoreFactories.<>f__mg$cache2);
			if (CoreFactories.<>f__mg$cache3 == null)
			{
				CoreFactories.<>f__mg$cache3 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateScrollerButton);
			}
			Factories.RegisterFactory<ScrollerButton>(CoreFactories.<>f__mg$cache3);
			Factories.RegisterFactory<ScrollView>((IUxmlAttributes _, CreationContext __) => new ScrollView());
			if (CoreFactories.<>f__mg$cache4 == null)
			{
				CoreFactories.<>f__mg$cache4 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateScroller);
			}
			Factories.RegisterFactory<Scroller>(CoreFactories.<>f__mg$cache4);
			if (CoreFactories.<>f__mg$cache5 == null)
			{
				CoreFactories.<>f__mg$cache5 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateSlider);
			}
			Factories.RegisterFactory<Slider>(CoreFactories.<>f__mg$cache5);
			Factories.RegisterFactory<TextField>((IUxmlAttributes _, CreationContext __) => new TextField());
			if (CoreFactories.<>f__mg$cache6 == null)
			{
				CoreFactories.<>f__mg$cache6 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateToggle);
			}
			Factories.RegisterFactory<Toggle>(CoreFactories.<>f__mg$cache6);
			Factories.RegisterFactory<VisualContainer>((IUxmlAttributes _, CreationContext __) => new VisualContainer());
			Factories.RegisterFactory<VisualElement>((IUxmlAttributes _, CreationContext __) => new VisualElement());
			if (CoreFactories.<>f__mg$cache7 == null)
			{
				CoreFactories.<>f__mg$cache7 = new Func<IUxmlAttributes, CreationContext, VisualElement>(CoreFactories.CreateTemplate);
			}
			Factories.RegisterFactory<TemplateContainer>(CoreFactories.<>f__mg$cache7);
		}

		private static VisualElement CreateButton(IUxmlAttributes bag, CreationContext ctx)
		{
			return new Button(null);
		}

		private static VisualElement CreateTemplate(IUxmlAttributes bag, CreationContext ctx)
		{
			string templateAlias = ((TemplateAsset)bag).templateAlias;
			VisualTreeAsset visualTreeAsset = ctx.visualTreeAsset.ResolveUsing(templateAlias);
			TemplateContainer templateContainer = new TemplateContainer(templateAlias);
			if (visualTreeAsset == null)
			{
				templateContainer.Add(new Label(string.Format("Unknown Element: '{0}'", templateAlias)));
			}
			else
			{
				visualTreeAsset.CloneTree(templateContainer, ctx.slotInsertionPoints);
			}
			if (visualTreeAsset == null)
			{
				Debug.LogErrorFormat("Could not resolve template with alias '{0}'", new object[]
				{
					templateAlias
				});
			}
			return templateContainer;
		}

		private static VisualElement CreateIMGUIContainer(IUxmlAttributes bag, CreationContext ctx)
		{
			return new IMGUIContainer(null);
		}

		private static VisualElement CreateRepeatButton(IUxmlAttributes bag, CreationContext ctx)
		{
			return new RepeatButton(null, bag.GetPropertyLong("delay", 0L), bag.GetPropertyLong("interval", 0L));
		}

		private static VisualElement CreateScrollerButton(IUxmlAttributes bag, CreationContext ctx)
		{
			return new ScrollerButton(null, bag.GetPropertyLong("delay", 0L), bag.GetPropertyLong("interval", 0L));
		}

		private static VisualElement CreateScroller(IUxmlAttributes bag, CreationContext ctx)
		{
			return new Scroller(bag.GetPropertyFloat("lowValue", 0f), bag.GetPropertyFloat("highValue", 0f), null, bag.GetPropertyEnum<Slider.Direction>("direction", Slider.Direction.Horizontal));
		}

		private static VisualElement CreateSlider(IUxmlAttributes bag, CreationContext ctx)
		{
			return new Slider(bag.GetPropertyFloat("lowValue", 0f), bag.GetPropertyFloat("highValue", 0f), null, bag.GetPropertyEnum<Slider.Direction>("direction", Slider.Direction.Horizontal), 10f);
		}

		private static VisualElement CreateToggle(IUxmlAttributes bag, CreationContext ctx)
		{
			return new Toggle(null);
		}
	}
}
