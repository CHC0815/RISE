using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using RISE.Code;
using RISE.Code.Tile;
//using RISE.Code.Pathfinding;
using RISE.Code.UI;
using RISE.Code.Building;
using RISE.Code.Player;
using RISE.Code.Units;
using MouseCursor = RISE.Code.MouseCursor;

namespace RISE
{
    /// <summary>
    /// RISE
    /// by Conrad Heinrich Carl
    /// </summary>
    public class Game1 : Game
    {
        #region graphics
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch gui;
        public static Camera mainCamera;
        public static Vector2 WindowSize = new Vector2(800, 480);
        public static List<IRenderable> renderables = new List<IRenderable>();
        #endregion

        public static World world;
        public static Dictionary<int, UIManager> currentUIManagers = new Dictionary<int, UIManager>();
        public static Dictionary<int, UIManager> UIs = new Dictionary<int, UIManager>();
        public static bool UI_debug = false;
        public static int PixelsPerTile = 16;

        public static Player player = null;

        //all gameobjects
        public static Dictionary<int, GameObject> gameObjects = new Dictionary<int, GameObject>();
        //all loaded textures
        public static Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();

        public static SpriteFont MAINFONT;

        public static bool isDebugMode = true;

        MouseRectangle mouseRectangle;
        public static UnitManager unitManager;

        Lable l;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.HardwareModeSwitch = true;
            graphics.PreferredBackBufferWidth = (int)WindowSize.X;
            graphics.PreferredBackBufferHeight = (int)WindowSize.Y;
            //graphics.ToggleFullScreen();
            graphics.SynchronizeWithVerticalRetrace = false;
            base.IsFixedTimeStep = false;
            base.IsMouseVisible = true;
            Content.RootDirectory = "Content";

            mouseRectangle = new MouseRectangle();
            unitManager = new UnitManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            mainCamera = new Camera(GraphicsDevice.Viewport);
            world = new World(256, 256, PixelsPerTile, GraphicsDevice);
            mainCamera.MoveCamera(new Vector2(world.Width / 2, world.Height / 2) * PixelsPerTile);
            player = new Player("Alpha KEK");

            mouseRectangle.Initialize(graphics);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //init main spritebatch and gui spritebatch
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gui = new SpriteBatch(GraphicsDevice);

            //util
            textures.Add(TextureNames.LINE, new Texture2D(GraphicsDevice, 1, 1));
            textures[TextureNames.LINE].SetData<Color>(new Color[] { Color.White });
            textures.Add(TextureNames.QUAD, this.Content.Load<Texture2D>("Quad"));
            textures.Add(TextureNames.MOUSE_RECTANGLE, this.Content.Load<Texture2D>("UI/MouseRectangle"));

            //tiles
            textures.Add(TextureNames.RISELOGO, this.Content.Load<Texture2D>("RISELOGO"));
            textures.Add(TextureNames.BUSH, this.Content.Load<Texture2D>("Tiles/Bush"));
            textures.Add(TextureNames.BUSH_BERRY, this.Content.Load<Texture2D>("Tiles/BerryBush"));
            textures.Add(TextureNames.DIRT, this.Content.Load<Texture2D>("Tiles/Dirt"));
            textures.Add(TextureNames.GRASS, this.Content.Load<Texture2D>("Tiles/Grass"));
            textures.Add(TextureNames.GRASS_THICK, this.Content.Load<Texture2D>("Tiles/GrassAtl"));
            textures.Add(TextureNames.HILL, this.Content.Load<Texture2D>("Tiles/Hill"));
            textures.Add(TextureNames.MOUNTAIN, this.Content.Load<Texture2D>("Tiles/Mountain"));
            textures.Add(TextureNames.SAND, this.Content.Load<Texture2D>("Tiles/Sand"));
            textures.Add(TextureNames.STONE, this.Content.Load<Texture2D>("Tiles/Stone"));
            textures.Add(TextureNames.TREE, this.Content.Load<Texture2D>("Tiles/Tree"));
            textures.Add(TextureNames.TREE_PINE, this.Content.Load<Texture2D>("Tiles/Pine"));
            textures.Add(TextureNames.WATER, this.Content.Load<Texture2D>("Tiles/Water"));

            //units
            textures.Add(TextureNames.WorkerNormal, this.Content.Load<Texture2D>("Units/worker"));
            textures.Add(TextureNames.WorkerHovered, this.Content.Load<Texture2D>("Units/worker_hovered"));
            textures.Add(TextureNames.WorkerSelected, this.Content.Load<Texture2D>("Units/worker_selected"));

            //ui stuff
            textures.Add(TextureNames.Button_normal, this.Content.Load<Texture2D>("UI/Button/Button_normal"));
            textures.Add(TextureNames.Button_pressed, this.Content.Load<Texture2D>("UI/Button/Button_clicked"));

            //buildings
            textures.Add(TextureNames.Town_center, this.Content.Load<Texture2D>("Buildings/TownCenter"));
            textures.Add(TextureNames.House, this.Content.Load<Texture2D>("Buildings/House"));
            textures.Add(TextureNames.Wall, this.Content.Load<Texture2D>("Buildings/Wall"));
            textures.Add(TextureNames.Tower, this.Content.Load<Texture2D>("Buildings/Tower"));

            //fonts
            MAINFONT = this.Content.Load<SpriteFont>("fps_font");
            
            afterContentLoading();
        }
        public void afterContentLoading()
        {
            mouseRectangle.LoadContent();
            mouseRectangle.UpdateOnce();

            InitUIManager();
            world.GenerateWorld();
            //Pathfinding.grid = new Grid(world, 0.5f);
        }
        /// <summary>
        /// outsource init of ui managers
        /// </summary>
        private void InitUIManager()
        {
            //main menu
            UIManager MainMenu = new UIManager();
            MainMenu.buttons.Add(new Button(new Vector2(650, 10), new Vector2(100,50), () => ButtonFunctions.ToggleBuildMenu(), "Build Menu"));
            UIs.Add(MenuID.MAINMENU, MainMenu);

            //build menu
            UIManager BuildMenu = new UIManager();
            BuildMenu.buttons.Add(new Button(new Vector2(650, 80), new Vector2(100, 50), () => ButtonFunctions.Building(BuildingType.TownCenter), "Town Center"));
            BuildMenu.buttons.Add(new Button(new Vector2(650, 140), new Vector2(100, 50), () => ButtonFunctions.Building(BuildingType.House), "House"));
            BuildMenu.buttons.Add(new Button(new Vector2(650, 200), new Vector2(100, 50), () => ButtonFunctions.Building(BuildingType.Wall), "Wall"));
            BuildMenu.buttons.Add(new Button(new Vector2(650, 260), new Vector2(100, 50), () => ButtonFunctions.Building(BuildingType.Tower), "Tower"));
            UIs.Add(MenuID.BUILDMENU, BuildMenu);

            //debug menu
            UIManager DEBUG_UI = new UIManager();
            DEBUG_UI.lables.Add(new FPS_Counter("FPS", new Vector2(1, 1)));
            DEBUG_UI.buttons.Add(new Button(new Vector2(1, 20), new Vector2(75, 75/2), () => ButtonFunctions.ToggleDebugMode(), "Debug"));
            l = new Lable("LAL", new Vector2(1, 60));
            DEBUG_UI.lables.Add(l);
            UIs.Add(MenuID.DEBUGMENU, DEBUG_UI);

            currentUIManagers.Add(MenuID.MAINMENU, UIs[MenuID.MAINMENU]);
            currentUIManagers.Add(MenuID.DEBUGMENU, UIs[MenuID.DEBUGMENU]);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(KeyBinds.Escape))
                Exit();
            MouseCursor.Update();
            mouseRectangle.Update(gameTime);
            unitManager.Update(gameTime, mouseRectangle, world);

            for (int i = 0; i < currentUIManagers.Keys.Count;i++)
            {
                currentUIManagers[i].Update(gameTime);
            }
            //updates every gameobject
            foreach (GameObject go in gameObjects.Values)
            {
                if(go.update)
                    go.Update(gameTime);
            }
            player.Update(gameTime);
            l.Text = "Current Mouse Pos: " + MouseCursor.Position / PixelsPerTile;
            l.baseColor = Color.White;
            //updates camera
            mainCamera.UpdateCamera(GraphicsDevice.Viewport, gameTime);
            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //BEGIN MAIN GAME
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, mainCamera.Transform);
            foreach (GameObject go in gameObjects.Values)
            {
                if (go.draw)
                {
                    spriteBatch.Draw(go.Texture, go.Position, go.SourceRectangle, go.baseColor, go.Rotation, go.Origin, 1.0f, SpriteEffects.None, 0f);
                }
            }

            foreach(IRenderable renderable in renderables)
            {
                renderable.Draw(spriteBatch, gameTime);
            }
            player.Draw(spriteBatch, gameTime);     
            mouseRectangle.Draw(spriteBatch);
            spriteBatch.End();

            //BEGIN GUI
            gui.Begin();
            foreach(UIManager manager in currentUIManagers.Values)
            {
                manager.Draw(gui, gameTime);
            }
            gui.End();
            base.Draw(gameTime);
        }
        /// <summary>
        /// Draws a line from start to end
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle = (float)System.Math.Atan2(edge.Y, edge.X);
            sb.Draw(textures[TextureNames.LINE],
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
        /// <summary>
        /// Draws the given rectangle
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="rect"></param>
        public static void DrawRectangle(SpriteBatch sb, Rectangle rect)
        {
            //top
            DrawLine(sb, new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y));
            //bottom
            DrawLine(sb , new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X + rect.Width, rect.Y + rect.Height));
            //left
            DrawLine(sb, new Vector2(rect.X, rect.Y), new Vector2(rect.X, rect.Y + rect.Height));
            //right
            DrawLine(sb, new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height));
        }

        /// <summary>
        /// This returns a new unique id for gameobjects
        /// </summary>
        private static int id = 0;
        public static int GetGameObjectId()
        {
            return id++;
        }
    }
}
