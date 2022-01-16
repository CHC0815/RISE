using RISE.Code.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.AI
{
    class AstarThreadWorker
    {
        public Astar astar;
        public int WorkerIDNumber;

        public AstarThreadWorker(Node startingNode, Node targetNode, World world, bool disableDiagonalPathfinding, int workerIDNumber)
        {
            if (startingNode.Position.X > world.Width || startingNode.Position.Y > world.Height)
                throw new Exception("Starting Node out of bounds: {" + startingNode.Position.ToString() + "} Make sure the Position is in array coordinates not in pixel coordinates");
            if (targetNode.Position.X > world.Width || startingNode.Position.Y > world.Height)
                throw new Exception("Target Node out of bounds: {" + targetNode.Position.ToString() + "} Make sure the Position is in array coordinates not in pixel coordinates");

            this.WorkerIDNumber = workerIDNumber;
            astar = new Astar(startingNode, targetNode, world, disableDiagonalPathfinding);
            astar.FindPath();
        }
    }
}
