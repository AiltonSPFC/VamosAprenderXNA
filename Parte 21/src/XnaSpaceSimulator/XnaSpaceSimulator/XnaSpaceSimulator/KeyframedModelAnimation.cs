using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaSpaceSimulator
{
    using Microsoft.Xna.Framework;

    class KeyframedModelAnimation
    {

        public ModelAnimationKeyframeList Frames
        { get; private set; }

        public Vector3 Position
        { get; private set; }

        public Vector3 Rotation
        { get; private set; }

        public TimeSpan ElapsedTime
        { get; private set; }

        public bool Looping
        { get; private set; }

        public bool AutoReverse
        { get; private set; }

        public KeyframedModelAnimation(
            ModelAnimationKeyframeList frames,
            bool looping = true,
            bool autoreverse = true
            )
        {
            this.Frames = frames;
            this.Looping = looping;
            this.ElapsedTime = TimeSpan.FromSeconds(0);
            this.AutoReverse = autoreverse;
        }

        bool reversing = false;
        public void Update(TimeSpan elapsed)
        {
            this.ElapsedTime += elapsed;
            var last = this.Frames.Last();

            if (this.ElapsedTime > last.Time && !this.Looping)
            {
                this.Position = last.Position;
                this.Rotation = last.Rotation;
            }
            else
            {
                if (this.ElapsedTime > last.Time)
                {
                    while (this.ElapsedTime > last.Time)
                        this.ElapsedTime -= last.Time;
                    reversing = !reversing && this.AutoReverse;
                }

                int index;
                ModelAnimationKeyframe previous, target;
                TimeSpan time;

                if (!reversing)
                {
                    time = this.ElapsedTime;
                    index = this.Frames.IndexOf(time);
                    previous = this.Frames[index - 1];
                    target = this.Frames[index];
                }
                else
                {
                    time = last.Time - this.ElapsedTime;
                    index = this.Frames.IndexOf(time);
                    previous = this.Frames[index];
                    target = this.Frames[index - 1];
                }

                var currentFrameTime = time - previous.Time;
                var endFrameTime = target.Time - previous.Time;

                var amount = (float)currentFrameTime.TotalSeconds /
                    (float)endFrameTime.TotalSeconds;

                this.Position = Vector3.Lerp(previous.Position, target.Position, amount);
                this.Rotation = Vector3.Lerp(previous.Rotation, target.Rotation, amount);
            }
        }
    }

    class ModelAnimationKeyframeList : List<ModelAnimationKeyframe>
    {
        public ModelAnimationKeyframeList(
            Vector3 startposition,
            Vector3 startrotation)
        {
            this.AddStep(
                startposition,
                startrotation,
                TimeSpan.FromSeconds(0)
                );
        }

        public ModelAnimationKeyframeList Translate(
            float dx = 0, float dy = 0, float dz = 0, int deltaseconds = 2
            )
        {
            return this.AddStep(
                new Vector3(dx, dy, dz),
                Vector3.Zero,
                deltaseconds
            );
        }

        public ModelAnimationKeyframeList Rotate(
            float dx = 0, float dy = 0, float dz = 0, int deltaseconds = 2
            )
        {
            return this.AddStep(
                Vector3.Zero,
                new Vector3(dx, dy, dz),
                deltaseconds
            );
        }

        public ModelAnimationKeyframeList AddStep(
            Vector3 deltaposition,
            Vector3 deltarotation,
            int deltaseconds = 2
            )
        {
            return this.AddStep(deltaposition, deltarotation, TimeSpan.FromSeconds(deltaseconds));
        }

        public ModelAnimationKeyframeList AddStep(
            Vector3 deltaposition,
            Vector3 deltarotation,
            TimeSpan deltatime
            )
        {
            if (this.Count == 0)
                this.Add(new ModelAnimationKeyframe(
                    deltaposition,
                    deltarotation,
                    deltatime)
                    );
            else
            {
                var previous = this.Last();
                this.Add(new ModelAnimationKeyframe(
                    previous.Position + deltaposition,
                    previous.Rotation + deltarotation,
                    previous.Time + deltatime)
                    );
            }
            return this;
        }

        public int IndexOf(TimeSpan offset)
        {
            if (offset > this.Last().Time) return -1;
            if (offset < this.First().Time) return -1;

            var result = 0;
            while (this[result].Time <= offset)
                result++;
            return result;
        }
    }
}
