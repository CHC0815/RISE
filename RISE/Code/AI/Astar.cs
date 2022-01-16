using Microsoft.Xna.Framework;
using RISE.Code.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RISE.Code.AI
{
    class Astar
    {
        World world;
        public Node[,] Nodes;
        Node CurrentNode;
        //Heap<Node> OpenList;
        List<Node> OpenList;
        List<Node> ClosedList;
        List<Node> FinalPath;

        Node StartingNode, TargetNode;

        public bool ReachedTarget;

        const int HV_G_Cost = 10;
        const int Diagonal_G_Cost = 14;

        public bool DisableDiagonalPathfinding;

        public Astar(Node StartingNode, Node TargetNode, World world, bool DisableDiagonalPathfinding)
        {
            this.StartingNode = StartingNode;
            this.TargetNode = TargetNode;
            this.world = world;
            this.DisableDiagonalPathfinding = DisableDiagonalPathfinding;
        }

        void CreateNodeList()
        {
            Nodes = new Node[world.Width, world.Height];
            for (int y = 0; y < world.Height; y++)
                for (int x = 0; x < world.Width; x++)
                    Nodes[x, y] = new Node(new Microsoft.Xna.Framework.Vector2(x, y));
        }

        void GenerateCollisionNodes()
        {
            //TODO performance by adding a list of collision objects
            /*
            for (int y = 0; y < world.Height; y++)
                for (int x = 0; x < world.Width; x++)
                    Nodes[x, y].Passable = isWalkable(x, y);
                    */
            for(int i = 0; i < world.notWalkablesTiles.Count;i++)
            {
                int x = (int)world.notWalkablesTiles[i].Position.X / Game1.PixelsPerTile;
                int y = (int)world.notWalkablesTiles[i].Position.Y / Game1.PixelsPerTile;

                Nodes[x, y].Passable = false;
            }
        }

        bool isWalkable(int x, int y)
        {
            return !(world.Tiles[x * world.Width + y].type == 0 || world.Tiles[x * world.Width + y].type == 9 || world.Tiles[x * world.Width + y].type == 10 || world.Tiles[x * world.Width + y].hasBuilding);
        }

        void FindNeighborsNodes()
        {
            Node NeighborNode;
            bool Add = false;

            OpenList.Remove(CurrentNode);
            //OpenList.RemoveFirst();

            if (!ClosedList.Contains(CurrentNode))
                ClosedList.Add(CurrentNode);


            //Check if we are in the bounds of the map.
            //Top Node
            if (CurrentNode.Position.Y - 1 >= 0)
            {
                NeighborNode = Nodes[(int)CurrentNode.Position.X, (int)CurrentNode.Position.Y - 1];

                if (!OpenList.Contains(NeighborNode))
                {
                    if (NeighborNode.Passable)
                    {
                        for (int i = 0; i < ClosedList.Count; i++)
                        {
                            if (NeighborNode.Position == ClosedList[i].Position)
                            {
                                Add = false;
                                break;
                            }
                            else
                                Add = true;
                        }
                    }

                    if (Add)
                    {
                        NeighborNode.Parent = CurrentNode;
                        NeighborNode.Diagonal = false;
                        OpenList.Add(NeighborNode);
                        Add = false;
                    }
                }
            }

            //Bottom Node
            if (CurrentNode.Position.Y + 1 < Nodes.GetLength(1))
            {
                NeighborNode = Nodes[(int)CurrentNode.Position.X, (int)CurrentNode.Position.Y + 1];

                if (!OpenList.Contains(NeighborNode))
                {
                    if (NeighborNode.Passable)
                    {
                        for (int i = 0; i < ClosedList.Count; i++)
                        {
                            if (NeighborNode.Position == ClosedList[i].Position)
                            {
                                Add = false;
                                break;
                            }
                            else
                                Add = true;
                        }
                    }

                    if (Add)
                    {
                        NeighborNode.Parent = CurrentNode;
                        NeighborNode.Diagonal = false;
                        OpenList.Add(NeighborNode);
                        Add = false;
                    }
                }
            }


            //Right Node
            if (CurrentNode.Position.X + 1 < Nodes.GetLength(0))
            {
                NeighborNode = Nodes[(int)CurrentNode.Position.X + 1, (int)CurrentNode.Position.Y];

                if (!OpenList.Contains(NeighborNode))
                {
                    if (NeighborNode.Passable)
                    {
                        for (int i = 0; i < ClosedList.Count; i++)
                        {
                            if (NeighborNode.Position == ClosedList[i].Position)
                            {
                                Add = false;
                                break;
                            }
                            else
                                Add = true;
                        }
                    }

                    if (Add)
                    {
                        NeighborNode.Parent = CurrentNode;
                        NeighborNode.Diagonal = false;
                        OpenList.Add(NeighborNode);
                        Add = false;
                    }
                }
            }

            //Left Node
            if (CurrentNode.Position.X - 1 >= 0)
            {
                NeighborNode = Nodes[(int)CurrentNode.Position.X - 1, (int)CurrentNode.Position.Y];

                if (!OpenList.Contains(NeighborNode))
                {
                    if (NeighborNode.Passable)
                    {
                        for (int i = 0; i < ClosedList.Count; i++)
                        {
                            if (NeighborNode.Position == ClosedList[i].Position)
                            {
                                Add = false;
                                break;
                            }
                            else
                                Add = true;
                        }
                    }

                    if (Add)
                    {
                        NeighborNode.Parent = CurrentNode;
                        NeighborNode.Diagonal = false;
                        OpenList.Add(NeighborNode);
                        Add = false;
                    }
                }
            }

            if (!DisableDiagonalPathfinding)
            {
                //Top-Right Node
                if (CurrentNode.Position.X + 1 < Nodes.GetLength(0) && CurrentNode.Position.Y - 1 >= 0)
                {
                    NeighborNode = Nodes[(int)CurrentNode.Position.X + 1, (int)CurrentNode.Position.Y - 1];

                    if (!OpenList.Contains(NeighborNode))
                    {
                        if (NeighborNode.Passable)
                        {
                            for (int i = 0; i < ClosedList.Count; i++)
                            {
                                if (NeighborNode.Position == ClosedList[i].Position)
                                {
                                    Add = false;
                                    break;
                                }
                                else
                                    Add = true;
                            }
                        }

                        if (Add)
                        {
                            NeighborNode.Parent = CurrentNode;
                            NeighborNode.Diagonal = true;
                            OpenList.Add(NeighborNode);
                            Add = false;
                        }
                    }
                }

                //Top-Left Node
                if (CurrentNode.Position.X - 1 >= 0 && CurrentNode.Position.Y - 1 >= 0)
                {
                    NeighborNode = Nodes[(int)CurrentNode.Position.X - 1, (int)CurrentNode.Position.Y - 1];

                    if (!OpenList.Contains(NeighborNode))
                    {
                        if (NeighborNode.Passable)
                        {
                            for (int i = 0; i < ClosedList.Count; i++)
                            {
                                if (NeighborNode.Position == ClosedList[i].Position)
                                {
                                    Add = false;
                                    break;
                                }
                                else
                                    Add = true;
                            }
                        }

                        if (Add)
                        {
                            NeighborNode.Parent = CurrentNode;
                            NeighborNode.Diagonal = true;
                            OpenList.Add(NeighborNode);
                            Add = false;
                        }
                    }
                }

                //Bottom-Right Node
                if (CurrentNode.Position.X + 1 < Nodes.GetLength(0) && CurrentNode.Position.Y + 1 < Nodes.GetLength(1))
                {
                    NeighborNode = Nodes[(int)CurrentNode.Position.X + 1, (int)CurrentNode.Position.Y + 1];

                    if (!OpenList.Contains(NeighborNode))
                    {
                        if (NeighborNode.Passable)
                        {
                            for (int i = 0; i < ClosedList.Count; i++)
                            {
                                if (NeighborNode.Position == ClosedList[i].Position)
                                {
                                    Add = false;
                                    break;
                                }
                                else
                                    Add = true;
                            }
                        }

                        if (Add)
                        {
                            NeighborNode.Parent = CurrentNode;
                            NeighborNode.Diagonal = true;
                            OpenList.Add(NeighborNode);
                            Add = false;
                        }
                    }
                }

                //Bottom-Left Node
                if (CurrentNode.Position.X - 1 >= 0 && CurrentNode.Position.Y + 1 < Nodes.GetLength(1))
                {
                    NeighborNode = Nodes[(int)CurrentNode.Position.X - 1, (int)CurrentNode.Position.Y + 1];

                    if (!OpenList.Contains(NeighborNode))
                    {
                        if (NeighborNode.Passable)
                        {
                            for (int i = 0; i < ClosedList.Count; i++)
                            {
                                if (NeighborNode.Position == ClosedList[i].Position)
                                {
                                    Add = false;
                                    break;
                                }
                                else
                                    Add = true;
                            }
                        }

                        if (Add)
                        {
                            NeighborNode.Parent = CurrentNode;
                            NeighborNode.Diagonal = true;
                            OpenList.Add(NeighborNode);
                            Add = false;
                        }
                    }
                }
            }
        }
        void Calculate_H_Value()
        {
            //distance from the starting node to the ending node.
            Vector2 Distance = Vector2.Zero;

            for (int x = 0; x < Nodes.GetLength(0); x++)
            {
                for (int y = 0; y < Nodes.GetLength(1); y++)
                {
                    Vector2 CurrentNodePosition = new Vector2(x, y);

                    //Current Node at Right
                    if (CurrentNodePosition.X <= TargetNode.Position.X)
                        Distance.X = TargetNode.Position.X - CurrentNodePosition.X;

                    //Current Node at Left
                    if (CurrentNodePosition.X >= TargetNode.Position.X)
                        Distance.X = CurrentNodePosition.X - TargetNode.Position.X;


                    //Current Node at Up
                    if (CurrentNodePosition.Y <= TargetNode.Position.Y)
                        Distance.Y = TargetNode.Position.Y - CurrentNodePosition.Y;

                    //Current Node at Down
                    if (CurrentNodePosition.Y >= TargetNode.Position.Y)
                        Distance.Y = CurrentNodePosition.Y - TargetNode.Position.Y;

                    Nodes[x, y].H_Value = (int)(Distance.X + Distance.Y);
                }
            }
        }

        void Calculate_G_Value()
        {
            for (int i = 0; i < OpenList.Count; i++)
            {
                if (OpenList[i].Diagonal && !DisableDiagonalPathfinding)
                    OpenList[i].G_Value = OpenList[i].Parent.G_Value + Diagonal_G_Cost;
                else
                    OpenList[i].G_Value = OpenList[i].Parent.G_Value + HV_G_Cost;
            }
        }
        void Calculate_F_Value()
        {
            for (int i = 0; i < OpenList.Count; i++)
                OpenList[i].F_Value = OpenList[i].G_Value + OpenList[i].H_Value;
        }
        Node GetMin_F_Vaule()
        {
            List<int> F_Values_List = new List<int>();
            int Lowest_F_Vaule = 0;


            for (int i = 0; i < OpenList.Count; i++)
                F_Values_List.Add(OpenList[i].F_Value);

            if (F_Values_List.Count > 0)
            {
                Lowest_F_Vaule = F_Values_List.Min();

                for (int i = 0; i < OpenList.Count; i++)
                {
                    if (OpenList[i].F_Value == Lowest_F_Vaule)
                        return OpenList[i];
                }
            }

            return CurrentNode;
        }
        void CalculateFinalPath()
        {
            List<Node> FinalPathTemp = new List<Node>();

            FinalPathTemp.Add(CurrentNode);
            FinalPathTemp.Add(CurrentNode.Parent);

            for (int i = 1; ; i++)
            {
                if (FinalPathTemp[i].Parent != null)
                    FinalPathTemp.Add(FinalPathTemp[i].Parent);
                else
                    break;
            }

            // reverse the list elemnts order from last to to begining.
            for (int i = FinalPathTemp.Count - 1; i >= 0; i--)
                FinalPath.Add(FinalPathTemp[i]);
        }
        public List<Vector2> GetFinalPath()
        {
            List<Vector2> FinalPathVec2 = new List<Vector2>();

            for (int i = 1; i < FinalPath.Count; i++)
                FinalPathVec2.Add(FinalPath[i].Position);

            return FinalPathVec2;
        }
        public void FindPath()
        {
            CreateNodeList();
            GenerateCollisionNodes();

            CurrentNode = this.StartingNode;

            OpenList = new List<Node>();
            ClosedList = new List<Node>();
            FinalPath = new List<Node>();

            Calculate_H_Value();

            while (true)
            {
                CurrentNode = GetMin_F_Vaule();

                FindNeighborsNodes();
                Calculate_G_Value();
                Calculate_F_Value();

                for (int i = 0; i < OpenList.Count; i++)
                {
                    if (OpenList[i].Position == TargetNode.Position)
                    {
                        CurrentNode = OpenList[i];
                        CalculateFinalPath();
                        ReachedTarget = true;
                        break;
                    }
                }

                if (ReachedTarget)
                    break;

                if (OpenList.Count == 0)
                {
                    ReachedTarget = false;
                    break;
                }
            }
        }
    }
}
