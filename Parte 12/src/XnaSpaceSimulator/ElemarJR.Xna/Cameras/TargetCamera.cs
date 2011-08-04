using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ElemarJR.Xna.Cameras
{
    public class TargetCamera : Camera, IPositionableCamera
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }

        public TargetCamera(Vector3 Position, Vector3 Target,
            GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = Position;
            this.Target = Target;
            Update();
        }

        public override void Update()
        {
            this.View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }
    }
}
