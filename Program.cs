using System.Reflection;

namespace MINI_RPG;
class Program
{
    static void Main()
    {
        Console.WriteLine("~~ MINI RPG ~~");
        Character player = null;

        bool playing = true;
        while (playing)
        {
            player = Character.Creation();

            Console.WriteLine("Du har skapat denna karaktär:\n");
            player.ShowInfo();
            Console.Write("Är du nöjd? ");
            if(YesNo())
            {
                playing = false;
            }
        }

        Console.WriteLine($"\nNu ska {player.Name} ut på äventyr!");
        Console.WriteLine("Nu kommer du att möta olika monster och för varje du besegrar blir nästa svårare!");
        PressKeyToContinue();

        /** Grupp
        // Skapa en lista för gruppen
        List<Character> grupp = new List<Character>();
        **/

        playing = false;
        int level = 1; // Börja äventyret på nivå 1

        while (playing)
        {
            // Skapa en ny fiende med en ökad nivå för varje ny loop
            Enemy enemy = new Enemy(level);

            Console.WriteLine($"Nivå: {level}");
            Console.WriteLine($"En {enemy.Name} dyker upp!\n");
            PressKeyToContinue();

            while (player.Health > 0 && enemy.Health > 0)
            {
                ShowStats(player, enemy, level);

                Actions(player, enemy);

                Console.Clear();
            }

            if (player.Health <= 0)
            {
                Console.WriteLine($"{player.Name} blev besegrad!\n");
                Console.WriteLine("Vill du skapa en ny karaktär och fortsätta spela?");
                if (YesNo())
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
            playing = YesNo();

            // Öka nivå
            level++;
        }
        Console.WriteLine($"\nTack för att du spelade! Du nådde nivå {level}!");
    }

    private static bool YesNo()
    {
        Console.Write("(1.Ja/2.Nej) ");
        ConsoleKeyInfo continueChoice = Console.ReadKey();

        switch(continueChoice.KeyChar)
        {
            case '1':
                return true;
            default:
                return false;
        }
    }

    private static void ShowStats(Character player, Enemy enemy, int level)
    {
        string dashes = new string('-', 20);

        // Visa fiendens information
        Console.WriteLine($"{enemy.Name} - nivå {level}");
        Console.WriteLine($"Hälsa: {enemy.Health}/{enemy.MaxHP}\n");

        Console.WriteLine(dashes);

        // Visa spelarens information
        Console.WriteLine($"\n{player.Name}");
        Console.WriteLine($"Hälsa: {player.Health}/{player.MaxHP}");
        Console.WriteLine($"Magi: {player.Magic}/{player.MaxMP}");
        Console.WriteLine($"Hälsodrycker: {player.Healthpotions}");
        if (!player.IsWarrior())
        {
            Console.WriteLine($"Magidrycker: {player.Magicpotions}");
        }
    }
    // Gör val
    private static void Actions(Character player, Enemy enemy)
    {
        List<string> actionList = new List<string>
    {
        "Använd specialattack",
        "Drick en hälsodryck",
        "Drick en magidryck",
        "Försök fly"
    };

        Console.WriteLine("\n\nVad vill du göra?");

        // Ta bort magidryck-stringen om du inte är en magiker eller druid
        if (player.IsWarrior())
        {
            actionList.Remove("Drick en magidryck");
        }

        // Visa alternativ för spelaren
        int i = 1;
        foreach (string action in actionList)
        {
            Console.Write($"{i}. ");
            Console.WriteLine(action);
            i++;
        }
        int choice = Check.ValidIntInput(1, actionList.Count);

        if (choice == 1)
        {
            // Implementera specialattacken här
            Fight(player, enemy);
        }
        else if (choice == 2)
        {
            // Spelaren dricker en hälsodryck om de har några
            player.UsePotion("HP");
        }
        else if (choice == 3)
        {
            // Spelaren dricker en magidryck om de har några (om din karaktär har sådana)
            if (!player.IsWarrior())
            {
                player.UsePotion("MP");
            }
            else
            {
                // Spelaren försöker fly från striden
                if (TryToEscape(player, enemy))
                {
                    Console.WriteLine("Du lyckades fly från striden!\n");
                }
                else
                {
                    Console.WriteLine("\nDu misslyckades med att fly och tvingas strida!\n");
                    Fight(player, enemy); // Fortsätt striden om spelaren misslyckas med att fly
                }
            }
        }
        else
        {
            Console.WriteLine("Ogiltigt val. Försök igen.\n");
        }
    }

    // spelare och fiende slåss
    private static void Fight(Character player, Enemy enemy)
    {
        enemy.Health -= player.Ability();
        player.Health -= enemy.Ability();
        PressKeyToContinue();
    }

    // Försök fly
    private static bool TryToEscape(Character player, Enemy enemy)
    {
        if (player.RollD20("att fly") > enemy.RollD20(""))
        {
            return true;
        }
        return false;
    }

    // En buffer innan man går vidare
    public static void PressKeyToContinue()
    {
        Console.Write("\nTryck valfri tangent för att fortsätta...");
        Console.ReadKey();
        Console.Clear();
    }
}