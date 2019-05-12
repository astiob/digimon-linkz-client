using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Linq
{
	internal class QueryableTransformer : ExpressionTransformer
	{
		protected override Expression VisitMethodCall(MethodCallExpression methodCall)
		{
			if (QueryableTransformer.IsQueryableExtension(methodCall.Method))
			{
				return this.ReplaceQueryableMethod(methodCall);
			}
			return base.VisitMethodCall(methodCall);
		}

		protected override Expression VisitLambda(LambdaExpression lambda)
		{
			return lambda;
		}

		protected override Expression VisitConstant(ConstantExpression constant)
		{
			IQueryableEnumerable queryableEnumerable = constant.Value as IQueryableEnumerable;
			if (queryableEnumerable == null)
			{
				return constant;
			}
			return Expression.Constant(queryableEnumerable.GetEnumerable());
		}

		private static bool IsQueryableExtension(MethodInfo method)
		{
			return QueryableTransformer.HasExtensionAttribute(method) && method.GetParameters()[0].ParameterType.IsAssignableTo(typeof(IQueryable));
		}

		private static bool HasExtensionAttribute(MethodInfo method)
		{
			return method.GetCustomAttributes(typeof(ExtensionAttribute), false).Length > 0;
		}

		private MethodCallExpression ReplaceQueryableMethod(MethodCallExpression old)
		{
			Expression obj = null;
			if (old.Object != null)
			{
				obj = this.Visit(old.Object);
			}
			MethodInfo methodInfo = QueryableTransformer.ReplaceQueryableMethod(old.Method);
			ParameterInfo[] parameters = methodInfo.GetParameters();
			Expression[] array = new Expression[old.Arguments.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = QueryableTransformer.UnquoteIfNeeded(this.Visit(old.Arguments[i]), parameters[i].ParameterType);
			}
			return new MethodCallExpression(obj, methodInfo, array.ToReadOnlyCollection<Expression>());
		}

		private static Expression UnquoteIfNeeded(Expression expression, Type delegateType)
		{
			if (expression.NodeType != ExpressionType.Quote)
			{
				return expression;
			}
			LambdaExpression lambdaExpression = (LambdaExpression)((UnaryExpression)expression).Operand;
			if (lambdaExpression.Type == delegateType)
			{
				return lambdaExpression;
			}
			return expression;
		}

		private static Type GetTargetDeclaringType(MethodInfo method)
		{
			return (method.DeclaringType != typeof(Queryable)) ? method.DeclaringType : typeof(Enumerable);
		}

		private static MethodInfo ReplaceQueryableMethod(MethodInfo method)
		{
			MethodInfo matchingMethod = QueryableTransformer.GetMatchingMethod(method, QueryableTransformer.GetTargetDeclaringType(method));
			if (matchingMethod != null)
			{
				return matchingMethod;
			}
			throw new InvalidOperationException(string.Format("There is no method {0} on type {1} that matches the specified arguments", method.Name, method.DeclaringType.FullName));
		}

		private static MethodInfo GetMatchingMethod(MethodInfo method, Type declaring)
		{
			MethodInfo[] methods = declaring.GetMethods();
			int i = 0;
			while (i < methods.Length)
			{
				MethodInfo methodInfo = methods[i];
				if (!QueryableTransformer.MethodMatch(methodInfo, method))
				{
					i++;
				}
				else
				{
					if (method.IsGenericMethod)
					{
						return methodInfo.MakeGenericMethodFrom(method);
					}
					return methodInfo;
				}
			}
			return null;
		}

		private static bool MethodMatch(MethodInfo candidate, MethodInfo method)
		{
			if (candidate.Name != method.Name)
			{
				return false;
			}
			if (!QueryableTransformer.HasExtensionAttribute(candidate))
			{
				return false;
			}
			Type[] parameterTypes = method.GetParameterTypes();
			if (parameterTypes.Length != candidate.GetParameters().Length)
			{
				return false;
			}
			if (method.IsGenericMethod)
			{
				if (!candidate.IsGenericMethod)
				{
					return false;
				}
				if (candidate.GetGenericArguments().Length != method.GetGenericArguments().Length)
				{
					return false;
				}
				candidate = candidate.MakeGenericMethodFrom(method);
			}
			if (!QueryableTransformer.TypeMatch(candidate.ReturnType, method.ReturnType))
			{
				return false;
			}
			Type[] parameterTypes2 = candidate.GetParameterTypes();
			if (parameterTypes2[0] != QueryableTransformer.GetComparableType(parameterTypes[0]))
			{
				return false;
			}
			for (int i = 1; i < parameterTypes2.Length; i++)
			{
				if (!QueryableTransformer.TypeMatch(parameterTypes2[i], parameterTypes[i]))
				{
					return false;
				}
			}
			return true;
		}

		private static bool TypeMatch(Type candidate, Type type)
		{
			return candidate == type || candidate == QueryableTransformer.GetComparableType(type);
		}

		private static Type GetComparableType(Type type)
		{
			if (type.IsGenericInstanceOf(typeof(IQueryable<>)))
			{
				type = typeof(IEnumerable<>).MakeGenericTypeFrom(type);
			}
			else if (type.IsGenericInstanceOf(typeof(IOrderedQueryable<>)))
			{
				type = typeof(IOrderedEnumerable<>).MakeGenericTypeFrom(type);
			}
			else if (type.IsGenericInstanceOf(typeof(Expression<>)))
			{
				type = type.GetFirstGenericArgument();
			}
			else if (type == typeof(IQueryable))
			{
				type = typeof(IEnumerable);
			}
			return type;
		}
	}
}
