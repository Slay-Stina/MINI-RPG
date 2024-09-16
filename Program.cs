using System.Reflection;

class Program
{
    static void Main()
    {
        Console.WriteLine("~~ MINI RPG ~~");
        Character player = null;

        bool finished = true;
        while (finished)    // Vet att det är semantiskt fel, men synd att inte använda metoden
        {
            player = Character.Creation();

            Console.WriteLine("Du har skapat denna karaktär:\n");
            player.ShowInfo();
            Console.Write("Är du nöjd? ");
            finished = Finished();
        }

        Console.WriteLine($"\nNu ska {player.Name} ut på äventyr!");
        Console.WriteLine("Nu kommer du att möta olika monster och för varje du besegrar blir nästa svårare!");
        PressKeyToContinue();

        /** Grupp
        // Skapa en lista för gruppen
        List<Character> grupp = new List<Character>();
        **/

        finished = false;
        int level = 1; // Börja äventyret på nivå 1

        while (!finished)
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
                if (!Finished())
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
            finished = Finished();

            // Öka nivå
            level++;
        }
        Console.WriteLine($"\nTack för att du spelade! Du nådde nivå {level}!");
    }

    private static bool Finished()
    {
        Console.Write("(Ja/Nej) ");
        string continueChoice = Console.ReadLine().ToLower();

        if (continueChoice != "ja")
        {
            return true;
        }
        return false;
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

class Check   // Kolla olika siffror
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

}
class Creature
{
    public string Name { get; set; }
    public string Race { get; set; }
    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Physique { get; set; }
    public int Health { get; set; }
    public int Magic { get; set; }
}
class Race
{
    public string RaceName { get; }
    public int StrengthBonus { get; }
    public int IntelligenceBonus { get; }
    public int PhysiqueBonus { get; }

    public Race(string racename, int strbonus, int intbonus, int phybonus)
    {
        RaceName = racename;
        StrengthBonus = strbonus;
        IntelligenceBonus = intbonus;
        PhysiqueBonus = phybonus;
    }

    public static List<Race> raceList = new List<Race>()
    {
        new Race("Människa", 1, 1, 1),
        new Race("Alv", 0, 2, 1),
        new Race("Dvärg", 1, 0, 2),
        new Race("Drakfödd", 2, 1, 0)
    };
    public static void ShowRaces()
    {
        Console.WriteLine($"\nVilket släkte är din karaktär? 1-{Race.raceList.Count}");
        for (int i = 0; i < Race.raceList.Count; i++)
        {
            Race raceInfo = Race.raceList[i];
            Console.WriteLine();
            Console.Write(i + 1 + ". ");
            Console.WriteLine(raceInfo.RaceName);
            if (raceInfo.StrengthBonus != 0)
            { Console.WriteLine($"Styrka: +{raceInfo.StrengthBonus}"); }
            if (raceInfo.IntelligenceBonus != 0)
            { Console.WriteLine($"Intelligens: +{raceInfo.IntelligenceBonus}"); }
            if (raceInfo.PhysiqueBonus != 0)
            { Console.WriteLine($"Fysik: +{raceInfo.PhysiqueBonus}"); }
        }
    }
}
class Proffession
{
    public string ProffName { get; }
    public string AbiltyName { get; }
    public int HPBonus { get; }
    public int MPBonus { get; }
    Proffession(string proffname, string abilityname, int hpBonus, int mpBonus)
    {
        ProffName = proffname;
        AbiltyName = abilityname;
        MPBonus = mpBonus;
        HPBonus = hpBonus;
    }
    public static List<Proffession> proffList = new List<Proffession>()
    {
        new Proffession("Krigare", "Kraftslag", 10, 0),
        new Proffession("Magiker", "Ljungeld", 0, 10),
        new Proffession("Riddare", "Förtrollat slag", 7, 3),
        new Proffession("Druid", "Djurform", 3, 7)
    };
    public static void ShowProff()
    {
        Console.Clear();
        Console.WriteLine($"Vad är ditt yrke? 1-{Proffession.proffList.Count} ");
        Console.WriteLine("Alla yrken börjar med 3st hälsodrycker som helar 10 hälsopoäng.");
        Console.WriteLine("Magiker och Druider börjar även med 2st magidrycker som ger tillbaka 10 magipoäng.");
        for (int i = 0; i < Proffession.proffList.Count; i++)
        {
            Proffession proffInfo = Proffession.proffList[i];
            Console.WriteLine();
            Console.Write(i + 1 + ". ");
            Console.WriteLine(proffInfo.ProffName);
            Console.WriteLine($"Specialattack: {proffInfo.AbiltyName}");
            Console.WriteLine($"Hälsobonus: +{proffInfo.HPBonus}");
            Console.WriteLine($"Magibonus: +{proffInfo.MPBonus}");
        }
    }
}
class Character : Creature
{
    public string Proffession { get; }
    public int Healthpotions { get; set; }
    public int Magicpotions { get; set; }
    public int MaxHP { get; set; }
    public int MaxMP { get; }
    public Character(string name, string race, string proff,
                        int str, int intel,
                        int phy, int hp, int mp)
    {
        Name = name;
        Race = race;
        Proffession = proff;
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
        Console.WriteLine($"Namn: {this.Name}");

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
            Console.Write(t6 + " ");
            rollList.Add(t6);
        }

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
        global::Race.ShowRaces();
        Race chosenRace = global::Race.raceList[Check.ValidIntInput(1, global::Race.raceList.Count) - 1];
        string race = chosenRace.RaceName;
        Console.Clear();

        // Slå fram dina attribut
        int str = RollAttribute(chosenRace.StrengthBonus, chosenRace.RaceName, "styrka");

        int intel = RollAttribute(chosenRace.IntelligenceBonus, chosenRace.RaceName, "intelligens");

        int phy = RollAttribute(chosenRace.PhysiqueBonus, chosenRace.RaceName, "fysik");

        // Välj Yrke
        global::Proffession.ShowProff();
        Proffession ChosenProff = global::Proffession.proffList[Check.ValidIntInput(1, global::Proffession.proffList.Count) - 1];
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
class Warrior : Character
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
class Magician : Character
{
    public Magician(string name, string race, string proff,
                    int str, int intel, int phy,
                    int hp, int mp)
        : base(name, race, proff, str, intel, phy, hp, mp)
    {
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
class Paladin : Character
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
class Druid : Character
{
    private static readonly string[] Animalform = { "Björn", "Örn", "Varg", "Järv", "Spindel" };
    public Druid(string name, string race, string proff,
                    int str, int intel, int phy,
                    int hp, int mp)
        : base(name, race, proff, str, intel, phy, hp, mp)
    {
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
class Enemy : Character
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
public enum CharacterAttributes
{
    Name,
    Race,
    Proffession,
    Strength,
    Intelligence,
    Physique,
    Health,
    Magic,
    Healthpotions,
    Magicpotions, // Bara för karaktärer som har det
}
