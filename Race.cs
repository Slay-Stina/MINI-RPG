using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Race
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
    internal static void ShowRaces()
    {
        Console.WriteLine($"\nVilket släkte är din karaktär? 1-{raceList.Count}");
        for (int i = 0; i < raceList.Count; i++)
        {
            Race raceInfo = raceList[i];
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
