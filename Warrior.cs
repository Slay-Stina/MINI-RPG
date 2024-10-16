using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Warrior : Character
{
    public Warrior() : base()
    {
        Class = Proffession.proffList[0].ProffName;
    }
    public override int Ability()
    {
        Console.WriteLine($"\n{this.Name} klyver med ett stort bredsvärd!");
        Console.Write($"{this.Name} gör {Strength} skada!\n");
        return Strength;
    }
}
