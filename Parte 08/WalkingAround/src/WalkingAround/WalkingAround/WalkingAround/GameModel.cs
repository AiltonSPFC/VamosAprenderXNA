using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WalkingAround
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class GameModel
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Vector3 BaseRotation { get; set; }

        public Model Model { get; private set; }

        Matrix[] modelTransforms;

        public GameModel(
            Model Model
            )
        {
            this.Model = Model;

            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            this.baseBoundingSphere = Model.ComputeBoundingSphere();

            this.Position = Vector3.Zero;
            this.Rotation = Vector3.Zero;
            this.Scale = Vector3.One;
        }

        public static implicit operator GameModel(Model @model)
        {
            return new GameModel(@model);
        }


        private BoundingSphere baseBoundingSphere;
        public BoundingSphere BoundingSphere
        {
            get
            {
                return baseBoundingSphere.Transform(
                    ComputeWorld()
                    );
            }
        }

        Matrix ComputeWorld()
        {
            return
                BaseRotation.CreateYawPitchRollMatrix() *
                Matrix.CreateScale(Scale) *
                Rotation.CreateYawPitchRollMatrix() *
                Matrix.CreateTranslation(Position);
        }

        public void Draw(Matrix View, Matrix Projection)
        {
            Matrix baseWorld = ComputeWorld();

            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                    * baseWorld;
                mesh.SetupEffects(localWorld, View, Projection);
                mesh.Draw();
            }
        }
    }
}
