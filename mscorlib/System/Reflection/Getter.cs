using System;

namespace System.Reflection
{
	internal delegate R Getter<T, R>(T _this);
}
