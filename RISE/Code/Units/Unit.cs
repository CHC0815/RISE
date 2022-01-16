using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RISE.Code.AI;
using RISE.Code.Tile;
using RISE.Code.UI;
using Node = RISE.Code.AI.Node;

namespace RISE.Code.Units
{
    public class Unit : GameObject, IhasViewDistance, IRenderable
    {
        public float maxHealth;
        public float Health;

        #region viewDistance
        private float _viewDistance = 48f;
        public float getViewDistance()
        {
            return this._viewDistance;
        }
        public void setViewDistance(float value)
        {
            this._viewDistance = value;
        }
        #endregion

        #region Pathfinding
        float Speed;
        public enum State { Normal, Hovered, Selected };
        public State UnitState;
        AstarThreadWorker astarThreadWorkerTemp, astarThreadWorker;
        List<Vector2> WayPointsList;
        WayPoint wayPoint;
        Rectangle rectangle;
        #endregion

        #region Textures
        Texture2D NormalTexture;
        Texture2D HoveredTexture;
        Texture2D SelectedTexture;
        #endregion

        //Node[,] nodes;
        public Unit(UnitType type, Vector2 position, float maxHealth) : base(Game1.GetGameObjectId(), textureByType(type)[0], position, 0f)
        {
            this.maxHealth = Health = maxHealth;
            Speed = 10f;
            WayPointsList = new List<Vector2>();
            wayPoint = new WayPoint();

            Texture2D[] textures = textureByType(type);
            this.Texture = textures[0];
            this.NormalTexture = textures[0];
            this.HoveredTexture = textures[1];
            this.SelectedTexture = textures[2];
            //astar.FindPath();
            //nodes = astar.Nodes;
        }

        public void UpdateUnit(GameTime gameTime, MouseRectangle mouseRectangle, World world, int UnitID, List<Unit> Units)
        {
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, 16, 16);
            if (mouseRectangle.rectangle.Intersects(rectangle))
            {
                UnitState = State.Hovered;
            }
            else if (UnitState != State.Selected)
            {
                UnitState = State.Normal;
            }
            if (!mouseRectangle.rectangle.Intersects(rectangle) && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed && UnitState == State.Selected)
            {
                UnitState = State.Normal;
            }
            if (mouseRectangle.rectangle.Intersects(rectangle) && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Released && UnitState == State.Hovered)
            {
                UnitState = State.Selected;
            }
            
            if (UnitState == State.Normal)
                Texture = NormalTexture;

            if (UnitState == State.Hovered)
                Texture = HoveredTexture;

            if (UnitState == State.Selected)
                Texture = SelectedTexture;
                
            Astar(gameTime, mouseRectangle, world, UnitID, Units);
        }

        void Astar(GameTime gameTime, MouseRectangle mouseRectangle, World world, int UnitID, List<Unit> Units)
        {
            if (MouseCursor.CurrentMouseState.RightButton == ButtonState.Pressed && MouseCursor.LastMouseState.RightButton == ButtonState.Released && UnitState == State.Selected)
            {
                Console.WriteLine("Lul a path muss gefunden werden...");
                astarThreadWorker = null;
                AstarManager.AddNewThreadWorker(new AI.Node(new Vector2((int)Position.X / Game1.PixelsPerTile, (int)Position.Y / Game1.PixelsPerTile)),
                                            new AI.Node(MouseCursor.CurrentMouseState.Position.ToVector2() / Game1.PixelsPerTile), world, false, UnitID);
            }

            AstarManager.AstarThreadWorkerResults.TryPeek(out astarThreadWorkerTemp);

            if (astarThreadWorkerTemp != null)
                if (astarThreadWorkerTemp.WorkerIDNumber == UnitID)
                {
                    Console.WriteLine("lal");
                    AstarManager.AstarThreadWorkerResults.TryDequeue(out astarThreadWorker);

                    if (astarThreadWorker != null)
                    {
                        wayPoint = new WayPoint();

                        WayPointsList = astarThreadWorker.astar.GetFinalPath();

                        for (int i = 0; i < WayPointsList.Count; i++)
                            WayPointsList[i] = new Vector2(WayPointsList[i].X * 16, WayPointsList[i].Y * 16);
                    }
                }

            if (WayPointsList.Count > 0)
            {
                Avoidence(gameTime, Units, UnitID);
                wayPoint.MoveTo(gameTime, this, WayPointsList, Speed);
            }
        }

        void Avoidence(GameTime gameTime, List<Unit> Units, int UnitID)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                if (Units[i].rectangle.Intersects(rectangle))
                {
                    float Distance1 = Vector2.Distance(Position, WayPointsList[WayPointsList.Count - 1]);
                    float Distance2 = Vector2.Distance(Units[i].Position, WayPointsList[WayPointsList.Count - 1]);

                    if (Distance1 > Distance2)
                    {
                        Vector2 OppositeDirection = Units[i].Position - Position;
                        OppositeDirection.Normalize();
                        Position -= OppositeDirection * (float)(Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                }
            }
        }

        public static Texture2D[] textureByType(UnitType type)
        {
            Texture2D[] textures = new Texture2D[3];
            switch (type)
            {
                case UnitType.Worker:
                default:
                    textures[0] = Game1.textures[TextureNames.WorkerNormal];
                    textures[1] = Game1.textures[TextureNames.WorkerHovered];
                    textures[2] = Game1.textures[TextureNames.WorkerSelected];
                    return textures;
            }
        }

        public Tile.Tile[] RevealTiles()
        {
            int r = (int)_viewDistance + 1;
            Tile.Tile[] tiles = new Tile.Tile[r * r * 4];
            int counter = 0;
            for (int x = 0; x < r * 2; x++)
            {
                for (int y = 0; y < r * 2; y++)
                {
                    int index = (x + (int)this.Position.X / Game1.PixelsPerTile) * Game1.world.Width + (y + (int)this.Position.Y / Game1.PixelsPerTile);
                    if (index < Game1.world.Tiles.Count && index >= 0)
                    {
                        Tile.Tile t = Game1.world.Tiles[index];
                        tiles[counter] = t;
                    }
                    counter++;
                }
            }
            List<Tile.Tile> revealedTiles = new List<Tile.Tile>();
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] != null)
                {
                    if (Vector2.Distance(this.Position, tiles[i].Position) < this._viewDistance)
                    {
                        if (!tiles[i].isRevealed)
                            revealedTiles.Add(tiles[i]);
                        tiles[i].isRevealed = true;
                    }
                }
            }
            return revealedTiles.ToArray();
        }
        //private Astar astar = new AI.Astar(new AI.Node(new Vector2(0,0)), new AI.Node(new Vector2(16,16)), Game1.world, false);
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            /*
            for(int i = 0; i < Game1.world.notWalkablesTiles.Count;i++)
            {
                spriteBatch.Draw(Game1.textures[TextureNames.QUAD], Game1.world.notWalkablesTiles[i].Position, Color.Red * 0.3f);
            }
            for(int x = 0; x < this.nodes.GetLength(0); x++)
                for (int y = 0; y < this.nodes.GetLength(1);y++)
                    if(!this.nodes[x, y].Passable)
                        spriteBatch.Draw(Game1.textures[TextureNames.QUAD], this.nodes[x, y].Position * Game1.PixelsPerTile, Color.Green * 0.3f);
                        */
        }
    }
    public enum UnitType
    {
        Worker
    }
}
