using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ElemarJR.Xna
{
    public static class KeyboardExtensionMethods
    {
        public static Vector3 ComputeVectorFromKeys(
            this KeyboardState state,
            Keys up = Keys.PageUp,
            Keys down = Keys.PageDown,
            Keys left = Keys.Left,
            Keys right = Keys.Right,
            Keys forward = Keys.Up,
            Keys backward = Keys.Down
            )
        {
            Vector3 result = Vector3.Zero;

            if (state.IsKeyDown(up))
                result += Vector3.Up;

            if (state.IsKeyDown(down))
                result += Vector3.Down;

            if (state.IsKeyDown(left))
                result += Vector3.Left;

            if (state.IsKeyDown(right))
                result += Vector3.Right;

            if (state.IsKeyDown(backward))
                result += Vector3.Backward;

            if (state.IsKeyDown(forward))
                result += Vector3.Forward;

            result.Normalize();

            return result;
        }
    }
}
