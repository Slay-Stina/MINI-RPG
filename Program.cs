using System.Reflection;

namespace MINI_RPG;
class Program
{
    static void Main()
    {
        Console.WriteLine("~~ MINI RPG ~~");
        Character player = new Character();

        bool playing = true;
        while (playing)
        {
            player = Character.Creation(player);

            Console.WriteLine("Du har skapat denna karaktär:\n");
            player.ShowInfo();
            Console.Write("Är du nöjd? ");
            if(Check.YesNo())
            {
                playing = false;
            }
        }

        Console.WriteLine($"\nNu ska {player.Name} ut på äventyr!");
        Console.WriteLine("Nu kommer du att möta olika monster och för varje du besegrar blir nästa svårare!");
        Check.AnyKey();

        int level = 1; // Börja äventyret på nivå 1

        playing = true;
        while (playing)
        {
            // Skapa en ny fiende med en ökad nivå för varje ny loop
            Enemy enemy = new Enemy(level);

            Console.WriteLine($"Nivå: {level}");
            Console.WriteLine($"En {enemy.Name} dyker upp!\n");
            Check.AnyKey();

            while (player.Health > 0 && enemy.Health > 0)
            {
                Check.Stats(player, enemy, level);

                Check.Actions(player, enemy);

                Console.Clear();
            }

            if (player.Health <= 0)
            {
                Console.WriteLine($"{player.Name} blev besegrad!\n");
                Console.WriteLine("Vill du skapa en ny karaktär och fortsätta spela?");
                if (Check.YesNo())
                {
                    Main();
                }
                else break;
            }
            else if (enemy.Health <= 0)
            {
                Console.WriteLine($"{player.Name} besegrade fienden!\n");
                Console.WriteLine("Dina drycker har fyllts på!");
                player.Healthpotions = 3;
                player.Magicpotions = 2;
            }
            // Checka om spelaren är redo att avsluta spelet eller fortsätta
            Console.Write("Vill du fortsätta spela? ");
            playing = Check.YesNo();
            Console.Clear();

            // Öka nivå
            level++;
        }
        Console.WriteLine($"\nTack för att du spelade! Du nådde nivå {level}!");
    }
}