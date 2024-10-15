using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINI_RPG;

internal class Check   // Kolla olika siffror
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
