using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaSpaceSimulator
{
    using Microsoft.Xna.Framework;

    public class ModelAnimation
    {
        public Vector3 StartPosition { get; private set; }
        public Vector3 StartRotation { get; private set; }

        public Vector3 EndPosition { get; private set; }
        public Vector3 EndRotation { get; private set; }

        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }

        public TimeSpan Duration { get; private set; }
        public bool Looping { get; private set; }
        public bool AutoReverse { get; private set; }

        TimeSpan elapsedTime = TimeSpan.FromSeconds(0);

        public ModelAnimation(
            Vector3 startposition,
            Vector3 startrotation,
            Vector3 endposition,
            Vector3 endrotation,
            TimeSpan duration,
            bool looping,
            bool autoreverse
            )
        {
            this.StartPosition = startposition;
            this.StartRotation = startrotation;
            this.EndPosition = endposition;
            this.EndRotation = endrotation;
            this.Duration = duration;
            this.Looping = looping;
            this.AutoReverse = autoreverse;
        }

        bool reversing = false;
        public void Update(TimeSpan elapsed)
        {
            this.elapsedTime += elapsed;

            var amount = (float)this.elapsedTime.TotalSeconds / (float)Duration.TotalSeconds;

            if (this.Looping)
            {
                if (amount > 1)
                {
                    this.elapsedTime = TimeSpan.FromSeconds(0);
                    reversing = !reversing;
                    while (amount > 1) amount--;
                }
            }
            else
                amount = MathHelper.Clamp(amount, 0f, 1f);

            if (reversing) amount = 1 - amount;

            Position = Vector3.Lerp(this.StartPosition, this.EndPosition, amount);
            Rotation = Vector3.Lerp(this.StartRotation, this.EndRotation, amount);

        }
    }
}
