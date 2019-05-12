using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
	internal class EmitContext
	{
		private CompilationContext context;

		private EmitContext parent;

		private LambdaExpression lambda;

		private DynamicMethod method;

		private LocalBuilder hoisted_store;

		private List<ParameterExpression> hoisted;

		public readonly ILGenerator ig;

		public EmitContext(CompilationContext context, EmitContext parent, LambdaExpression lambda)
		{
			this.context = context;
			this.parent = parent;
			this.lambda = lambda;
			this.hoisted = context.GetHoistedLocals(lambda);
			this.method = new DynamicMethod("lambda_method", lambda.GetReturnType(), EmitContext.CreateParameterTypes(lambda.Parameters), typeof(ExecutionScope), true);
			this.ig = this.method.GetILGenerator();
		}

		public bool HasHoistedLocals
		{
			get
			{
				return this.hoisted != null && this.hoisted.Count > 0;
			}
		}

		public LambdaExpression Lambda
		{
			get
			{
				return this.lambda;
			}
		}

		public void Emit()
		{
			if (this.HasHoistedLocals)
			{
				this.EmitStoreHoistedLocals();
			}
			this.lambda.EmitBody(this);
		}

		private static Type[] CreateParameterTypes(IList<ParameterExpression> parameters)
		{
			Type[] array = new Type[parameters.Count + 1];
			array[0] = typeof(ExecutionScope);
			for (int i = 0; i < parameters.Count; i++)
			{
				array[i + 1] = parameters[i].Type;
			}
			return array;
		}

		public bool IsLocalParameter(ParameterExpression parameter, ref int position)
		{
			position = this.lambda.Parameters.IndexOf(parameter);
			if (position > -1)
			{
				position++;
				return true;
			}
			return false;
		}

		public Delegate CreateDelegate(ExecutionScope scope)
		{
			return this.method.CreateDelegate(this.lambda.Type, scope);
		}

		public void Emit(Expression expression)
		{
			expression.Emit(this);
		}

		public LocalBuilder EmitStored(Expression expression)
		{
			LocalBuilder localBuilder = this.ig.DeclareLocal(expression.Type);
			expression.Emit(this);
			this.ig.Emit(OpCodes.Stloc, localBuilder);
			return localBuilder;
		}

		public void EmitLoadAddress(Expression expression)
		{
			this.ig.Emit(OpCodes.Ldloca, this.EmitStored(expression));
		}

		public void EmitLoadSubject(Expression expression)
		{
			if (expression.Type.IsValueType)
			{
				this.EmitLoadAddress(expression);
				return;
			}
			this.Emit(expression);
		}

		public void EmitLoadSubject(LocalBuilder local)
		{
			if (local.LocalType.IsValueType)
			{
				this.EmitLoadAddress(local);
				return;
			}
			this.EmitLoad(local);
		}

		public void EmitLoadAddress(LocalBuilder local)
		{
			this.ig.Emit(OpCodes.Ldloca, local);
		}

		public void EmitLoad(LocalBuilder local)
		{
			this.ig.Emit(OpCodes.Ldloc, local);
		}

		public void EmitCall(LocalBuilder local, IList<Expression> arguments, MethodInfo method)
		{
			this.EmitLoadSubject(local);
			this.EmitArguments(method, arguments);
			this.EmitCall(method);
		}

		public void EmitCall(LocalBuilder local, MethodInfo method)
		{
			this.EmitLoadSubject(local);
			this.EmitCall(method);
		}

		public void EmitCall(Expression expression, MethodInfo method)
		{
			if (!method.IsStatic)
			{
				this.EmitLoadSubject(expression);
			}
			this.EmitCall(method);
		}

		public void EmitCall(Expression expression, IList<Expression> arguments, MethodInfo method)
		{
			if (!method.IsStatic)
			{
				this.EmitLoadSubject(expression);
			}
			this.EmitArguments(method, arguments);
			this.EmitCall(method);
		}

		private void EmitArguments(MethodInfo method, IList<Expression> arguments)
		{
			ParameterInfo[] parameters = method.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Expression expression = arguments[i];
				if (parameterInfo.ParameterType.IsByRef)
				{
					this.ig.Emit(OpCodes.Ldloca, this.EmitStored(expression));
				}
				else
				{
					this.Emit(arguments[i]);
				}
			}
		}

		public void EmitCall(MethodInfo method)
		{
			this.ig.Emit((!method.IsVirtual) ? OpCodes.Call : OpCodes.Callvirt, method);
		}

		public void EmitNullableHasValue(LocalBuilder local)
		{
			this.EmitCall(local, "get_HasValue");
		}

		public void EmitNullableInitialize(LocalBuilder local)
		{
			this.ig.Emit(OpCodes.Ldloca, local);
			this.ig.Emit(OpCodes.Initobj, local.LocalType);
			this.ig.Emit(OpCodes.Ldloc, local);
		}

		public void EmitNullableGetValue(LocalBuilder local)
		{
			this.EmitCall(local, "get_Value");
		}

		public void EmitNullableGetValueOrDefault(LocalBuilder local)
		{
			this.EmitCall(local, "GetValueOrDefault");
		}

		private void EmitCall(LocalBuilder local, string method_name)
		{
			this.EmitCall(local, local.LocalType.GetMethod(method_name, Type.EmptyTypes));
		}

		public void EmitNullableNew(Type of)
		{
			this.ig.Emit(OpCodes.Newobj, of.GetConstructor(new Type[]
			{
				of.GetFirstGenericArgument()
			}));
		}

		public void EmitCollection<T>(IEnumerable<T> collection) where T : Expression
		{
			foreach (T t in collection)
			{
				t.Emit(this);
			}
		}

		public void EmitCollection(IEnumerable<ElementInit> initializers, LocalBuilder local)
		{
			foreach (ElementInit elementInit in initializers)
			{
				elementInit.Emit(this, local);
			}
		}

		public void EmitCollection(IEnumerable<MemberBinding> bindings, LocalBuilder local)
		{
			foreach (MemberBinding memberBinding in bindings)
			{
				memberBinding.Emit(this, local);
			}
		}

		public void EmitIsInst(Expression expression, Type candidate)
		{
			expression.Emit(this);
			Type type = expression.Type;
			if (type.IsValueType)
			{
				this.ig.Emit(OpCodes.Box, type);
			}
			this.ig.Emit(OpCodes.Isinst, candidate);
		}

		public void EmitScope()
		{
			this.ig.Emit(OpCodes.Ldarg_0);
		}

		public void EmitReadGlobal(object global)
		{
			this.EmitReadGlobal(global, global.GetType());
		}

		public void EmitLoadGlobals()
		{
			this.EmitScope();
			this.ig.Emit(OpCodes.Ldfld, typeof(ExecutionScope).GetField("Globals"));
		}

		public void EmitReadGlobal(object global, Type type)
		{
			this.EmitLoadGlobals();
			this.ig.Emit(OpCodes.Ldc_I4, this.AddGlobal(global, type));
			this.ig.Emit(OpCodes.Ldelem, typeof(object));
			this.EmitLoadStrongBoxValue(type);
		}

		public void EmitLoadStrongBoxValue(Type type)
		{
			Type type2 = type.MakeStrongBoxType();
			this.ig.Emit(OpCodes.Isinst, type2);
			this.ig.Emit(OpCodes.Ldfld, type2.GetField("Value"));
		}

		private int AddGlobal(object value, Type type)
		{
			return this.context.AddGlobal(EmitContext.CreateStrongBox(value, type));
		}

		public void EmitCreateDelegate(LambdaExpression lambda)
		{
			this.EmitScope();
			this.ig.Emit(OpCodes.Ldc_I4, this.AddChildContext(lambda));
			if (this.hoisted_store != null)
			{
				this.ig.Emit(OpCodes.Ldloc, this.hoisted_store);
			}
			else
			{
				this.ig.Emit(OpCodes.Ldnull);
			}
			this.ig.Emit(OpCodes.Callvirt, typeof(ExecutionScope).GetMethod("CreateDelegate"));
			this.ig.Emit(OpCodes.Castclass, lambda.Type);
		}

		private void EmitStoreHoistedLocals()
		{
			this.EmitHoistedLocalsStore();
			for (int i = 0; i < this.hoisted.Count; i++)
			{
				this.EmitStoreHoistedLocal(i, this.hoisted[i]);
			}
		}

		private void EmitStoreHoistedLocal(int position, ParameterExpression parameter)
		{
			this.ig.Emit(OpCodes.Ldloc, this.hoisted_store);
			this.ig.Emit(OpCodes.Ldc_I4, position);
			parameter.Emit(this);
			this.EmitCreateStrongBox(parameter.Type);
			this.ig.Emit(OpCodes.Stelem, typeof(object));
		}

		public void EmitLoadHoistedLocalsStore()
		{
			this.ig.Emit(OpCodes.Ldloc, this.hoisted_store);
		}

		private void EmitCreateStrongBox(Type type)
		{
			this.ig.Emit(OpCodes.Newobj, type.MakeStrongBoxType().GetConstructor(new Type[]
			{
				type
			}));
		}

		private void EmitHoistedLocalsStore()
		{
			this.EmitScope();
			this.hoisted_store = this.ig.DeclareLocal(typeof(object[]));
			this.ig.Emit(OpCodes.Callvirt, typeof(ExecutionScope).GetMethod("CreateHoistedLocals"));
			this.ig.Emit(OpCodes.Stloc, this.hoisted_store);
		}

		public void EmitLoadLocals()
		{
			this.ig.Emit(OpCodes.Ldfld, typeof(ExecutionScope).GetField("Locals"));
		}

		public void EmitParentScope()
		{
			this.ig.Emit(OpCodes.Ldfld, typeof(ExecutionScope).GetField("Parent"));
		}

		public void EmitIsolateExpression()
		{
			this.ig.Emit(OpCodes.Callvirt, typeof(ExecutionScope).GetMethod("IsolateExpression"));
		}

		public int IndexOfHoistedLocal(ParameterExpression parameter)
		{
			if (!this.HasHoistedLocals)
			{
				return -1;
			}
			return this.hoisted.IndexOf(parameter);
		}

		public bool IsHoistedLocal(ParameterExpression parameter, ref int level, ref int position)
		{
			if (this.parent == null)
			{
				return false;
			}
			if (this.parent.hoisted != null)
			{
				position = this.parent.hoisted.IndexOf(parameter);
				if (position > -1)
				{
					return true;
				}
			}
			level++;
			return this.parent.IsHoistedLocal(parameter, ref level, ref position);
		}

		private int AddChildContext(LambdaExpression lambda)
		{
			return this.context.AddCompilationUnit(this, lambda);
		}

		private static object CreateStrongBox(object value, Type type)
		{
			return Activator.CreateInstance(type.MakeStrongBoxType(), new object[]
			{
				value
			});
		}
	}
}
