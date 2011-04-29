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

namespace WorkingWith3DModels
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
    
        public Camera CurrentCamera { get; protected set; }
        public Model CurrentModel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
            CurrentCamera = new Camera(this, new Vector3(0, 0, 55));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.CurrentModel = Content.Load<Model>(@"models\spaceship");
        }


        float rotationStep = MathHelper.PiOver4 / 30;
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                worldRotation *= Matrix.CreateRotationY(-rotationStep);
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                worldRotation *= Matrix.CreateRotationY(rotationStep);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                worldRotation *= Matrix.CreateRotationX(-rotationStep);
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                worldRotation *= Matrix.CreateRotationX(rotationStep);


            if (Keyboard.GetState().IsKeyDown(Keys.L))
                worldTranslation *= Matrix.CreateTranslation(new Vector3(-0.01f, 0, 0));
            else if (Keyboard.GetState().IsKeyDown(Keys.R))
                worldTranslation *= Matrix.CreateTranslation(new Vector3(0.01f, 0, 0));

            if (Keyboard.GetState().IsKeyDown(Keys.T))
                worldTranslation *= Matrix.CreateTranslation(new Vector3(0, 0.01f, 0));
            else if (Keyboard.GetState().IsKeyDown(Keys.B))
                worldTranslation *= Matrix.CreateTranslation(new Vector3(0, -0.01f, 0));

            if (Keyboard.GetState().IsKeyDown(Keys.I))
                worldScale *= Matrix.CreateScale(1.01f);
            else if (Keyboard.GetState().IsKeyDown(Keys.O))
                worldScale *= Matrix.CreateScale(0.99f);

            base.Update(gameTime);
        }

        Matrix worldRotation = Matrix.Identity;
        Matrix worldTranslation = Matrix.Identity;
        Matrix worldScale = Matrix.Identity;

        protected override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in CurrentModel.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = this.CurrentCamera.Projection;
                    be.View = this.CurrentCamera.View;
                    be.World = worldScale * worldRotation * worldTranslation
                        * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }

    }
}
