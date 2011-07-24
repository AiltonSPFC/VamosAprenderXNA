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

        ChaseCamera camera;

        readonly List<GameModel> models = new List<GameModel>();
        GameModel spaceship;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceship = new GameModel(Content.Load<Model>("spaceship"))
            {
                Position = new Vector3(0, 400, 0),
                Scale = new Vector3(50f),
                BaseRotation = new Vector3(0, MathHelper.Pi, 0)
            }
            .SetEffect(Content.Load<Effect>("TotalLightEffect"));
            
            models.Add(spaceship);

            var ground = new GameModel(Content.Load<Model>("ground"))
                .SetEffect(Content.Load<Effect>("TotalLightEffect"));

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

        // Called when the game should update itself
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

            if (keyState.IsKeyDown(Keys.W))
                camera.PositionOffset += Vector3.Forward * 10;

            if (keyState.IsKeyDown(Keys.S))
                camera.PositionOffset += Vector3.Backward * 10;

            if (keyState.IsKeyDown(Keys.A))
                camera.PositionOffset += Vector3.Left * 10;

            if (keyState.IsKeyDown(Keys.D))
                camera.PositionOffset += Vector3.Right * 10;

            ((ChaseCamera)camera).Move(
                spaceship.Position,
                spaceship.Rotation
                );

            foreach (var model in this.models)
            {
                model.UpdateCameraPosition(((ChaseCamera)camera).Position);
            }

            camera.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (GameModel model in models)
                if (camera.IsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}
