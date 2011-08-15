using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaSpaceSimulator
{
    using ElemarJR.Xna;
    using ElemarJR.Xna.Cameras;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    class Spaceship : GameComponent
    {
        public ChaseCamera Camera { get; private set; }
        public GameModel Model { get; private set; }
        
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Forward { get; set; }
        public Keys Backward { get; set; }
        public Keys Up { get; set; }
        public Keys Down { get; set; }

        public Spaceship(
            Game game, 
            GameModel model, 
            Viewport viewport
            )
            : base(game)
        {
            this.Model = model;
            this.Camera = new ChaseCamera(
                new Vector3(0, 400, 1500),
                new Vector3(0, 200, 0),
                new Vector3(0, 0, 0),
                game.GraphicsDevice,
                model.Position, viewport);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateCamera();
            UpdatePosition(gameTime);
            base.Update(gameTime);
        }

        void UpdateCamera()
        {
            this.Camera.Move(
                Model.Position,
                Model.Rotation
                );

            this.Camera.Update();
        }

        void UpdatePosition(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector3 rotChange = new Vector3(0, 0, 0);

            if (keyState.IsKeyDown(Up))
                rotChange += new Vector3(1, 0, 0);

            if (keyState.IsKeyDown(Down))
                rotChange += new Vector3(-1, 0, 0);

            if (keyState.IsKeyDown(Left))
                rotChange += new Vector3(0, 1, 0);

            if (keyState.IsKeyDown(Right))
                rotChange += new Vector3(0, -1, 0);

            Model.Rotation += rotChange * .025f;

            if (!keyState.IsKeyDown(Forward) &&
                !keyState.IsKeyDown(Backward))
                return;

            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                Model.Rotation.Y, Model.Rotation.X, Model.Rotation.Z
                );

            Model.Position +=
                Vector3.Transform(
                    keyState.IsKeyDown(Forward) ? Vector3.Forward : Vector3.Backward,
                    rotation
                    )
                * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;

        }

    }
}
