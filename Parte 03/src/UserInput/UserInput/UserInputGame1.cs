using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UserInput
{
    public class UserInputGame1 : Game
    {
        Floor floor;
        Player player;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public UserInputGame1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            floor = new Floor(this);
            player = new Player(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            floor.LoadContent();
            player.LoadContent();

            player.Position = new Vector2(
                Window.ClientBounds.Width / 2,
                Window.ClientBounds.Height - 32
                );
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (player.CurrentAction == PlayerActions.Die)
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
                    player.Reset();

            floor.Update(gameTime);
            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            floor.Draw(spriteBatch, gameTime);
            player.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
