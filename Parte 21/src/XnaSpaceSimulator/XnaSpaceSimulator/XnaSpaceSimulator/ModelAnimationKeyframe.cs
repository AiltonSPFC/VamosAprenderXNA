using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaSpaceSimulator
{
    using Microsoft.Xna.Framework;

    class ModelAnimationKeyframe
    {
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public TimeSpan Time { get; private set; }

        public ModelAnimationKeyframe(
            Vector3 position,
            Vector3 rotation,
            TimeSpan time
            )
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Time = time;
        }
    }
}
