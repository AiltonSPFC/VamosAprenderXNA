using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XnaSpaceSimulator
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        FreeCamera camera;

        GameModel ground;
        BillboardSystem trees;
        private MouseState lastMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private void CreateBillboardSystem()
        {
            var r = new Random();
            var positions = new Vector3[150];
            var tree = Content.Load<Texture2D>("tree");

            for (int i = 0; i < positions.Length; i++)
            {
                var x = (float)(r.NextDouble() - 0.5) * 20000;
                var y = (float)(r.NextDouble() - 0.5) * 20000;
                positions[i] = new Vector3(x, tree.Bounds.Height, y);
            }

            trees = new BillboardSystem(GraphicsDevice, Content,
                tree, new Vector2(tree.Bounds.Width, tree.Bounds.Height),
                positions);
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ground = Content.Load<Model>("ground");
            this.CreateBillboardSystem();

            camera = new FreeCamera(new Vector3(0, 700, 3000),
                MathHelper.ToRadians(0),
                MathHelper.ToRadians(5),
                GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;
            ((FreeCamera)camera).Rotate(deltaX * .005f, deltaY * .005f);

            Vector3 translation = Vector3.Zero;
            if (keyState.IsKeyDown(Keys.Up))
                translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.Down))
                translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.Left))
                translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.Right))
                translation += Vector3.Right;

            translation *= 4 *
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            ((FreeCamera)camera).Move(translation);

            camera.Update();

            lastMouseState = mouseState;

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ground.Draw(camera.View, camera.Projection);
            trees.Draw(camera.View, camera.Projection, camera.Up, camera.Right);

            base.Draw(gameTime);
        }
    }
}
