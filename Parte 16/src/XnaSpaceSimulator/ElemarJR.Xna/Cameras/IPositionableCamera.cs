using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ElemarJR.Xna.Cameras
{
    public interface IPositionableCamera
    {
        Vector3 Position { get;  }
        Vector3 Target { get;  }

        BoundingFrustum Frustum { get; }
        bool IsInView(BoundingBox box);
        bool IsInView(BoundingSphere sphere);
        Matrix Projection { get; }
        void Update();
        Matrix View { get; }
        Viewport Viewport {get;}
    }
}
