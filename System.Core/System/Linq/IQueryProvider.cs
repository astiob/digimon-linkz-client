using System;
using System.Linq.Expressions;

namespace System.Linq
{
	/// <summary>Defines methods to create and execute queries that are described by an <see cref="T:System.Linq.IQueryable" /> object.</summary>
	/// <filterpriority>2</filterpriority>
	public interface IQueryProvider
	{
		/// <summary>Constructs an <see cref="T:System.Linq.IQueryable" /> object that can evaluate the query represented by a specified expression tree.</summary>
		/// <returns>An <see cref="T:System.Linq.IQueryable" /> that can evaluate the query represented by the specified expression tree.</returns>
		/// <param name="expression">An expression tree that represents a LINQ query.</param>
		IQueryable CreateQuery(Expression expression);

		/// <summary>Executes the query represented by a specified expression tree.</summary>
		/// <returns>The value that results from executing the specified query.</returns>
		/// <param name="expression">An expression tree that represents a LINQ query.</param>
		object Execute(Expression expression);

		/// <summary>Constructs an <see cref="T:System.Linq.IQueryable`1" /> object that can evaluate the query represented by a specified expression tree.</summary>
		/// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that can evaluate the query represented by the specified expression tree.</returns>
		/// <param name="expression">An expression tree that represents a LINQ query.</param>
		/// <typeparam name="TElement">The type of the elements of the <see cref="T:System.Linq.IQueryable`1" /> that is returned.</typeparam>
		IQueryable<TElement> CreateQuery<TElement>(Expression expression);

		/// <summary>Executes the strongly-typed query represented by a specified expression tree.</summary>
		/// <returns>The value that results from executing the specified query.</returns>
		/// <param name="expression">An expression tree that represents a LINQ query.</param>
		/// <typeparam name="TResult">The type of the value that results from executing the query.</typeparam>
		TResult Execute<TResult>(Expression expression);
	}
}
