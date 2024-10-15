using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Warrior : Character
{
    public Warrior(string name, string race, string proff,
                    int str, int intel, int phy,
                    int hp, int mp)
        : base(name, race, proff, str, intel, phy, hp, mp)
    {
    }
    public override int Ability()
    {
        Console.WriteLine($"\n{this.Name} klyver med ett stort bredsvärd!");
        Console.Write($"{this.Name} gör {Strength} skada!\n");
        return Strength;
    }
}
