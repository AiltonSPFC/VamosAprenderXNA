using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ElemarJR.Xna.Cameras
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

        public Viewport Viewport { get; set; }

        public BoundingFrustum Frustum { get; private set; }


        public Camera(GraphicsDevice graphicsDevice, Viewport viewport)
        {
            this.GraphicsDevice = graphicsDevice;
            this.Viewport = viewport;
            ComputePerspectiveProjectionMatrix(MathHelper.PiOver4);
        }

        private void ComputePerspectiveProjectionMatrix(float fieldOfView)
        {
            float aspectRatio = 0;
            if (this.Viewport.Width < 5)
            {
                PresentationParameters pp = GraphicsDevice.PresentationParameters;
                aspectRatio =
                    (float)pp.BackBufferWidth /
                    (float)pp.BackBufferHeight;
            }
            else
            {
                aspectRatio = (float)Viewport.Width / (float)Viewport.Height;
            }
            

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
