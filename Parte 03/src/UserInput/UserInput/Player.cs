using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UserInput
{
    enum PlayerActions
    {
        Idle = 0,
        Run = 1,
        Jump = 2,
        Celebrate = 3,
        Die = 4
    }

    class Player : IGameElement
    {
        readonly Animation[] Animations = new Animation[5];

        public Player(Game game)
        {
            this.Game = game;

            Animations[(int)PlayerActions.Idle] =
                new Animation(game, @"Images\Sprites\Player\Idle");

            Animations[(int)PlayerActions.Run] =
                new Animation(game, @"Images\Sprites\Player\Run", 10);

            Animations[(int)PlayerActions.Jump] =
                new Animation(game, @"Images\Sprites\Player\Jump", 11);
            Animations[(int)PlayerActions.Jump].AutoRepeat = false;

            Animations[(int)PlayerActions.Celebrate] =
                new Animation(game, @"Images\Sprites\Player\Celebrate", 11);

            Animations[(int)PlayerActions.Die] =
                new Animation(game, @"Images\Sprites\Player\Die", 12);
            Animations[(int)PlayerActions.Die].AutoRepeat = false;
        }

        public void Reset()
        {
            CurrentAction = PlayerActions.Idle;
            Position = new Vector2(
                Game.Window.ClientBounds.Width / 2,
                Position.Y);
        }

        public Game Game
        {
            get;
            private set;
        }

        public Vector2 Position { get; set; }
        public PlayerActions CurrentAction { get; private set; }
        public bool LeftToRight { get; private set; }

        public void LoadContent()
        {
            for (int i = 0; i < 5; i++)
                Animations[i].LoadContent();

        }

        int speed = 4;
        public void Update(GameTime gameTime)
        {
            if (this.CurrentAction != PlayerActions.Die)
            {


                float x = this.Position.X;
                float y = this.Position.Y;

                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
                {
                    x -= speed;
                    this.CurrentAction = PlayerActions.Run;
                    LeftToRight = false;
                }
                else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
                {
                    x += speed;
                    this.CurrentAction = PlayerActions.Run;
                    LeftToRight = true;
                }
                else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.C))
                {
                    this.CurrentAction = PlayerActions.Celebrate;
                }
                else
                {
                    this.CurrentAction = PlayerActions.Idle;
                }

                this.Position = new Vector2(x, y);

                if (x < 32) this.CurrentAction = PlayerActions.Die;
                if (x > Game.Window.ClientBounds.Width - 32)
                    this.CurrentAction = PlayerActions.Die;

                Animations[(int)CurrentAction].LeftToRight = LeftToRight;
            }

            Animations[(int)CurrentAction].Position = new Vector2(
                this.Position.X - Animations[(int)CurrentAction].Width / 2,
                this.Position.Y - Animations[(int)CurrentAction].Height
                );

            Animations[(int)CurrentAction].Update(gameTime);
        }

        public void Draw(SpriteBatch spbatch, GameTime gameTime)
        {
            Animations[(int)CurrentAction].Draw(spbatch, gameTime);
        }

        public void UnloadContent()
        {
        }
    }
}
