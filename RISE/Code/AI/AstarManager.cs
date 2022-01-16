using RISE.Code.Tile;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RISE.Code.AI
{
    static class AstarManager
    {
        public static ConcurrentQueue<AstarThreadWorker> AstarThreadWorkerResults = new ConcurrentQueue<AstarThreadWorker>();
        public static void AddNewThreadWorker(Node StartingNode, Node TargetNode, World world, bool DisableDiagonalPathfinding, int WorkerIDNumber)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                AstarThreadWorker astarWorker = new AstarThreadWorker(StartingNode, TargetNode, world, DisableDiagonalPathfinding, WorkerIDNumber);
                AstarThreadWorkerResults.Enqueue(astarWorker);
            }));
        }
    }
}
