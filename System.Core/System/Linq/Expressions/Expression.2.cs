using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace System.Linq.Expressions
{
	/// <summary>Provides the base class from which the classes that represent expression tree nodes are derived. It also contains static (Shared in Visual Basic) factory methods to create the various node types. This is an abstract class.</summary>
	public abstract class Expression
	{
		internal const BindingFlags PublicInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

		internal const BindingFlags NonPublicInstance = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		internal const BindingFlags PublicStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;

		internal const BindingFlags AllInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		internal const BindingFlags AllStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		internal const BindingFlags All = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		private ExpressionType node_type;

		private Type type;

		/// <summary>Initializes a new instance of the <see cref="T:System.Linq.Expressions.Expression" /> class.</summary>
		/// <param name="nodeType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> to set as the node type.</param>
		/// <param name="type">The <see cref="T:System.Type" /> to set as the type of the expression that this <see cref="T:System.Linq.Expressions.Expression" /> represents.</param>
		protected Expression(ExpressionType node_type, Type type)
		{
			this.node_type = node_type;
			this.type = type;
		}

		/// <summary>Gets the node type of this <see cref="T:System.Linq.Expressions.Expression" />.</summary>
		/// <returns>One of the <see cref="T:System.Linq.Expressions.ExpressionType" /> values.</returns>
		public ExpressionType NodeType
		{
			get
			{
				return this.node_type;
			}
		}

		/// <summary>Gets the static type of the expression that this <see cref="T:System.Linq.Expressions.Expression" /> represents.</summary>
		/// <returns>The <see cref="T:System.Type" /> that represents the static type of the expression.</returns>
		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>Returns a textual representation of the <see cref="T:System.Linq.Expressions.Expression" />.</summary>
		/// <returns>A textual representation of the <see cref="T:System.Linq.Expressions.Expression" />.</returns>
		public override string ToString()
		{
			return ExpressionPrinter.ToString(this);
		}

		private static MethodInfo GetUnaryOperator(string oper_name, Type declaring, Type param)
		{
			return Expression.GetUnaryOperator(oper_name, declaring, param, null);
		}

		private static MethodInfo GetUnaryOperator(string oper_name, Type declaring, Type param, Type ret)
		{
			MethodInfo[] methods = declaring.GetNotNullableType().GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			foreach (MethodInfo methodInfo in methods)
			{
				if (!(methodInfo.Name != oper_name))
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 1)
					{
						if (!methodInfo.IsGenericMethod)
						{
							if (Expression.IsAssignableToParameterType(param.GetNotNullableType(), parameters[0]))
							{
								if (ret == null || methodInfo.ReturnType == ret.GetNotNullableType())
								{
									return methodInfo;
								}
							}
						}
					}
				}
			}
			return null;
		}

		internal static MethodInfo GetTrueOperator(Type self)
		{
			return Expression.GetBooleanOperator("op_True", self);
		}

		internal static MethodInfo GetFalseOperator(Type self)
		{
			return Expression.GetBooleanOperator("op_False", self);
		}

		private static MethodInfo GetBooleanOperator(string op, Type self)
		{
			return Expression.GetUnaryOperator(op, self, self, typeof(bool));
		}

		private static bool IsAssignableToParameterType(Type type, ParameterInfo param)
		{
			Type type2 = param.ParameterType;
			if (type2.IsByRef)
			{
				type2 = type2.GetElementType();
			}
			return type.GetNotNullableType().IsAssignableTo(type2);
		}

		private static MethodInfo CheckUnaryMethod(MethodInfo method, Type param)
		{
			if (method.ReturnType == typeof(void))
			{
				throw new ArgumentException("Specified method must return a value", "method");
			}
			if (!method.IsStatic)
			{
				throw new ArgumentException("Method must be static", "method");
			}
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != 1)
			{
				throw new ArgumentException("Must have only one parameters", "method");
			}
			if (!Expression.IsAssignableToParameterType(param.GetNotNullableType(), parameters[0]))
			{
				throw new InvalidOperationException("left-side argument type does not match expression type");
			}
			return method;
		}

		private static MethodInfo UnaryCoreCheck(string oper_name, Expression expression, MethodInfo method, Func<Type, bool> validator)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (method != null)
			{
				return Expression.CheckUnaryMethod(method, expression.Type);
			}
			Type notNullableType = expression.Type.GetNotNullableType();
			if (validator(notNullableType))
			{
				return null;
			}
			if (oper_name != null)
			{
				method = Expression.GetUnaryOperator(oper_name, notNullableType, expression.Type);
				if (method != null)
				{
					return method;
				}
			}
			throw new InvalidOperationException(string.Format("Operation {0} not defined for {1}", (oper_name == null) ? "is" : oper_name.Substring(3), expression.Type));
		}

		private static MethodInfo GetBinaryOperator(string oper_name, Type on_type, Expression left, Expression right)
		{
			MethodInfo[] methods = on_type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			foreach (MethodInfo methodInfo in methods)
			{
				if (!(methodInfo.Name != oper_name))
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 2)
					{
						if (!methodInfo.IsGenericMethod)
						{
							if (Expression.IsAssignableToParameterType(left.Type, parameters[0]))
							{
								if (Expression.IsAssignableToParameterType(right.Type, parameters[1]))
								{
									return methodInfo;
								}
							}
						}
					}
				}
			}
			return null;
		}

		private static MethodInfo BinaryCoreCheck(string oper_name, Expression left, Expression right, MethodInfo method)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			if (method != null)
			{
				if (method.ReturnType == typeof(void))
				{
					throw new ArgumentException("Specified method must return a value", "method");
				}
				if (!method.IsStatic)
				{
					throw new ArgumentException("Method must be static", "method");
				}
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters.Length != 2)
				{
					throw new ArgumentException("Must have only two parameters", "method");
				}
				if (!Expression.IsAssignableToParameterType(left.Type, parameters[0]))
				{
					throw new InvalidOperationException("left-side argument type does not match left expression type");
				}
				if (!Expression.IsAssignableToParameterType(right.Type, parameters[1]))
				{
					throw new InvalidOperationException("right-side argument type does not match right expression type");
				}
				return method;
			}
			else
			{
				Type type = left.Type;
				Type type2 = right.Type;
				Type notNullableType = type.GetNotNullableType();
				Type notNullableType2 = type2.GetNotNullableType();
				if ((oper_name == "op_BitwiseOr" || oper_name == "op_BitwiseAnd") && notNullableType == typeof(bool) && notNullableType == notNullableType2 && type == type2)
				{
					return null;
				}
				if (Expression.IsNumber(notNullableType))
				{
					if (notNullableType == notNullableType2 && type == type2)
					{
						return null;
					}
					if (oper_name != null)
					{
						method = Expression.GetBinaryOperator(oper_name, notNullableType2, left, right);
						if (method != null)
						{
							return method;
						}
					}
				}
				if (oper_name != null)
				{
					method = Expression.GetBinaryOperator(oper_name, notNullableType, left, right);
					if (method != null)
					{
						return method;
					}
				}
				if (oper_name == "op_Equality" || oper_name == "op_Inequality")
				{
					if (!type.IsValueType && !type2.IsValueType)
					{
						return null;
					}
					if (type == type2 && notNullableType.IsEnum)
					{
						return null;
					}
					if (type == type2 && notNullableType == typeof(bool))
					{
						return null;
					}
				}
				if ((oper_name == "op_LeftShift" || oper_name == "op_RightShift") && Expression.IsInt(notNullableType) && notNullableType2 == typeof(int))
				{
					return null;
				}
				throw new InvalidOperationException(string.Format("Operation {0} not defined for {1} and {2}", (oper_name == null) ? "is" : oper_name.Substring(3), type, type2));
			}
		}

		private static MethodInfo BinaryBitwiseCoreCheck(string oper_name, Expression left, Expression right, MethodInfo method)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			if (method == null && left.Type == right.Type && Expression.IsIntOrBool(left.Type))
			{
				return null;
			}
			method = Expression.BinaryCoreCheck(oper_name, left, right, method);
			if (method == null && (left.Type == typeof(double) || left.Type == typeof(float)))
			{
				throw new InvalidOperationException("Types not supported");
			}
			return method;
		}

		private static BinaryExpression MakeSimpleBinary(ExpressionType et, Expression left, Expression right, MethodInfo method)
		{
			bool flag;
			Type type;
			if (method == null)
			{
				flag = left.Type.IsNullable();
				type = left.Type;
			}
			else
			{
				ParameterInfo[] parameters = method.GetParameters();
				ParameterInfo parameterInfo = parameters[0];
				ParameterInfo parameterInfo2 = parameters[1];
				if (Expression.IsAssignableToOperatorParameter(left, parameterInfo) && Expression.IsAssignableToOperatorParameter(right, parameterInfo2))
				{
					flag = false;
					type = method.ReturnType;
				}
				else
				{
					if (!left.Type.IsNullable() || !right.Type.IsNullable() || left.Type.GetNotNullableType() != parameterInfo.ParameterType || right.Type.GetNotNullableType() != parameterInfo2.ParameterType || method.ReturnType.IsNullable())
					{
						throw new InvalidOperationException();
					}
					flag = true;
					type = method.ReturnType.MakeNullableType();
				}
			}
			return new BinaryExpression(et, type, left, right, flag, flag, method, null);
		}

		private static bool IsAssignableToOperatorParameter(Expression expression, ParameterInfo parameter)
		{
			return expression.Type == parameter.ParameterType || (!expression.Type.IsNullable() && !parameter.ParameterType.IsNullable() && Expression.IsAssignableToParameterType(expression.Type, parameter));
		}

		private static UnaryExpression MakeSimpleUnary(ExpressionType et, Expression expression, MethodInfo method)
		{
			Type self;
			bool is_lifted;
			if (method == null)
			{
				self = expression.Type;
				is_lifted = self.IsNullable();
			}
			else
			{
				ParameterInfo parameterInfo = method.GetParameters()[0];
				if (Expression.IsAssignableToOperatorParameter(expression, parameterInfo))
				{
					is_lifted = false;
					self = method.ReturnType;
				}
				else
				{
					if (!expression.Type.IsNullable() || expression.Type.GetNotNullableType() != parameterInfo.ParameterType || method.ReturnType.IsNullable())
					{
						throw new InvalidOperationException();
					}
					is_lifted = true;
					self = method.ReturnType.MakeNullableType();
				}
			}
			return new UnaryExpression(et, expression, self, method, is_lifted);
		}

		private static BinaryExpression MakeBoolBinary(ExpressionType et, Expression left, Expression right, bool liftToNull, MethodInfo method)
		{
			bool is_lifted;
			Type type;
			if (method == null)
			{
				if (!left.Type.IsNullable() && !right.Type.IsNullable())
				{
					is_lifted = false;
					liftToNull = false;
					type = typeof(bool);
				}
				else
				{
					if (!left.Type.IsNullable() || !right.Type.IsNullable())
					{
						throw new InvalidOperationException();
					}
					is_lifted = true;
					type = ((!liftToNull) ? typeof(bool) : typeof(bool?));
				}
			}
			else
			{
				ParameterInfo[] parameters = method.GetParameters();
				ParameterInfo parameterInfo = parameters[0];
				ParameterInfo parameterInfo2 = parameters[1];
				if (Expression.IsAssignableToOperatorParameter(left, parameterInfo) && Expression.IsAssignableToOperatorParameter(right, parameterInfo2))
				{
					is_lifted = false;
					liftToNull = false;
					type = method.ReturnType;
				}
				else
				{
					if (!left.Type.IsNullable() || !right.Type.IsNullable() || left.Type.GetNotNullableType() != parameterInfo.ParameterType || right.Type.GetNotNullableType() != parameterInfo2.ParameterType)
					{
						throw new InvalidOperationException();
					}
					is_lifted = true;
					if (method.ReturnType == typeof(bool))
					{
						type = ((!liftToNull) ? typeof(bool) : typeof(bool?));
					}
					else
					{
						if (method.ReturnType.IsNullable())
						{
							throw new InvalidOperationException();
						}
						type = method.ReturnType.MakeNullableType();
					}
				}
			}
			return new BinaryExpression(et, type, left, right, liftToNull, is_lifted, method, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic addition operation that does not have overflow checking.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Add" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Add(Expression left, Expression right)
		{
			return Expression.Add(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic addition operation that does not have overflow checking. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Add" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Add(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Addition", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.Add, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic addition operation that has overflow checking.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression AddChecked(Expression left, Expression right)
		{
			return Expression.AddChecked(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic addition operation that has overflow checking. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AddChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the addition operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression AddChecked(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Addition", left, right, method);
			if (method == null && (left.Type == typeof(byte) || left.Type == typeof(sbyte)))
			{
				throw new InvalidOperationException(string.Format("AddChecked not defined for {0} and {1}", left.Type, right.Type));
			}
			return Expression.MakeSimpleBinary(ExpressionType.AddChecked, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic subtraction operation that does not have overflow checking.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Subtract" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Subtract(Expression left, Expression right)
		{
			return Expression.Subtract(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic subtraction operation that does not have overflow checking. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Subtract" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Subtract(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Subtraction", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.Subtract, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic subtraction operation that has overflow checking.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression SubtractChecked(Expression left, Expression right)
		{
			return Expression.SubtractChecked(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic subtraction operation that has overflow checking. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.SubtractChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the subtraction operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression SubtractChecked(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Subtraction", left, right, method);
			if (method == null && (left.Type == typeof(byte) || left.Type == typeof(sbyte)))
			{
				throw new InvalidOperationException(string.Format("SubtractChecked not defined for {0} and {1}", left.Type, right.Type));
			}
			return Expression.MakeSimpleBinary(ExpressionType.SubtractChecked, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic remainder operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Modulo" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The modulus operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Modulo(Expression left, Expression right)
		{
			return Expression.Modulo(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic remainder operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Modulo" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the modulus operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Modulo(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Modulus", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.Modulo, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic multiplication operation that does not have overflow checking.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Multiply" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Multiply(Expression left, Expression right)
		{
			return Expression.Multiply(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic multiplication operation that does not have overflow checking and for which the implementing method is specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Multiply" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Multiply(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Multiply", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.Multiply, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic multiplication operation that has overflow checking.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression MultiplyChecked(Expression left, Expression right)
		{
			return Expression.MultiplyChecked(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic multiplication operation that has overflow checking. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MultiplyChecked" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the multiplication operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression MultiplyChecked(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Multiply", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.MultiplyChecked, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic division operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Divide" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The division operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Divide(Expression left, Expression right)
		{
			return Expression.Divide(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an arithmetic division operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Divide" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the division operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Divide(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Division", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.Divide, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents raising a number to a power.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Power" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The exponentiation operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-<paramref name="left" />.Type and/or <paramref name="right" />.Type are not <see cref="T:System.Double" />.</exception>
		public static BinaryExpression Power(Expression left, Expression right)
		{
			return Expression.Power(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents raising a number to a power. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Power" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the exponentiation operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-<paramref name="method" /> is null and <paramref name="left" />.Type and/or <paramref name="right" />.Type are not <see cref="T:System.Double" />.</exception>
		public static BinaryExpression Power(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck(null, left, right, method);
			if (left.Type.GetNotNullableType() != typeof(double))
			{
				throw new InvalidOperationException("Power only supports double arguments");
			}
			return Expression.MakeSimpleBinary(ExpressionType.Power, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise AND operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.And" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The bitwise AND operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression And(Expression left, Expression right)
		{
			return Expression.And(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise AND operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.And" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the bitwise AND operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression And(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryBitwiseCoreCheck("op_BitwiseAnd", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.And, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise OR operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Or" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The bitwise OR operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Or(Expression left, Expression right)
		{
			return Expression.Or(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise OR operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Or" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the bitwise OR operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Or(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryBitwiseCoreCheck("op_BitwiseOr", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.Or, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise XOR operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOr" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The XOR operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression ExclusiveOr(Expression left, Expression right)
		{
			return Expression.ExclusiveOr(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise XOR operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ExclusiveOr" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the XOR operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression ExclusiveOr(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryBitwiseCoreCheck("op_ExclusiveOr", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.ExclusiveOr, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise left-shift operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The left-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression LeftShift(Expression left, Expression right)
		{
			return Expression.LeftShift(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise left-shift operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LeftShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the left-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression LeftShift(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryBitwiseCoreCheck("op_LeftShift", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.LeftShift, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise right-shift operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The right-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression RightShift(Expression left, Expression right)
		{
			return Expression.RightShift(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a bitwise right-shift operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.RightShift" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the right-shift operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression RightShift(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_RightShift", left, right, method);
			return Expression.MakeSimpleBinary(ExpressionType.RightShift, left, right, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a conditional AND operation that evaluates the second operand only if it has to.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAlso" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The bitwise AND operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-<paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
		public static BinaryExpression AndAlso(Expression left, Expression right)
		{
			return Expression.AndAlso(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a conditional AND operation that evaluates the second operand only if it has to. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.AndAlso" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the bitwise AND operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-<paramref name="method" /> is null and <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
		public static BinaryExpression AndAlso(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.ConditionalBinaryCheck("op_BitwiseAnd", left, right, method);
			return Expression.MakeBoolBinary(ExpressionType.AndAlso, left, right, true, method);
		}

		private static MethodInfo ConditionalBinaryCheck(string oper, Expression left, Expression right, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck(oper, left, right, method);
			if (method == null)
			{
				if (left.Type.GetNotNullableType() != typeof(bool))
				{
					throw new InvalidOperationException("Only booleans are allowed");
				}
			}
			else
			{
				Type notNullableType = left.Type.GetNotNullableType();
				if (left.Type != right.Type || method.ReturnType != notNullableType)
				{
					throw new ArgumentException("left, right and return type must match");
				}
				MethodInfo trueOperator = Expression.GetTrueOperator(notNullableType);
				MethodInfo falseOperator = Expression.GetFalseOperator(notNullableType);
				if (trueOperator == null || falseOperator == null)
				{
					throw new ArgumentException("Operators true and false are required but not defined");
				}
			}
			return method;
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a conditional OR operation that evaluates the second operand only if it has to.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrElse" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The bitwise OR operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-<paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
		public static BinaryExpression OrElse(Expression left, Expression right)
		{
			return Expression.OrElse(left, right, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a conditional OR operation that evaluates the second operand only if it has to. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.OrElse" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the bitwise OR operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.-or-<paramref name="method" /> is null and <paramref name="left" />.Type and <paramref name="right" />.Type are not the same Boolean type.</exception>
		public static BinaryExpression OrElse(Expression left, Expression right, MethodInfo method)
		{
			method = Expression.ConditionalBinaryCheck("op_BitwiseOr", left, right, method);
			return Expression.MakeBoolBinary(ExpressionType.OrElse, left, right, true, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an equality comparison.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The equality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Equal(Expression left, Expression right)
		{
			return Expression.Equal(left, right, false, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an equality comparison. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Equal" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="liftToNull">true to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to true; false to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to false.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the equality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression Equal(Expression left, Expression right, bool liftToNull, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Equality", left, right, method);
			return Expression.MakeBoolBinary(ExpressionType.Equal, left, right, liftToNull, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an inequality comparison.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The inequality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression NotEqual(Expression left, Expression right)
		{
			return Expression.NotEqual(left, right, false, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents an inequality comparison. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NotEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="liftToNull">true to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to true; false to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to false.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the inequality operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression NotEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_Inequality", left, right, method);
			return Expression.MakeBoolBinary(ExpressionType.NotEqual, left, right, liftToNull, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "greater than" numeric comparison.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The "greater than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression GreaterThan(Expression left, Expression right)
		{
			return Expression.GreaterThan(left, right, false, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "greater than" numeric comparison. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="liftToNull">true to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to true; false to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to false.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the "greater than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression GreaterThan(Expression left, Expression right, bool liftToNull, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_GreaterThan", left, right, method);
			return Expression.MakeBoolBinary(ExpressionType.GreaterThan, left, right, liftToNull, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "greater than or equal" numeric comparison.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The "greater than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right)
		{
			return Expression.GreaterThanOrEqual(left, right, false, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "greater than or equal" numeric comparison. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.GreaterThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="liftToNull">true to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to true; false to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to false.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the "greater than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression GreaterThanOrEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_GreaterThanOrEqual", left, right, method);
			return Expression.MakeBoolBinary(ExpressionType.GreaterThanOrEqual, left, right, liftToNull, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "less than" numeric comparison.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The "less than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression LessThan(Expression left, Expression right)
		{
			return Expression.LessThan(left, right, false, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a "less than" numeric comparison. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThan" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="liftToNull">true to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to true; false to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to false.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the "less than" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression LessThan(Expression left, Expression right, bool liftToNull, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_LessThan", left, right, method);
			return Expression.MakeBoolBinary(ExpressionType.LessThan, left, right, liftToNull, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a " less than or equal" numeric comparison.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The "less than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression LessThanOrEqual(Expression left, Expression right)
		{
			return Expression.LessThanOrEqual(left, right, false, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a " less than or equal" numeric comparison. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.LessThanOrEqual" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" />, <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" />, and <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="liftToNull">true to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to true; false to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to false.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly two arguments.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the "less than or equal" operator is not defined for <paramref name="left" />.Type and <paramref name="right" />.Type.</exception>
		public static BinaryExpression LessThanOrEqual(Expression left, Expression right, bool liftToNull, MethodInfo method)
		{
			method = Expression.BinaryCoreCheck("op_LessThanOrEqual", left, right, method);
			return Expression.MakeBoolBinary(ExpressionType.LessThanOrEqual, left, right, liftToNull, method);
		}

		private static void CheckArray(Expression array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (!array.Type.IsArray)
			{
				throw new ArgumentException("The array argument must be of type array");
			}
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents applying an array index operator to an array of rank one.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ArrayIndex" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="array">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="index">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> or <paramref name="index" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" />.Type does not represent an array type.-or-<paramref name="array" />.Type represents an array type whose rank is not 1.-or-<paramref name="index" />.Type does not represent the <see cref="T:System.Int32" /> type.</exception>
		public static BinaryExpression ArrayIndex(Expression array, Expression index)
		{
			Expression.CheckArray(array);
			if (index == null)
			{
				throw new ArgumentNullException("index");
			}
			if (array.Type.GetArrayRank() != 1)
			{
				throw new ArgumentException("The array argument must be a single dimensional array");
			}
			if (index.Type != typeof(int))
			{
				throw new ArgumentException("The index must be of type int");
			}
			return new BinaryExpression(ExpressionType.ArrayIndex, array.Type.GetElementType(), array, index);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a coalescing operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of <paramref name="left" /> does not represent a reference type or a nullable value type.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="left" />.Type and <paramref name="right" />.Type are not convertible to each other.</exception>
		public static BinaryExpression Coalesce(Expression left, Expression right)
		{
			return Expression.Coalesce(left, right, null);
		}

		private static BinaryExpression MakeCoalesce(Expression left, Expression right)
		{
			Type type = null;
			if (left.Type.IsNullable())
			{
				Type notNullableType = left.Type.GetNotNullableType();
				if (!right.Type.IsNullable() && right.Type.IsAssignableTo(notNullableType))
				{
					type = notNullableType;
				}
			}
			if (type == null && right.Type.IsAssignableTo(left.Type))
			{
				type = left.Type;
			}
			if (type == null && left.Type.IsNullable() && left.Type.GetNotNullableType().IsAssignableTo(right.Type))
			{
				type = right.Type;
			}
			if (type == null)
			{
				throw new ArgumentException("Incompatible argument types");
			}
			return new BinaryExpression(ExpressionType.Coalesce, type, left, right, false, false, null, null);
		}

		private static BinaryExpression MakeConvertedCoalesce(Expression left, Expression right, LambdaExpression conversion)
		{
			MethodInfo invokeMethod = conversion.Type.GetInvokeMethod();
			Expression.CheckNotVoid(invokeMethod.ReturnType);
			if (invokeMethod.ReturnType != right.Type)
			{
				throw new InvalidOperationException("Conversion return type doesn't march right type");
			}
			ParameterInfo[] parameters = invokeMethod.GetParameters();
			if (parameters.Length != 1)
			{
				throw new ArgumentException("Conversion has wrong number of parameters");
			}
			if (!Expression.IsAssignableToParameterType(left.Type, parameters[0]))
			{
				throw new InvalidOperationException("Conversion argument doesn't marcht left type");
			}
			return new BinaryExpression(ExpressionType.Coalesce, right.Type, left, right, false, false, null, conversion);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" /> that represents a coalescing operation, given a conversion function.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.BinaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" /> and the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" />, <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> and <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> properties set to the specified values.</returns>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Left" /> property equal to.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Right" /> property equal to.</param>
		/// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> to set the <see cref="P:System.Linq.Expressions.BinaryExpression.Conversion" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="left" />.Type and <paramref name="right" />.Type are not convertible to each other.-or-<paramref name="conversion" /> is not null and <paramref name="conversion" />.Type is a delegate type that does not take exactly one argument.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of <paramref name="left" /> does not represent a reference type or a nullable value type.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of <paramref name="left" /> represents a type that is not assignable to the parameter type of the delegate type <paramref name="conversion" />.Type.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of <paramref name="right" /> is not equal to the return type of the delegate type <paramref name="conversion" />.Type.</exception>
		public static BinaryExpression Coalesce(Expression left, Expression right, LambdaExpression conversion)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			if (left.Type.IsValueType && !left.Type.IsNullable())
			{
				throw new InvalidOperationException("Left expression can never be null");
			}
			if (conversion != null)
			{
				return Expression.MakeConvertedCoalesce(left, right, conversion);
			}
			return Expression.MakeCoalesce(left, right);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" />, given the left and right operands, by calling an appropriate factory method.</summary>
		/// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpression" /> that results from calling the appropriate factory method.</returns>
		/// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the left operand.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the right operand.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right)
		{
			return Expression.MakeBinary(binaryType, left, right, false, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" />, given the left operand, right operand and implementing method, by calling the appropriate factory method.</summary>
		/// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpression" /> that results from calling the appropriate factory method.</returns>
		/// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the left operand.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the right operand.</param>
		/// <param name="liftToNull">true to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to true; false to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to false.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that specifies the implementing method.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo method)
		{
			return Expression.MakeBinary(binaryType, left, right, liftToNull, method, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.BinaryExpression" />, given the left operand, right operand, implementing method and type conversion function, by calling the appropriate factory method.</summary>
		/// <returns>The <see cref="T:System.Linq.Expressions.BinaryExpression" /> that results from calling the appropriate factory method.</returns>
		/// <param name="binaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of binary operation.</param>
		/// <param name="left">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the left operand.</param>
		/// <param name="right">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the right operand.</param>
		/// <param name="liftToNull">true to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to true; false to set <see cref="P:System.Linq.Expressions.BinaryExpression.IsLiftedToNull" /> to false.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that specifies the implementing method.</param>
		/// <param name="conversion">A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that represents a type conversion function. This parameter is used only if <paramref name="binaryType" /> is <see cref="F:System.Linq.Expressions.ExpressionType.Coalesce" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="binaryType" /> does not correspond to a binary expression node.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="left" /> or <paramref name="right" /> is null.</exception>
		public static BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo method, LambdaExpression conversion)
		{
			switch (binaryType)
			{
			case ExpressionType.Add:
				return Expression.Add(left, right, method);
			case ExpressionType.AddChecked:
				return Expression.AddChecked(left, right, method);
			case ExpressionType.And:
				return Expression.And(left, right, method);
			case ExpressionType.AndAlso:
				return Expression.AndAlso(left, right);
			case ExpressionType.Coalesce:
				return Expression.Coalesce(left, right, conversion);
			case ExpressionType.Divide:
				return Expression.Divide(left, right, method);
			case ExpressionType.Equal:
				return Expression.Equal(left, right, liftToNull, method);
			case ExpressionType.ExclusiveOr:
				return Expression.ExclusiveOr(left, right, method);
			case ExpressionType.GreaterThan:
				return Expression.GreaterThan(left, right, liftToNull, method);
			case ExpressionType.GreaterThanOrEqual:
				return Expression.GreaterThanOrEqual(left, right, liftToNull, method);
			case ExpressionType.LeftShift:
				return Expression.LeftShift(left, right, method);
			case ExpressionType.LessThan:
				return Expression.LessThan(left, right, liftToNull, method);
			case ExpressionType.LessThanOrEqual:
				return Expression.LessThanOrEqual(left, right, liftToNull, method);
			case ExpressionType.Modulo:
				return Expression.Modulo(left, right, method);
			case ExpressionType.Multiply:
				return Expression.Multiply(left, right, method);
			case ExpressionType.MultiplyChecked:
				return Expression.MultiplyChecked(left, right, method);
			case ExpressionType.NotEqual:
				return Expression.NotEqual(left, right, liftToNull, method);
			case ExpressionType.Or:
				return Expression.Or(left, right, method);
			case ExpressionType.OrElse:
				return Expression.OrElse(left, right);
			case ExpressionType.Power:
				return Expression.Power(left, right, method);
			case ExpressionType.RightShift:
				return Expression.RightShift(left, right, method);
			case ExpressionType.Subtract:
				return Expression.Subtract(left, right, method);
			case ExpressionType.SubtractChecked:
				return Expression.SubtractChecked(left, right, method);
			}
			throw new ArgumentException("MakeBinary expect a binary node type");
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents applying an array index operator to an array of rank more than one.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="array">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to.</param>
		/// <param name="indexes">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> or <paramref name="indexes" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" />.Type does not represent an array type.-or-The rank of <paramref name="array" />.Type does not match the number of elements in <paramref name="indexes" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="indexes" /> does not represent the <see cref="T:System.Int32" /> type.</exception>
		public static MethodCallExpression ArrayIndex(Expression array, params Expression[] indexes)
		{
			return Expression.ArrayIndex(array, indexes);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents applying an array index operator to an array of rank more than one.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="array">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to.</param>
		/// <param name="indexes">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> or <paramref name="indexes" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" />.Type does not represent an array type.-or-The rank of <paramref name="array" />.Type does not match the number of elements in <paramref name="indexes" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="indexes" /> does not represent the <see cref="T:System.Int32" /> type.</exception>
		public static MethodCallExpression ArrayIndex(Expression array, IEnumerable<Expression> indexes)
		{
			Expression.CheckArray(array);
			if (indexes == null)
			{
				throw new ArgumentNullException("indexes");
			}
			ReadOnlyCollection<Expression> readOnlyCollection = indexes.ToReadOnlyCollection<Expression>();
			if (array.Type.GetArrayRank() != readOnlyCollection.Count)
			{
				throw new ArgumentException("The number of arguments doesn't match the rank of the array");
			}
			foreach (Expression expression in readOnlyCollection)
			{
				if (expression.Type != typeof(int))
				{
					throw new ArgumentException("The index must be of type int");
				}
			}
			return Expression.Call(array, array.Type.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy), readOnlyCollection);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents getting the length of a one-dimensional array.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ArrayLength" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to <paramref name="array" />.</returns>
		/// <param name="array">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" />.Type does not represent an array type.</exception>
		public static UnaryExpression ArrayLength(Expression array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (!array.Type.IsArray)
			{
				throw new ArgumentException("The type of the expression must me Array");
			}
			if (array.Type.GetArrayRank() != 1)
			{
				throw new ArgumentException("The array must be a single dimensional array");
			}
			return new UnaryExpression(ExpressionType.ArrayLength, array, typeof(int));
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberAssignment" /> that represents the initialization of a field or property.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberAssignment" /> that has <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> equal to <see cref="F:System.Linq.Expressions.MemberBindingType.Assignment" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberAssignment.Expression" /> properties set to the specified values.</returns>
		/// <param name="member">A <see cref="T:System.Reflection.MemberInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberAssignment.Expression" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="member" /> or <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="member" /> does not represent a field or property.-or-The property represented by <paramref name="member" /> does not have a set accessor.-or-<paramref name="expression" />.Type is not assignable to the type of the field or property that <paramref name="member" /> represents.</exception>
		public static MemberAssignment Bind(MemberInfo member, Expression expression)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			Type type = null;
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null && propertyInfo.GetSetMethod(true) != null)
			{
				type = propertyInfo.PropertyType;
			}
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				type = fieldInfo.FieldType;
			}
			if (type == null)
			{
				throw new ArgumentException("member");
			}
			if (!expression.Type.IsAssignableTo(type))
			{
				throw new ArgumentException("member");
			}
			return new MemberAssignment(member, expression);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberAssignment" /> that represents the initialization of a member by using a property accessor method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberAssignment" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.Assignment" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and the <see cref="P:System.Linq.Expressions.MemberAssignment.Expression" /> property set to <paramref name="expression" />.</returns>
		/// <param name="propertyAccessor">A <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberAssignment.Expression" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="propertyAccessor" /> or <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The property accessed by <paramref name="propertyAccessor" /> does not have a set accessor.-or-<paramref name="expression" />.Type is not assignable to the type of the field or property that <paramref name="member" /> represents.</exception>
		public static MemberAssignment Bind(MethodInfo propertyAccessor, Expression expression)
		{
			if (propertyAccessor == null)
			{
				throw new ArgumentNullException("propertyAccessor");
			}
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			Expression.CheckNonGenericMethod(propertyAccessor);
			PropertyInfo associatedProperty = Expression.GetAssociatedProperty(propertyAccessor);
			if (associatedProperty == null)
			{
				throw new ArgumentException("propertyAccessor");
			}
			if (associatedProperty.GetSetMethod(true) == null)
			{
				throw new ArgumentException("setter");
			}
			if (!expression.Type.IsAssignableTo(associatedProperty.PropertyType))
			{
				throw new ArgumentException("member");
			}
			return new MemberAssignment(associatedProperty, expression);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method that takes no arguments.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to (pass null for a static (Shared in Visual Basic) method).</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="method" /> is null.-or-<paramref name="instance" /> is null and <paramref name="method" /> represents an instance method.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.</exception>
		public static MethodCallExpression Call(Expression instance, MethodInfo method)
		{
			return Expression.Call(instance, method, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a static (Shared in Visual Basic) method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents a static (Shared in Visual Basic) method to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
		/// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="method" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
		public static MethodCallExpression Call(MethodInfo method, params Expression[] arguments)
		{
			return Expression.Call(null, method, arguments);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method that takes arguments.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" />, <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" />, and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to (pass null for a static (Shared in Visual Basic) method).</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
		/// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="method" /> is null.-or-<paramref name="instance" /> is null and <paramref name="method" /> represents an instance method.-or-<paramref name="arguments" /> is not null and one or more of its elements is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.-or-The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
		public static MethodCallExpression Call(Expression instance, MethodInfo method, params Expression[] arguments)
		{
			return Expression.Call(instance, method, arguments);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a method that takes arguments.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" /> and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" />, <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" />, and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to (pass null for a static (Shared in Visual Basic) method).</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property equal to.</param>
		/// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="method" /> is null.-or-<paramref name="instance" /> is null and <paramref name="method" /> represents an instance method.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="instance" />.Type is not assignable to the declaring type of the method represented by <paramref name="method" />.-or-The number of elements in <paramref name="arguments" /> does not equal the number of parameters for the method represented by <paramref name="method" />.-or-One or more of the elements of <paramref name="arguments" /> is not assignable to the corresponding parameter for the method represented by <paramref name="method" />.</exception>
		public static MethodCallExpression Call(Expression instance, MethodInfo method, IEnumerable<Expression> arguments)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (instance == null && !method.IsStatic)
			{
				throw new ArgumentNullException("instance");
			}
			if (method.IsStatic && instance != null)
			{
				throw new ArgumentException("instance");
			}
			if (!method.IsStatic && !instance.Type.IsAssignableTo(method.DeclaringType))
			{
				throw new ArgumentException("Type is not assignable to the declaring type of the method");
			}
			ReadOnlyCollection<Expression> arguments2 = Expression.CheckMethodArguments(method, arguments);
			return new MethodCallExpression(instance, method, arguments2);
		}

		private static Type[] CollectTypes(IEnumerable<Expression> expressions)
		{
			return expressions.Select((Expression arg) => arg.Type).ToArray<Type>();
		}

		private static MethodInfo TryMakeGeneric(MethodInfo method, Type[] args)
		{
			if (method == null)
			{
				return null;
			}
			if (!method.IsGenericMethod && (args == null || args.Length == 0))
			{
				return method;
			}
			if (args.Length == method.GetGenericArguments().Length)
			{
				return method.MakeGenericMethod(args);
			}
			return null;
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to an instance method by calling the appropriate factory method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" />, the <see cref="P:System.Linq.Expressions.MethodCallExpression.Object" /> property equal to <paramref name="instance" />, <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> set to the <see cref="T:System.Reflection.MethodInfo" /> that represents the specified instance method, and <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> set to the specified arguments.</returns>
		/// <param name="instance">An <see cref="T:System.Linq.Expressions.Expression" /> whose <see cref="P:System.Linq.Expressions.Expression.Type" /> property value will be searched for a specific method.</param>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="typeArguments">An array of <see cref="T:System.Type" /> objects that specify the type parameters of the method.</param>
		/// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects that represents the arguments to the method.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="instance" /> or <paramref name="methodName" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No method whose name is <paramref name="methodName" />, whose type parameters match <paramref name="typeArguments" />, and whose parameter types match <paramref name="arguments" /> is found in <paramref name="instance" />.Type or its base types.-or-More than one method whose name is <paramref name="methodName" />, whose type parameters match <paramref name="typeArguments" />, and whose parameter types match <paramref name="arguments" /> is found in <paramref name="instance" />.Type or its base types.</exception>
		public static MethodCallExpression Call(Expression instance, string methodName, Type[] typeArguments, params Expression[] arguments)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			MethodInfo method = Expression.TryGetMethod(instance.Type, methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, Expression.CollectTypes(arguments), typeArguments);
			ReadOnlyCollection<Expression> arguments2 = Expression.CheckMethodArguments(method, arguments);
			return new MethodCallExpression(instance, method, arguments2);
		}

		private static bool MethodMatch(MethodInfo method, string name, Type[] parameterTypes, Type[] argumentTypes)
		{
			if (method.Name != name)
			{
				return false;
			}
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != parameterTypes.Length)
			{
				return false;
			}
			if (method.IsGenericMethod && method.IsGenericMethodDefinition)
			{
				MethodInfo methodInfo = Expression.TryMakeGeneric(method, argumentTypes);
				return methodInfo != null && Expression.MethodMatch(methodInfo, name, parameterTypes, argumentTypes);
			}
			if (!method.IsGenericMethod && argumentTypes != null && argumentTypes.Length > 0)
			{
				return false;
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				Type type = parameterTypes[i];
				ParameterInfo parameterInfo = parameters[i];
				if (!Expression.IsAssignableToParameterType(type, parameterInfo) && !Expression.IsExpressionOfParameter(type, parameterInfo.ParameterType))
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsExpressionOfParameter(Type type, Type ptype)
		{
			return ptype.IsGenericInstanceOf(typeof(Expression<>)) && ptype.GetFirstGenericArgument() == type;
		}

		private static MethodInfo TryGetMethod(Type type, string methodName, BindingFlags flags, Type[] parameterTypes, Type[] argumentTypes)
		{
			IEnumerable<MethodInfo> source = type.GetMethods(flags).Where((MethodInfo meth) => Expression.MethodMatch(meth, methodName, parameterTypes, argumentTypes));
			if (source.Count<MethodInfo>() > 1)
			{
				throw new InvalidOperationException("Too many method candidates");
			}
			MethodInfo methodInfo = Expression.TryMakeGeneric(source.FirstOrDefault<MethodInfo>(), argumentTypes);
			if (methodInfo != null)
			{
				return methodInfo;
			}
			throw new InvalidOperationException("No such method");
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that represents a call to a static (Shared in Visual Basic) method by calling the appropriate factory method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MethodCallExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Call" />, the <see cref="P:System.Linq.Expressions.MethodCallExpression.Method" /> property set to the <see cref="T:System.Reflection.MethodInfo" /> that represents the specified static (Shared in Visual Basic) method, and the <see cref="P:System.Linq.Expressions.MethodCallExpression.Arguments" /> property set to the specified arguments.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> that specifies the type that contains the specified static (Shared in Visual Basic) method.</param>
		/// <param name="methodName">The name of the method.</param>
		/// <param name="typeArguments">An array of <see cref="T:System.Type" /> objects that specify the type parameters of the method.</param>
		/// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects that represent the arguments to the method.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="methodName" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No method whose name is <paramref name="methodName" />, whose type parameters match <paramref name="typeArguments" />, and whose parameter types match <paramref name="arguments" /> is found in <paramref name="type" /> or its base types.-or-More than one method whose name is <paramref name="methodName" />, whose type parameters match <paramref name="typeArguments" />, and whose parameter types match <paramref name="arguments" /> is found in <paramref name="type" /> or its base types.</exception>
		public static MethodCallExpression Call(Type type, string methodName, Type[] typeArguments, params Expression[] arguments)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			MethodInfo method = Expression.TryGetMethod(type, methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, Expression.CollectTypes(arguments), typeArguments);
			ReadOnlyCollection<Expression> arguments2 = Expression.CheckMethodArguments(method, arguments);
			return new MethodCallExpression(method, arguments2);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ConditionalExpression" />.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ConditionalExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Conditional" /> and the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" />, <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" />, and <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> properties set to the specified values.</returns>
		/// <param name="test">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.Test" /> property equal to.</param>
		/// <param name="ifTrue">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfTrue" /> property equal to.</param>
		/// <param name="ifFalse">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.ConditionalExpression.IfFalse" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="test" /> or <paramref name="ifTrue" /> or <paramref name="ifFalse" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="test" />.Type is not <see cref="T:System.Boolean" />.-or-<paramref name="ifTrue" />.Type is not equal to <paramref name="ifFalse" />.Type.</exception>
		public static ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse)
		{
			if (test == null)
			{
				throw new ArgumentNullException("test");
			}
			if (ifTrue == null)
			{
				throw new ArgumentNullException("ifTrue");
			}
			if (ifFalse == null)
			{
				throw new ArgumentNullException("ifFalse");
			}
			if (test.Type != typeof(bool))
			{
				throw new ArgumentException("Test expression should be of type bool");
			}
			if (ifTrue.Type != ifFalse.Type)
			{
				throw new ArgumentException("The ifTrue and ifFalse type do not match");
			}
			return new ConditionalExpression(test, ifTrue, ifFalse);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ConstantExpression" /> that has the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> property set to the specified value.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ConstantExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Constant" /> and the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> property set to the specified value.</returns>
		/// <param name="value">An <see cref="T:System.Object" /> to set the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> property equal to.</param>
		public static ConstantExpression Constant(object value)
		{
			if (value == null)
			{
				return new ConstantExpression(null, typeof(object));
			}
			return Expression.Constant(value, value.GetType());
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ConstantExpression" /> that has the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ConstantExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Constant" /> and the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</returns>
		/// <param name="value">An <see cref="T:System.Object" /> to set the <see cref="P:System.Linq.Expressions.ConstantExpression.Value" /> property equal to.</param>
		/// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not null and <paramref name="type" /> is not assignable from the dynamic type of <paramref name="value" />.</exception>
		public static ConstantExpression Constant(object value, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (value == null)
			{
				if (type.IsValueType && !type.IsNullable())
				{
					throw new ArgumentException();
				}
			}
			else if ((!type.IsValueType || !type.IsNullable()) && !value.GetType().IsAssignableTo(type))
			{
				throw new ArgumentException();
			}
			return new ConstantExpression(value, type);
		}

		private static bool IsConvertiblePrimitive(Type type)
		{
			Type notNullableType = type.GetNotNullableType();
			return notNullableType != typeof(bool) && (notNullableType.IsEnum || notNullableType.IsPrimitive);
		}

		internal static bool IsPrimitiveConversion(Type type, Type target)
		{
			return type == target || (type.IsNullable() && target == type.GetNotNullableType()) || (target.IsNullable() && type == target.GetNotNullableType()) || (Expression.IsConvertiblePrimitive(type) && Expression.IsConvertiblePrimitive(target));
		}

		internal static bool IsReferenceConversion(Type type, Type target)
		{
			return type == target || (type == typeof(object) || target == typeof(object)) || (type.IsInterface || target.IsInterface) || (!type.IsValueType && !target.IsValueType && (type.IsAssignableTo(target) || target.IsAssignableTo(type)));
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a conversion operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Convert" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="type" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.</exception>
		public static UnaryExpression Convert(Expression expression, Type type)
		{
			return Expression.Convert(expression, type, null);
		}

		private static MethodInfo GetUserConversionMethod(Type type, Type target)
		{
			MethodInfo unaryOperator = Expression.GetUnaryOperator("op_Explicit", type, type, target);
			if (unaryOperator == null)
			{
				unaryOperator = Expression.GetUnaryOperator("op_Implicit", type, type, target);
			}
			if (unaryOperator == null)
			{
				unaryOperator = Expression.GetUnaryOperator("op_Explicit", target, type, target);
			}
			if (unaryOperator == null)
			{
				unaryOperator = Expression.GetUnaryOperator("op_Implicit", target, type, target);
			}
			if (unaryOperator == null)
			{
				throw new InvalidOperationException();
			}
			return unaryOperator;
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a conversion operation for which the implementing method is specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Convert" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" />, <see cref="P:System.Linq.Expressions.Expression.Type" />, and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="type" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly one argument.</exception>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one method that matches the <paramref name="method" /> description was found.</exception>
		/// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.-or-<paramref name="expression" />.Type is not assignable to the argument type of the method represented by <paramref name="method" />.-or-The return type of the method represented by <paramref name="method" /> is not assignable to <paramref name="type" />.-or-<paramref name="expression" />.Type or <paramref name="type" /> is a nullable value type and the corresponding non-nullable value type does not equal the argument type or the return type, respectively, of the method represented by <paramref name="method" />.</exception>
		public static UnaryExpression Convert(Expression expression, Type type, MethodInfo method)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Type param = expression.Type;
			if (method != null)
			{
				Expression.CheckUnaryMethod(method, param);
			}
			else if (!Expression.IsPrimitiveConversion(param, type) && !Expression.IsReferenceConversion(param, type))
			{
				method = Expression.GetUserConversionMethod(param, type);
			}
			return new UnaryExpression(ExpressionType.Convert, expression, type, method, Expression.IsConvertNodeLifted(method, expression, type));
		}

		private static bool IsConvertNodeLifted(MethodInfo method, Expression operand, Type target)
		{
			if (method == null)
			{
				return operand.Type.IsNullable() || target.IsNullable();
			}
			return (operand.Type.IsNullable() && !Expression.ParameterMatch(method, operand.Type)) || (target.IsNullable() && !Expression.ReturnTypeMatch(method, target));
		}

		private static bool ParameterMatch(MethodInfo method, Type type)
		{
			return method.GetParameters()[0].ParameterType == type;
		}

		private static bool ReturnTypeMatch(MethodInfo method, Type type)
		{
			return method.ReturnType == type;
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a conversion operation that throws an exception if the target type is overflowed.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ConvertChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="type" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.</exception>
		public static UnaryExpression ConvertChecked(Expression expression, Type type)
		{
			return Expression.ConvertChecked(expression, type, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a conversion operation that throws an exception if the target type is overflowed and for which the implementing method is specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ConvertChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" />, <see cref="P:System.Linq.Expressions.Expression.Type" />, and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="type" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly one argument.</exception>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one method that matches the <paramref name="method" /> description was found.</exception>
		/// <exception cref="T:System.InvalidOperationException">No conversion operator is defined between <paramref name="expression" />.Type and <paramref name="type" />.-or-<paramref name="expression" />.Type is not assignable to the argument type of the method represented by <paramref name="method" />.-or-The return type of the method represented by <paramref name="method" /> is not assignable to <paramref name="type" />.-or-<paramref name="expression" />.Type or <paramref name="type" /> is a nullable value type and the corresponding non-nullable value type does not equal the argument type or the return type, respectively, of the method represented by <paramref name="method" />.</exception>
		public static UnaryExpression ConvertChecked(Expression expression, Type type, MethodInfo method)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Type param = expression.Type;
			if (method != null)
			{
				Expression.CheckUnaryMethod(method, param);
			}
			else
			{
				if (Expression.IsReferenceConversion(param, type))
				{
					return Expression.Convert(expression, type, method);
				}
				if (!Expression.IsPrimitiveConversion(param, type))
				{
					method = Expression.GetUserConversionMethod(param, type);
				}
			}
			return new UnaryExpression(ExpressionType.ConvertChecked, expression, type, method, Expression.IsConvertNodeLifted(method, expression, type));
		}

		/// <summary>Creates an <see cref="T:System.Linq.Expressions.ElementInit" />, given an array of values as the second argument.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.ElementInit" /> that has the <see cref="P:System.Linq.Expressions.ElementInit.AddMethod" /> and <see cref="P:System.Linq.Expressions.ElementInit.Arguments" /> properties set to the specified values.</returns>
		/// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.ElementInit.AddMethod" /> property equal to.</param>
		/// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to set the <see cref="P:System.Linq.Expressions.ElementInit.Arguments" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="addMethod" /> or <paramref name="arguments" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The method that addMethod represents is not named "Add" (case insensitive).-or-The method that addMethod represents is not an instance method.-or-arguments does not contain the same number of elements as the number of parameters for the method that addMethod represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the method that <paramref name="addMethod" /> represents.</exception>
		public static ElementInit ElementInit(MethodInfo addMethod, params Expression[] arguments)
		{
			return Expression.ElementInit(addMethod, arguments);
		}

		/// <summary>Creates an <see cref="T:System.Linq.Expressions.ElementInit" />, given an <see cref="T:System.Collections.Generic.IEnumerable`1" /> as the second argument.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.ElementInit" /> that has the <see cref="P:System.Linq.Expressions.ElementInit.AddMethod" /> and <see cref="P:System.Linq.Expressions.ElementInit.Arguments" /> properties set to the specified values.</returns>
		/// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.ElementInit.AddMethod" /> property equal to.</param>
		/// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to set the <see cref="P:System.Linq.Expressions.ElementInit.Arguments" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="addMethod" /> or <paramref name="arguments" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The method that <paramref name="addMethod" /> represents is not named "Add" (case insensitive).-or-The method that <paramref name="addMethod" /> represents is not an instance method.-or-<paramref name="arguments" /> does not contain the same number of elements as the number of parameters for the method that <paramref name="addMethod" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the method that <paramref name="addMethod" /> represents.</exception>
		public static ElementInit ElementInit(MethodInfo addMethod, IEnumerable<Expression> arguments)
		{
			if (addMethod == null)
			{
				throw new ArgumentNullException("addMethod");
			}
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			if (addMethod.Name.ToLower(CultureInfo.InvariantCulture) != "add")
			{
				throw new ArgumentException("addMethod");
			}
			if (addMethod.IsStatic)
			{
				throw new ArgumentException("addMethod must be an instance method", "addMethod");
			}
			ReadOnlyCollection<Expression> arguments2 = Expression.CheckMethodArguments(addMethod, arguments);
			return new ElementInit(addMethod, arguments2);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a field.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" /> and the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> and <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property equal to.</param>
		/// <param name="field">The <see cref="T:System.Reflection.FieldInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="field" /> is null.-or-The field represented by <paramref name="field" /> is not static (Shared in Visual Basic) and <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="expression" />.Type is not assignable to the declaring type of the field represented by <paramref name="field" />.</exception>
		public static MemberExpression Field(Expression expression, FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			if (!field.IsStatic)
			{
				if (expression == null)
				{
					throw new ArgumentNullException("expression");
				}
				if (!expression.Type.IsAssignableTo(field.DeclaringType))
				{
					throw new ArgumentException("field");
				}
			}
			else if (expression != null)
			{
				throw new ArgumentException("expression");
			}
			return new MemberExpression(expression, field, field.FieldType);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a field given the name of the field.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" />, the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property set to <paramref name="expression" />, and the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property set to the <see cref="T:System.Reflection.FieldInfo" /> that represents the field denoted by <paramref name="fieldName" />.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> whose <see cref="P:System.Linq.Expressions.Expression.Type" /> contains a field named <paramref name="fieldName" />.</param>
		/// <param name="fieldName">The name of a field.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="fieldName" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">No field named <paramref name="fieldName" /> is defined in <paramref name="expression" />.Type or its base types.</exception>
		public static MemberExpression Field(Expression expression, string fieldName)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			FieldInfo field = expression.Type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			if (field == null)
			{
				throw new ArgumentException(string.Format("No field named {0} on {1}", fieldName, expression.Type));
			}
			return new MemberExpression(expression, field, field.FieldType);
		}

		/// <summary>Creates a <see cref="T:System.Type" /> object that represents a generic System.Action delegate type that has specific type arguments.</summary>
		/// <returns>The type of a System.Action delegate that has the specified type arguments.</returns>
		/// <param name="typeArgs">An array of zero to four <see cref="T:System.Type" /> objects that specify the type arguments for the System.Action delegate type.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="typeArgs" /> contains more than four elements.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeArgs" /> is null.</exception>
		public static Type GetActionType(params Type[] typeArgs)
		{
			if (typeArgs == null)
			{
				throw new ArgumentNullException("typeArgs");
			}
			if (typeArgs.Length > 4)
			{
				throw new ArgumentException("No Action type of this arity");
			}
			if (typeArgs.Length == 0)
			{
				return typeof(Action);
			}
			Type type = null;
			switch (typeArgs.Length)
			{
			case 1:
				type = typeof(Action<>);
				break;
			case 2:
				type = typeof(Action<, >);
				break;
			case 3:
				type = typeof(Action<, , >);
				break;
			case 4:
				type = typeof(Action<, , , >);
				break;
			}
			return type.MakeGenericType(typeArgs);
		}

		/// <summary>Creates a <see cref="T:System.Type" /> object that represents a generic System.Func delegate type that has specific type arguments.</summary>
		/// <returns>The type of a System.Func delegate that has the specified type arguments.</returns>
		/// <param name="typeArgs">An array of one to five <see cref="T:System.Type" /> objects that specify the type arguments for the System.Func delegate type.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="typeArgs" /> contains less than one or more than five elements.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeArgs" /> is null.</exception>
		public static Type GetFuncType(params Type[] typeArgs)
		{
			if (typeArgs == null)
			{
				throw new ArgumentNullException("typeArgs");
			}
			if (typeArgs.Length < 1 || typeArgs.Length > 5)
			{
				throw new ArgumentException("No Func type of this arity");
			}
			Type type = null;
			switch (typeArgs.Length)
			{
			case 1:
				type = typeof(Func<>);
				break;
			case 2:
				type = typeof(Func<, >);
				break;
			case 3:
				type = typeof(Func<, , >);
				break;
			case 4:
				type = typeof(Func<, , , >);
				break;
			case 5:
				type = typeof(Func<, , , , >);
				break;
			}
			return type.MakeGenericType(typeArgs);
		}

		/// <summary>Creates an <see cref="T:System.Linq.Expressions.InvocationExpression" />.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.InvocationExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Invoke" /> and the <see cref="P:System.Linq.Expressions.InvocationExpression.Expression" /> and <see cref="P:System.Linq.Expressions.InvocationExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.InvocationExpression.Expression" /> property equal to.</param>
		/// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.InvocationExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="expression" />.Type does not represent a delegate type or an <see cref="T:System.Linq.Expressions.Expression`1" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the delegate represented by <paramref name="expression" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="arguments" /> does not contain the same number of elements as the list of parameters for the delegate represented by <paramref name="expression" />.</exception>
		public static InvocationExpression Invoke(Expression expression, params Expression[] arguments)
		{
			return Expression.Invoke(expression, arguments);
		}

		private static Type GetInvokableType(Type t)
		{
			if (t.IsAssignableTo(typeof(Delegate)))
			{
				return t;
			}
			return Expression.GetGenericType(t, typeof(Expression<>));
		}

		private static Type GetGenericType(Type t, Type def)
		{
			if (t == null)
			{
				return null;
			}
			if (t.IsGenericType && t.GetGenericTypeDefinition() == def)
			{
				return t;
			}
			return Expression.GetGenericType(t.BaseType, def);
		}

		/// <summary>Creates an <see cref="T:System.Linq.Expressions.InvocationExpression" />.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.InvocationExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Invoke" /> and the <see cref="P:System.Linq.Expressions.InvocationExpression.Expression" /> and <see cref="P:System.Linq.Expressions.InvocationExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.InvocationExpression.Expression" /> property equal to.</param>
		/// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.InvocationExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="expression" />.Type does not represent a delegate type or an <see cref="T:System.Linq.Expressions.Expression`1" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the delegate represented by <paramref name="expression" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="arguments" /> does not contain the same number of elements as the list of parameters for the delegate represented by <paramref name="expression" />.</exception>
		public static InvocationExpression Invoke(Expression expression, IEnumerable<Expression> arguments)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			Type invokableType = Expression.GetInvokableType(expression.Type);
			if (invokableType == null)
			{
				throw new ArgumentException("The type of the expression is not invokable");
			}
			ReadOnlyCollection<Expression> readOnlyCollection = arguments.ToReadOnlyCollection<Expression>();
			Expression.CheckForNull<Expression>(readOnlyCollection, "arguments");
			MethodInfo invokeMethod = invokableType.GetInvokeMethod();
			if (invokeMethod == null)
			{
				throw new ArgumentException("expression");
			}
			if (invokeMethod.GetParameters().Length != readOnlyCollection.Count)
			{
				throw new InvalidOperationException("Arguments count doesn't match parameters length");
			}
			readOnlyCollection = Expression.CheckMethodArguments(invokeMethod, readOnlyCollection);
			return new InvocationExpression(expression, invokeMethod.ReturnType, readOnlyCollection);
		}

		private static bool CanAssign(Type target, Type source)
		{
			return !(target.IsValueType ^ source.IsValueType) && source.IsAssignableTo(target);
		}

		private static Expression CheckLambda(Type delegateType, Expression body, ReadOnlyCollection<ParameterExpression> parameters)
		{
			if (!delegateType.IsSubclassOf(typeof(Delegate)))
			{
				throw new ArgumentException("delegateType");
			}
			MethodInfo invokeMethod = delegateType.GetInvokeMethod();
			if (invokeMethod == null)
			{
				throw new ArgumentException("delegate must contain an Invoke method", "delegateType");
			}
			ParameterInfo[] parameters2 = invokeMethod.GetParameters();
			if (parameters2.Length != parameters.Count)
			{
				throw new ArgumentException(string.Format("Different number of arguments in delegate {0}", delegateType), "delegateType");
			}
			for (int i = 0; i < parameters2.Length; i++)
			{
				ParameterExpression parameterExpression = parameters[i];
				if (parameterExpression == null)
				{
					throw new ArgumentNullException("parameters");
				}
				if (!Expression.CanAssign(parameterExpression.Type, parameters2[i].ParameterType))
				{
					throw new ArgumentException(string.Format("Can not assign a {0} to a {1}", parameters2[i].ParameterType, parameterExpression.Type));
				}
			}
			if (invokeMethod.ReturnType == typeof(void) || Expression.CanAssign(invokeMethod.ReturnType, body.Type))
			{
				return body;
			}
			if (invokeMethod.ReturnType.IsExpression())
			{
				return Expression.Quote(body);
			}
			throw new ArgumentException(string.Format("body type {0} can not be assigned to {1}", body.Type, invokeMethod.ReturnType));
		}

		/// <summary>Creates an <see cref="T:System.Linq.Expressions.Expression`1" /> where the delegate type is known at compile time.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression`1" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
		/// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
		/// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
		/// <typeparam name="TDelegate">A delegate type.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="body" /> is null.-or-One or more elements in <paramref name="parameters" /> are null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="TDelegate" /> is not a delegate type.-or-<paramref name="body" />.Type represents a type that is not assignable to the return type of <paramref name="TDelegate" />.-or-<paramref name="parameters" /> does not contain the same number of elements as the list of parameters for <paramref name="TDelegate" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of <paramref name="TDelegate" />.</exception>
		public static Expression<TDelegate> Lambda<TDelegate>(Expression body, params ParameterExpression[] parameters)
		{
			return Expression.Lambda<TDelegate>(body, parameters);
		}

		/// <summary>Creates an <see cref="T:System.Linq.Expressions.Expression`1" /> where the delegate type is known at compile time.</summary>
		/// <returns>An <see cref="T:System.Linq.Expressions.Expression`1" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
		/// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
		/// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
		/// <typeparam name="TDelegate">A delegate type.</typeparam>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="body" /> is null.-or-One or more elements in <paramref name="parameters" /> are null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="TDelegate" /> is not a delegate type.-or-<paramref name="body" />.Type represents a type that is not assignable to the return type of <paramref name="TDelegate" />.-or-<paramref name="parameters" /> does not contain the same number of elements as the list of parameters for <paramref name="TDelegate" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of <paramref name="TDelegate" />.</exception>
		public static Expression<TDelegate> Lambda<TDelegate>(Expression body, IEnumerable<ParameterExpression> parameters)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			ReadOnlyCollection<ParameterExpression> parameters2 = parameters.ToReadOnlyCollection<ParameterExpression>();
			body = Expression.CheckLambda(typeof(TDelegate), body, parameters2);
			return new Expression<TDelegate>(body, parameters2);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpression" /> by first constructing a delegate type.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.LambdaExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
		/// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
		/// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="body" /> is null.-or-One or more elements of <paramref name="parameters" /> are null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="parameters" /> contains more than four elements.</exception>
		public static LambdaExpression Lambda(Expression body, params ParameterExpression[] parameters)
		{
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			if (parameters.Length > 4)
			{
				throw new ArgumentException("Too many parameters");
			}
			return Expression.Lambda(Expression.GetDelegateType(body.Type, parameters), body, parameters);
		}

		private static Type GetDelegateType(Type return_type, ParameterExpression[] parameters)
		{
			if (parameters == null)
			{
				parameters = new ParameterExpression[0];
			}
			if (return_type == typeof(void))
			{
				return Expression.GetActionType(parameters.Select((ParameterExpression p) => p.Type).ToArray<Type>());
			}
			Type[] array = new Type[parameters.Length + 1];
			for (int i = 0; i < array.Length - 1; i++)
			{
				array[i] = parameters[i].Type;
			}
			array[array.Length - 1] = return_type;
			return Expression.GetFuncType(array);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpression" /> and can be used when the delegate type is not known at compile time.</summary>
		/// <returns>An object that represents a lambda expression which has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
		/// <param name="delegateType">A <see cref="T:System.Type" /> that represents a delegate type.</param>
		/// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
		/// <param name="parameters">An array of <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="delegateType" /> or <paramref name="body" /> is null.-or-One or more elements in <paramref name="parameters" /> are null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="delegateType" /> does not represent a delegate type.-or-<paramref name="body" />.Type represents a type that is not assignable to the return type of the delegate type represented by <paramref name="delegateType" />.-or-<paramref name="parameters" /> does not contain the same number of elements as the list of parameters for the delegate type represented by <paramref name="delegateType" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of the delegate type represented by <paramref name="delegateType" />.</exception>
		public static LambdaExpression Lambda(Type delegateType, Expression body, params ParameterExpression[] parameters)
		{
			return Expression.Lambda(delegateType, body, parameters);
		}

		private static LambdaExpression CreateExpressionOf(Type type, Expression body, ReadOnlyCollection<ParameterExpression> parameters)
		{
			return (LambdaExpression)typeof(Expression<>).MakeGenericType(new Type[]
			{
				type
			}).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null, new Type[]
			{
				typeof(Expression),
				typeof(ReadOnlyCollection<ParameterExpression>)
			}, null).Invoke(new object[]
			{
				body,
				parameters
			});
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.LambdaExpression" /> and can be used when the delegate type is not known at compile time.</summary>
		/// <returns>An object that represents a lambda expression which has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Lambda" /> and the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> and <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> properties set to the specified values.</returns>
		/// <param name="delegateType">A <see cref="T:System.Type" /> that represents a delegate type.</param>
		/// <param name="body">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.LambdaExpression.Body" /> property equal to.</param>
		/// <param name="parameters">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ParameterExpression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.LambdaExpression.Parameters" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="delegateType" /> or <paramref name="body" /> is null.-or-One or more elements in <paramref name="parameters" /> are null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="delegateType" /> does not represent a delegate type.-or-<paramref name="body" />.Type represents a type that is not assignable to the return type of the delegate type represented by <paramref name="delegateType" />.-or-<paramref name="parameters" /> does not contain the same number of elements as the list of parameters for the delegate type represented by <paramref name="delegateType" />.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="parameters" /> is not assignable from the type of the corresponding parameter type of the delegate type represented by <paramref name="delegateType" />.</exception>
		public static LambdaExpression Lambda(Type delegateType, Expression body, IEnumerable<ParameterExpression> parameters)
		{
			if (delegateType == null)
			{
				throw new ArgumentNullException("delegateType");
			}
			if (body == null)
			{
				throw new ArgumentNullException("body");
			}
			ReadOnlyCollection<ParameterExpression> parameters2 = parameters.ToReadOnlyCollection<ParameterExpression>();
			body = Expression.CheckLambda(delegateType, body, parameters2);
			return Expression.CreateExpressionOf(delegateType, body, parameters2);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBinding" /> where the member is a field or property.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberListBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> properties set to the specified values.</returns>
		/// <param name="member">A <see cref="T:System.Reflection.MemberInfo" /> that represents a field or property to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
		/// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="member" /> is null. -or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Reflection.FieldInfo.FieldType" /> or <see cref="P:System.Reflection.PropertyInfo.PropertyType" /> of the field or property that <paramref name="member" /> represents does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
		public static MemberListBinding ListBind(MemberInfo member, params ElementInit[] initializers)
		{
			return Expression.ListBind(member, initializers);
		}

		private static void CheckIsAssignableToIEnumerable(Type t)
		{
			if (!t.IsAssignableTo(typeof(IEnumerable)))
			{
				throw new ArgumentException(string.Format("Type {0} doesn't implemen IEnumerable", t));
			}
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBinding" /> where the member is a field or property.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberListBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> properties set to the specified values.</returns>
		/// <param name="member">A <see cref="T:System.Reflection.MemberInfo" /> that represents a field or property to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
		/// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="member" /> is null. -or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Reflection.FieldInfo.FieldType" /> or <see cref="P:System.Reflection.PropertyInfo.PropertyType" /> of the field or property that <paramref name="member" /> represents does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
		public static MemberListBinding ListBind(MemberInfo member, IEnumerable<ElementInit> initializers)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (initializers == null)
			{
				throw new ArgumentNullException("initializers");
			}
			ReadOnlyCollection<ElementInit> readOnlyCollection = initializers.ToReadOnlyCollection<ElementInit>();
			Expression.CheckForNull<ElementInit>(readOnlyCollection, "initializers");
			member.OnFieldOrProperty(delegate(FieldInfo field)
			{
				Expression.CheckIsAssignableToIEnumerable(field.FieldType);
			}, delegate(PropertyInfo prop)
			{
				Expression.CheckIsAssignableToIEnumerable(prop.PropertyType);
			});
			return new MemberListBinding(member, readOnlyCollection);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBinding" /> object based on a specified property accessor method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberListBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.MemberInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> populated with the elements of <paramref name="initializers" />.</returns>
		/// <param name="propertyAccessor">A <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
		/// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="propertyAccessor" /> is null. -or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The <see cref="P:System.Reflection.PropertyInfo.PropertyType" /> of the property that the method represented by <paramref name="propertyAccessor" /> accesses does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
		public static MemberListBinding ListBind(MethodInfo propertyAccessor, params ElementInit[] initializers)
		{
			return Expression.ListBind(propertyAccessor, initializers);
		}

		private static void CheckForNull<T>(ReadOnlyCollection<T> collection, string name) where T : class
		{
			foreach (T t in collection)
			{
				if (t == null)
				{
					throw new ArgumentNullException(name);
				}
			}
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberListBinding" /> based on a specified property accessor method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberListBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.ListBinding" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.MemberInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> populated with the elements of <paramref name="initializers" />.</returns>
		/// <param name="propertyAccessor">A <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
		/// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberListBinding.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="propertyAccessor" /> is null. -or-One or more elements of <paramref name="initializers" /> are null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The <see cref="P:System.Reflection.PropertyInfo.PropertyType" /> of the property that the method represented by <paramref name="propertyAccessor" /> accesses does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
		public static MemberListBinding ListBind(MethodInfo propertyAccessor, IEnumerable<ElementInit> initializers)
		{
			if (propertyAccessor == null)
			{
				throw new ArgumentNullException("propertyAccessor");
			}
			if (initializers == null)
			{
				throw new ArgumentNullException("initializers");
			}
			ReadOnlyCollection<ElementInit> readOnlyCollection = initializers.ToReadOnlyCollection<ElementInit>();
			Expression.CheckForNull<ElementInit>(readOnlyCollection, "initializers");
			PropertyInfo associatedProperty = Expression.GetAssociatedProperty(propertyAccessor);
			if (associatedProperty == null)
			{
				throw new ArgumentException("propertyAccessor");
			}
			Expression.CheckIsAssignableToIEnumerable(associatedProperty.PropertyType);
			return new MemberListBinding(associatedProperty, readOnlyCollection);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses specified <see cref="T:System.Linq.Expressions.ElementInit" /> objects to initialize a collection.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> and <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> properties set to the specified values.</returns>
		/// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
		/// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newExpression" /> or <paramref name="initializers" /> is null.-or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
		public static ListInitExpression ListInit(NewExpression newExpression, params ElementInit[] initializers)
		{
			return Expression.ListInit(newExpression, initializers);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses specified <see cref="T:System.Linq.Expressions.ElementInit" /> objects to initialize a collection.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> and <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> properties set to the specified values.</returns>
		/// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
		/// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newExpression" /> or <paramref name="initializers" /> is null.-or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
		public static ListInitExpression ListInit(NewExpression newExpression, IEnumerable<ElementInit> initializers)
		{
			ReadOnlyCollection<ElementInit> initializers2 = Expression.CheckListInit<ElementInit>(newExpression, initializers);
			return new ListInitExpression(newExpression, initializers2);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses a method named "Add" to add elements to a collection.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property set to the specified value.</returns>
		/// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
		/// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newExpression" /> or <paramref name="initializers" /> is null.-or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no instance method named "Add" (case insensitive) declared in <paramref name="newExpression" />.Type or its base type.-or-The add method on <paramref name="newExpression" />.Type or its base type does not take exactly one argument.-or-The type represented by the <see cref="P:System.Linq.Expressions.Expression.Type" /> property of the first element of <paramref name="initializers" /> is not assignable to the argument type of the add method on <paramref name="newExpression" />.Type or its base type.-or-More than one argument-compatible method named "Add" (case-insensitive) exists on <paramref name="newExpression" />.Type and/or its base type.</exception>
		public static ListInitExpression ListInit(NewExpression newExpression, params Expression[] initializers)
		{
			return Expression.ListInit(newExpression, initializers);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses a method named "Add" to add elements to a collection.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property set to the specified value.</returns>
		/// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
		/// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newExpression" /> or <paramref name="initializers" /> is null.-or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no instance method named "Add" (case insensitive) declared in <paramref name="newExpression" />.Type or its base type.-or-The add method on <paramref name="newExpression" />.Type or its base type does not take exactly one argument.-or-The type represented by the <see cref="P:System.Linq.Expressions.Expression.Type" /> property of the first element of <paramref name="initializers" /> is not assignable to the argument type of the add method on <paramref name="newExpression" />.Type or its base type.-or-More than one argument-compatible method named "Add" (case-insensitive) exists on <paramref name="newExpression" />.Type and/or its base type.</exception>
		public static ListInitExpression ListInit(NewExpression newExpression, IEnumerable<Expression> initializers)
		{
			ReadOnlyCollection<Expression> readOnlyCollection = Expression.CheckListInit<Expression>(newExpression, initializers);
			MethodInfo addMethod = Expression.GetAddMethod(newExpression.Type, readOnlyCollection[0].Type);
			if (addMethod == null)
			{
				throw new InvalidOperationException("No suitable add method found");
			}
			return new ListInitExpression(newExpression, Expression.CreateInitializers(addMethod, readOnlyCollection));
		}

		private static ReadOnlyCollection<ElementInit> CreateInitializers(MethodInfo add_method, ReadOnlyCollection<Expression> initializers)
		{
			return initializers.Select((Expression init) => Expression.ElementInit(add_method, new Expression[]
			{
				init
			})).ToReadOnlyCollection<ElementInit>();
		}

		private static MethodInfo GetAddMethod(Type type, Type arg)
		{
			return type.GetMethod("Add", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, new Type[]
			{
				arg
			}, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses a specified method to add elements to a collection.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property set to the specified value.</returns>
		/// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
		/// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfo" /> that represents an instance method that takes one argument, that adds an element to a collection.</param>
		/// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newExpression" /> or <paramref name="initializers" /> is null.-or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.-or-<paramref name="addMethod" /> is not null and it does not represent an instance method named "Add" (case insensitive) that takes exactly one argument.-or-<paramref name="addMethod" /> is not null and the type represented by the <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="initializers" /> is not assignable to the argument type of the method that <paramref name="addMethod" /> represents.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="addMethod" /> is null and no instance method named "Add" that takes one type-compatible argument exists on <paramref name="newExpression" />.Type or its base type.</exception>
		public static ListInitExpression ListInit(NewExpression newExpression, MethodInfo addMethod, params Expression[] initializers)
		{
			return Expression.ListInit(newExpression, addMethod, initializers);
		}

		private static ReadOnlyCollection<T> CheckListInit<T>(NewExpression newExpression, IEnumerable<T> initializers) where T : class
		{
			if (newExpression == null)
			{
				throw new ArgumentNullException("newExpression");
			}
			if (initializers == null)
			{
				throw new ArgumentNullException("initializers");
			}
			if (!newExpression.Type.IsAssignableTo(typeof(IEnumerable)))
			{
				throw new InvalidOperationException("The type of the new expression does not implement IEnumerable");
			}
			ReadOnlyCollection<T> readOnlyCollection = initializers.ToReadOnlyCollection<T>();
			if (readOnlyCollection.Count == 0)
			{
				throw new ArgumentException("Empty initializers");
			}
			Expression.CheckForNull<T>(readOnlyCollection, "initializers");
			return readOnlyCollection;
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ListInitExpression" /> that uses a specified method to add elements to a collection.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ListInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.ListInit" /> and the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property set to the specified value.</returns>
		/// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.ListInitExpression.NewExpression" /> property equal to.</param>
		/// <param name="addMethod">A <see cref="T:System.Reflection.MethodInfo" /> that represents an instance method named "Add" (case insensitive), that adds an element to a collection.</param>
		/// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.ListInitExpression.Initializers" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newExpression" /> or <paramref name="initializers" /> is null.-or-One or more elements of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="newExpression" />.Type does not implement <see cref="T:System.Collections.IEnumerable" />.-or-<paramref name="addMethod" /> is not null and it does not represent an instance method named "Add" (case insensitive) that takes exactly one argument.-or-<paramref name="addMethod" /> is not null and the type represented by the <see cref="P:System.Linq.Expressions.Expression.Type" /> property of one or more elements of <paramref name="initializers" /> is not assignable to the argument type of the method that <paramref name="addMethod" /> represents.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="addMethod" /> is null and no instance method named "Add" that takes one type-compatible argument exists on <paramref name="newExpression" />.Type or its base type.</exception>
		public static ListInitExpression ListInit(NewExpression newExpression, MethodInfo addMethod, IEnumerable<Expression> initializers)
		{
			ReadOnlyCollection<Expression> readOnlyCollection = Expression.CheckListInit<Expression>(newExpression, initializers);
			if (addMethod != null)
			{
				if (addMethod.Name.ToLower(CultureInfo.InvariantCulture) != "add")
				{
					throw new ArgumentException("addMethod");
				}
				ParameterInfo[] parameters = addMethod.GetParameters();
				if (parameters.Length != 1)
				{
					throw new ArgumentException("addMethod");
				}
				foreach (Expression expression in readOnlyCollection)
				{
					if (!Expression.IsAssignableToParameterType(expression.Type, parameters[0]))
					{
						throw new InvalidOperationException("Initializer not assignable to the add method parameter type");
					}
				}
			}
			if (addMethod == null)
			{
				addMethod = Expression.GetAddMethod(newExpression.Type, readOnlyCollection[0].Type);
			}
			if (addMethod == null)
			{
				throw new InvalidOperationException("No suitable add method found");
			}
			return new ListInitExpression(newExpression, Expression.CreateInitializers(addMethod, readOnlyCollection));
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing either a field or a property.</summary>
		/// <returns>The <see cref="T:System.Linq.Expressions.MemberExpression" /> that results from calling the appropriate factory method.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the object that the member belongs to.</param>
		/// <param name="member">The <see cref="T:System.Reflection.MemberInfo" /> that describes the field or property to be accessed.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="member" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="member" /> does not represent a field or property.</exception>
		public static MemberExpression MakeMemberAccess(Expression expression, MemberInfo member)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				return Expression.Field(expression, fieldInfo);
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				return Expression.Property(expression, propertyInfo);
			}
			throw new ArgumentException("Member should either be a field or a property");
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" />, given an operand, by calling the appropriate factory method.</summary>
		/// <returns>The <see cref="T:System.Linq.Expressions.UnaryExpression" /> that results from calling the appropriate factory method.</returns>
		/// <param name="unaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of unary operation.</param>
		/// <param name="operand">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the operand.</param>
		/// <param name="type">The <see cref="T:System.Type" /> that specifies the type to be converted to (pass null if not applicable).</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="unaryType" /> does not correspond to a unary expression node.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="operand" /> is null.</exception>
		public static UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type)
		{
			return Expression.MakeUnary(unaryType, operand, type, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" />, given an operand and implementing method, by calling the appropriate factory method.</summary>
		/// <returns>The <see cref="T:System.Linq.Expressions.UnaryExpression" /> that results from calling the appropriate factory method.</returns>
		/// <param name="unaryType">The <see cref="T:System.Linq.Expressions.ExpressionType" /> that specifies the type of unary operation.</param>
		/// <param name="operand">An <see cref="T:System.Linq.Expressions.Expression" /> that represents the operand.</param>
		/// <param name="type">The <see cref="T:System.Type" /> that specifies the type to be converted to (pass null if not applicable).</param>
		/// <param name="method">The <see cref="T:System.Reflection.MethodInfo" /> that represents the implementing method.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="unaryType" /> does not correspond to a unary expression node.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="operand" /> is null.</exception>
		public static UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type, MethodInfo method)
		{
			switch (unaryType)
			{
			case ExpressionType.Negate:
				return Expression.Negate(operand, method);
			case ExpressionType.UnaryPlus:
				return Expression.UnaryPlus(operand, method);
			case ExpressionType.NegateChecked:
				return Expression.NegateChecked(operand, method);
			default:
				if (unaryType == ExpressionType.Convert)
				{
					return Expression.Convert(operand, type, method);
				}
				if (unaryType == ExpressionType.ConvertChecked)
				{
					return Expression.ConvertChecked(operand, type, method);
				}
				if (unaryType == ExpressionType.ArrayLength)
				{
					return Expression.ArrayLength(operand);
				}
				if (unaryType == ExpressionType.Quote)
				{
					return Expression.Quote(operand);
				}
				if (unaryType != ExpressionType.TypeAs)
				{
					throw new ArgumentException("MakeUnary expect an unary operator");
				}
				return Expression.TypeAs(operand, type);
			case ExpressionType.Not:
				return Expression.Not(operand, method);
			}
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that represents the recursive initialization of members of a field or property.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
		/// <param name="member">The <see cref="T:System.Reflection.MemberInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
		/// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="member" /> or <paramref name="bindings" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the field or property that <paramref name="member" /> represents.</exception>
		public static MemberMemberBinding MemberBind(MemberInfo member, params MemberBinding[] bindings)
		{
			return Expression.MemberBind(member, bindings);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that represents the recursive initialization of members of a field or property.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBinding" /> and the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> and <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
		/// <param name="member">The <see cref="T:System.Reflection.MemberInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property equal to.</param>
		/// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="member" /> or <paramref name="bindings" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="member" /> does not represent a field or property.-or-The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the field or property that <paramref name="member" /> represents.</exception>
		public static MemberMemberBinding MemberBind(MemberInfo member, IEnumerable<MemberBinding> bindings)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			Type type = member.OnFieldOrProperty((FieldInfo field) => field.FieldType, (PropertyInfo prop) => prop.PropertyType);
			return new MemberMemberBinding(member, Expression.CheckMemberBindings(type, bindings));
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that represents the recursive initialization of members of a member that is accessed by using a property accessor method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBinding" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
		/// <param name="propertyAccessor">The <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
		/// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="propertyAccessor" /> or <paramref name="bindings" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the property accessed by the method that <paramref name="propertyAccessor" /> represents.</exception>
		public static MemberMemberBinding MemberBind(MethodInfo propertyAccessor, params MemberBinding[] bindings)
		{
			return Expression.MemberBind(propertyAccessor, bindings);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that represents the recursive initialization of members of a member that is accessed by using a property accessor method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberMemberBinding" /> that has the <see cref="P:System.Linq.Expressions.MemberBinding.BindingType" /> property equal to <see cref="F:System.Linq.Expressions.MemberBindingType.MemberBinding" />, the <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
		/// <param name="propertyAccessor">The <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
		/// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberMemberBinding.Bindings" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="propertyAccessor" /> or <paramref name="bindings" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="propertyAccessor" /> does not represent a property accessor method.-or-The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type of the property accessed by the method that <paramref name="propertyAccessor" /> represents.</exception>
		public static MemberMemberBinding MemberBind(MethodInfo propertyAccessor, IEnumerable<MemberBinding> bindings)
		{
			if (propertyAccessor == null)
			{
				throw new ArgumentNullException("propertyAccessor");
			}
			ReadOnlyCollection<MemberBinding> collection = bindings.ToReadOnlyCollection<MemberBinding>();
			Expression.CheckForNull<MemberBinding>(collection, "bindings");
			PropertyInfo associatedProperty = Expression.GetAssociatedProperty(propertyAccessor);
			if (associatedProperty == null)
			{
				throw new ArgumentException("propertyAccessor");
			}
			return new MemberMemberBinding(associatedProperty, Expression.CheckMemberBindings(associatedProperty.PropertyType, bindings));
		}

		private static ReadOnlyCollection<MemberBinding> CheckMemberBindings(Type type, IEnumerable<MemberBinding> bindings)
		{
			if (bindings == null)
			{
				throw new ArgumentNullException("bindings");
			}
			ReadOnlyCollection<MemberBinding> readOnlyCollection = bindings.ToReadOnlyCollection<MemberBinding>();
			Expression.CheckForNull<MemberBinding>(readOnlyCollection, "bindings");
			foreach (MemberBinding memberBinding in readOnlyCollection)
			{
				if (!type.IsAssignableTo(memberBinding.Member.DeclaringType))
				{
					throw new ArgumentException("Type not assignable to member type");
				}
			}
			return readOnlyCollection;
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberInitExpression" />.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberInit" /> and the <see cref="P:System.Linq.Expressions.MemberInitExpression.NewExpression" /> and <see cref="P:System.Linq.Expressions.MemberInitExpression.Bindings" /> properties set to the specified values.</returns>
		/// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.MemberInitExpression.NewExpression" /> property equal to.</param>
		/// <param name="bindings">An array of <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberInitExpression.Bindings" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newExpression" /> or <paramref name="bindings" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type that <paramref name="newExpression" />.Type represents.</exception>
		public static MemberInitExpression MemberInit(NewExpression newExpression, params MemberBinding[] bindings)
		{
			return Expression.MemberInit(newExpression, bindings);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberInitExpression" />.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberInitExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberInit" /> and the <see cref="P:System.Linq.Expressions.MemberInitExpression.NewExpression" /> and <see cref="P:System.Linq.Expressions.MemberInitExpression.Bindings" /> properties set to the specified values.</returns>
		/// <param name="newExpression">A <see cref="T:System.Linq.Expressions.NewExpression" /> to set the <see cref="P:System.Linq.Expressions.MemberInitExpression.NewExpression" /> property equal to.</param>
		/// <param name="bindings">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.MemberBinding" /> objects to use to populate the <see cref="P:System.Linq.Expressions.MemberInitExpression.Bindings" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="newExpression" /> or <paramref name="bindings" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.MemberBinding.Member" /> property of an element of <paramref name="bindings" /> does not represent a member of the type that <paramref name="newExpression" />.Type represents.</exception>
		public static MemberInitExpression MemberInit(NewExpression newExpression, IEnumerable<MemberBinding> bindings)
		{
			if (newExpression == null)
			{
				throw new ArgumentNullException("newExpression");
			}
			return new MemberInitExpression(newExpression, Expression.CheckMemberBindings(newExpression.Type, bindings));
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an arithmetic negation operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Negate" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The unary minus operator is not defined for <paramref name="expression" />.Type.</exception>
		public static UnaryExpression Negate(Expression expression)
		{
			return Expression.Negate(expression, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an arithmetic negation operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Negate" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly one argument.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the unary minus operator is not defined for <paramref name="expression" />.Type.-or-<paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
		public static UnaryExpression Negate(Expression expression, MethodInfo method)
		{
			method = Expression.UnaryCoreCheck("op_UnaryNegation", expression, method, (Type type) => Expression.IsSignedNumber(type));
			return Expression.MakeSimpleUnary(ExpressionType.Negate, expression, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an arithmetic negation operation that has overflow checking.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NegateChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The unary minus operator is not defined for <paramref name="expression" />.Type.</exception>
		public static UnaryExpression NegateChecked(Expression expression)
		{
			return Expression.NegateChecked(expression, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an arithmetic negation operation that has overflow checking. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NegateChecked" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly one argument.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the unary minus operator is not defined for <paramref name="expression" />.Type.-or-<paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
		public static UnaryExpression NegateChecked(Expression expression, MethodInfo method)
		{
			method = Expression.UnaryCoreCheck("op_UnaryNegation", expression, method, (Type type) => Expression.IsSignedNumber(type));
			return Expression.MakeSimpleUnary(ExpressionType.NegateChecked, expression, method);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor that takes no arguments.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property set to the specified value.</returns>
		/// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="constructor" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The constructor that <paramref name="constructor" /> represents has at least one parameter.</exception>
		public static NewExpression New(ConstructorInfo constructor)
		{
			if (constructor == null)
			{
				throw new ArgumentNullException("constructor");
			}
			if (constructor.GetParameters().Length > 0)
			{
				throw new ArgumentException("Constructor must be parameter less");
			}
			return new NewExpression(constructor, Enumerable.ToReadOnlyCollection((IEnumerable<TSource>)null), null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the parameterless constructor of the specified type.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property set to the <see cref="T:System.Reflection.ConstructorInfo" /> that represents the parameterless constructor of the specified type.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that has a constructor that takes no arguments.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The type that <paramref name="type" /> represents does not have a parameterless constructor.</exception>
		public static NewExpression New(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Expression.CheckNotVoid(type);
			ReadOnlyCollection<Expression> arguments = Enumerable.ToReadOnlyCollection((IEnumerable<TSource>)null);
			if (type.IsValueType)
			{
				return new NewExpression(type, arguments);
			}
			ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
			{
				throw new ArgumentException("Type doesn't have a parameter less constructor");
			}
			return new NewExpression(constructor, arguments, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor with the specified arguments.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> and <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
		/// <param name="arguments">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="constructor" /> is null.-or-An element of <paramref name="arguments" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="arguments" /> does match the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.</exception>
		public static NewExpression New(ConstructorInfo constructor, params Expression[] arguments)
		{
			return Expression.New(constructor, arguments);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor with the specified arguments.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> and <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> properties set to the specified values.</returns>
		/// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
		/// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="constructor" /> is null.-or-An element of <paramref name="arguments" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.</exception>
		public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments)
		{
			if (constructor == null)
			{
				throw new ArgumentNullException("constructor");
			}
			ReadOnlyCollection<Expression> arguments2 = Expression.CheckMethodArguments(constructor, arguments);
			return new NewExpression(constructor, arguments2, null);
		}

		private static IList<Expression> CreateArgumentList(IEnumerable<Expression> arguments)
		{
			if (arguments == null)
			{
				return arguments.ToReadOnlyCollection<Expression>();
			}
			return arguments.ToList<Expression>();
		}

		private static void CheckNonGenericMethod(MethodBase method)
		{
			if (method.IsGenericMethodDefinition || method.ContainsGenericParameters)
			{
				throw new ArgumentException("Can not used open generic methods");
			}
		}

		private static ReadOnlyCollection<Expression> CheckMethodArguments(MethodBase method, IEnumerable<Expression> args)
		{
			Expression.CheckNonGenericMethod(method);
			IList<Expression> list = Expression.CreateArgumentList(args);
			ParameterInfo[] parameters = method.GetParameters();
			if (list.Count != parameters.Length)
			{
				throw new ArgumentException("The number of arguments doesn't match the number of parameters");
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				if (list[i] == null)
				{
					throw new ArgumentNullException("arguments");
				}
				if (!Expression.IsAssignableToParameterType(list[i].Type, parameters[i]))
				{
					if (!parameters[i].ParameterType.IsExpression())
					{
						throw new ArgumentException("arguments");
					}
					list[i] = Expression.Quote(list[i]);
				}
			}
			return list.ToReadOnlyCollection<Expression>();
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified as an array.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" />, <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> and <see cref="P:System.Linq.Expressions.NewExpression.Members" /> properties set to the specified values.</returns>
		/// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
		/// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> collection.</param>
		/// <param name="members">An array of <see cref="T:System.Reflection.MemberInfo" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Members" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="constructor" /> is null.-or-An element of <paramref name="arguments" /> is null.-or-An element of <paramref name="members" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.-or-The <paramref name="members" /> parameter does not have the same number of elements as <paramref name="arguments" />.-or-An element of <paramref name="arguments" /> has a <see cref="P:System.Linq.Expressions.Expression.Type" /> property that represents a type that is not assignable to the type of the member that is represented by the corresponding element of <paramref name="members" />.-or-An element of <paramref name="members" /> represents a property that does not have a get accessor.</exception>
		public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments, params MemberInfo[] members)
		{
			return Expression.New(constructor, arguments, members);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewExpression" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.New" /> and the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" />, <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> and <see cref="P:System.Linq.Expressions.NewExpression.Members" /> properties set to the specified values.</returns>
		/// <param name="constructor">The <see cref="T:System.Reflection.ConstructorInfo" /> to set the <see cref="P:System.Linq.Expressions.NewExpression.Constructor" /> property equal to.</param>
		/// <param name="arguments">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Arguments" /> collection.</param>
		/// <param name="members">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Reflection.MemberInfo" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewExpression.Members" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="constructor" /> is null.-or-An element of <paramref name="arguments" /> is null.-or-An element of <paramref name="members" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="arguments" /> parameter does not contain the same number of elements as the number of parameters for the constructor that <paramref name="constructor" /> represents.-or-The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="arguments" /> is not assignable to the type of the corresponding parameter of the constructor that <paramref name="constructor" /> represents.-or-The <paramref name="members" /> parameter does not have the same number of elements as <paramref name="arguments" />.-or-An element of <paramref name="arguments" /> has a <see cref="P:System.Linq.Expressions.Expression.Type" /> property that represents a type that is not assignable to the type of the member that is represented by the corresponding element of <paramref name="members" />.-or-An element of members represents a property that does not have a get accessor.</exception>
		public static NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments, IEnumerable<MemberInfo> members)
		{
			if (constructor == null)
			{
				throw new ArgumentNullException("constructor");
			}
			ReadOnlyCollection<Expression> readOnlyCollection = arguments.ToReadOnlyCollection<Expression>();
			ReadOnlyCollection<MemberInfo> readOnlyCollection2 = members.ToReadOnlyCollection<MemberInfo>();
			Expression.CheckForNull<Expression>(readOnlyCollection, "arguments");
			Expression.CheckForNull<MemberInfo>(readOnlyCollection2, "members");
			readOnlyCollection = Expression.CheckMethodArguments(constructor, arguments);
			if (readOnlyCollection.Count != readOnlyCollection2.Count)
			{
				throw new ArgumentException("Arguments count does not match members count");
			}
			for (int i = 0; i < readOnlyCollection2.Count; i++)
			{
				MemberInfo memberInfo = readOnlyCollection2[i];
				MemberTypes memberType = memberInfo.MemberType;
				Type type;
				if (memberType != MemberTypes.Field)
				{
					if (memberType != MemberTypes.Method)
					{
						if (memberType != MemberTypes.Property)
						{
							throw new ArgumentException("Member type not allowed");
						}
						PropertyInfo propertyInfo = memberInfo as PropertyInfo;
						if (propertyInfo.GetGetMethod(true) == null)
						{
							throw new ArgumentException("Property must have a getter");
						}
						type = (memberInfo as PropertyInfo).PropertyType;
					}
					else
					{
						type = (memberInfo as MethodInfo).ReturnType;
					}
				}
				else
				{
					type = (memberInfo as FieldInfo).FieldType;
				}
				if (!readOnlyCollection[i].Type.IsAssignableTo(type))
				{
					throw new ArgumentException("Argument type not assignable to member type");
				}
			}
			return new NewExpression(constructor, readOnlyCollection, readOnlyCollection2);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that represents creating an array that has a specified rank.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayBounds" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> property set to the specified value.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the element type of the array.</param>
		/// <param name="bounds">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="bounds" /> is null.-or-An element of <paramref name="bounds" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="bounds" /> does not represent an integral type.</exception>
		public static NewArrayExpression NewArrayBounds(Type type, params Expression[] bounds)
		{
			return Expression.NewArrayBounds(type, bounds);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that represents creating an array that has a specified rank.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayBounds" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> property set to the specified value.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the element type of the array.</param>
		/// <param name="bounds">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="bounds" /> is null.-or-An element of <paramref name="bounds" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="bounds" /> does not represent an integral type.</exception>
		public static NewArrayExpression NewArrayBounds(Type type, IEnumerable<Expression> bounds)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (bounds == null)
			{
				throw new ArgumentNullException("bounds");
			}
			Expression.CheckNotVoid(type);
			ReadOnlyCollection<Expression> readOnlyCollection = bounds.ToReadOnlyCollection<Expression>();
			foreach (Expression expression in readOnlyCollection)
			{
				if (!Expression.IsInt(expression.Type))
				{
					throw new ArgumentException("The bounds collection can only contain expression of integers types");
				}
			}
			return new NewArrayExpression(ExpressionType.NewArrayBounds, type.MakeArrayType(readOnlyCollection.Count), readOnlyCollection);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayInit" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> property set to the specified value.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the element type of the array.</param>
		/// <param name="initializers">An array of <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="initializers" /> is null.-or-An element of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="initializers" /> represents a type that is not assignable to the type <paramref name="type" />.</exception>
		public static NewArrayExpression NewArrayInit(Type type, params Expression[] initializers)
		{
			return Expression.NewArrayInit(type, initializers);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.NewArrayExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.NewArrayInit" /> and the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> property set to the specified value.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the element type of the array.</param>
		/// <param name="initializers">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains <see cref="T:System.Linq.Expressions.Expression" /> objects to use to populate the <see cref="P:System.Linq.Expressions.NewArrayExpression.Expressions" /> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="initializers" /> is null.-or-An element of <paramref name="initializers" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Linq.Expressions.Expression.Type" /> property of an element of <paramref name="initializers" /> represents a type that is not assignable to the type that <paramref name="type" /> represents.</exception>
		public static NewArrayExpression NewArrayInit(Type type, IEnumerable<Expression> initializers)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (initializers == null)
			{
				throw new ArgumentNullException("initializers");
			}
			Expression.CheckNotVoid(type);
			ReadOnlyCollection<Expression> readOnlyCollection = initializers.ToReadOnlyCollection<Expression>();
			foreach (Expression expression in readOnlyCollection)
			{
				if (expression == null)
				{
					throw new ArgumentNullException("initializers");
				}
				if (!expression.Type.IsAssignableTo(type))
				{
					throw new InvalidOperationException(string.Format("{0} IsAssignableTo {1}, expression [ {2} ] : {3}", new object[]
					{
						expression.Type,
						type,
						expression.NodeType,
						expression
					}));
				}
			}
			return new NewArrayExpression(ExpressionType.NewArrayInit, type.MakeArrayType(), readOnlyCollection);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a bitwise complement operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Not" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The unary not operator is not defined for <paramref name="expression" />.Type.</exception>
		public static UnaryExpression Not(Expression expression)
		{
			return Expression.Not(expression, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a bitwise complement operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Not" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly one argument.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the unary not operator is not defined for <paramref name="expression" />.Type.-or-<paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
		public static UnaryExpression Not(Expression expression, MethodInfo method)
		{
			Func<Type, bool> validator = (Type type) => Expression.IsIntOrBool(type);
			method = Expression.UnaryCoreCheck("op_LogicalNot", expression, method, validator);
			if (method == null)
			{
				method = Expression.UnaryCoreCheck("op_OnesComplement", expression, method, validator);
			}
			return Expression.MakeSimpleUnary(ExpressionType.Not, expression, method);
		}

		private static void CheckNotVoid(Type type)
		{
			if (type == typeof(void))
			{
				throw new ArgumentException("Type can't be void");
			}
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.ParameterExpression" />.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.ParameterExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Parameter" /> and the <see cref="P:System.Linq.Expressions.Expression.Type" /> and <see cref="P:System.Linq.Expressions.ParameterExpression.Name" /> properties set to the specified values.</returns>
		/// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
		/// <param name="name">The value to set the <see cref="P:System.Linq.Expressions.ParameterExpression.Name" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null.</exception>
		public static ParameterExpression Parameter(Type type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Expression.CheckNotVoid(type);
			return new ParameterExpression(type, name);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a property by using a property accessor method.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" />, the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property set to <paramref name="expression" /> and the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property equal to.</param>
		/// <param name="propertyAccessor">The <see cref="T:System.Reflection.MethodInfo" /> that represents a property accessor method.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="propertyAccessor" /> is null.-or-The method that <paramref name="propertyAccessor" /> represents is not static (Shared in Visual Basic) and <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="expression" />.Type is not assignable to the declaring type of the method represented by <paramref name="propertyAccessor" />.-or-The method that <paramref name="propertyAccessor" /> represents is not a property accessor method.</exception>
		public static MemberExpression Property(Expression expression, MethodInfo propertyAccessor)
		{
			if (propertyAccessor == null)
			{
				throw new ArgumentNullException("propertyAccessor");
			}
			Expression.CheckNonGenericMethod(propertyAccessor);
			if (!propertyAccessor.IsStatic)
			{
				if (expression == null)
				{
					throw new ArgumentNullException("expression");
				}
				if (!expression.Type.IsAssignableTo(propertyAccessor.DeclaringType))
				{
					throw new ArgumentException("expression");
				}
			}
			PropertyInfo associatedProperty = Expression.GetAssociatedProperty(propertyAccessor);
			if (associatedProperty == null)
			{
				throw new ArgumentException(string.Format("Method {0} has no associated property", propertyAccessor));
			}
			return new MemberExpression(expression, associatedProperty, associatedProperty.PropertyType);
		}

		private static PropertyInfo GetAssociatedProperty(MethodInfo method)
		{
			if (method == null)
			{
				return null;
			}
			foreach (PropertyInfo propertyInfo in method.DeclaringType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (method.Equals(propertyInfo.GetGetMethod(true)))
				{
					return propertyInfo;
				}
				if (method.Equals(propertyInfo.GetSetMethod(true)))
				{
					return propertyInfo;
				}
			}
			return null;
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a property.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" /> and the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> and <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property equal to.</param>
		/// <param name="property">The <see cref="T:System.Reflection.PropertyInfo" /> to set the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="property" /> is null.-or-The property that <paramref name="property" /> represents is not static (Shared in Visual Basic) and <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="expression" />.Type is not assignable to the declaring type of the property that <paramref name="property" /> represents.</exception>
		public static MemberExpression Property(Expression expression, PropertyInfo property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			MethodInfo getMethod = property.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException("getter");
			}
			if (!getMethod.IsStatic)
			{
				if (expression == null)
				{
					throw new ArgumentNullException("expression");
				}
				if (!expression.Type.IsAssignableTo(property.DeclaringType))
				{
					throw new ArgumentException("expression");
				}
			}
			else if (expression != null)
			{
				throw new ArgumentException("expression");
			}
			return new MemberExpression(expression, property, property.PropertyType);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a property given the name of the property.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" />, the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property set to <paramref name="expression" />, and the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> that represents the property denoted by <paramref name="propertyName" />.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> whose <see cref="P:System.Linq.Expressions.Expression.Type" /> contains a property named <paramref name="propertyName" />.</param>
		/// <param name="propertyName">The name of a property.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="propertyName" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">No property named <paramref name="propertyName" /> is defined in <paramref name="expression" />.Type or its base types.</exception>
		public static MemberExpression Property(Expression expression, string propertyName)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			PropertyInfo property = expression.Type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			if (property == null)
			{
				throw new ArgumentException(string.Format("No property named {0} on {1}", propertyName, expression.Type));
			}
			return new MemberExpression(expression, property, property.PropertyType);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.MemberExpression" /> that represents accessing a property or field given the name of the property or field.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.MemberExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.MemberAccess" />, the <see cref="P:System.Linq.Expressions.MemberExpression.Expression" /> property set to <paramref name="expression" />, and the <see cref="P:System.Linq.Expressions.MemberExpression.Member" /> property set to the <see cref="T:System.Reflection.PropertyInfo" /> or <see cref="T:System.Reflection.FieldInfo" /> that represents the property or field denoted by <paramref name="propertyOrFieldName" />.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> whose <see cref="P:System.Linq.Expressions.Expression.Type" /> contains a property or field named <paramref name="propertyOrFieldName" />.</param>
		/// <param name="propertyOrFieldName">The name of a property or field.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="propertyOrFieldName" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">No property or field named <paramref name="propertyOrFieldName" /> is defined in <paramref name="expression" />.Type or its base types.</exception>
		public static MemberExpression PropertyOrField(Expression expression, string propertyOrFieldName)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (propertyOrFieldName == null)
			{
				throw new ArgumentNullException("propertyOrFieldName");
			}
			PropertyInfo property = expression.Type.GetProperty(propertyOrFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			if (property != null)
			{
				return new MemberExpression(expression, property, property.PropertyType);
			}
			FieldInfo field = expression.Type.GetField(propertyOrFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			if (field != null)
			{
				return new MemberExpression(expression, field, field.FieldType);
			}
			throw new ArgumentException(string.Format("No field or property named {0} on {1}", propertyOrFieldName, expression.Type));
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an expression that has a constant value of type <see cref="T:System.Linq.Expressions.Expression" />.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.Quote" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		public static UnaryExpression Quote(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			return new UnaryExpression(ExpressionType.Quote, expression, expression.GetType());
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents an explicit reference or boxing conversion where null is supplied if the conversion fails.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.TypeAs" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.Expression.Type" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.Expression.Type" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="type" /> is null.</exception>
		public static UnaryExpression TypeAs(Expression expression, Type type)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.IsValueType && !type.IsNullable())
			{
				throw new ArgumentException("TypeAs expect a reference or a nullable type");
			}
			return new UnaryExpression(ExpressionType.TypeAs, expression, type);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.TypeBinaryExpression" />.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.TypeBinaryExpression" /> for which the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property is equal to <see cref="F:System.Linq.Expressions.ExpressionType.TypeIs" /> and for which the <see cref="P:System.Linq.Expressions.TypeBinaryExpression.Expression" /> and <see cref="P:System.Linq.Expressions.TypeBinaryExpression.TypeOperand" /> properties are set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpression.Expression" /> property equal to.</param>
		/// <param name="type">A <see cref="T:System.Type" /> to set the <see cref="P:System.Linq.Expressions.TypeBinaryExpression.TypeOperand" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> or <paramref name="type" /> is null.</exception>
		public static TypeBinaryExpression TypeIs(Expression expression, Type type)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Expression.CheckNotVoid(type);
			return new TypeBinaryExpression(ExpressionType.TypeIs, expression, type, typeof(bool));
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a unary plus operation.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.UnaryPlus" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property set to the specified value.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The unary plus operator is not defined for <paramref name="expression" />.Type.</exception>
		public static UnaryExpression UnaryPlus(Expression expression)
		{
			return Expression.UnaryPlus(expression, null);
		}

		/// <summary>Creates a <see cref="T:System.Linq.Expressions.UnaryExpression" /> that represents a unary plus operation. The implementing method can be specified.</summary>
		/// <returns>A <see cref="T:System.Linq.Expressions.UnaryExpression" /> that has the <see cref="P:System.Linq.Expressions.Expression.NodeType" /> property equal to <see cref="F:System.Linq.Expressions.ExpressionType.UnaryPlus" /> and the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> and <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> properties set to the specified values.</returns>
		/// <param name="expression">An <see cref="T:System.Linq.Expressions.Expression" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Operand" /> property equal to.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> to set the <see cref="P:System.Linq.Expressions.UnaryExpression.Method" /> property equal to.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="expression" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="method" /> is not null and the method it represents returns void, is not static (Shared in Visual Basic), or does not take exactly one argument.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="method" /> is null and the unary plus operator is not defined for <paramref name="expression" />.Type.-or-<paramref name="expression" />.Type (or its corresponding non-nullable type if it is a nullable value type) is not assignable to the argument type of the method represented by <paramref name="method" />.</exception>
		public static UnaryExpression UnaryPlus(Expression expression, MethodInfo method)
		{
			method = Expression.UnaryCoreCheck("op_UnaryPlus", expression, method, (Type type) => Expression.IsNumber(type));
			return Expression.MakeSimpleUnary(ExpressionType.UnaryPlus, expression, method);
		}

		private static bool IsInt(Type t)
		{
			return t == typeof(byte) || t == typeof(sbyte) || t == typeof(short) || t == typeof(ushort) || t == typeof(int) || t == typeof(uint) || t == typeof(long) || t == typeof(ulong);
		}

		private static bool IsIntOrBool(Type t)
		{
			return Expression.IsInt(t) || t == typeof(bool);
		}

		private static bool IsNumber(Type t)
		{
			return Expression.IsInt(t) || t == typeof(float) || t == typeof(double);
		}

		private static bool IsSignedNumber(Type t)
		{
			return Expression.IsNumber(t) && !Expression.IsUnsigned(t);
		}

		internal static bool IsUnsigned(Type t)
		{
			if (t.IsPointer)
			{
				return Expression.IsUnsigned(t.GetElementType());
			}
			return t == typeof(ushort) || t == typeof(uint) || t == typeof(ulong) || t == typeof(byte);
		}

		internal virtual void Emit(EmitContext ec)
		{
			throw new NotImplementedException(string.Format("Emit method is not implemented in expression type {0}", base.GetType()));
		}
	}
}
