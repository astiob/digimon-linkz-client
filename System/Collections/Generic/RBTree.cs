using System;

namespace System.Collections.Generic
{
	internal class RBTree : IEnumerable, IEnumerable<RBTree.Node>
	{
		private RBTree.Node root;

		private object hlp;

		private uint version;

		[ThreadStatic]
		private static List<RBTree.Node> cached_path;

		public RBTree(object hlp)
		{
			this.hlp = hlp;
		}

		IEnumerator<RBTree.Node> IEnumerable<RBTree.Node>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		private static List<RBTree.Node> alloc_path()
		{
			if (RBTree.cached_path == null)
			{
				return new List<RBTree.Node>();
			}
			List<RBTree.Node> result = RBTree.cached_path;
			RBTree.cached_path = null;
			return result;
		}

		private static void release_path(List<RBTree.Node> path)
		{
			if (RBTree.cached_path == null || RBTree.cached_path.Capacity < path.Capacity)
			{
				path.Clear();
				RBTree.cached_path = path;
			}
		}

		public void Clear()
		{
			this.root = null;
			this.version += 1u;
		}

		public RBTree.Node Intern<T>(T key, RBTree.Node new_node)
		{
			if (this.root == null)
			{
				if (new_node == null)
				{
					new_node = ((RBTree.INodeHelper<T>)this.hlp).CreateNode(key);
				}
				this.root = new_node;
				this.root.IsBlack = true;
				this.version += 1u;
				return this.root;
			}
			List<RBTree.Node> list = RBTree.alloc_path();
			int in_tree_cmp = this.find_key<T>(key, list);
			RBTree.Node node = list[list.Count - 1];
			if (node == null)
			{
				if (new_node == null)
				{
					new_node = ((RBTree.INodeHelper<T>)this.hlp).CreateNode(key);
				}
				node = this.do_insert(in_tree_cmp, new_node, list);
			}
			RBTree.release_path(list);
			return node;
		}

		public RBTree.Node Remove<T>(T key)
		{
			if (this.root == null)
			{
				return null;
			}
			List<RBTree.Node> path = RBTree.alloc_path();
			int num = this.find_key<T>(key, path);
			RBTree.Node result = null;
			if (num == 0)
			{
				result = this.do_remove(path);
			}
			RBTree.release_path(path);
			return result;
		}

		public RBTree.Node Lookup<T>(T key)
		{
			RBTree.INodeHelper<T> nodeHelper = (RBTree.INodeHelper<T>)this.hlp;
			RBTree.Node node;
			int num;
			for (node = this.root; node != null; node = ((num >= 0) ? node.right : node.left))
			{
				num = nodeHelper.Compare(key, node);
				if (num == 0)
				{
					break;
				}
			}
			return node;
		}

		public int Count
		{
			get
			{
				return (int)((this.root != null) ? this.root.Size : 0u);
			}
		}

		public RBTree.Node this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("index");
				}
				RBTree.Node node = this.root;
				while (node != null)
				{
					int num = (int)((node.left != null) ? node.left.Size : 0u);
					if (index == num)
					{
						return node;
					}
					if (index < num)
					{
						node = node.left;
					}
					else
					{
						index -= num + 1;
						node = node.right;
					}
				}
				throw new SystemException("Internal Error: index calculation");
			}
		}

		public RBTree.NodeEnumerator GetEnumerator()
		{
			return new RBTree.NodeEnumerator(this);
		}

		private int find_key<T>(T key, List<RBTree.Node> path)
		{
			RBTree.INodeHelper<T> nodeHelper = (RBTree.INodeHelper<T>)this.hlp;
			int num = 0;
			RBTree.Node node = this.root;
			if (path != null)
			{
				path.Add(this.root);
			}
			while (node != null)
			{
				num = nodeHelper.Compare(key, node);
				if (num == 0)
				{
					return num;
				}
				RBTree.Node item;
				if (num < 0)
				{
					item = node.right;
					node = node.left;
				}
				else
				{
					item = node.left;
					node = node.right;
				}
				if (path != null)
				{
					path.Add(item);
					path.Add(node);
				}
			}
			return num;
		}

		private RBTree.Node do_insert(int in_tree_cmp, RBTree.Node current, List<RBTree.Node> path)
		{
			path[path.Count - 1] = current;
			RBTree.Node node = path[path.Count - 3];
			if (in_tree_cmp < 0)
			{
				node.left = current;
			}
			else
			{
				node.right = current;
			}
			for (int i = 0; i < path.Count - 2; i += 2)
			{
				path[i].Size += 1u;
			}
			if (!node.IsBlack)
			{
				this.rebalance_insert(path);
			}
			if (!this.root.IsBlack)
			{
				throw new SystemException("Internal error: root is not black");
			}
			this.version += 1u;
			return current;
		}

		private RBTree.Node do_remove(List<RBTree.Node> path)
		{
			int num = path.Count - 1;
			RBTree.Node node = path[num];
			if (node.left != null)
			{
				RBTree.Node node2 = RBTree.right_most(node.left, node.right, path);
				node.SwapValue(node2);
				if (node2.left != null)
				{
					RBTree.Node left = node2.left;
					path.Add(null);
					path.Add(left);
					node2.SwapValue(left);
				}
			}
			else if (node.right != null)
			{
				RBTree.Node right = node.right;
				path.Add(null);
				path.Add(right);
				node.SwapValue(right);
			}
			num = path.Count - 1;
			node = path[num];
			if (node.Size != 1u)
			{
				throw new SystemException("Internal Error: red-black violation somewhere");
			}
			path[num] = null;
			this.node_reparent((num != 0) ? path[num - 2] : null, node, 0u, null);
			for (int i = 0; i < path.Count - 2; i += 2)
			{
				path[i].Size -= 1u;
			}
			if (num != 0 && node.IsBlack)
			{
				this.rebalance_delete(path);
			}
			if (this.root != null && !this.root.IsBlack)
			{
				throw new SystemException("Internal Error: root is not black");
			}
			this.version += 1u;
			return node;
		}

		private void rebalance_insert(List<RBTree.Node> path)
		{
			int num = path.Count - 1;
			while (path[num - 3] != null && !path[num - 3].IsBlack)
			{
				RBTree.Node node = path[num - 2];
				bool isBlack = true;
				path[num - 3].IsBlack = isBlack;
				node.IsBlack = isBlack;
				num -= 4;
				if (num == 0)
				{
					return;
				}
				path[num].IsBlack = false;
				if (path[num - 2].IsBlack)
				{
					return;
				}
			}
			this.rebalance_insert__rotate_final(num, path);
		}

		private void rebalance_delete(List<RBTree.Node> path)
		{
			int num = path.Count - 1;
			for (;;)
			{
				RBTree.Node node = path[num - 1];
				if (!node.IsBlack)
				{
					num = this.ensure_sibling_black(num, path);
					node = path[num - 1];
				}
				if ((node.left != null && !node.left.IsBlack) || (node.right != null && !node.right.IsBlack))
				{
					break;
				}
				node.IsBlack = false;
				num -= 2;
				if (num == 0)
				{
					return;
				}
				if (!path[num].IsBlack)
				{
					goto Block_5;
				}
			}
			this.rebalance_delete__rotate_final(num, path);
			return;
			Block_5:
			path[num].IsBlack = true;
		}

		private void rebalance_insert__rotate_final(int curpos, List<RBTree.Node> path)
		{
			RBTree.Node node = path[curpos];
			RBTree.Node node2 = path[curpos - 2];
			RBTree.Node node3 = path[curpos - 4];
			uint size = node3.Size;
			bool flag = node2 == node3.left;
			bool flag2 = node == node2.left;
			RBTree.Node node4;
			if (flag && flag2)
			{
				node3.left = node2.right;
				node2.right = node3;
				node4 = node2;
			}
			else if (flag && !flag2)
			{
				node3.left = node.right;
				node.right = node3;
				node2.right = node.left;
				node.left = node2;
				node4 = node;
			}
			else if (!flag && flag2)
			{
				node3.right = node.left;
				node.left = node3;
				node2.left = node.right;
				node.right = node2;
				node4 = node;
			}
			else
			{
				node3.right = node2.left;
				node2.left = node3;
				node4 = node2;
			}
			node3.FixSize();
			node3.IsBlack = false;
			if (node4 != node2)
			{
				node2.FixSize();
			}
			node4.IsBlack = true;
			this.node_reparent((curpos != 4) ? path[curpos - 6] : null, node3, size, node4);
		}

		private void rebalance_delete__rotate_final(int curpos, List<RBTree.Node> path)
		{
			RBTree.Node node = path[curpos - 1];
			RBTree.Node node2 = path[curpos - 2];
			uint size = node2.Size;
			bool isBlack = node2.IsBlack;
			RBTree.Node node3;
			if (node2.right == node)
			{
				if (node.right == null || node.right.IsBlack)
				{
					RBTree.Node left = node.left;
					node2.right = left.left;
					left.left = node2;
					node.left = left.right;
					left.right = node;
					node3 = left;
				}
				else
				{
					node2.right = node.left;
					node.left = node2;
					node.right.IsBlack = true;
					node3 = node;
				}
			}
			else if (node.left == null || node.left.IsBlack)
			{
				RBTree.Node right = node.right;
				node2.left = right.right;
				right.right = node2;
				node.right = right.left;
				right.left = node;
				node3 = right;
			}
			else
			{
				node2.left = node.right;
				node.right = node2;
				node.left.IsBlack = true;
				node3 = node;
			}
			node2.FixSize();
			node2.IsBlack = true;
			if (node3 != node)
			{
				node.FixSize();
			}
			node3.IsBlack = isBlack;
			this.node_reparent((curpos != 2) ? path[curpos - 4] : null, node2, size, node3);
		}

		private int ensure_sibling_black(int curpos, List<RBTree.Node> path)
		{
			RBTree.Node value = path[curpos];
			RBTree.Node node = path[curpos - 1];
			RBTree.Node node2 = path[curpos - 2];
			uint size = node2.Size;
			bool flag;
			if (node2.right == node)
			{
				node2.right = node.left;
				node.left = node2;
				flag = true;
			}
			else
			{
				node2.left = node.right;
				node.right = node2;
				flag = false;
			}
			node2.FixSize();
			node2.IsBlack = false;
			node.IsBlack = true;
			this.node_reparent((curpos != 2) ? path[curpos - 4] : null, node2, size, node);
			if (curpos + 1 == path.Count)
			{
				path.Add(null);
				path.Add(null);
			}
			path[curpos - 2] = node;
			path[curpos - 1] = ((!flag) ? node.left : node.right);
			path[curpos] = node2;
			path[curpos + 1] = ((!flag) ? node2.left : node2.right);
			path[curpos + 2] = value;
			return curpos + 2;
		}

		private void node_reparent(RBTree.Node orig_parent, RBTree.Node orig, uint orig_size, RBTree.Node updated)
		{
			if (updated != null && updated.FixSize() != orig_size)
			{
				throw new SystemException("Internal error: rotation");
			}
			if (orig == this.root)
			{
				this.root = updated;
			}
			else if (orig == orig_parent.left)
			{
				orig_parent.left = updated;
			}
			else
			{
				if (orig != orig_parent.right)
				{
					throw new SystemException("Internal error: path error");
				}
				orig_parent.right = updated;
			}
		}

		private static RBTree.Node right_most(RBTree.Node current, RBTree.Node sibling, List<RBTree.Node> path)
		{
			for (;;)
			{
				path.Add(sibling);
				path.Add(current);
				if (current.right == null)
				{
					break;
				}
				sibling = current.left;
				current = current.right;
			}
			return current;
		}

		public interface INodeHelper<T>
		{
			int Compare(T key, RBTree.Node node);

			RBTree.Node CreateNode(T key);
		}

		public abstract class Node
		{
			private const uint black_mask = 1u;

			private const int black_shift = 1;

			public RBTree.Node left;

			public RBTree.Node right;

			private uint size_black;

			public Node()
			{
				this.size_black = 2u;
			}

			public bool IsBlack
			{
				get
				{
					return (this.size_black & 1u) == 1u;
				}
				set
				{
					this.size_black = ((!value) ? (this.size_black & 4294967294u) : (this.size_black | 1u));
				}
			}

			public uint Size
			{
				get
				{
					return this.size_black >> 1;
				}
				set
				{
					this.size_black = (value << 1 | (this.size_black & 1u));
				}
			}

			public uint FixSize()
			{
				this.Size = 1u;
				if (this.left != null)
				{
					this.Size += this.left.Size;
				}
				if (this.right != null)
				{
					this.Size += this.right.Size;
				}
				return this.Size;
			}

			public abstract void SwapValue(RBTree.Node other);
		}

		public struct NodeEnumerator : IEnumerator, IDisposable, IEnumerator<RBTree.Node>
		{
			private RBTree tree;

			private uint version;

			private Stack<RBTree.Node> pennants;

			internal NodeEnumerator(RBTree tree)
			{
				this.tree = tree;
				this.version = tree.version;
				this.pennants = null;
			}

			object IEnumerator.Current
			{
				get
				{
					this.check_current();
					return this.Current;
				}
			}

			public void Reset()
			{
				this.check_version();
				this.pennants = null;
			}

			public RBTree.Node Current
			{
				get
				{
					return this.pennants.Peek();
				}
			}

			public bool MoveNext()
			{
				this.check_version();
				RBTree.Node node;
				if (this.pennants == null)
				{
					if (this.tree.root == null)
					{
						return false;
					}
					this.pennants = new Stack<RBTree.Node>();
					node = this.tree.root;
				}
				else
				{
					if (this.pennants.Count == 0)
					{
						return false;
					}
					RBTree.Node node2 = this.pennants.Pop();
					node = node2.right;
				}
				while (node != null)
				{
					this.pennants.Push(node);
					node = node.left;
				}
				return this.pennants.Count != 0;
			}

			public void Dispose()
			{
				this.tree = null;
				this.pennants = null;
			}

			private void check_version()
			{
				if (this.tree == null)
				{
					throw new ObjectDisposedException("enumerator");
				}
				if (this.version != this.tree.version)
				{
					throw new InvalidOperationException("tree modified");
				}
			}

			internal void check_current()
			{
				this.check_version();
				if (this.pennants == null)
				{
					throw new InvalidOperationException("state invalid before the first MoveNext()");
				}
			}
		}
	}
}
