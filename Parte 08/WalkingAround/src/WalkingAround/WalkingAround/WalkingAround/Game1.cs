using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace WalkingAround
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<GameModel> models = new List<GameModel>();
        Camera camera;

        MouseState lastMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            models.Add(Content.Load<Model>("ground"));


            camera = new WalkingCamera(new Vector3(0, 700, 3000),
                MathHelper.ToRadians(0),
                MathHelper.ToRadians(5),
                GraphicsDevice);

            lastMouseState = Mouse.GetState();
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            UpdateCamera(gameTime);

            base.Update(gameTime);
        }

        void UpdateCamera(GameTime gameTime)
        {
            //// Get the new keyboard and mouse state
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            //// Determine how much the camera should turn
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            //// Rotate the camera
            ((WalkingCamera)camera).Rotate(deltaX * .005f, deltaY * .005f);

            Vector3 translation = Vector3.Zero;
            if (keyState.IsKeyDown(Keys.Up)) translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.Down)) translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.Left)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.Right)) translation += Vector3.Right;

            if (keyState.IsKeyDown(Keys.PageUp)) translation += Vector3.Up;
            if (keyState.IsKeyDown(Keys.PageDown)) translation += Vector3.Down;

            //// Move 4 units per millisecond, independent of frame rate
            translation *= 4 * 
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            ((WalkingCamera)camera).Move(translation);

            //// Update the camera
            camera.Update();

            //// Update the mouse state
            lastMouseState = mouseState;
        }

        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (GameModel model in models)
                if (camera.IsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection);


            base.Draw(gameTime);
        }
    }
}
