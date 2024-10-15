using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Paladin : Character
{
    public Paladin(string name, string race, string proff,
                    int str, int intel, int phy,
                    int hp, int mp)
        : base(name, race, proff, str, intel, phy, hp, mp)
    {
    }

    public override int Ability()
    {
        int hybridDMG = (Intelligence + Strength) / 2;
        Console.WriteLine($"\n {this.Name} förtrollar sitt spjut och hugger!");
        Console.WriteLine($"{this.Name} gör {hybridDMG} skada!\n");
        Console.WriteLine(" -3 magipoäng\n");
        Magic -= 3;
        return hybridDMG;
    }
}
