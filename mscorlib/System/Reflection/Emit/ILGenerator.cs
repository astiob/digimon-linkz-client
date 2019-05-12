using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Generates Microsoft intermediate language (MSIL) instructions.</summary>
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_ILGenerator))]
	public class ILGenerator : _ILGenerator
	{
		private const int defaultFixupSize = 4;

		private const int defaultLabelsSize = 4;

		private const int defaultExceptionStackSize = 2;

		private static readonly Type void_type = typeof(void);

		private byte[] code;

		private int code_len;

		private int max_stack;

		private int cur_stack;

		private LocalBuilder[] locals;

		private ILExceptionInfo[] ex_handlers;

		private int num_token_fixups;

		private ILTokenInfo[] token_fixups;

		private ILGenerator.LabelData[] labels;

		private int num_labels;

		private ILGenerator.LabelFixup[] fixups;

		private int num_fixups;

		internal Module module;

		private int cur_block;

		private Stack open_blocks;

		private TokenGenerator token_gen;

		private ArrayList sequencePointLists;

		private SequencePointList currentSequence;

		internal ILGenerator(Module m, TokenGenerator token_gen, int size)
		{
			if (size < 0)
			{
				size = 128;
			}
			this.code = new byte[size];
			this.token_fixups = new ILTokenInfo[8];
			this.module = m;
			this.token_gen = token_gen;
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array that receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _ILGenerator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _ILGenerator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _ILGenerator.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Provides access to properties and methods exposed by an object.</summary>
		/// <param name="dispIdMember">Identifies the member.</param>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="lcid">The locale context in which to interpret arguments.</param>
		/// <param name="wFlags">Flags describing the context of the call.</param>
		/// <param name="pDispParams">Pointer to a structure containing an array of arguments, an array of argument DISPIDs for named arguments, and counts for the number of elements in the arrays.</param>
		/// <param name="pVarResult">Pointer to the location where the result is to be stored.</param>
		/// <param name="pExcepInfo">Pointer to a structure that contains exception information.</param>
		/// <param name="puArgErr">The index of the first argument that has an error.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		void _ILGenerator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		private void add_token_fixup(MemberInfo mi)
		{
			if (this.num_token_fixups == this.token_fixups.Length)
			{
				ILTokenInfo[] array = new ILTokenInfo[this.num_token_fixups * 2];
				this.token_fixups.CopyTo(array, 0);
				this.token_fixups = array;
			}
			this.token_fixups[this.num_token_fixups].member = mi;
			this.token_fixups[this.num_token_fixups++].code_pos = this.code_len;
		}

		private void make_room(int nbytes)
		{
			if (this.code_len + nbytes < this.code.Length)
			{
				return;
			}
			byte[] destinationArray = new byte[(this.code_len + nbytes) * 2 + 128];
			Array.Copy(this.code, 0, destinationArray, 0, this.code.Length);
			this.code = destinationArray;
		}

		private void emit_int(int val)
		{
			this.code[this.code_len++] = (byte)(val & 255);
			this.code[this.code_len++] = (byte)(val >> 8 & 255);
			this.code[this.code_len++] = (byte)(val >> 16 & 255);
			this.code[this.code_len++] = (byte)(val >> 24 & 255);
		}

		private void ll_emit(OpCode opcode)
		{
			if (opcode.Size == 2)
			{
				this.code[this.code_len++] = opcode.op1;
			}
			this.code[this.code_len++] = opcode.op2;
			switch (opcode.StackBehaviourPush)
			{
			case StackBehaviour.Push1:
			case StackBehaviour.Pushi:
			case StackBehaviour.Pushi8:
			case StackBehaviour.Pushr4:
			case StackBehaviour.Pushr8:
			case StackBehaviour.Pushref:
			case StackBehaviour.Varpush:
				this.cur_stack++;
				break;
			case StackBehaviour.Push1_push1:
				this.cur_stack += 2;
				break;
			}
			if (this.max_stack < this.cur_stack)
			{
				this.max_stack = this.cur_stack;
			}
			switch (opcode.StackBehaviourPop)
			{
			case StackBehaviour.Pop1:
			case StackBehaviour.Popi:
			case StackBehaviour.Popref:
				this.cur_stack--;
				break;
			case StackBehaviour.Pop1_pop1:
			case StackBehaviour.Popi_pop1:
			case StackBehaviour.Popi_popi:
			case StackBehaviour.Popi_popi8:
			case StackBehaviour.Popi_popr4:
			case StackBehaviour.Popi_popr8:
			case StackBehaviour.Popref_pop1:
			case StackBehaviour.Popref_popi:
				this.cur_stack -= 2;
				break;
			case StackBehaviour.Popi_popi_popi:
			case StackBehaviour.Popref_popi_popi:
			case StackBehaviour.Popref_popi_popi8:
			case StackBehaviour.Popref_popi_popr4:
			case StackBehaviour.Popref_popi_popr8:
			case StackBehaviour.Popref_popi_popref:
				this.cur_stack -= 3;
				break;
			}
		}

		private static int target_len(OpCode opcode)
		{
			if (opcode.OperandType == OperandType.InlineBrTarget)
			{
				return 4;
			}
			return 1;
		}

		private void InternalEndClause()
		{
			int num = this.ex_handlers[this.cur_block].LastClauseType();
			switch (num + 1)
			{
			case 0:
			case 1:
			case 2:
				this.Emit(OpCodes.Leave, this.ex_handlers[this.cur_block].end);
				break;
			case 3:
			case 5:
				this.Emit(OpCodes.Endfinally);
				break;
			}
		}

		/// <summary>Begins a catch block.</summary>
		/// <param name="exceptionType">The <see cref="T:System.Type" /> object that represents the exception. </param>
		/// <exception cref="T:System.ArgumentException">The catch block is within a filtered exception. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="exceptionType" /> is null, and the exception filter block has not returned a value that indicates that finally blocks should be run until this catch block is located. </exception>
		/// <exception cref="T:System.NotSupportedException">The Microsoft intermediate language (MSIL) being generated is not currently in an exception block. </exception>
		public virtual void BeginCatchBlock(Type exceptionType)
		{
			if (this.open_blocks == null)
			{
				this.open_blocks = new Stack(2);
			}
			if (this.open_blocks.Count <= 0)
			{
				throw new NotSupportedException("Not in an exception block");
			}
			if (exceptionType != null && exceptionType.IsUserType)
			{
				throw new NotSupportedException("User defined subclasses of System.Type are not yet supported.");
			}
			if (this.ex_handlers[this.cur_block].LastClauseType() == -1)
			{
				if (exceptionType != null)
				{
					throw new ArgumentException("Do not supply an exception type for filter clause");
				}
				this.Emit(OpCodes.Endfilter);
				this.ex_handlers[this.cur_block].PatchFilterClause(this.code_len);
			}
			else
			{
				this.InternalEndClause();
				this.ex_handlers[this.cur_block].AddCatch(exceptionType, this.code_len);
			}
			this.cur_stack = 1;
			if (this.max_stack < this.cur_stack)
			{
				this.max_stack = this.cur_stack;
			}
		}

		/// <summary>Begins an exception block for a filtered exception.</summary>
		/// <exception cref="T:System.NotSupportedException">The Microsoft intermediate language (MSIL) being generated is not currently in an exception block. -or-This <see cref="T:System.Reflection.Emit.ILGenerator" /> belongs to a <see cref="T:System.Reflection.Emit.DynamicMethod" />.</exception>
		public virtual void BeginExceptFilterBlock()
		{
			if (this.open_blocks == null)
			{
				this.open_blocks = new Stack(2);
			}
			if (this.open_blocks.Count <= 0)
			{
				throw new NotSupportedException("Not in an exception block");
			}
			this.InternalEndClause();
			this.ex_handlers[this.cur_block].AddFilter(this.code_len);
		}

		/// <summary>Begins an exception block for a non-filtered exception.</summary>
		/// <returns>The label for the end of the block. This will leave you in the correct place to execute finally blocks or to finish the try.</returns>
		public virtual Label BeginExceptionBlock()
		{
			if (this.open_blocks == null)
			{
				this.open_blocks = new Stack(2);
			}
			if (this.ex_handlers != null)
			{
				this.cur_block = this.ex_handlers.Length;
				ILExceptionInfo[] destinationArray = new ILExceptionInfo[this.cur_block + 1];
				Array.Copy(this.ex_handlers, destinationArray, this.cur_block);
				this.ex_handlers = destinationArray;
			}
			else
			{
				this.ex_handlers = new ILExceptionInfo[1];
				this.cur_block = 0;
			}
			this.open_blocks.Push(this.cur_block);
			this.ex_handlers[this.cur_block].start = this.code_len;
			return this.ex_handlers[this.cur_block].end = this.DefineLabel();
		}

		/// <summary>Begins an exception fault block in the Microsoft intermediate language (MSIL) stream.</summary>
		/// <exception cref="T:System.NotSupportedException">The MSIL being generated is not currently in an exception block. -or-This <see cref="T:System.Reflection.Emit.ILGenerator" /> belongs to a <see cref="T:System.Reflection.Emit.DynamicMethod" />.</exception>
		public virtual void BeginFaultBlock()
		{
			if (this.open_blocks == null)
			{
				this.open_blocks = new Stack(2);
			}
			if (this.open_blocks.Count <= 0)
			{
				throw new NotSupportedException("Not in an exception block");
			}
			if (this.ex_handlers[this.cur_block].LastClauseType() == -1)
			{
				this.Emit(OpCodes.Leave, this.ex_handlers[this.cur_block].end);
				this.ex_handlers[this.cur_block].PatchFilterClause(this.code_len);
			}
			this.InternalEndClause();
			this.ex_handlers[this.cur_block].AddFault(this.code_len);
		}

		/// <summary>Begins a finally block in the Microsoft intermediate language (MSIL) instruction stream.</summary>
		/// <exception cref="T:System.NotSupportedException">The MSIL being generated is not currently in an exception block. </exception>
		public virtual void BeginFinallyBlock()
		{
			if (this.open_blocks == null)
			{
				this.open_blocks = new Stack(2);
			}
			if (this.open_blocks.Count <= 0)
			{
				throw new NotSupportedException("Not in an exception block");
			}
			this.InternalEndClause();
			if (this.ex_handlers[this.cur_block].LastClauseType() == -1)
			{
				this.Emit(OpCodes.Leave, this.ex_handlers[this.cur_block].end);
				this.ex_handlers[this.cur_block].PatchFilterClause(this.code_len);
			}
			this.ex_handlers[this.cur_block].AddFinally(this.code_len);
		}

		/// <summary>Begins a lexical scope.</summary>
		/// <exception cref="T:System.NotSupportedException">This <see cref="T:System.Reflection.Emit.ILGenerator" /> belongs to a <see cref="T:System.Reflection.Emit.DynamicMethod" />.</exception>
		public virtual void BeginScope()
		{
		}

		/// <summary>Declares a local variable of the specified type.</summary>
		/// <returns>The declared local variable.</returns>
		/// <param name="localType">A <see cref="T:System.Type" /> object that represents the type of the local variable. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="localType" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created by the <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> method. </exception>
		public virtual LocalBuilder DeclareLocal(Type localType)
		{
			return this.DeclareLocal(localType, false);
		}

		/// <summary>Declares a local variable of the specified type, optionally pinning the object referred to by the variable.</summary>
		/// <returns>A <see cref="T:System.Reflection.Emit.LocalBuilder" /> object that represents the local variable.</returns>
		/// <param name="localType">A <see cref="T:System.Type" /> object that represents the type of the local variable.</param>
		/// <param name="pinned">true to pin the object in memory; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="localType" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The containing type has been created by the <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> method.-or-The method body of the enclosing method has been created by the <see cref="M:System.Reflection.Emit.MethodBuilder.CreateMethodBody(System.Byte[],System.Int32)" /> method. </exception>
		/// <exception cref="T:System.NotSupportedException">The method with which this <see cref="T:System.Reflection.Emit.ILGenerator" /> is associated is not represented by a <see cref="T:System.Reflection.Emit.MethodBuilder" />.</exception>
		public virtual LocalBuilder DeclareLocal(Type localType, bool pinned)
		{
			if (localType == null)
			{
				throw new ArgumentNullException("localType");
			}
			if (localType.IsUserType)
			{
				throw new NotSupportedException("User defined subclasses of System.Type are not yet supported.");
			}
			LocalBuilder localBuilder = new LocalBuilder(localType, this);
			localBuilder.is_pinned = pinned;
			if (this.locals != null)
			{
				LocalBuilder[] array = new LocalBuilder[this.locals.Length + 1];
				Array.Copy(this.locals, array, this.locals.Length);
				array[this.locals.Length] = localBuilder;
				this.locals = array;
			}
			else
			{
				this.locals = new LocalBuilder[1];
				this.locals[0] = localBuilder;
			}
			localBuilder.position = (ushort)(this.locals.Length - 1);
			return localBuilder;
		}

		/// <summary>Declares a new label.</summary>
		/// <returns>Returns a new label that can be used as a token for branching.</returns>
		public virtual Label DefineLabel()
		{
			if (this.labels == null)
			{
				this.labels = new ILGenerator.LabelData[4];
			}
			else if (this.num_labels >= this.labels.Length)
			{
				ILGenerator.LabelData[] destinationArray = new ILGenerator.LabelData[this.labels.Length * 2];
				Array.Copy(this.labels, destinationArray, this.labels.Length);
				this.labels = destinationArray;
			}
			this.labels[this.num_labels] = new ILGenerator.LabelData(-1, 0);
			return new Label(this.num_labels++);
		}

		/// <summary>Puts the specified instruction onto the stream of instructions.</summary>
		/// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream. </param>
		public virtual void Emit(OpCode opcode)
		{
			this.make_room(2);
			this.ll_emit(opcode);
		}

		/// <summary>Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
		/// <param name="arg">The character argument pushed onto the stream immediately after the instruction. </param>
		public virtual void Emit(OpCode opcode, byte arg)
		{
			this.make_room(3);
			this.ll_emit(opcode);
			this.code[this.code_len++] = arg;
		}

		/// <summary>Puts the specified instruction and metadata token for the specified constructor onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="con">A ConstructorInfo representing a constructor. </param>
		[ComVisible(true)]
		public virtual void Emit(OpCode opcode, ConstructorInfo con)
		{
			int token = this.token_gen.GetToken(con);
			this.make_room(6);
			this.ll_emit(opcode);
			if (con.DeclaringType.Module == this.module)
			{
				this.add_token_fixup(con);
			}
			this.emit_int(token);
			if (opcode.StackBehaviourPop == StackBehaviour.Varpop)
			{
				this.cur_stack -= con.GetParameterCount();
			}
		}

		/// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be put onto the stream. Defined in the OpCodes enumeration. </param>
		/// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction. </param>
		public virtual void Emit(OpCode opcode, double arg)
		{
			byte[] bytes = BitConverter.GetBytes(arg);
			this.make_room(10);
			this.ll_emit(opcode);
			if (BitConverter.IsLittleEndian)
			{
				Array.Copy(bytes, 0, this.code, this.code_len, 8);
				this.code_len += 8;
			}
			else
			{
				this.code[this.code_len++] = bytes[7];
				this.code[this.code_len++] = bytes[6];
				this.code[this.code_len++] = bytes[5];
				this.code[this.code_len++] = bytes[4];
				this.code[this.code_len++] = bytes[3];
				this.code[this.code_len++] = bytes[2];
				this.code[this.code_len++] = bytes[1];
				this.code[this.code_len++] = bytes[0];
			}
		}

		/// <summary>Puts the specified instruction and metadata token for the specified field onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="field">A FieldInfo representing a field. </param>
		public virtual void Emit(OpCode opcode, FieldInfo field)
		{
			int token = this.token_gen.GetToken(field);
			this.make_room(6);
			this.ll_emit(opcode);
			if (field.DeclaringType.Module == this.module)
			{
				this.add_token_fixup(field);
			}
			this.emit_int(token);
		}

		/// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="arg">The Int argument pushed onto the stream immediately after the instruction. </param>
		public virtual void Emit(OpCode opcode, short arg)
		{
			this.make_room(4);
			this.ll_emit(opcode);
			this.code[this.code_len++] = (byte)(arg & 255);
			this.code[this.code_len++] = (byte)(arg >> 8 & 255);
		}

		/// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
		/// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction. </param>
		public virtual void Emit(OpCode opcode, int arg)
		{
			this.make_room(6);
			this.ll_emit(opcode);
			this.emit_int(arg);
		}

		/// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
		/// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction. </param>
		public virtual void Emit(OpCode opcode, long arg)
		{
			this.make_room(10);
			this.ll_emit(opcode);
			this.code[this.code_len++] = (byte)(arg & 255L);
			this.code[this.code_len++] = (byte)(arg >> 8 & 255L);
			this.code[this.code_len++] = (byte)(arg >> 16 & 255L);
			this.code[this.code_len++] = (byte)(arg >> 24 & 255L);
			this.code[this.code_len++] = (byte)(arg >> 32 & 255L);
			this.code[this.code_len++] = (byte)(arg >> 40 & 255L);
			this.code[this.code_len++] = (byte)(arg >> 48 & 255L);
			this.code[this.code_len++] = (byte)(arg >> 56 & 255L);
		}

		/// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="label">The label to which to branch from this location. </param>
		public virtual void Emit(OpCode opcode, Label label)
		{
			int num = ILGenerator.target_len(opcode);
			this.make_room(6);
			this.ll_emit(opcode);
			if (this.cur_stack > this.labels[label.label].maxStack)
			{
				this.labels[label.label].maxStack = this.cur_stack;
			}
			if (this.fixups == null)
			{
				this.fixups = new ILGenerator.LabelFixup[4];
			}
			else if (this.num_fixups >= this.fixups.Length)
			{
				ILGenerator.LabelFixup[] destinationArray = new ILGenerator.LabelFixup[this.fixups.Length * 2];
				Array.Copy(this.fixups, destinationArray, this.fixups.Length);
				this.fixups = destinationArray;
			}
			this.fixups[this.num_fixups].offset = num;
			this.fixups[this.num_fixups].pos = this.code_len;
			this.fixups[this.num_fixups].label_idx = label.label;
			this.num_fixups++;
			this.code_len += num;
		}

		/// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to include a label when fixes are done.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="labels">The array of label objects to which to branch from this location. All of the labels will be used. </param>
		public virtual void Emit(OpCode opcode, Label[] labels)
		{
			if (labels == null)
			{
				throw new ArgumentNullException("labels");
			}
			int num = labels.Length;
			this.make_room(6 + num * 4);
			this.ll_emit(opcode);
			for (int i = 0; i < num; i++)
			{
				if (this.cur_stack > this.labels[labels[i].label].maxStack)
				{
					this.labels[labels[i].label].maxStack = this.cur_stack;
				}
			}
			this.emit_int(num);
			if (this.fixups == null)
			{
				this.fixups = new ILGenerator.LabelFixup[4 + num];
			}
			else if (this.num_fixups + num >= this.fixups.Length)
			{
				ILGenerator.LabelFixup[] destinationArray = new ILGenerator.LabelFixup[num + this.fixups.Length * 2];
				Array.Copy(this.fixups, destinationArray, this.fixups.Length);
				this.fixups = destinationArray;
			}
			int j = 0;
			int num2 = num * 4;
			while (j < num)
			{
				this.fixups[this.num_fixups].offset = num2;
				this.fixups[this.num_fixups].pos = this.code_len;
				this.fixups[this.num_fixups].label_idx = labels[j].label;
				this.num_fixups++;
				this.code_len += 4;
				j++;
				num2 -= 4;
			}
		}

		/// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the index of the given local variable.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="local">A local variable. </param>
		/// <exception cref="T:System.ArgumentException">The parent method of the <paramref name="local" /> parameter does not match the method associated with this <see cref="T:System.Reflection.Emit.ILGenerator" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="local" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="opcode" /> is a single-byte instruction, and <paramref name="local" /> represents a local variable with an index greater than Byte.MaxValue. </exception>
		public virtual void Emit(OpCode opcode, LocalBuilder local)
		{
			if (local == null)
			{
				throw new ArgumentNullException("local");
			}
			uint position = (uint)local.position;
			bool flag = false;
			bool flag2 = false;
			this.make_room(6);
			if (local.ilgen != this)
			{
				throw new ArgumentException("Trying to emit a local from a different ILGenerator.");
			}
			if (opcode.StackBehaviourPop == StackBehaviour.Pop1)
			{
				this.cur_stack--;
				flag2 = true;
			}
			else
			{
				this.cur_stack++;
				if (this.cur_stack > this.max_stack)
				{
					this.max_stack = this.cur_stack;
				}
				flag = (opcode.StackBehaviourPush == StackBehaviour.Pushi);
			}
			if (flag)
			{
				if (position < 256u)
				{
					this.code[this.code_len++] = 18;
					this.code[this.code_len++] = (byte)position;
				}
				else
				{
					this.code[this.code_len++] = 254;
					this.code[this.code_len++] = 13;
					this.code[this.code_len++] = (byte)(position & 255u);
					this.code[this.code_len++] = (byte)(position >> 8 & 255u);
				}
			}
			else if (flag2)
			{
				if (position < 4u)
				{
					this.code[this.code_len++] = (byte)(10u + position);
				}
				else if (position < 256u)
				{
					this.code[this.code_len++] = 19;
					this.code[this.code_len++] = (byte)position;
				}
				else
				{
					this.code[this.code_len++] = 254;
					this.code[this.code_len++] = 14;
					this.code[this.code_len++] = (byte)(position & 255u);
					this.code[this.code_len++] = (byte)(position >> 8 & 255u);
				}
			}
			else if (position < 4u)
			{
				this.code[this.code_len++] = (byte)(6u + position);
			}
			else if (position < 256u)
			{
				this.code[this.code_len++] = 17;
				this.code[this.code_len++] = (byte)position;
			}
			else
			{
				this.code[this.code_len++] = 254;
				this.code[this.code_len++] = 12;
				this.code[this.code_len++] = (byte)(position & 255u);
				this.code[this.code_len++] = (byte)(position >> 8 & 255u);
			}
		}

		/// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given method.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="meth">A MethodInfo representing a method. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="meth" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="meth" /> is a generic method for which the <see cref="P:System.Reflection.MethodInfo.IsGenericMethodDefinition" /> property is false.</exception>
		public virtual void Emit(OpCode opcode, MethodInfo meth)
		{
			if (meth == null)
			{
				throw new ArgumentNullException("meth");
			}
			if (meth is DynamicMethod && (opcode == OpCodes.Ldftn || opcode == OpCodes.Ldvirtftn || opcode == OpCodes.Ldtoken))
			{
				throw new ArgumentException("Ldtoken, Ldftn and Ldvirtftn OpCodes cannot target DynamicMethods.");
			}
			int token = this.token_gen.GetToken(meth);
			this.make_room(6);
			this.ll_emit(opcode);
			Type declaringType = meth.DeclaringType;
			if (declaringType != null && declaringType.Module == this.module)
			{
				this.add_token_fixup(meth);
			}
			this.emit_int(token);
			if (meth.ReturnType != ILGenerator.void_type)
			{
				this.cur_stack++;
			}
			if (opcode.StackBehaviourPop == StackBehaviour.Varpop)
			{
				this.cur_stack -= meth.GetParameterCount();
			}
		}

		private void Emit(OpCode opcode, MethodInfo method, int token)
		{
			this.make_room(6);
			this.ll_emit(opcode);
			Type declaringType = method.DeclaringType;
			if (declaringType != null && declaringType.Module == this.module)
			{
				this.add_token_fixup(method);
			}
			this.emit_int(token);
			if (method.ReturnType != ILGenerator.void_type)
			{
				this.cur_stack++;
			}
			if (opcode.StackBehaviourPop == StackBehaviour.Varpop)
			{
				this.cur_stack -= method.GetParameterCount();
			}
		}

		/// <summary>Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
		/// <param name="arg">The character argument pushed onto the stream immediately after the instruction. </param>
		[CLSCompliant(false)]
		public void Emit(OpCode opcode, sbyte arg)
		{
			this.make_room(3);
			this.ll_emit(opcode);
			this.code[this.code_len++] = (byte)arg;
		}

		/// <summary>Puts the specified instruction and a signature token onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="signature">A helper for constructing a signature token. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="signature" /> is null. </exception>
		public virtual void Emit(OpCode opcode, SignatureHelper signature)
		{
			int token = this.token_gen.GetToken(signature);
			this.make_room(6);
			this.ll_emit(opcode);
			this.emit_int(token);
		}

		/// <summary>Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream of instructions.</summary>
		/// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
		/// <param name="arg">The Single argument pushed onto the stream immediately after the instruction. </param>
		public virtual void Emit(OpCode opcode, float arg)
		{
			byte[] bytes = BitConverter.GetBytes(arg);
			this.make_room(6);
			this.ll_emit(opcode);
			if (BitConverter.IsLittleEndian)
			{
				Array.Copy(bytes, 0, this.code, this.code_len, 4);
				this.code_len += 4;
			}
			else
			{
				this.code[this.code_len++] = bytes[3];
				this.code[this.code_len++] = bytes[2];
				this.code[this.code_len++] = bytes[1];
				this.code[this.code_len++] = bytes[0];
			}
		}

		/// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given string.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. </param>
		/// <param name="str">The String to be emitted. </param>
		public virtual void Emit(OpCode opcode, string str)
		{
			int token = this.token_gen.GetToken(str);
			this.make_room(6);
			this.ll_emit(opcode);
			this.emit_int(token);
		}

		/// <summary>Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the metadata token for the given type.</summary>
		/// <param name="opcode">The MSIL instruction to be put onto the stream. </param>
		/// <param name="cls">A Type. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cls" /> is null. </exception>
		public virtual void Emit(OpCode opcode, Type cls)
		{
			this.make_room(6);
			this.ll_emit(opcode);
			this.emit_int(this.token_gen.GetToken(cls));
		}

		/// <summary>Puts a call or callvirt instruction onto the Microsoft intermediate language (MSIL) stream to call a varargs method.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. Must be <see cref="F:System.Reflection.Emit.OpCodes.Call" />, <see cref="F:System.Reflection.Emit.OpCodes.Callvirt" />, or <see cref="F:System.Reflection.Emit.OpCodes.Newobj" />.</param>
		/// <param name="methodInfo">The varargs method to be called. </param>
		/// <param name="optionalParameterTypes">The types of the optional arguments if the method is a varargs method; otherwise, null. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="methodInfo" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The calling convention for the method is not varargs, but optional parameter types are supplied. This exception is thrown in the .NET Framework versions 1.0 and 1.1, In subsequent versions, no exception is thrown.</exception>
		[MonoLimitation("vararg methods are not supported")]
		public virtual void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			short value = opcode.Value;
			if (value != OpCodes.Call.Value && value != OpCodes.Callvirt.Value)
			{
				throw new NotSupportedException("Only Call and CallVirt are allowed");
			}
			if ((methodInfo.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				optionalParameterTypes = null;
			}
			if (optionalParameterTypes == null)
			{
				this.Emit(opcode, methodInfo);
				return;
			}
			if ((methodInfo.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				throw new InvalidOperationException("Method is not VarArgs method and optional types were passed");
			}
			int token = this.token_gen.GetToken(methodInfo, optionalParameterTypes);
			this.Emit(opcode, methodInfo, token);
		}

		/// <summary>Puts a <see cref="F:System.Reflection.Emit.OpCodes.Calli" /> instruction onto the Microsoft intermediate language (MSIL) stream, specifying an unmanaged calling convention for the indirect call.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. Must be <see cref="F:System.Reflection.Emit.OpCodes.Calli" />.</param>
		/// <param name="unmanagedCallConv">The unmanaged calling convention to be used. </param>
		/// <param name="returnType">The <see cref="T:System.Type" /> of the result. </param>
		/// <param name="parameterTypes">The types of the required arguments to the instruction. </param>
		public virtual void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(this.module, (CallingConventions)0, unmanagedCallConv, returnType, parameterTypes);
			this.Emit(opcode, methodSigHelper);
		}

		/// <summary>Puts a <see cref="F:System.Reflection.Emit.OpCodes.Calli" /> instruction onto the Microsoft intermediate language (MSIL) stream, specifying a managed calling convention for the indirect call.</summary>
		/// <param name="opcode">The MSIL instruction to be emitted onto the stream. Must be <see cref="F:System.Reflection.Emit.OpCodes.Calli" />. </param>
		/// <param name="callingConvention">The managed calling convention to be used. </param>
		/// <param name="returnType">The <see cref="T:System.Type" /> of the result. </param>
		/// <param name="parameterTypes">The types of the required arguments to the instruction. </param>
		/// <param name="optionalParameterTypes">The types of the optional arguments for varargs calls. </param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="optionalParameterTypes" /> is not null, but <paramref name="callingConvention" /> does not include the <see cref="F:System.Reflection.CallingConventions.VarArgs" /> flag.</exception>
		public virtual void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			if (optionalParameterTypes != null)
			{
				throw new NotImplementedException();
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(this.module, callingConvention, (CallingConvention)0, returnType, parameterTypes);
			this.Emit(opcode, methodSigHelper);
		}

		/// <summary>Emits the Microsoft intermediate language (MSIL) necessary to call <see cref="Overload:System.Console.WriteLine" /> with the given field.</summary>
		/// <param name="fld">The field whose value is to be written to the console. </param>
		/// <exception cref="T:System.ArgumentException">There is no overload of the <see cref="Overload:System.Console.WriteLine" /> method that accepts the type of the specified field. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fld" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">The type of the field is <see cref="T:System.Reflection.Emit.TypeBuilder" /> or <see cref="T:System.Reflection.Emit.EnumBuilder" />, which are not supported. </exception>
		public virtual void EmitWriteLine(FieldInfo fld)
		{
			if (fld == null)
			{
				throw new ArgumentNullException("fld");
			}
			if (fld.IsStatic)
			{
				this.Emit(OpCodes.Ldsfld, fld);
			}
			else
			{
				this.Emit(OpCodes.Ldarg_0);
				this.Emit(OpCodes.Ldfld, fld);
			}
			this.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[]
			{
				fld.FieldType
			}));
		}

		/// <summary>Emits the Microsoft intermediate language (MSIL) necessary to call <see cref="Overload:System.Console.WriteLine" /> with the given local variable.</summary>
		/// <param name="localBuilder">The local variable whose value is to be written to the console. </param>
		/// <exception cref="T:System.ArgumentException">The type of <paramref name="localBuilder" /> is <see cref="T:System.Reflection.Emit.TypeBuilder" /> or <see cref="T:System.Reflection.Emit.EnumBuilder" />, which are not supported. -or-There is no overload of <see cref="Overload:System.Console.WriteLine" /> that accepts the type of <paramref name="localBuilder" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="localBuilder" /> is null. </exception>
		public virtual void EmitWriteLine(LocalBuilder localBuilder)
		{
			if (localBuilder == null)
			{
				throw new ArgumentNullException("localBuilder");
			}
			if (localBuilder.LocalType is TypeBuilder)
			{
				throw new ArgumentException("Output streams do not support TypeBuilders.");
			}
			this.Emit(OpCodes.Ldloc, localBuilder);
			this.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[]
			{
				localBuilder.LocalType
			}));
		}

		/// <summary>Emits the Microsoft intermediate language (MSIL) to call <see cref="Overload:System.Console.WriteLine" /> with a string.</summary>
		/// <param name="value">The string to be printed. </param>
		public virtual void EmitWriteLine(string value)
		{
			this.Emit(OpCodes.Ldstr, value);
			this.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[]
			{
				typeof(string)
			}));
		}

		/// <summary>Ends an exception block.</summary>
		/// <exception cref="T:System.InvalidOperationException">The end exception block occurs in an unexpected place in the code stream. </exception>
		/// <exception cref="T:System.NotSupportedException">The Microsoft intermediate language (MSIL) being generated is not currently in an exception block. </exception>
		public virtual void EndExceptionBlock()
		{
			if (this.open_blocks == null)
			{
				this.open_blocks = new Stack(2);
			}
			if (this.open_blocks.Count <= 0)
			{
				throw new NotSupportedException("Not in an exception block");
			}
			if (this.ex_handlers[this.cur_block].LastClauseType() == -1)
			{
				throw new InvalidOperationException("Incorrect code generation for exception block.");
			}
			this.InternalEndClause();
			this.MarkLabel(this.ex_handlers[this.cur_block].end);
			this.ex_handlers[this.cur_block].End(this.code_len);
			this.ex_handlers[this.cur_block].Debug(this.cur_block);
			this.open_blocks.Pop();
			if (this.open_blocks.Count > 0)
			{
				this.cur_block = (int)this.open_blocks.Peek();
			}
		}

		/// <summary>Ends a lexical scope.</summary>
		/// <exception cref="T:System.NotSupportedException">This <see cref="T:System.Reflection.Emit.ILGenerator" /> belongs to a <see cref="T:System.Reflection.Emit.DynamicMethod" />.</exception>
		public virtual void EndScope()
		{
		}

		/// <summary>Marks the Microsoft intermediate language (MSIL) stream's current position with the given label.</summary>
		/// <param name="loc">The label for which to set an index. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="loc" /> represents an invalid index into the label array.-or- An index for <paramref name="loc" /> has already been defined. </exception>
		public virtual void MarkLabel(Label loc)
		{
			if (loc.label < 0 || loc.label >= this.num_labels)
			{
				throw new ArgumentException("The label is not valid");
			}
			if (this.labels[loc.label].addr >= 0)
			{
				throw new ArgumentException("The label was already defined");
			}
			this.labels[loc.label].addr = this.code_len;
			if (this.labels[loc.label].maxStack > this.cur_stack)
			{
				this.cur_stack = this.labels[loc.label].maxStack;
			}
		}

		/// <summary>Marks a sequence point in the Microsoft intermediate language (MSIL) stream.</summary>
		/// <param name="document">The document for which the sequence point is being defined. </param>
		/// <param name="startLine">The line where the sequence point begins. </param>
		/// <param name="startColumn">The column in the line where the sequence point begins. </param>
		/// <param name="endLine">The line where the sequence point ends. </param>
		/// <param name="endColumn">The column in the line where the sequence point ends. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startLine" /> or <paramref name="endLine" /> is &lt;= 0. </exception>
		/// <exception cref="T:System.NotSupportedException">This <see cref="T:System.Reflection.Emit.ILGenerator" /> belongs to a <see cref="T:System.Reflection.Emit.DynamicMethod" />.</exception>
		public virtual void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
		{
			if (this.currentSequence == null || this.currentSequence.Document != document)
			{
				if (this.sequencePointLists == null)
				{
					this.sequencePointLists = new ArrayList();
				}
				this.currentSequence = new SequencePointList(document);
				this.sequencePointLists.Add(this.currentSequence);
			}
			this.currentSequence.AddSequencePoint(this.code_len, startLine, startColumn, endLine, endColumn);
		}

		internal void GenerateDebugInfo(ISymbolWriter symbolWriter)
		{
			if (this.sequencePointLists != null)
			{
				SequencePointList sequencePointList = (SequencePointList)this.sequencePointLists[0];
				SequencePointList sequencePointList2 = (SequencePointList)this.sequencePointLists[this.sequencePointLists.Count - 1];
				symbolWriter.SetMethodSourceRange(sequencePointList.Document, sequencePointList.StartLine, sequencePointList.StartColumn, sequencePointList2.Document, sequencePointList2.EndLine, sequencePointList2.EndColumn);
				foreach (object obj in this.sequencePointLists)
				{
					SequencePointList sequencePointList3 = (SequencePointList)obj;
					symbolWriter.DefineSequencePoints(sequencePointList3.Document, sequencePointList3.GetOffsets(), sequencePointList3.GetLines(), sequencePointList3.GetColumns(), sequencePointList3.GetEndLines(), sequencePointList3.GetEndColumns());
				}
				if (this.locals != null)
				{
					foreach (LocalBuilder localBuilder in this.locals)
					{
						if (localBuilder.Name != null && localBuilder.Name.Length > 0)
						{
							SignatureHelper localVarSigHelper = SignatureHelper.GetLocalVarSigHelper(this.module);
							localVarSigHelper.AddArgument(localBuilder.LocalType);
							byte[] signature = localVarSigHelper.GetSignature();
							symbolWriter.DefineLocalVariable(localBuilder.Name, FieldAttributes.Public, signature, SymAddressKind.ILOffset, (int)localBuilder.position, 0, 0, localBuilder.StartOffset, localBuilder.EndOffset);
						}
					}
				}
				this.sequencePointLists = null;
			}
		}

		internal bool HasDebugInfo
		{
			get
			{
				return this.sequencePointLists != null;
			}
		}

		/// <summary>Emits an instruction to throw an exception.</summary>
		/// <param name="excType">The class of the type of exception to throw. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="excType" /> is not the <see cref="T:System.Exception" /> class or a derived class of <see cref="T:System.Exception" />.-or- The type does not have a default constructor. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="excType" /> is null. </exception>
		public virtual void ThrowException(Type excType)
		{
			if (excType == null)
			{
				throw new ArgumentNullException("excType");
			}
			if (excType != typeof(Exception) && !excType.IsSubclassOf(typeof(Exception)))
			{
				throw new ArgumentException("Type should be an exception type", "excType");
			}
			ConstructorInfo constructor = excType.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
			{
				throw new ArgumentException("Type should have a default constructor", "excType");
			}
			this.Emit(OpCodes.Newobj, constructor);
			this.Emit(OpCodes.Throw);
		}

		/// <summary>Specifies the namespace to be used in evaluating locals and watches for the current active lexical scope.</summary>
		/// <param name="usingNamespace">The namespace to be used in evaluating locals and watches for the current active lexical scope </param>
		/// <exception cref="T:System.ArgumentException">Length of <paramref name="usingNamespace" /> is zero. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="usingNamespace" /> is null. </exception>
		/// <exception cref="T:System.NotSupportedException">This <see cref="T:System.Reflection.Emit.ILGenerator" /> belongs to a <see cref="T:System.Reflection.Emit.DynamicMethod" />.</exception>
		[MonoTODO("Not implemented")]
		public virtual void UsingNamespace(string usingNamespace)
		{
			throw new NotImplementedException();
		}

		internal void label_fixup()
		{
			for (int i = 0; i < this.num_fixups; i++)
			{
				if (this.labels[this.fixups[i].label_idx].addr < 0)
				{
					throw new ArgumentException("Label not marked");
				}
				int num = this.labels[this.fixups[i].label_idx].addr - (this.fixups[i].pos + this.fixups[i].offset);
				if (this.fixups[i].offset == 1)
				{
					this.code[this.fixups[i].pos] = (byte)((sbyte)num);
				}
				else
				{
					int num2 = this.code_len;
					this.code_len = this.fixups[i].pos;
					this.emit_int(num);
					this.code_len = num2;
				}
			}
		}

		[Obsolete("Use ILOffset")]
		internal static int Mono_GetCurrentOffset(ILGenerator ig)
		{
			return ig.code_len;
		}

		private struct LabelFixup
		{
			public int offset;

			public int pos;

			public int label_idx;
		}

		private struct LabelData
		{
			public int addr;

			public int maxStack;

			public LabelData(int addr, int maxStack)
			{
				this.addr = addr;
				this.maxStack = maxStack;
			}
		}
	}
}
