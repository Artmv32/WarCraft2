using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCraft2.Common
{
    public interface IDiagnostics
    {
        void SetParam(string param, string value);
    }

    public class DiagnosticsComponent : DrawableGameComponent, IDiagnostics
    {
        private readonly Dictionary<string, string> _params = new Dictionary<string, string>();
        private SpriteFont _font;
        private SpriteBatch _sb;

        public DiagnosticsComponent(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            _font = Game.Content.Load<SpriteFont>("DefaultFont");
            _sb = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            _sb.Begin();
            var start = new Vector2(5);
            int i = 0;
            foreach (var param in _params)
            {
                _sb.DrawString(_font, param.Value, start + new Vector2(0, 15 * i++), Color.Red);
            }
            _sb.End();
            base.Draw(gameTime);
        }

        public void SetParam(string param, string value)
        {
            _params[param] = value;
        }
    }
}
