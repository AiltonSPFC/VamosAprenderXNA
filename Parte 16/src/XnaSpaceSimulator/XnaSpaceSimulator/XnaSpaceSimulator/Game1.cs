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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        readonly List<IDrawableModel> Models = new List<IDrawableModel>();
        Effect effect;
        Spaceship spaceship;
        GameModel ground;

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
            effect = Content.Load<Effect>("projection");

            var viewport = graphics.GraphicsDevice.Viewport;
            effect.Parameters["ViewportWidth"].SetValue(viewport.Width);
            effect.Parameters["ViewportHeight"].SetValue(viewport.Width);

            var texture = Content.Load<Texture2D>("xnalogo");
            effect.Parameters["ProjectedTexture"].SetValue(texture);

            var scale = 3.0f;
            var projection = Matrix.CreateOrthographicOffCenter(
                (-texture.Width / 2) * scale,
                (texture.Width / 2) * scale,
                (-texture.Height / 2) * scale,
                (texture.Height / 2) * scale,
                -100000, 100000);


            var view = Matrix.CreateLookAt(
                new Vector3(1500, 1500, 1500),
                new Vector3(0, 150, 0),
                Vector3.Up
                );

            effect.Parameters["ProjectorViewProjection"].SetValue(view * projection);
            effect.Parameters["ProjectorEnabled"].SetValue(true);

            LoadSpaceship();
            LoadTeapot();
            LoadGround();
        }

        private void LoadTeapot()
        {
            var teapot = new GameModel(Content.Load<Model>("teapot"))
            {
                Position = new Vector3(0, 0, 0),
                Scale = new Vector3(50f),
                BaseRotation = new Vector3(0, MathHelper.Pi, 0),
                Rotation = new Vector3(0, MathHelper.Pi, 0)
            };

            var local = effect.Clone();
            local.Parameters["LightColor"].SetValue(new Vector3(1, 0, 0));

            teapot.SetEffect(local);

            var component = new MoveableModel(this, teapot)
            {
                Left = Keys.A,
                Right = Keys.D,
                Forward = Keys.W,
                Backward = Keys.S,
                Up = Keys.R,
                Down = Keys.F
            };

            Components.Add(component);
            Models.Add(teapot);
        }

        private void LoadGround()
        {
            ground = Content.Load<Model>("ground");
            ground.SetEffect(effect);
            Models.Add(ground);
        }

        private void LoadSpaceship()
        {

            var model = new GameModel(Content.Load<Model>("spaceship"))
            {
                Position = new Vector3(0, 400, 0),
                Scale = new Vector3(50f),
                BaseRotation = new Vector3(0, MathHelper.Pi, 0),
                Rotation = new Vector3(0, MathHelper.Pi, 0)
            };
            model.SetEffect(effect);

            spaceship = new Spaceship(this, model, graphics.GraphicsDevice.Viewport)
            {
                Up = Keys.PageUp,
                Down = Keys.PageDown,
                Left = Keys.Left,
                Right = Keys.Right,
                Forward = Keys.Up,
                Backward = Keys.Down
            };

            this.Components.Add(spaceship);
            Models.Add(spaceship.Model);
        }

        protected override void Update(GameTime gameTime)
        {
            spaceship.Model.UpdateCameraPosition(
                spaceship.Camera.Position);

            ground.UpdateCameraPosition(spaceship.Camera.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            DrawScene(spaceship.Camera);

            base.Draw(gameTime);
        }

        public void DrawScene(Camera camera)
        {
            GraphicsDevice.Viewport = camera.Viewport;
            foreach (var model in Models)
                model.Draw(camera.View, camera.Projection);
        }
    }
}
