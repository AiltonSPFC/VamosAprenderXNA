using System;

namespace UserInput
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //using (var game = new AnimationGame())
            using (var game = new UserInputGame1())
            {
                game.Run();
            }
        }
    }
#endif
}

