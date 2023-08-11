namespace GuessMyNumber
{
    public class GuessMyNumber
    {
        private readonly int max;
        private readonly int min;
        private readonly int number;

        public GuessMyNumber(int min, int max)
        {
            Random random = new Random();
            this.min = min;
            this.max = max;
            number = random.Next(min, max);
        }

        public void Start()
        {
            int? guess;
            int score = 0;
            do
            {
                guess = ReadNumberFromConsole();
                if (guess == null) continue;
                score++;
                Console.Write($"Try number:{score}, your guess:{guess} ");
                if (guess < number)
                {
                    Console.WriteLine($"is too small");
                }
                else if (guess > number)
                {
                    Console.WriteLine($"is too big");
                }

            }
            while (guess != number);
            Console.WriteLine("is the correct number");
            Console.WriteLine("****** Press Enter to exit ******");
        }

        private int? ReadNumberFromConsole()
        {
            Console.WriteLine($"enter your guess ({min}-{max})");
            string? s = Console.ReadLine();
            if (s == null) return null;
            bool isNumber = int.TryParse(s, out int guess);
            if (!isNumber) return null;
            if (guess < min || guess > max) return null;
            return guess;
        }
    }

    internal class Program
    {
        public static void Main()
        {
            var app = new GuessMyNumber(10, 20);
            app.Start();
            Console.ReadLine();
        }
    }
}