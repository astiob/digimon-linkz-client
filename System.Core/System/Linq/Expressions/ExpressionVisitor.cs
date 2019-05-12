using System;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
	internal abstract class ExpressionVisitor
	{
		protected virtual void Visit(Expression expression)
		{
			if (expression == null)
			{
				return;
			}
			switch (expression.NodeType)
			{
			case ExpressionType.Add:
			case ExpressionType.AddChecked:
			case ExpressionType.And:
			case ExpressionType.AndAlso:
			case ExpressionType.ArrayIndex:
			case ExpressionType.Coalesce:
			case ExpressionType.Divide:
			case ExpressionType.Equal:
			case ExpressionType.ExclusiveOr:
			case ExpressionType.GreaterThan:
			case ExpressionType.GreaterThanOrEqual:
			case ExpressionType.LeftShift:
			case ExpressionType.LessThan:
			case ExpressionType.LessThanOrEqual:
			case ExpressionType.Modulo:
			case ExpressionType.Multiply:
			case ExpressionType.MultiplyChecked:
			case ExpressionType.NotEqual:
			case ExpressionType.Or:
			case ExpressionType.OrElse:
			case ExpressionType.Power:
			case ExpressionType.RightShift:
			case ExpressionType.Subtract:
			case ExpressionType.SubtractChecked:
				this.VisitBinary((BinaryExpression)expression);
				break;
			case ExpressionType.ArrayLength:
			case ExpressionType.Convert:
			case ExpressionType.ConvertChecked:
			case ExpressionType.Negate:
			case ExpressionType.UnaryPlus:
			case ExpressionType.NegateChecked:
			case ExpressionType.Not:
			case ExpressionType.Quote:
			case ExpressionType.TypeAs:
				this.VisitUnary((UnaryExpression)expression);
				break;
			case ExpressionType.Call:
				this.VisitMethodCall((MethodCallExpression)expression);
				break;
			case ExpressionType.Conditional:
				this.VisitConditional((ConditionalExpression)expression);
				break;
			case ExpressionType.Constant:
				this.VisitConstant((ConstantExpression)expression);
				break;
			case ExpressionType.Invoke:
				this.VisitInvocation((InvocationExpression)expression);
				break;
			case ExpressionType.Lambda:
				this.VisitLambda((LambdaExpression)expression);
				break;
			case ExpressionType.ListInit:
				this.VisitListInit((ListInitExpression)expression);
				break;
			case ExpressionType.MemberAccess:
				this.VisitMemberAccess((MemberExpression)expression);
				break;
			case ExpressionType.MemberInit:
				this.VisitMemberInit((MemberInitExpression)expression);
				break;
			case ExpressionType.New:
				this.VisitNew((NewExpression)expression);
				break;
			case ExpressionType.NewArrayInit:
			case ExpressionType.NewArrayBounds:
				this.VisitNewArray((NewArrayExpression)expression);
				break;
			case ExpressionType.Parameter:
				this.VisitParameter((ParameterExpression)expression);
				break;
			case ExpressionType.TypeIs:
				this.VisitTypeIs((TypeBinaryExpression)expression);
				break;
			default:
				throw new ArgumentException(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
			}
		}

		protected virtual void VisitBinding(MemberBinding binding)
		{
			switch (binding.BindingType)
			{
			case MemberBindingType.Assignment:
				this.VisitMemberAssignment((MemberAssignment)binding);
				break;
			case MemberBindingType.MemberBinding:
				this.VisitMemberMemberBinding((MemberMemberBinding)binding);
				break;
			case MemberBindingType.ListBinding:
				this.VisitMemberListBinding((MemberListBinding)binding);
				break;
			default:
				throw new ArgumentException(string.Format("Unhandled binding type '{0}'", binding.BindingType));
			}
		}

		protected virtual void VisitElementInitializer(ElementInit initializer)
		{
			this.VisitExpressionList(initializer.Arguments);
		}

		protected virtual void VisitUnary(UnaryExpression unary)
		{
			this.Visit(unary.Operand);
		}

		protected virtual void VisitBinary(BinaryExpression binary)
		{
			this.Visit(binary.Left);
			this.Visit(binary.Right);
			this.Visit(binary.Conversion);
		}

		protected virtual void VisitTypeIs(TypeBinaryExpression type)
		{
			this.Visit(type.Expression);
		}

		protected virtual void VisitConstant(ConstantExpression constant)
		{
		}

		protected virtual void VisitConditional(ConditionalExpression conditional)
		{
			this.Visit(conditional.Test);
			this.Visit(conditional.IfTrue);
			this.Visit(conditional.IfFalse);
		}

		protected virtual void VisitParameter(ParameterExpression parameter)
		{
		}

		protected virtual void VisitMemberAccess(MemberExpression member)
		{
			this.Visit(member.Expression);
		}

		protected virtual void VisitMethodCall(MethodCallExpression methodCall)
		{
			this.Visit(methodCall.Object);
			this.VisitExpressionList(methodCall.Arguments);
		}

		protected virtual void VisitList<T>(ReadOnlyCollection<T> list, Action<T> visitor)
		{
			foreach (T obj in list)
			{
				visitor(obj);
			}
		}

		protected virtual void VisitExpressionList(ReadOnlyCollection<Expression> list)
		{
			this.VisitList<Expression>(list, new Action<Expression>(this.Visit));
		}

		protected virtual void VisitMemberAssignment(MemberAssignment assignment)
		{
			this.Visit(assignment.Expression);
		}

		protected virtual void VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			this.VisitBindingList(binding.Bindings);
		}

		protected virtual void VisitMemberListBinding(MemberListBinding binding)
		{
			this.VisitElementInitializerList(binding.Initializers);
		}

		protected virtual void VisitBindingList(ReadOnlyCollection<MemberBinding> list)
		{
			this.VisitList<MemberBinding>(list, new Action<MemberBinding>(this.VisitBinding));
		}

		protected virtual void VisitElementInitializerList(ReadOnlyCollection<ElementInit> list)
		{
			this.VisitList<ElementInit>(list, new Action<ElementInit>(this.VisitElementInitializer));
		}

		protected virtual void VisitLambda(LambdaExpression lambda)
		{
			this.Visit(lambda.Body);
		}

		protected virtual void VisitNew(NewExpression nex)
		{
			this.VisitExpressionList(nex.Arguments);
		}

		protected virtual void VisitMemberInit(MemberInitExpression init)
		{
			this.VisitNew(init.NewExpression);
			this.VisitBindingList(init.Bindings);
		}

		protected virtual void VisitListInit(ListInitExpression init)
		{
			this.VisitNew(init.NewExpression);
			this.VisitElementInitializerList(init.Initializers);
		}

		protected virtual void VisitNewArray(NewArrayExpression newArray)
		{
			this.VisitExpressionList(newArray.Expressions);
		}

		protected virtual void VisitInvocation(InvocationExpression invocation)
		{
			this.VisitExpressionList(invocation.Arguments);
			this.Visit(invocation.Expression);
		}
	}
}
