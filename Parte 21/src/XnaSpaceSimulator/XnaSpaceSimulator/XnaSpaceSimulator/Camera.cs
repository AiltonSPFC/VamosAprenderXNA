using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaSpaceSimulator
{
    public abstract class Camera
    {
        Matrix view;
        Matrix projection;
        protected GraphicsDevice GraphicsDevice;

        public Matrix Projection
        {
            get { return projection; }
            protected set
            {
                projection = value;
                ComputeFrustum();
            }
        }

        public Matrix View
        {
            get { return view; }
            protected set
            {
                view = value;
                ComputeFrustum();
            }
        }

        public BoundingFrustum Frustum { get; private set; }


        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            ComputePerspectiveProjectionMatrix(MathHelper.PiOver4);
        }

        private void ComputePerspectiveProjectionMatrix(float fieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            float aspectRatio =
                (float)pp.BackBufferWidth /
                (float)pp.BackBufferHeight;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, 0.1f, 1000000.0f);
        }

        public virtual void Update()
        {
        }

        private void ComputeFrustum()
        {
            Matrix viewProjection = View * Projection;
            Frustum = new BoundingFrustum(viewProjection);
        }

        public bool IsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        public bool IsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
