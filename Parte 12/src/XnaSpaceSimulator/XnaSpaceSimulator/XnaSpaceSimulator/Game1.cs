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

using ElemarJR.Xna.Cameras;
using ElemarJR.Xna;
using ElemarJR.Xna.Environmental;

namespace XnaSpaceSimulator
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        IPositionableCamera camera;

        readonly List<IDrawableModel> models = new List<IDrawableModel>();
        GameModel spaceship;
        Terrain ground;

        //Effect groundEffect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {

            var device = graphics.GraphicsDevice;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceship = new GameModel(Content.Load<Model>("spaceship"))
            {
                Position = new Vector3(0, 3500, 0),
                Scale = new Vector3(50f),
                BaseRotation = new Vector3(0, MathHelper.Pi, 0),
                Rotation = new Vector3(0, MathHelper.Pi, 0)
            };

            models.Add(spaceship);

            ground = new Terrain(
                Content.Load<Texture2D>("heightmap1"),
                30, 4800, device
                );

            models.Add(ground);

            camera = new ChaseCamera(
                new Vector3(0, 400, 1500),
                new Vector3(0, 200, 0),
                new Vector3(0, 0, 0),
                GraphicsDevice);


        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateModel(gameTime);
            UpdateCamera(gameTime);

            base.Update(gameTime);
        }

        void UpdateModel(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector3 rotChange = new Vector3(0, 0, 0);

            if (keyState.IsKeyDown(Keys.PageUp))
                rotChange += new Vector3(1, 0, 0);

            if (keyState.IsKeyDown(Keys.PageDown))
                rotChange += new Vector3(-1, 0, 0);

            if (keyState.IsKeyDown(Keys.Left))
                rotChange += new Vector3(0, 1, 0);

            if (keyState.IsKeyDown(Keys.Right))
                rotChange += new Vector3(0, -1, 0);

            spaceship.Rotation += rotChange * .025f;

            if (!keyState.IsKeyDown(Keys.Up) &&
                !keyState.IsKeyDown(Keys.Down))
                return;

            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                spaceship.Rotation.Y, spaceship.Rotation.X, spaceship.Rotation.Z
                );

            spaceship.Position +=
                Vector3.Transform(
                    keyState.IsKeyDown(Keys.Up) ? Vector3.Forward : Vector3.Backward,
                    rotation
                    )
                * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
        }

        void UpdateCamera(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            var chase = camera as ChaseCamera;
            if (chase != null)
            {
                if (keyState.IsKeyDown(Keys.W))
                    chase.PositionOffset += Vector3.Forward * 10;

                if (keyState.IsKeyDown(Keys.S))
                    chase.PositionOffset += Vector3.Backward * 10;

                if (keyState.IsKeyDown(Keys.A))
                    chase.PositionOffset += Vector3.Left * 10;

                if (keyState.IsKeyDown(Keys.D))
                    chase.PositionOffset += Vector3.Right * 10;


                if (keyState.IsKeyDown(Keys.R))
                    chase.PositionOffset += Vector3.Up * 10;

                if (keyState.IsKeyDown(Keys.F))
                    chase.PositionOffset += Vector3.Down * 10;

                chase.Move(
                    spaceship.Position,
                    spaceship.Rotation
                    );

                foreach (var model in this.models)
                    if (model is GameModel)
                        ((GameModel)model).UpdateCameraPosition(chase.Position);
            }
            else
            {
                foreach (var model in this.models)
                    if (model is GameModel)
                        ((GameModel)model).UpdateCameraPosition(((FreeCamera)camera).Position);
            }

            camera.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (IDrawableModel model in models)
                model.Draw(camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}
