using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarCraft2.Common;
using WarCraft2.Navigation;

namespace WarCraft2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MoverComponent _mover;
        private NavigationArea _navi;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Components.Add(_mover = new MoverComponent(this));
            Components.Add(_navi = new NavigationArea(this));
            var diag = new DiagnosticsComponent(this);
            Components.Add(diag);
            Services.AddService<IDiagnostics>(diag);
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

            _navi.TopLeft = new Vector2(10);
            _navi.Map = new NavigationCell[16, 16];

            var area = _navi;
            area.Map[1, 1] = NavigationCell.Unit;
            //area.Map[10, 0] = NavigationCell.PermanentObstacle;
            //area.Map[10, 2] = NavigationCell.PermanentObstacle;
            //area.Map[10, 3] = NavigationCell.PermanentObstacle;
            //area.Map[10, 4] = NavigationCell.PermanentObstacle;

            //for (int i = 0; i < 15; i++)
            //{
            //    area.Map[i, 8] = NavigationCell.PermanentObstacle;
            //}
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                var m = Mouse.GetState().Position;
                _mover.Create(new Vector2(), new Vector2(m.X, m.Y), 1.0f / 1.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _navi.TopLeft -= new Vector2(5);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                var m = Mouse.GetState().Position;
                //_navi.FindRoute(new Point(12, 12 ), new Point(m.X / 32, m.Y / 32));
                for (int i = 0; i < 35; i++)
                {
                    _navi.FindRoute(new Point(15, 15), new Point(m.X / 32, m.Y / 32));
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
