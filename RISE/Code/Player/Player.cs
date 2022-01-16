using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RISE.Code.Building;

namespace RISE.Code.Player
{
    public class Player
    {
        #region Buildings
        public string Name;
        public bool isBuilding = false;
        public bool isPlacing = false;
        public Building.Building currentBuilding = null;
        private ButtonState lastState;
        #endregion

        Settings settings;

        public Player(string name)
        {
            this.Name = name;
            settings = new Settings();
        }
        public void Update(GameTime gameTime)
        {
            if(isBuilding && currentBuilding != null)
            {
                currentBuilding.Position = Mouse.GetState().Position.ToVector2() + Game1.mainCamera.Position - Game1.WindowSize/2;
                currentBuilding.Position = new Vector2((int)(currentBuilding.Position.X / Game1.PixelsPerTile) * Game1.PixelsPerTile + Game1.PixelsPerTile, ((int)(currentBuilding.Position.Y) / Game1.PixelsPerTile) * Game1.PixelsPerTile + Game1.PixelsPerTile) - currentBuilding.Size;
                if (Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().LeftButton != lastState)
                {
                    if(isPlacing)
                        TryPlaceBuilding();
                    isPlacing = true;
                }

            }
            lastState = Mouse.GetState().LeftButton;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (currentBuilding != null && !currentBuilding.isUndergroundFree())
            {
                Tile.Tile[] tiles = currentBuilding.GetTilesBelow();
                for(int i = 0; i < tiles.Length;i++)
                {
                    if(!currentBuilding.isTileWalkable(tiles[i]) && tiles[i] != null)
                        spriteBatch.Draw(Game1.textures[TextureNames.QUAD], tiles[i].Position, Color.FromNonPremultiplied(0, 255, 0, 255));
                }
            }
        }
        private void TryPlaceBuilding()
        {
            Console.WriteLine(currentBuilding.isUndergroundFree());
            if (!currentBuilding.isUndergroundFree())
                return;
            currentBuilding.BuildingPlaced();
            currentBuilding = null;
            isPlacing = false;
            isPlacing = false;
        }
    }
}
