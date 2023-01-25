using System;

public static class NumConverter
{
    public static string GetConvertedNumber(int amount)
    {
        double val = 0;

        if (amount >= 1000000)       // Millions
        {
            val = Math.Round((amount / 1000000d), 2);
            return val + "m";
        }
        else if (amount >= 1000)     // Thousands
        {
            val = Math.Round((amount / 1000d), 1);
            return val + "k";
        }

        return "" + amount;
    }

    public static string GetConvertedArmy(int amount)
    {
        // Unit tests showed this to be the most efficient way
        
        // Code
        // string armyText = "The army consists of";
        // for (int i = 0; i < armyUnits.Length; i++)
        // {
        //     armyText += NumConverter.GetConvertedArmy(armyUnits[i]) + armyUnitName[i] + ","
        //     if (i == armyUnits.Length-1)
        //         armyText -= the last comma, idk how
        // }

        // Output "The army consists of (a legion of) Swordsmen, (zounds of) Archers, (a pack of) Trolls, and an unimaginable number of Slaves"

        if (amount >= 100000)               // Hundred Thousands
        {
            if (amount >= 100000000)        // > 100 Million
                return " an unimaginable number of ";

            if (amount >= 1000000)          // Millions
                return " a legion of ";

            return " zounds of ";

        }
        if (amount >= 200)                  // Two Hundred
        {
            if (amount >= 10000)            // Ten Thousands
                return " a swarm of ";

            if (amount >= 1000)             // Thousands
                return " a throng of ";

            if (amount >= 500)              // Half Thousand
                return " a horde of ";

            return " lots of ";
        }

        return " a few ";                   // <Twenty
    }
    
    
}
