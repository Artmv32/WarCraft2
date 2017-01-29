using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarCraft2.Common;
using WarCraft2.Navigation;
using WarCraft2.PathFinder;

namespace WarCraft2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            var pathFinder = new PathFinderImplementation(this);
            Components.Add(pathFinder);
            Services.AddService<IPathFinder>(pathFinder);

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
            var map = new MapCell[512, 512];
            map[1, 1] = MapCell.Obstacle;
            map[10, 0] = MapCell.Obstacle;
            map[10, 2] = MapCell.Obstacle;
            map[10, 3] = MapCell.Obstacle;
            map[10, 4] = MapCell.Obstacle;

            for (int i = 0; i < 15; i++)
            {
                map[i, 8] = MapCell.Obstacle;
            }

            bool isLeft = true;
            for (int i = 20; i < 500; i += 2)
            {
                isLeft = !isLeft;
                for (int j = isLeft ? 0 : 1; j < (isLeft ? 512 : 511); j++)
                {
                    map[j, i] = MapCell.Obstacle;
                }
            }

            Services.GetService<IPathFinder>().Initialize(map);

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
                //_mover.Create(new Vector2(), new Vector2(m.X, m.Y), 1.0f / 1.0f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                //_navi.TopLeft -= new Vector2(5);
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
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                var m = Mouse.GetState().Position;
                var pathFinder = Services.GetService<IPathFinder>();
                var path = pathFinder.FindRoute(new Point(pathFinder.Width - 1, pathFinder.Height - 1), new Point(m.X / 32, m.Y / 32));
            }

            base.Draw(gameTime);
        }
    }
}
