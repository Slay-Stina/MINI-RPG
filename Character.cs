using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Character : Creature
{
    public string Class { get; }
    public int Healthpotions { get; set; }
    public int Magicpotions { get; set; }
    public int MaxHP { get; set; }
    public int MaxMP { get; }
    public Character(string name, string race, string proff,
                        int str, int intel,
                        int phy, int hp, int mp)
    {
        Name = name;
        Type = race;
        Class = proff;
        Strength = str;
        Intelligence = intel;
        Physique = phy;
        Health = hp;
        Magic = mp;
        Healthpotions = 3;
        MaxHP = Health;
        MaxMP = Magic;
    }

    // Hämtar namn på alla klassmedlemmar och dess värden i objektet
    public virtual void ShowInfo()
    {
        Console.WriteLine($"Namn: {Name}");

        foreach (CharacterAttributes attribute in Enum.GetValues(typeof(CharacterAttributes)))
        {
            if (attribute != CharacterAttributes.Name)
            {
                if (GetPropertyStringValue(attribute) == null || GetPropertyStringValue(attribute) == "0")
                {
                    continue; // Hoppa över egenskaper utan värde
                }
                Console.WriteLine($"{attribute}: {GetPropertyStringValue(attribute)}");
            }
        }
        Console.WriteLine();
    }
    // Hämtar värden utifrån egenskapsnamnen
    private string GetPropertyStringValue(CharacterAttributes attribute)
    {
        PropertyInfo property = GetType().GetProperty(attribute.ToString());

        if (property != null)
        {
            return property.GetValue(this).ToString();
        }

        return null;
    }

    // 4d6 minus den sämsta
    private static int RollAttribute(int bonus, string race, string attribute)
    {
        Random rnd = new Random();
        Console.WriteLine($"Nu kommer vi slå för {attribute}!\nVi slår 4st T6 och tar bort den sämsta!");
        Program.PressKeyToContinue();

        List<int> rollList = new List<int>();

        Console.WriteLine("Du slog:");
        for (int i = 0; i < 4; i++)
        {
            int t6 = rnd.Next(1, 7);
            Thread.Sleep(300);
            Console.Write(t6 + " ");
            rollList.Add(t6);
        }

        Thread.Sleep(300);
        Console.Write($" och {rollList.Min()} är det minsta slaget, det tar vi bort.\nResterande total: ");
        rollList.Remove(rollList.Min());
        Console.WriteLine(rollList.Sum());
        Console.WriteLine($"\nDin bonus för {attribute} för att du är en {race.ToLower()} är +{bonus}.");
        int result = rollList.Sum() + bonus;
        Console.WriteLine($"Din totala {attribute} blir: {result}");
        Program.PressKeyToContinue();
        return result;
    }
    // Skapa ny karaktär
    public static Character Creation()
    {
        Console.WriteLine("\nSkapa en ny karaktär!");

        Console.Write("\nNamn: ");
        string name = Console.ReadLine();

        // Välj släkte
        Race.ShowRaces();
        Race chosenRace = Race.raceList[Check.ValidIntInput(1, Race.raceList.Count) - 1];
        string race = chosenRace.RaceName;
        Console.Clear();

        // Slå fram dina attribut
        int str = RollAttribute(chosenRace.StrengthBonus, chosenRace.RaceName, "styrka");

        int intel = RollAttribute(chosenRace.IntelligenceBonus, chosenRace.RaceName, "intelligens");

        int phy = RollAttribute(chosenRace.PhysiqueBonus, chosenRace.RaceName, "fysik");

        // Välj Yrke
        Proffession.ShowProff();
        Proffession ChosenProff = Proffession.proffList[Check.ValidIntInput(1, Proffession.proffList.Count) - 1];
        string proff = ChosenProff.ProffName;

        // Räkna fram hälsa och magi
        Console.Clear();
        int HP = phy + ChosenProff.HPBonus;
        int MP = intel + ChosenProff.MPBonus;
        Console.WriteLine($"Din hälsa är baserat på din fysik och ditt yrke vilket tillsammans blir {HP} hälsopoäng!");
        Console.WriteLine($"Din magi är baserat på din intelligens och ditt yrke vilket tillsammans blir {MP} magipoäng");
        Program.PressKeyToContinue();

        // Skapa karaktären
        Character character;

        switch (proff.ToLower())
        {
            case "krigare":
                character = new Warrior(name, race, proff, str, intel, phy, HP, MP);
                break;
            case "magiker":
                character = new Magician(name, race, proff, str, intel, phy, HP, MP);
                break;
            case "riddare":
                character = new Paladin(name, race, proff, str, intel, phy, HP, MP);
                break;
            case "druid":
                character = new Druid(name, race, proff, str, intel, phy, HP, MP);
                break;
            default:
                character = new Character(name, race, proff, str, intel, phy, HP, MP);
                break;
        }
        return character;
    }
    // Använd din specialattack
    public virtual int Ability()
    {
        Console.WriteLine($"\n {this.Name} attackerar");
        return Strength;
    }
    // Använd en hälso-/magidryck
    public void UsePotion(string potionType)
    {
        if (potionType == "HP")
        {
            if (this.Health < this.MaxHP)
            {
                if (this.Healthpotions > 0)
                {
                    int amountToAdd = Math.Min(10, this.MaxHP - this.Health); // Räknar ut hur mycket som helas, men högts 10

                    this.Health += amountToAdd;
                    Console.WriteLine($"*GULP* {this.Name} dricker en hälsodryck och återfår {amountToAdd} HP.");
                    this.Healthpotions--;
                }
                else
                {
                    Console.WriteLine($"{this.Name} har inga fler hälsodrycker.");
                }
            }
            else
            {
                Console.WriteLine($"{this.Name} har full hälsa och kan inte dricka fler hälsodrycker.");
            }
            Program.PressKeyToContinue();
        }
        else if (potionType == "MP")
        {
            if (this.Magic < this.MaxMP)
            {
                if (this.Magicpotions > 0)
                {
                    int amountToAdd = Math.Min(10, this.MaxMP - this.Magic); // Kontrollera så att vi inte överskrider MaxMP
                    this.Magic += amountToAdd;
                    Console.WriteLine($"*GULP* {this.Name} dricker en magidryck och återfår {amountToAdd} magipoäng.");
                    this.Magicpotions--;
                }
                else
                {
                    Console.WriteLine($"{this.Name} har inga fler magidrycker.");
                }
            }
            else
            {
                Console.WriteLine($"{this.Name} har full magi och kan inte dricka fler magidrycker.");
            }
            Program.PressKeyToContinue();
        }
        else
        {
            Console.WriteLine($"Ogiltig drycktyp: {potionType}");
        }
    }
    public int RollD20(string attribute)
    {
        Random rnd = new Random();
        if (this is not Enemy)
        {
            Console.Write($"Tryck valfri tangent för att slå en T20 för {attribute}...");
            Console.ReadKey();
        }
        int result = rnd.Next(1, 21);
        Console.WriteLine($"\n{this.Name} slog {result}");
        return result;
    }
    public bool IsWarrior()
    {
        return this is Warrior;
    }
}

