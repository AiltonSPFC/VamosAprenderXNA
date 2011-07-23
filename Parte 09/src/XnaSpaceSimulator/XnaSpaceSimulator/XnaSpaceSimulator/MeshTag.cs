using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaSpaceSimulator
{
    class MeshTag
    {
        public Vector3 Color { get; private set; }
        public Texture2D Texture { get; private set; }
        public float SpecularPower { get; private set; }
        public Effect CachedEffect { get; set; }

        public MeshTag(
            Vector3 color,
            Texture2D texture,
            float specularPower
            )
        {
            this.Color = color;
            this.Texture = texture;
            this.SpecularPower = specularPower;
        }
    }
}
