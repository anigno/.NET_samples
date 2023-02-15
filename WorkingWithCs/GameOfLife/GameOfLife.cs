namespace GameOfLife
{
    internal class GameOfLife
    {
        private readonly int[,,] worlds = new int[2, 10, 10];
        private int currentWorldIndex = 0;
        private int nextWorldIndex = 1;

        public GameOfLife(IEnumerable<int[]> livingCoords)
        {
            InitWorldZero(livingCoords);
        }

        /// <summary>runs the game of life</summary>
        /// <param name="generations">number of generation to run</param>
        public void Run(int generations)
        {
            PrintCurrentWorld(0);
            for (int a = 0; a < generations; a++)
            {
                nextWorldIndex = (currentWorldIndex + 1) % 2;
                CalculateNextWorld();
                currentWorldIndex = nextWorldIndex;
                PrintCurrentWorld(a + 1);
            }
        }

        private void InitWorldZero(IEnumerable<int[]> livingCoords)
        {
            foreach (int[] live in livingCoords)
            {
                worlds[0, live[0], live[1]] = 1;
            }
        }

        private void CalculateNextWorld()
        {
            for (int y = 0; y < worlds.GetLength(1); y++)
            {
                for (int x = 0; x < worlds.GetLength(2); x++)
                {
                    //count lives around y,x
                    int lives_cnt = 0;
                    for (int iy = -1; iy <= 1; iy++)
                    {
                        if (y + iy < 0 || y + iy > 9) continue;
                        for (int ix = -1; ix <= 1; ix++)
                        {
                            if (x + ix < 0 || x + ix > 9) continue;
                            if (ix != 0 || iy != 0)
                            {
                                lives_cnt += worlds[currentWorldIndex, y + iy, x + ix];
                            }
                        }
                    }
                    //calculate next gen lives or born
                    int isNextLive;
                    if (worlds[currentWorldIndex, y, x] == 1)
                    {
                        isNextLive = (lives_cnt == 2 || lives_cnt == 3) ? 1 : 0; //lives_cnt or not
                    }
                    else
                    {
                        isNextLive = (lives_cnt == 3) ? 1 : 0; //born or not
                    }
                    worlds[nextWorldIndex, y, x] = isNextLive;
                }
            }
        }

        private void PrintCurrentWorld(int generation)
        {
            Console.WriteLine($"generation: {generation}");
            for (int y = 0; y < worlds.GetLength(2); y++)
            {
                for (int x = 0; x < worlds.GetLength(1); x++)
                {
                    var sign = worlds[currentWorldIndex, y, x] == 1 ? "#" : ".";
                    Console.Write($"{sign} ");
                }
                Console.WriteLine();
            }
        }
    }
}
