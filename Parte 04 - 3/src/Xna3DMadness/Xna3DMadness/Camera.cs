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
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }


        public Camera(Game game, Vector3 position) :
            this(game, position, Vector3.Zero)
        { }

        public Camera(Game game, Vector3 position, Vector3 target) :
            this(game, position, target, Vector3.Up)
        { }

        public Camera(Game game, Vector3 position, Vector3 target, Vector3 up)
            : base(game)
        {
            var ratio = (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height;

            View = Matrix.CreateLookAt(position, target, up);
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                ratio, 1, 100
                );
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
