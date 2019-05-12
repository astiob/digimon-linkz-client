using System;
using System.Collections;

namespace System.Diagnostics
{
	/// <summary>Correlates traces that are part of a logical transaction.</summary>
	/// <filterpriority>2</filterpriority>
	public class CorrelationManager
	{
		private Guid activity;

		private Stack op_stack = new Stack();

		internal CorrelationManager()
		{
		}

		/// <summary>Gets or sets the identity for a global activity.</summary>
		/// <returns>A <see cref="T:System.Guid" /> structure that identifies the global activity.</returns>
		/// <filterpriority>1</filterpriority>
		public Guid ActivityId
		{
			get
			{
				return this.activity;
			}
			set
			{
				this.activity = value;
			}
		}

		/// <summary>Gets the logical operation stack from the call context.</summary>
		/// <returns>A <see cref="T:System.Collections.Stack" /> object that represents the logical operation stack for the call context.</returns>
		/// <filterpriority>1</filterpriority>
		public Stack LogicalOperationStack
		{
			get
			{
				return this.op_stack;
			}
		}

		/// <summary>Starts a logical operation on a thread.</summary>
		/// <filterpriority>1</filterpriority>
		public void StartLogicalOperation()
		{
			this.StartLogicalOperation(Guid.NewGuid());
		}

		/// <summary>Starts a logical operation with the specified identity on a thread.</summary>
		/// <param name="operationId">An object identifying the operation.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="operationId" /> parameter is null. </exception>
		/// <filterpriority>1</filterpriority>
		public void StartLogicalOperation(object operationId)
		{
			this.op_stack.Push(operationId);
		}

		/// <summary>Stops the current logical operation.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.CorrelationManager.LogicalOperationStack" /> property is an empty stack.</exception>
		/// <filterpriority>1</filterpriority>
		public void StopLogicalOperation()
		{
			this.op_stack.Pop();
		}
	}
}
