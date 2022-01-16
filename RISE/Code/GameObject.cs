using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RISE.Code
{
    public abstract class GameObject
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Color baseColor = Color.White;
        public float Rotation = 0f;
        public Vector2 Origin = new Vector2(0f, 0f);
        public Rectangle SourceRectangle;
        public bool update = true;
        public bool draw = true;
        public bool isSelected = false;
        public int GameObjectID;

        public GameObject(int id, Texture2D texture, Vector2 position, float rotation)
        {
            this.GameObjectID = id;
            this.Texture = texture;
            this.Position = position;
            this.Rotation = rotation;
            this.Origin = new Vector2(this.Texture.Width / 2, this.Texture.Height / 2);
            this.SourceRectangle = new Rectangle(0, 0, this.Texture.Width, this.Texture.Height);
            Start();
        }
        public virtual void Start()
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Destroy()
        {

        }
    }
    public interface IhasViewDistance
    {
        float getViewDistance();
        Tile.Tile[] RevealTiles();
    }
    public interface IRenderable
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
