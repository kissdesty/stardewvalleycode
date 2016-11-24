using System;
using System.Collections.Generic;

namespace StardewValley
{
	// Token: 0x0200003C RID: 60
	public class PriorityQueue
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x0003C51D File Offset: 0x0003A71D
		public PriorityQueue()
		{
			this.nodes = new SortedDictionary<int, Queue<PathNode>>();
			this.total_size = 0;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0003C537 File Offset: 0x0003A737
		public bool IsEmpty()
		{
			return this.total_size == 0;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0003C542 File Offset: 0x0003A742
		public bool Contains(PathNode p, int priority)
		{
			return this.nodes.ContainsKey(priority) && this.nodes[priority].Contains(p);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0003C568 File Offset: 0x0003A768
		public PathNode Dequeue()
		{
			if (!this.IsEmpty())
			{
				foreach (Queue<PathNode> q in this.nodes.Values)
				{
					if (q.Count > 0)
					{
						this.total_size--;
						return q.Dequeue();
					}
				}
			}
			return null;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0003C5E4 File Offset: 0x0003A7E4
		public object Peek()
		{
			if (!this.IsEmpty())
			{
				foreach (Queue<PathNode> q in this.nodes.Values)
				{
					if (q.Count > 0)
					{
						return q.Peek();
					}
				}
			}
			return null;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0003C654 File Offset: 0x0003A854
		public object Dequeue(int priority)
		{
			this.total_size--;
			return this.nodes[priority].Dequeue();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0003C678 File Offset: 0x0003A878
		public void Enqueue(PathNode item, int priority)
		{
			if (!this.nodes.ContainsKey(priority))
			{
				this.nodes.Add(priority, new Queue<PathNode>());
				this.Enqueue(item, priority);
				return;
			}
			this.nodes[priority].Enqueue(item);
			this.total_size++;
		}

		// Token: 0x0400035C RID: 860
		private int total_size;

		// Token: 0x0400035D RID: 861
		private SortedDictionary<int, Queue<PathNode>> nodes;
	}
}
