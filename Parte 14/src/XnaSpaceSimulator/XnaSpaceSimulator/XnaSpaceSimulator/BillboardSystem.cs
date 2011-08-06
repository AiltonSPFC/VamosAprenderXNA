using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace XnaSpaceSimulator
{
    class BillboardSystem
    {
        public GraphicsDevice Device { get; private set; }
        public Texture2D Texture { get; private set; }
        int BillboardCount = 0;

        IndexBuffer IBuffer;
        VertexBuffer VBuffer;

        Effect BillboardEffect;

        public BillboardSystem(
            GraphicsDevice device,
            ContentManager content,
            Texture2D texture,
            Vector2 billboardSize,
            Vector3[] positions
            )
        {
            this.BillboardCount = positions.Length;
            this.Device = device;
            this.Texture = texture;
            //this.BillboardSize = billboardSize;
            this.CreateBillboards(positions);

            this.BillboardEffect = content.Load<Effect>
                ("BillboardEffect");

            BillboardEffect.Parameters["Texture"].SetValue(texture);
            BillboardEffect.Parameters["Size"].SetValue(billboardSize);
        }

        void CreateBillboards(Vector3[] positions)
        {
            var indices = new int[positions.Length * 6];
            var vertices = new VertexPositionTexture[positions.Length * 4];
            int j = 0;

            for (int i = 0; i < positions.Length * 4; i += 4)
            {
                var pos = positions[i / 4];
                vertices[i + 0] = new VertexPositionTexture(pos, Vector2.Zero);
                vertices[i + 1] = new VertexPositionTexture(pos, new Vector2(0, 1));
                vertices[i + 2] = new VertexPositionTexture(pos, new Vector2(1, 1));
                vertices[i + 3] = new VertexPositionTexture(pos, new Vector2(1, 0));

                indices[j++] = i + 0;
                indices[j++] = i + 3;
                indices[j++] = i + 2;
                indices[j++] = i + 2;
                indices[j++] = i + 1;
                indices[j++] = i + 0;
            }

            VBuffer = new VertexBuffer(
                Device,
                typeof(VertexPositionTexture),
                positions.Length * 4,
                BufferUsage.WriteOnly);

            VBuffer.SetData<VertexPositionTexture>(vertices);

            IBuffer = new IndexBuffer(
                Device,
                IndexElementSize.ThirtyTwoBits,
                positions.Length * 6,
                BufferUsage.WriteOnly);

            IBuffer.SetData<int>(indices);
        }

        public void Draw(Matrix view, Matrix projection, Vector3 up, Vector3 right)
        {
            BillboardEffect.Parameters["View"].SetValue(view);
            BillboardEffect.Parameters["Projection"].SetValue(projection);
            BillboardEffect.Parameters["CameraUp"].SetValue(up);
            BillboardEffect.Parameters["CameraSide"].SetValue(right);

            BillboardEffect.CurrentTechnique.Passes[0].Apply();

            Device.SetVertexBuffer(this.VBuffer);
            Device.Indices = this.IBuffer;

            Device.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0, 0,
                BillboardCount * 4,
                0,
                BillboardCount * 2
                );

            Device.SetVertexBuffer(null);
            Device.Indices = null;
        }
    }
}
