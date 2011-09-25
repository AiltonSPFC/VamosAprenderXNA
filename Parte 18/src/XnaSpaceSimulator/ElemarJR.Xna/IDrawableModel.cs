using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ElemarJR.Xna
{
    public interface IDrawableModel
    {
        void Draw(Matrix View, Matrix Projection);
    }
}
