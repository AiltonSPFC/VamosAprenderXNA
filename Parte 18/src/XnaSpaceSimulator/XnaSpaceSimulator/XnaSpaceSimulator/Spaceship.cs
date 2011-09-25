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

    class Spaceship : MoveableModel
    {
        public ChaseCamera Camera { get; private set; }
        

        public Spaceship(
            Game game, 
            GameModel model, 
            Viewport viewport
            )
            : base(game, model)
        {
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

    }
}
