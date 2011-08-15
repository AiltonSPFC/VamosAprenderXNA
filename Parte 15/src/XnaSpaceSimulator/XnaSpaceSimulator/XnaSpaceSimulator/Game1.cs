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

        public readonly List<IDrawableModel> Models = new List<IDrawableModel>();
        Spaceship spaceship1;
        Spaceship spaceship2;
        Terrain ground;

        Viewport viewport1;
        Viewport viewport2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
#if DEBUG
            this.Components.Add(new FpsGameComponent(this, graphics));
#endif
            SetupViewports();
            LoadSpaceship1();
            LoadSpaceship2();
            LoadGround();
        }

        private void SetupViewports()
        {
            var defaultViewport = graphics.GraphicsDevice.Viewport;
            viewport1 = defaultViewport;
            viewport2 = defaultViewport;

            viewport1.Width /= 2;
            viewport1.Width--;
            viewport2.Width /= 2;
            viewport2.Width--;

            viewport1.X =
                Window.ClientBounds.Width - viewport2.Width;
        }

        private void LoadGround()
        {
            var effect = Content.Load<Effect>("BasicTerrainEffect");
            effect.Parameters["Texture"].SetValue(Content.Load<Texture2D>("grass"));

            ground = new Terrain(
                Content.Load<Texture2D>("heightmap1"),
                effect,
                30, 4800, this.GraphicsDevice
                );

            Models.Add(ground);
        }

        private void LoadSpaceship2()
        {
            spaceship2 = new Spaceship(this, new GameModel(Content.Load<Model>("ship"))
            {
                Position = new Vector3(0, 3500, 4000),
                Scale = new Vector3(0.7f),
                BaseRotation = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            }, viewport2)
            {
                Up = Keys.R,
                Down = Keys.F,
                Left = Keys.A,
                Right = Keys.D,
                Forward = Keys.W,
                Backward = Keys.S
            };

            this.Components.Add(spaceship2);
            Models.Add(spaceship2.Model);
        }

        private void LoadSpaceship1()
        {
            spaceship1 = new Spaceship(this, new GameModel(Content.Load<Model>("spaceship"))
            {
                Position = new Vector3(0, 3700, -2800),
                Scale = new Vector3(50f),
                BaseRotation = new Vector3(0, MathHelper.Pi, 0),
                Rotation = new Vector3(0, MathHelper.Pi, 0)
            }, viewport1)
            {
                Up = Keys.PageUp,
                Down = Keys.PageDown,
                Left = Keys.Left,
                Right = Keys.Right,
                Forward = Keys.Up,
                Backward = Keys.Down
            };
            this.Components.Add(spaceship1);
            Models.Add(spaceship1.Model);
        }

        protected override void Draw(GameTime gameTime)
        {
            DrawScene(spaceship1.Camera);
            DrawScene(spaceship2.Camera);
            
            base.Draw(gameTime);
        }

        public void DrawScene(Camera camera)
        {
            GraphicsDevice.Viewport = camera.Viewport;
            foreach (IDrawableModel model in Models)
                model.Draw(camera.View, camera.Projection);
        }
    }
}
