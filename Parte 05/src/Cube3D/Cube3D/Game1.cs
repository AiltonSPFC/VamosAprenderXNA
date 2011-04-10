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
using Xna3DMadness;

namespace Cube3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        VertexPositionColor[] verts;
        VertexBuffer vertexBuffer;
        BasicEffect effect;
        Camera camera;
        protected override void LoadContent()
        {
            camera = new Camera(this, new Vector3(0, 0, 5));
            Components.Add(camera);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var tlf = new Vector3(-1.0f,  1.0f, -1.0f);
            var blf = new Vector3(-1.0f, -1.0f, -1.0f);
            var trf = new Vector3( 1.0f,  1.0f, -1.0f);
            var brf = new Vector3( 1.0f, -1.0f, -1.0f);
            var tlb = new Vector3(-1.0f,  1.0f,  1.0f);
            var trb = new Vector3( 1.0f,  1.0f,  1.0f);
            var blb = new Vector3(-1.0f, -1.0f,  1.0f);
            var brb = new Vector3( 1.0f, -1.0f,  1.0f);

            verts = new [] {
                // front
                new VertexPositionColor(tlf, Color.Red),
                new VertexPositionColor(blf, Color.Red),
                new VertexPositionColor(trf, Color.Red),
                new VertexPositionColor(blf, Color.Red),
                new VertexPositionColor(brf, Color.Red),
                new VertexPositionColor(trf, Color.Red),

                // back
                new VertexPositionColor(tlb, Color.Blue),
                new VertexPositionColor(trb, Color.Blue),
                new VertexPositionColor(blb, Color.Blue),
                new VertexPositionColor(blb, Color.Blue),
                new VertexPositionColor(trb, Color.Blue),
                new VertexPositionColor(brb, Color.Blue),

                // top
                new VertexPositionColor(tlf, Color.Green),
                new VertexPositionColor(trb, Color.Green),
                new VertexPositionColor(tlb, Color.Green),
                new VertexPositionColor(tlf, Color.Green),
                new VertexPositionColor(trf, Color.Green),
                new VertexPositionColor(trb, Color.Green),

                // bottom
                new VertexPositionColor(blf, Color.Yellow),
                new VertexPositionColor(blb, Color.Yellow),
                new VertexPositionColor(brb, Color.Yellow),
                new VertexPositionColor(blf, Color.Yellow),
                new VertexPositionColor(brb, Color.Yellow),
                new VertexPositionColor(brf, Color.Yellow),

                // left
                new VertexPositionColor(tlf, Color.Purple),
                new VertexPositionColor(blb, Color.Purple),
                new VertexPositionColor(blf, Color.Purple),
                new VertexPositionColor(tlb, Color.Purple),
                new VertexPositionColor(blb, Color.Purple),
                new VertexPositionColor(tlf, Color.Purple),

                // right
                new VertexPositionColor(trf, Color.Magenta),
                new VertexPositionColor(brf, Color.Magenta),
                new VertexPositionColor(brb, Color.Magenta),
                new VertexPositionColor(trb, Color.Magenta),
                new VertexPositionColor(trf, Color.Magenta),
                new VertexPositionColor(brb, Color.Magenta),
            };

            

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor),
                verts.Length, BufferUsage.None);

            vertexBuffer.SetData(verts);

            effect = new BasicEffect(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            effect.World = worldScale * worldRotation * worldTranslation;
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.VertexColorEnabled = true;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                    verts, 0, 12);
            }

            base.Draw(gameTime);
        }
    }
}
