using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UserInput
{
    class AnimationGame : Game
    {
        Animation run;
        Animation jump;
        Animation celebrate;
        Animation die;
        Animation idle;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public AnimationGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            run = new Animation(this, @"Images\Sprites\Player\Run", 10);
            jump = new Animation(this, @"Images\Sprites\Player\Jump", 11);
            celebrate = new Animation(this, @"Images\Sprites\Player\Celebrate", 11);
            die = new Animation(this, @"Images\Sprites\Player\Die", 12);
            idle = new Animation(this, @"Images\Sprites\Player\Idle");
        }

        protected override void LoadContent()
        {
            run.LoadContent();
            jump.LoadContent();
            celebrate.LoadContent();
            die.LoadContent();
            idle.LoadContent();

            int x, y;
            y = (Window.ClientBounds.Height - 64) / 2;
            x = (Window.ClientBounds.Width - (64 * 5)) / 2;

            run.Position = new Vector2(x, y);
            jump.Position = new Vector2(x += 64, y);
            celebrate.Position = new Vector2(x += 64, y);
            die.Position = new Vector2(x += 64, y);
            idle.Position = new Vector2(x += 64, y);

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            run.Update(gameTime);
            jump.Update(gameTime);
            celebrate.Update(gameTime);
            die.Update(gameTime);
            idle.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            run.Draw(spriteBatch, gameTime);
            jump.Draw(spriteBatch, gameTime);
            celebrate.Draw(spriteBatch, gameTime);
            die.Draw(spriteBatch, gameTime);
            idle.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
