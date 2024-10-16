using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Check   // Kolla olika saker
{
    public static int ValidIntInput(int minValue, int maxValue)
    {
        int output = 0;
        bool validInput = false;

        while (!validInput)
        {
            string input = Console.ReadLine();
            validInput = int.TryParse(input, out output);

            if (!validInput || output < minValue || output > maxValue)
            {
                Console.Write($"Ogiltig inmatning. Skriv ett heltal mellan {minValue} och {maxValue}: ");
                validInput = false; // Återställ validInput om inmatningen är ogiltig
            }
        }

        return output;
    }

    internal static void Actions(Character player, Enemy enemy)
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
            actionList.Remove(actionList[2]);
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

    private static void Fight(Character player, Enemy enemy)
    {
        enemy.Health -= player.Ability();
        player.Health -= enemy.Ability();
        Check.AnyKey();
    }

    private static bool TryToEscape(Character player, Enemy enemy)
    {
        if (player.RollD20("att fly") > enemy.RollD20(""))
        {
            return true;
        }
        return false;
    }

    internal static void AnyKey()
    {
        Console.Write("\nTryck valfri tangent för att fortsätta...");
        Console.ReadKey();
        Console.Clear();
    }

    internal static bool YesNo()
    {
        Console.Write("(1.Ja/2.Nej) ");
        ConsoleKeyInfo continueChoice = Console.ReadKey();

        switch (continueChoice.KeyChar)
        {
            case '1':
                return true;
            default:
                return false;
        }
    }

    internal static void Stats(Character player, Enemy enemy, int level)
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
}
