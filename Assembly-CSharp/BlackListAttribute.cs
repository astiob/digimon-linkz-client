using System;

[AttributeUsage(AttributeTargets.Property)]
internal class BlackListAttribute : System.Attribute
{
	public BlackListAttribute(string name)
	{
		this.Name = name;
	}

	public string Name { get; private set; }
}
