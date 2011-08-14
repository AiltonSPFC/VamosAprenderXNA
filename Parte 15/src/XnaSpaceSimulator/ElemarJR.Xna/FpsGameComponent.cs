using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace ElemarJR.Xna
{
public class FpsGameComponent : DrawableGameComponent
{
    readonly GameWindow Window;
    public FpsGameComponent(Game game, GraphicsDeviceManager manager)
        : base(game)
    {
        this.Window = game.Window;
        manager.SynchronizeWithVerticalRetrace = false;
        game.IsFixedTimeStep = true;
    }

    float FramesCount = 0f;
    float TimeSinceLastUpdate = 0f;
    float Fps;


    public override void Draw(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        this.FramesCount++;
        this.TimeSinceLastUpdate += elapsed;
        if (TimeSinceLastUpdate >= 1)
        {
            this.Fps = FramesCount / TimeSinceLastUpdate;
            this.Window.Title =
                string.Format("FPS: {0}", Fps);
            this.FramesCount = 0;
            TimeSinceLastUpdate -= 1f;
        }
    }
}
}
