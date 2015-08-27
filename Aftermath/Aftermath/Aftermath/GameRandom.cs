using System;

namespace Aftermath
{
    public static class GameRandom
    {
        private static Random _random = new Random();

        public static Random Random
        {
            get { return _random; }
        }
    }
}