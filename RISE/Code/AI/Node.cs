using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.AI
{
    class Node : IHeapItem<Node>
    {
        internal Node Parent;
        internal Vector2 Position;
        internal bool Diagonal;
        internal bool Passable = true;
        internal int H_Value;
        internal int G_Value;
        internal int F_Value;

        /// <summary>
        /// Node constructor
        /// </summary>
        /// <param name="position"></param>
        public Node(Vector2 position)
        {
            this.Position = position;
        }


        private int heapIndex;
        public int HeapIndex { get { return heapIndex; }  set { heapIndex = value; } }

        public int CompareTo(Node other)
        {
            int compare = F_Value.CompareTo(other.F_Value);
            if (compare == 0)
            {
                compare = H_Value.CompareTo(other.H_Value);
            }
            return -compare;
        }
    }
}
