using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using WarCraft2.Common;

namespace WarCraft2
{
    public struct MovingTicket
    {
        private MoverComponent.MovingInfo _info;

        public Vector2 Position
        {
            get { return _info.Position; }
        }

        public bool IsMoving
        {
            get { return _info.IsMoving; }
        }

        public MovingTicket(MoverComponent.MovingInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            _info = info;
        }

        public void Stop()
        {
            _info.Stop = true;
        }
    }

    public class MoverComponent : DrawableGameComponent
    {
        private readonly LinkedList<MovingInfo> _movingInfos = new LinkedList<MovingInfo>();
        private readonly Pool<MovingInfo> _pool = new Pool<MovingInfo>(50);

#if DEBUG
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private SpriteFont _spriteFont;
#endif

        public class MovingInfo
        {
            public Vector2 Start { get; set; }

            public Vector2 Position { get; set; }

            public Vector2 End { get; set; }

            public Vector2 Direction { get; set; }

            public float Distance { get; set; }

            public float Speed { get; set; }

            public int Elapsed { get; set; }

            public bool IsMoving { get; set; }

            public bool Stop { get; set; }
        }

        public MoverComponent(Game game) : base(game)
        {
        }

        public MovingTicket Create(Vector2 start, Vector2 end, float speed)
        {
            float distance = Vector2.Distance(start, end);
            Vector2 direction = Vector2.Normalize(end - start);
            var item = _pool.New();
            item.Position = item.Start;
            item.Start = start;
            item.End = end;
            item.Direction = direction;
            item.IsMoving = true;
            item.Speed = speed;
            item.Distance = Vector2.Distance(start, end);
            _movingInfos.AddLast(item);
            return new MovingTicket(item);
        }

        protected override void LoadContent()
        {
#if DEBUG
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _texture = Game.Content.Load<Texture2D>("Arrow");
            _spriteFont = Game.Content.Load<SpriteFont>("DefaultFont");
#endif

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var item = _movingInfos.First;
            while (item != null)
            {
                var next = item.Next;
                var info = item.Value;
                if (info.IsMoving && !info.Stop)
                {
                    info.Position += info.Direction * info.Speed * gameTime.ElapsedGameTime.Milliseconds;

                    if (Vector2.Distance(info.Start, info.Position) >= info.Distance)
                    {
                        info.Position = info.End;
                        info.IsMoving = false;
                    }
                }
                else
                {
                    _movingInfos.Remove(item);
                    _pool.Return(item.Value);
                }
                item = next;
            }

            base.Update(gameTime);
        }

#if DEBUG
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            foreach (var item in _movingInfos)
            {
                _spriteBatch.Draw(_texture, item.Position, Color.White);
            }

            _spriteBatch.DrawString(_spriteFont, string.Format("Pool capacity: {0}, active objects: {1}", _pool.Count.ToString(), _movingInfos.Count.ToString()), new Vector2(10, 10), Color.Red);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
#endif
    }
}
