using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Proffession
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
    internal static void ShowProff()
    {
        Console.WriteLine($"Vad är ditt yrke? 1-{proffList.Count} ");
        Console.WriteLine("Alla yrken börjar med 3st hälsodrycker som helar 10 hälsopoäng.");
        Console.WriteLine("Magiker och Druider börjar även med 2st magidrycker som ger tillbaka 10 magipoäng.");
        for (int i = 0; i < proffList.Count; i++)
        {
            Proffession proffInfo = proffList[i];
            Console.WriteLine();
            Console.Write(i + 1 + ". ");
            Console.WriteLine(proffInfo.ProffName);
            Console.WriteLine($"Specialattack: {proffInfo.AbiltyName}");
            Console.WriteLine($"Hälsobonus: +{proffInfo.HPBonus}");
            Console.WriteLine($"Magibonus: +{proffInfo.MPBonus}");
        }
    }
}