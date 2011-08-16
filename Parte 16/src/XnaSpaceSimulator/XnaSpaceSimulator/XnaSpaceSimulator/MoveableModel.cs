using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElemarJR.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XnaSpaceSimulator
{
    class MoveableModel : GameComponent
    {
        public GameModel Model { get; private set; }
        
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Forward { get; set; }
        public Keys Backward { get; set; }
        public Keys Up { get; set; }
        public Keys Down { get; set; }

        public MoveableModel(
            Game game, 
            GameModel model 
            )
            : base(game)
        {
            this.Model = model;
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePosition(gameTime);
            base.Update(gameTime);
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
