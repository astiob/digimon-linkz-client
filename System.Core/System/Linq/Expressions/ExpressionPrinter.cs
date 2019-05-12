using System;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Linq.Expressions
{
	internal class ExpressionPrinter : ExpressionVisitor
	{
		private const string ListSeparator = ", ";

		private StringBuilder builder;

		private ExpressionPrinter(StringBuilder builder)
		{
			this.builder = builder;
		}

		private ExpressionPrinter() : this(new StringBuilder())
		{
		}

		public static string ToString(Expression expression)
		{
			ExpressionPrinter expressionPrinter = new ExpressionPrinter();
			expressionPrinter.Visit(expression);
			return expressionPrinter.builder.ToString();
		}

		public static string ToString(ElementInit init)
		{
			ExpressionPrinter expressionPrinter = new ExpressionPrinter();
			expressionPrinter.VisitElementInitializer(init);
			return expressionPrinter.builder.ToString();
		}

		public static string ToString(MemberBinding binding)
		{
			ExpressionPrinter expressionPrinter = new ExpressionPrinter();
			expressionPrinter.VisitBinding(binding);
			return expressionPrinter.builder.ToString();
		}

		private void Print(string str)
		{
			this.builder.Append(str);
		}

		private void Print(object obj)
		{
			this.builder.Append(obj);
		}

		private void Print(string str, params object[] objs)
		{
			this.builder.AppendFormat(str, objs);
		}

		protected override void VisitElementInitializer(ElementInit initializer)
		{
			this.Print(initializer.AddMethod);
			this.Print("(");
			this.VisitExpressionList(initializer.Arguments);
			this.Print(")");
		}

		protected override void VisitUnary(UnaryExpression unary)
		{
			ExpressionType nodeType = unary.NodeType;
			if (nodeType != ExpressionType.Convert && nodeType != ExpressionType.ConvertChecked)
			{
				if (nodeType == ExpressionType.Negate)
				{
					this.Print("-");
					this.Visit(unary.Operand);
					return;
				}
				if (nodeType == ExpressionType.UnaryPlus)
				{
					this.Print("+");
					this.Visit(unary.Operand);
					return;
				}
				if (nodeType != ExpressionType.ArrayLength && nodeType != ExpressionType.Not)
				{
					if (nodeType == ExpressionType.Quote)
					{
						this.Visit(unary.Operand);
						return;
					}
					if (nodeType != ExpressionType.TypeAs)
					{
						throw new NotImplementedException();
					}
					this.Print("(");
					this.Visit(unary.Operand);
					this.Print(" As {0})", new object[]
					{
						unary.Type.Name
					});
					return;
				}
			}
			this.Print("{0}(", new object[]
			{
				unary.NodeType
			});
			this.Visit(unary.Operand);
			this.Print(")");
		}

		private static string OperatorToString(BinaryExpression binary)
		{
			switch (binary.NodeType)
			{
			case ExpressionType.Add:
			case ExpressionType.AddChecked:
				return "+";
			case ExpressionType.And:
				return (!ExpressionPrinter.IsBoolean(binary)) ? "&" : "And";
			case ExpressionType.AndAlso:
				return "&&";
			case ExpressionType.Coalesce:
				return "??";
			case ExpressionType.Divide:
				return "/";
			case ExpressionType.Equal:
				return "=";
			case ExpressionType.ExclusiveOr:
				return "^";
			case ExpressionType.GreaterThan:
				return ">";
			case ExpressionType.GreaterThanOrEqual:
				return ">=";
			case ExpressionType.LeftShift:
				return "<<";
			case ExpressionType.LessThan:
				return "<";
			case ExpressionType.LessThanOrEqual:
				return "<=";
			case ExpressionType.Modulo:
				return "%";
			case ExpressionType.Multiply:
			case ExpressionType.MultiplyChecked:
				return "*";
			case ExpressionType.NotEqual:
				return "!=";
			case ExpressionType.Or:
				return (!ExpressionPrinter.IsBoolean(binary)) ? "|" : "Or";
			case ExpressionType.OrElse:
				return "||";
			case ExpressionType.Power:
				return "^";
			case ExpressionType.RightShift:
				return ">>";
			case ExpressionType.Subtract:
			case ExpressionType.SubtractChecked:
				return "-";
			}
			return null;
		}

		private static bool IsBoolean(Expression expression)
		{
			return expression.Type == typeof(bool) || expression.Type == typeof(bool?);
		}

		private void PrintArrayIndex(BinaryExpression index)
		{
			this.Visit(index.Left);
			this.Print("[");
			this.Visit(index.Right);
			this.Print("]");
		}

		protected override void VisitBinary(BinaryExpression binary)
		{
			ExpressionType nodeType = binary.NodeType;
			if (nodeType != ExpressionType.ArrayIndex)
			{
				this.Print("(");
				this.Visit(binary.Left);
				this.Print(" {0} ", new object[]
				{
					ExpressionPrinter.OperatorToString(binary)
				});
				this.Visit(binary.Right);
				this.Print(")");
				return;
			}
			this.PrintArrayIndex(binary);
		}

		protected override void VisitTypeIs(TypeBinaryExpression type)
		{
			ExpressionType nodeType = type.NodeType;
			if (nodeType != ExpressionType.TypeIs)
			{
				throw new NotImplementedException();
			}
			this.Print("(");
			this.Visit(type.Expression);
			this.Print(" Is {0})", new object[]
			{
				type.TypeOperand.Name
			});
		}

		protected override void VisitConstant(ConstantExpression constant)
		{
			object value = constant.Value;
			if (value == null)
			{
				this.Print("null");
			}
			else if (value is string)
			{
				this.Print("\"");
				this.Print(value);
				this.Print("\"");
			}
			else if (!ExpressionPrinter.HasStringRepresentation(value))
			{
				this.Print("value(");
				this.Print(value);
				this.Print(")");
			}
			else
			{
				this.Print(value);
			}
		}

		private static bool HasStringRepresentation(object obj)
		{
			return obj.ToString() != obj.GetType().ToString();
		}

		protected override void VisitConditional(ConditionalExpression conditional)
		{
			this.Print("IIF(");
			this.Visit(conditional.Test);
			this.Print(", ");
			this.Visit(conditional.IfTrue);
			this.Print(", ");
			this.Visit(conditional.IfFalse);
			this.Print(")");
		}

		protected override void VisitParameter(ParameterExpression parameter)
		{
			this.Print(parameter.Name ?? "<param>");
		}

		protected override void VisitMemberAccess(MemberExpression access)
		{
			if (access.Expression == null)
			{
				this.Print(access.Member.DeclaringType.Name);
			}
			else
			{
				this.Visit(access.Expression);
			}
			this.Print(".{0}", new object[]
			{
				access.Member.Name
			});
		}

		protected override void VisitMethodCall(MethodCallExpression call)
		{
			if (call.Object != null)
			{
				this.Visit(call.Object);
				this.Print(".");
			}
			this.Print(call.Method.Name);
			this.Print("(");
			this.VisitExpressionList(call.Arguments);
			this.Print(")");
		}

		protected override void VisitMemberAssignment(MemberAssignment assignment)
		{
			this.Print("{0} = ", new object[]
			{
				assignment.Member.Name
			});
			this.Visit(assignment.Expression);
		}

		protected override void VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			this.Print(binding.Member.Name);
			this.Print(" = {");
			this.VisitList<MemberBinding>(binding.Bindings, new Action<MemberBinding>(this.VisitBinding));
			this.Print("}");
		}

		protected override void VisitMemberListBinding(MemberListBinding binding)
		{
			this.Print(binding.Member.Name);
			this.Print(" = {");
			this.VisitList<ElementInit>(binding.Initializers, new Action<ElementInit>(this.VisitElementInitializer));
			this.Print("}");
		}

		protected override void VisitList<T>(ReadOnlyCollection<T> list, Action<T> visitor)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					this.Print(", ");
				}
				visitor(list[i]);
			}
		}

		protected override void VisitLambda(LambdaExpression lambda)
		{
			if (lambda.Parameters.Count != 1)
			{
				this.Print("(");
				this.VisitList<ParameterExpression>(lambda.Parameters, new Action<ParameterExpression>(this.Visit));
				this.Print(")");
			}
			else
			{
				this.Visit(lambda.Parameters[0]);
			}
			this.Print(" => ");
			this.Visit(lambda.Body);
		}

		protected override void VisitNew(NewExpression nex)
		{
			this.Print("new {0}(", new object[]
			{
				nex.Type.Name
			});
			if (nex.Members != null && nex.Members.Count > 0)
			{
				for (int i = 0; i < nex.Members.Count; i++)
				{
					if (i > 0)
					{
						this.Print(", ");
					}
					this.Print("{0} = ", new object[]
					{
						nex.Members[i].Name
					});
					this.Visit(nex.Arguments[i]);
				}
			}
			else
			{
				this.VisitExpressionList(nex.Arguments);
			}
			this.Print(")");
		}

		protected override void VisitMemberInit(MemberInitExpression init)
		{
			this.Visit(init.NewExpression);
			this.Print(" {");
			this.VisitList<MemberBinding>(init.Bindings, new Action<MemberBinding>(this.VisitBinding));
			this.Print("}");
		}

		protected override void VisitListInit(ListInitExpression init)
		{
			this.Visit(init.NewExpression);
			this.Print(" {");
			this.VisitList<ElementInit>(init.Initializers, new Action<ElementInit>(this.VisitElementInitializer));
			this.Print("}");
		}

		protected override void VisitNewArray(NewArrayExpression newArray)
		{
			this.Print("new ");
			ExpressionType nodeType = newArray.NodeType;
			if (nodeType == ExpressionType.NewArrayInit)
			{
				this.Print("[] {");
				this.VisitExpressionList(newArray.Expressions);
				this.Print("}");
				return;
			}
			if (nodeType != ExpressionType.NewArrayBounds)
			{
				throw new NotSupportedException();
			}
			this.Print(newArray.Type);
			this.Print("(");
			this.VisitExpressionList(newArray.Expressions);
			this.Print(")");
		}

		protected override void VisitInvocation(InvocationExpression invocation)
		{
			this.Print("Invoke(");
			this.Visit(invocation.Expression);
			if (invocation.Arguments.Count != 0)
			{
				this.Print(", ");
				this.VisitExpressionList(invocation.Arguments);
			}
			this.Print(")");
		}
	}
}
