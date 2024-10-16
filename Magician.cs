using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Magician : Character
{
    public Magician() : base()
    {
        Class = Proffession.proffList[1].ProffName;
        Magicpotions = 2;
    }
    public override int Ability()
    {
        if (Magic < 5)
        {
            Console.WriteLine("\nDu har inte nog med magi för att kasta magi!");
            Console.WriteLine($"Du slår med din stav istället! Du skadar {Strength}!\n");
            return Strength;
        }
        Console.WriteLine($"\n{this.Name} frammanar en storm som blixtrar!");
        Console.WriteLine($"Blixten skadar {Intelligence}!");
        Console.WriteLine(" -5 magipoäng\n");
        Magic -= 5;
        return Intelligence;
    }
}
