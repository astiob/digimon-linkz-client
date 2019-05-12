using System;

public class ClassSingleton<T> where T : class, new()
{
	private static readonly T instance = Activator.CreateInstance<T>();

	protected ClassSingleton()
	{
	}

	public static T Instance
	{
		get
		{
			return ClassSingleton<T>.instance;
		}
	}
}
