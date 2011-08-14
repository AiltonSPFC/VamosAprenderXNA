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


        IPositionableCamera camera1;
        IPositionableCamera camera2;

        readonly List<IDrawableModel> models = new List<IDrawableModel>();
        GameModel spaceship1;
        GameModel spaceship2;

        Viewport defaultViewport;
        Viewport topViewport;
        Viewport bottomViewport;

        Terrain ground;

        //Effect groundEffect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            this.Components.Add(new FpsGameComponent(this, graphics));

            var device = graphics.GraphicsDevice;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceship1 = new GameModel(Content.Load<Model>("spaceship"))
            {
                Position = new Vector3(0, 3700, -2800),
                Scale = new Vector3(50f),
                BaseRotation = new Vector3(0, MathHelper.Pi, 0),
                Rotation = new Vector3(0, MathHelper.Pi, 0)
            };

            models.Add(spaceship1);


            spaceship2 = new GameModel(Content.Load<Model>("ship"))
            {
                Position = new Vector3(0, 3500, 4000),
                Scale = new Vector3(0.7f),
                BaseRotation = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };

            models.Add(spaceship2);

            var effect = Content.Load<Effect>("BasicTerrainEffect");
            effect.Parameters["Texture"].SetValue(Content.Load<Texture2D>("grass"));

            ground = new Terrain(
                Content.Load<Texture2D>("heightmap1"),
                effect,
                30, 4800, device
                );

            models.Add(ground);

            defaultViewport = graphics.GraphicsDevice.Viewport;
            topViewport = defaultViewport;
            bottomViewport = defaultViewport;

            //topViewport.Height /= 2;
            //topViewport.Height--;
            //bottomViewport.Height /= 2;
            //bottomViewport.Height--;
            //bottomViewport.Y =
            //    Window.ClientBounds.Height - bottomViewport.Height;

            topViewport.Width /= 2;
            topViewport.Width--;
            bottomViewport.Width /= 2;
            bottomViewport.Width--;

            bottomViewport.X =
                Window.ClientBounds.Width - bottomViewport.Width;

            camera1 = new ChaseCamera(
                new Vector3(0, 400, 1500),
                new Vector3(0, 200, 0),
                new Vector3(0, 0, 0),
                GraphicsDevice,
                new Vector3(0, 3700, -2800), topViewport);

            camera2 = new ChaseCamera(
                new Vector3(0, 400, 1500),
                new Vector3(0, 200, 0),
                new Vector3(0, 0, 0),
                GraphicsDevice,
                new Vector3(0, 3500, 4000), bottomViewport);


        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateSpaceship1(gameTime);
            UpdateSpaceship2(gameTime);
            UpdateCamera(gameTime);

            base.Update(gameTime);
        }

        void UpdateSpaceship1(GameTime gameTime)
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

            spaceship1.Rotation += rotChange * .025f;

            if (!keyState.IsKeyDown(Keys.Up) &&
                !keyState.IsKeyDown(Keys.Down))
                return;

            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                spaceship1.Rotation.Y, spaceship1.Rotation.X, spaceship1.Rotation.Z
                );

            spaceship1.Position +=
                Vector3.Transform(
                    keyState.IsKeyDown(Keys.Up) ? Vector3.Forward : Vector3.Backward,
                    rotation
                    )
                * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
        }

        void UpdateSpaceship2(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector3 rotChange = new Vector3(0, 0, 0);

            if (keyState.IsKeyDown(Keys.R))
                rotChange += new Vector3(1, 0, 0);

            if (keyState.IsKeyDown(Keys.F))
                rotChange += new Vector3(-1, 0, 0);

            if (keyState.IsKeyDown(Keys.A))
                rotChange += new Vector3(0, 1, 0);

            if (keyState.IsKeyDown(Keys.D))
                rotChange += new Vector3(0, -1, 0);

            spaceship2.Rotation += rotChange * .025f;

            if (!keyState.IsKeyDown(Keys.W) &&
                !keyState.IsKeyDown(Keys.S))
                return;

            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                spaceship2.Rotation.Y, spaceship2.Rotation.X, spaceship2.Rotation.Z
                );

            spaceship2.Position +=
                Vector3.Transform(
                    keyState.IsKeyDown(Keys.W) ? Vector3.Forward : Vector3.Backward,
                    rotation
                    )
                * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
        }

        void UpdateCamera(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            var chase = camera1 as ChaseCamera;
            chase.Move(
                spaceship1.Position,
                spaceship1.Rotation
                );
            camera1.Update();


            chase = camera2 as ChaseCamera;
            chase.Move(
                spaceship2.Position,
                spaceship2.Rotation
                );

            camera2.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Viewport = topViewport;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (IDrawableModel model in models)
                model.Draw(camera1.View, camera1.Projection);


            GraphicsDevice.Viewport = bottomViewport;
            foreach (IDrawableModel model in models)
                model.Draw(camera2.View, camera2.Projection);

            base.Draw(gameTime);
        }
    }
}
