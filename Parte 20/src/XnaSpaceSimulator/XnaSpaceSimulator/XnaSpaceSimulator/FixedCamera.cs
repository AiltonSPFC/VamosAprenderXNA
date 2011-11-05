using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaSpaceSimulator
{
    public class FixedCamera : Camera
    {
        public FixedCamera(Vector3 position, Vector3 target,
            GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Vector3 forward = (target - position);
            Vector3 side = Vector3.Cross(forward, Vector3.Up);
            Vector3 up = Vector3.Cross(forward, side);
            View = Matrix.CreateLookAt(position, target, -up);
        }

    }
}
