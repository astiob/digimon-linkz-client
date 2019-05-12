using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
	internal class QueryableEnumerable<TElement> : IEnumerable, IOrderedQueryable, IQueryProvider, IQueryable, IQueryableEnumerable, IEnumerable<TElement>, IQueryableEnumerable<TElement>, IQueryable<TElement>, IOrderedQueryable<TElement>
	{
		private Expression expression;

		private IEnumerable<TElement> enumerable;

		public QueryableEnumerable(IEnumerable<TElement> enumerable)
		{
			this.expression = Expression.Constant(this);
			this.enumerable = enumerable;
		}

		public QueryableEnumerable(Expression expression)
		{
			this.expression = expression;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public Type ElementType
		{
			get
			{
				return typeof(TElement);
			}
		}

		public Expression Expression
		{
			get
			{
				return this.expression;
			}
		}

		public IQueryProvider Provider
		{
			get
			{
				return this;
			}
		}

		public IEnumerable GetEnumerable()
		{
			return this.enumerable;
		}

		public IEnumerator<TElement> GetEnumerator()
		{
			return this.Execute<IEnumerable<TElement>>(this.expression).GetEnumerator();
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return (IQueryable)Activator.CreateInstance(typeof(QueryableEnumerable<>).MakeGenericType(new Type[]
			{
				expression.Type.GetFirstGenericArgument()
			}), new object[]
			{
				expression
			});
		}

		public object Execute(Expression expression)
		{
			LambdaExpression lambdaExpression = Expression.Lambda(QueryableEnumerable<TElement>.TransformQueryable(expression), new ParameterExpression[0]);
			return lambdaExpression.Compile().DynamicInvoke(new object[0]);
		}

		private static Expression TransformQueryable(Expression expression)
		{
			return new QueryableTransformer().Transform(expression);
		}

		public IQueryable<TElem> CreateQuery<TElem>(Expression expression)
		{
			return new QueryableEnumerable<TElem>(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			Expression<Func<TResult>> expression2 = Expression.Lambda<Func<TResult>>(QueryableEnumerable<TElement>.TransformQueryable(expression), new ParameterExpression[0]);
			return expression2.Compile()();
		}
	}
}
