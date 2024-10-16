using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Character : Creature
{
    public string Class { get; set; }
    public string Type { get; set; }
    public int Healthpotions { get; set; }
    public int Magicpotions { get; set; }
    public int MaxHP { get; set; }
    public int MaxMP { get; set; }
    public Character()
    {
        Healthpotions = 3;
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
        Check.AnyKey();

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
        Check.AnyKey();
        return result;
    }
    // Skapa ny karaktär
    public static Character Creation(Character player)
    {
        Console.WriteLine("\nSkapa en ny karaktär!");

        Console.Write("\nNamn: ");
        string name = Console.ReadLine();

        // Välj Yrke
        Console.Clear();
        Proffession.ShowProff();
        Proffession ChosenProff = Proffession.proffList[Check.ValidIntInput(1, Proffession.proffList.Count) - 1];

        switch (ChosenProff.ProffName.ToLower())
        {
            case "krigare":
                player = new Warrior();
                break;
            case "magiker":
                player = new Magician();
                break;
            case "riddare":
                player = new Paladin();
                break;
            case "druid":
                player = new Druid();
                break;
            default:
                player = new Character();
                break;
        }
        player.Name = name;

        // Välj släkte
        Console.Clear();
        Race.ShowRaces();
        Race chosenRace = Race.raceList[Check.ValidIntInput(1, Race.raceList.Count) - 1];
        player.Type = chosenRace.RaceName;
        Console.Clear();

        // Slå fram dina attribut
        player.Strength = RollAttribute(chosenRace.StrengthBonus, chosenRace.RaceName, "styrka");

        player.Intelligence = RollAttribute(chosenRace.IntelligenceBonus, chosenRace.RaceName, "intelligens");

        player.Physique = RollAttribute(chosenRace.PhysiqueBonus, chosenRace.RaceName, "fysik");

        

        // Räkna fram hälsa och magi
        Console.Clear();
        player.Health = player.Physique + ChosenProff.HPBonus;
        player.Magic = player.Intelligence + ChosenProff.MPBonus;
        player.MaxHP = player.Health;
        player.MaxMP = player.Magic;
        Console.WriteLine($"Din hälsa är baserat på din fysik och ditt yrke vilket tillsammans blir {player.Health} hälsopoäng!");
        Console.WriteLine($"Din magi är baserat på din intelligens och ditt yrke vilket tillsammans blir {player.Magic} magipoäng");
        Check.AnyKey();

        
        return player;
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
            Check.AnyKey();
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
            Check.AnyKey();
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

