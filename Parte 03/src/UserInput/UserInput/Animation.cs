using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UserInput
{
    class Animation : IGameElement
    {
        Texture2D texture;
        Rectangle[] frames;

        public Animation(Game game, string resource,
            int columns = 1,
            int rows = 1,
            int frameInterval = 60)
        {
            this.Game = game;
            this.Resource = resource;
            this.Columns = columns;
            this.Rows = rows;
            this.Position = Vector2.Zero;
            this.FrameInterval = frameInterval;
            this.AutoRepeat = true;
        }

        public string Resource { get; private set; }
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public int FrameInterval { get; private set; }
        public bool LeftToRight { get; set; }
        public bool AutoRepeat { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Game Game { get; private set; }
        public Vector2 Position { get; set; }

        public void Reset()
        {
            this.currentFrame = 0;
        }

        public void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>(this.Resource);
            var cellCount = Columns * Rows;

            double incx = texture.Width / (double)Columns;
            double incy = texture.Height / (double)Rows;

            this.Width = (int)incx;
            this.Height = (int)incy;

            frames = new Rectangle[cellCount];
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                {
                    var index = (i * Columns) + j;
                    frames[index] = new Rectangle(
                        (int)(incx * j),
                        (int)(incy * i),
                        this.Width,
                        this.Height
                        );
                }
        }

        int currentFrame;
        int timeSizeLastFrame;
        public void Update(GameTime gameTime)
        {
            timeSizeLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSizeLastFrame > FrameInterval)
            {
                timeSizeLastFrame -= FrameInterval;
                currentFrame++;
                if (currentFrame >= frames.Length)
                    currentFrame = (AutoRepeat ? 0 : frames.Length - 1);
            }
        }

        public void Draw(SpriteBatch spbatch, GameTime gameTime)
        {
            if (LeftToRight)
                spbatch.Draw(texture, this.Position, frames[currentFrame],
                    Color.White, 0, Vector2.Zero, Vector2.One,
                    SpriteEffects.FlipHorizontally, 0);
            else
                spbatch.Draw(texture, this.Position, frames[currentFrame],
                    Color.White);

        }

        public void UnloadContent()
        { }
    }
}
