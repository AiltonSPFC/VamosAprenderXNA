using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ElemarJR.Xna.Environmental
{
    public class Terrain : IDrawableModel
    {
        RasterizerState rs = new RasterizerState();

        public Terrain(Texture2D heightMap,
            float cellSize,
            int maxHeight,
            GraphicsDevice device)
        {
            rs.FillMode = FillMode.WireFrame;
            this.HeightMap = heightMap;
            this.Width = heightMap.Width;
            this.Height = maxHeight;
            this.Length = heightMap.Height;
            this.CellSize = cellSize;

            this.Device = device;
            this.Effect = new BasicEffect(device);

            this.VerticesCount = Width * Length;
            this.IndexCount = (Width - 1) * (Length - 1) * 6;

            this.VertexBuffer = new VertexBuffer(device,
                typeof(VertexPositionColor), VerticesCount,
                BufferUsage.WriteOnly);

            this.IndexBuffer = new IndexBuffer(device,
                IndexElementSize.ThirtyTwoBits,
                IndexCount, BufferUsage.WriteOnly);

            ComputeHeights();
            CreateVertices();
            CreateIndices();

            this.VertexBuffer.SetData<VertexPositionColor>(Vertices);
            this.IndexBuffer.SetData<int>(Indexes);
        }

        void ComputeHeights()
        {
            var heightMapColors = new Color[Width * Length];
            HeightMap.GetData<Color>(heightMapColors);

            Heights = new float[Width, Length];

            for (int y = 0; y < Length; y++)
                for (int x = 0; x < Width; x++)
                    Heights[x, y] = heightMapColors[y * Width + x].R / 255.0f * Height;
        }

        void CreateVertices()
        {
            this.Vertices = new VertexPositionColor[VerticesCount];
            Vector3 offset = new Vector3((Width / 2f) * CellSize, 0, (Length / 2f) * CellSize);

            for (int z = 0; z < Length; z++)
                for (int x = 0; x < Width; x++)
                {
                    Vector3 position = new Vector3(
                        x * this.CellSize,
                        Heights[x, z],
                        z * CellSize
                        ) - offset;

                    this.Vertices[z * Width + x] = new VertexPositionColor(position, Color.Green);
                }
        }

        void CreateIndices()
        {
            this.Indexes = new int[this.IndexCount];
            int index = 0;
            for (int x = 0; x < Width - 1; x++)
                for (int z = 0; z < Length - 1; z++)
                {
                    int ul = z * Width + x;
                    int ur = ul + 1;
                    int ll = ul + Width;
                    int lr = ll + 1;

                    Indexes[index++] = ul;
                    Indexes[index++] = ur;
                    Indexes[index++] = ll;

                    Indexes[index++] = ll;
                    Indexes[index++] = ur;
                    Indexes[index++] = lr;
                }
        }


        float[,] Heights;
        int[] Indexes;

        public readonly Texture2D HeightMap;
        public readonly int Width;
        public readonly int Height;
        public readonly int Length;
        public readonly float CellSize;

        readonly int VerticesCount;
        VertexPositionColor[] Vertices;

        readonly int IndexCount;
        readonly VertexBuffer VertexBuffer;
        readonly IndexBuffer IndexBuffer;
        GraphicsDevice Device;
        BasicEffect Effect;


        public void Draw(Matrix View, Matrix Projection)
        {
            Device.SetVertexBuffer(VertexBuffer);
            Device.Indices = IndexBuffer;
            var old = Device.RasterizerState;
            Device.RasterizerState = rs;

            Effect.World = Matrix.Identity;
            Effect.View = View;
            Effect.Projection = Projection;
            Effect.TextureEnabled = false;
            Effect.VertexColorEnabled = false;

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Device.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0, 0,
                    VerticesCount, 0,
                    IndexCount / 3);
            }

            Device.RasterizerState = old;
        }
    }
}
