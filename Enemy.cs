using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Enemy : Character
{
    private static readonly string[] EnemyNames = { "Goblin", "Ork", "Troll", "Skelett", "Spindel" };
    private static readonly string[] Weapons = { "svärd", "pilbåge", "klubb", "dolk", "spjut" };
    public string Weapon { get; }
    public Enemy(int level) : base(RandomEnemyName(), "Monster", "Fiende",
                                    CalculateStrength(level), CalculateIntelligence(level),
                                    CalculatePhysique(level), 0, 0)
    {
        Weapon = RandomWeapon();
        Health = CalculateHealth(level, Physique);
        MaxHP = Health;
    }

    private static string RandomEnemyName()
    {
        Random random = new Random();
        int index = random.Next(EnemyNames.Length);
        return EnemyNames[index];
    }
    private static string RandomWeapon()
    {
        Random random = new Random();
        int index = random.Next(Weapons.Length);
        return Weapons[index];
    }
    private static int CalculateStrength(int level)
    {
        int baseValue = RollAttribute(level);
        return baseValue;
    }

    private static int CalculateIntelligence(int level)
    {
        int baseValue = RollAttribute(level);
        return baseValue;
    }

    private static int CalculatePhysique(int level)
    {
        int baseValue = RollAttribute(level);
        return baseValue;
    }

    private static int CalculateHealth(int level, int phy)
    {
        int baseValue = (level * 5) + phy;
        return baseValue;
    }
    public override int Ability()
    {
        Console.WriteLine($"{this.Name} attackerar spelaren med en {Weapon}-attack!");
        Console.WriteLine($"{this.Name} skadar {Strength}\n");
        return Strength;
    }
    private static int RollAttribute(int bonus)
    {
        Random rnd = new Random();
        List<int> rolls = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            int t6 = rnd.Next(1, 7);
            rolls.Add(t6);
        }

        rolls.Remove(rolls.Min());
        int total = rolls.Sum() + bonus;

        return total;
    }
}
