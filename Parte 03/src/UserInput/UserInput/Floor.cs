using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UserInput
{
    class Floor : IGameElement
    {
        public Floor(Game game)
        {
            this.Game = game;
        }

        public Game Game { get; private set; }

        readonly Texture2D[] Textures = new Texture2D[7];
        public void LoadContent()
        {
            for (int i = 0; i < 7; i++)
                Textures[i] = Game.Content.Load<Texture2D>(
                    string.Format(@"Images\Tiles\BlockA{0}", i)
                    );

        }

        public void UnloadContent()
        { }

        public void Update(GameTime gameTime)
        { }

        public void Draw(SpriteBatch spbatch, GameTime gameTime)
        {
            Vector2 p = new Vector2(0, Game.Window.ClientBounds.Height - 32);
            int index = 0;
            while (p.X < Game.Window.ClientBounds.Width)
            {
                index = (index + 1) % 7;
                var texture = Textures[index];
                spbatch.Draw(texture, p, Color.White);
                p.X += 40;
            }
        }
    }
}
