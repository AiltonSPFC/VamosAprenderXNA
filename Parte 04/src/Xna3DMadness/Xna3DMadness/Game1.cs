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

namespace Xna3DMadness
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;

        Texture2D texture;

        Matrix worldTranslation = Matrix.Identity;
        Matrix worldRotation = Matrix.Identity;
        Matrix worldScale = Matrix.Identity;

        //VertexPositionColor[] verts;
        VertexPositionTexture[] verts;
        VertexBuffer vertexBuffer;
        BasicEffect effect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = Content.Load<Texture2D>(@"Textures\Lighthouse");

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;

            // camera
            camera = new Camera(this, new Vector3(0, 0, 5));
            Components.Add(camera);

            //verts = new[]
            //    {
            //        new VertexPositionColor(new Vector3(0, 1, 0), Color.Blue),
            //        new VertexPositionColor(new Vector3(1, -1, 0), Color.Red),
            //        new VertexPositionColor(new Vector3(-1, -1, 0), Color.Green)
            //    };

            //verts = new[]
            //{
            //    new VertexPositionColor(new Vector3(-1, 1, 0), Color.Blue),
            //    new VertexPositionColor(new Vector3(1 , 1, 0), Color.Yellow),
            //    new VertexPositionColor(new Vector3(-1,-1, 0), Color.Green),
            //    new VertexPositionColor(new Vector3( 1,-1, 0), Color.Red),
            //};

            verts = new[]
        {
            new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0,0)),
            new VertexPositionTexture(new Vector3(1 , 1, 0), new Vector2(1,0)),
            new VertexPositionTexture(new Vector3(-1,-1, 0), new Vector2(0,1)),
            new VertexPositionTexture(new Vector3( 1,-1, 0), new Vector2(0,1))
        };

            
           //vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor),
           //    verts.Length, BufferUsage.None);

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                verts.Length, BufferUsage.None);
           vertexBuffer.SetData(verts);

            // effect
            effect = new BasicEffect(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
                worldTranslation *= Matrix.CreateTranslation(-.01f, 0, 0);

            if (keyboardState.IsKeyDown(Keys.Right))
                worldTranslation *= Matrix.CreateTranslation(.01f, 0, 0);

            if (keyboardState.IsKeyDown(Keys.Up))
                worldTranslation *= Matrix.CreateTranslation(0, .01f, 0);

            if (keyboardState.IsKeyDown(Keys.Down))
                worldTranslation *= Matrix.CreateTranslation(0, -.01f, 0);

            if (keyboardState.IsKeyDown(Keys.I))
                worldScale *= Matrix.CreateScale(1.02f);

            if (keyboardState.IsKeyDown(Keys.O))
                worldScale *= Matrix.CreateScale(0.98f);

            worldRotation *= Matrix.CreateRotationY(MathHelper.PiOver4 / 60);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //
            effect.World = worldScale * worldRotation * worldTranslation;
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            //effect.VertexColorEnabled = true;
            effect.Texture = texture;
            effect.TextureEnabled = true;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //GraphicsDevice.DrawUserPrimitives
                //    (PrimitiveType.TriangleStrip, verts, 0, 1);

                GraphicsDevice.DrawUserPrimitives
                    (PrimitiveType.TriangleStrip, verts, 0, 2);
            }

            base.Draw(gameTime);
        }
    }
}
