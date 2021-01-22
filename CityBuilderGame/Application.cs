﻿using System;

namespace CityBuilderGame
{
    class Application
    {
        static void Main(string[] args)
        {
            using Game game = new Game(800, 600, "City Build Game");
            game.Run();
        }
    }
}
