using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ElemarJR.Xna
{
    public static class ModelExtensions
    {
        public static BoundingSphere ComputeBoundingSphere(
            this Model that
            )
        {
            var result = new BoundingSphere(Vector3.Zero, 0);

            var modelTransforms = new Matrix[that.Bones.Count];
            that.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in that.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(
                    modelTransforms[mesh.ParentBone.Index]);

                result = result.MergeWith(transformed);
            }
            return result;
        }

        public static BoundingSphere MergeWith(
            this BoundingSphere first,
            BoundingSphere second
            )
        {
            return BoundingSphere.CreateMerged(first, second);
        }

        public static BasicEffect SetWorld(
            this BasicEffect that,
            Matrix world
                )
        {
            that.World = world;
            return that;
        }

        public static BasicEffect SetView(
            this BasicEffect that,
            Matrix view
                )
        {
            that.View = view;
            return that;
        }

        public static BasicEffect SetProjection(
            this BasicEffect that,
            Matrix projection
                )
        {
            that.Projection = projection;
            return that;
        }

        public static ModelMesh SetupEffects(this ModelMesh mesh,
            Matrix world,
            Matrix view,
            Matrix projection
                )
        {
            foreach (var meshPart in mesh.MeshParts)
                if (meshPart.Effect is BasicEffect)
                    ((BasicEffect)meshPart.Effect)
                        .SetWorld(world)
                        .SetView(view)
                        .SetProjection(projection)
                        .EnableDefaultLighting();
                else
                {
                    var effect = meshPart.Effect;
                    effect.CurrentTechnique = effect.Techniques["Technique1"];
                    effect.Parameters["World"].SetValue(world);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                }

            return mesh;
        }
    }


}
