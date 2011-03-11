using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UserInput
{
    interface IGameElement
    {
        Game Game { get; }

        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spbatch, GameTime gameTime);
        void UnloadContent();
    }
}
