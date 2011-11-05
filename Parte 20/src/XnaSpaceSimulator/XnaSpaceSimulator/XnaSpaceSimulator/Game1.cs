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

        FixedCamera camera;
        ModelAnimation animation;

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
                Position = new Vector3(0, 400, -1000),
                Scale = new Vector3(50f),
            };

            animation = new ModelAnimation(
                spaceship.Position,
                Vector3.Zero,
                spaceship.Position + new Vector3(0, 0, 2600),
                new Vector3(0, MathHelper.Pi, 0),
                TimeSpan.FromSeconds(5),
                true,
                true
                );

            models.Add(spaceship);
            models.Add(Content.Load<Model>("ground"));

            var d = 2000;
            camera = new FixedCamera(
                new Vector3(-d, d, -d),
                Vector3.Zero,
                GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animation.Update(gameTime.ElapsedGameTime);
            spaceship.Position = animation.Position;
            spaceship.Rotation = animation.Rotation;
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
