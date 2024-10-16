using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Druid : Character
{
    private static readonly string[] Animalform = { "Björn", "Örn", "Varg", "Järv", "Spindel" };
    public Druid() : base()
    {
        Class = Proffession.proffList[3].ProffName;
        Magicpotions = 2;
    }
    private static string RandomAnimalform()
    {
        Random random = new Random();
        int index = random.Next(Animalform.Length);
        return Animalform[index];
    }
    public override int Ability()
    {
        if (Magic < 5)
        {
            Console.WriteLine("\nDu har inte nog med magi för att förvandla dig!\n");
            Console.WriteLine($"Du slår med din påk istället! Du skadar {Strength}!\n");
            return Strength;
        }
        int hybridDMG = (Intelligence + Strength) / 2;
        Console.Write($"\n{this.Name} förvandlar sig till en {RandomAnimalform()} och attackerar!\n");
        Console.Write($"{this.Name} gör {hybridDMG} skada och förvandlas tillbaka.");
        Console.WriteLine(" -3 magipoäng\n");
        Magic -= 3;
        return hybridDMG;
    }
}
