using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ElemarJR.Xna.Cameras
{
    public class FreeCamera : Camera, IPositionableCamera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Target { get; private set; }

        public FreeCamera(
            Vector3 position, 
            float yaw, 
            float pitch,
            GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = position;
            this.Yaw = yaw;
            this.Pitch = pitch;
        }

        public void Rotate(float yawChange, float pitchChange)
        {
            this.Yaw += yawChange;
            this.Pitch += pitchChange;
        }

        public void Move(Vector3 translation)
        {
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;
        }

        public override void Update()
        {
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
