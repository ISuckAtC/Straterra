using System;
using System.Globalization;
using UnityEngine;

public static class NumConverter
{
    public static string GetConvertedTime(int amount)
    {
        int hours = amount / 3600;
        int minutes = (amount - hours * 3600) / 60;
        int seconds = amount - hours * 3600 - minutes * 60;

        string output = "";
        
        if (hours > 0)
        {
            output += hours + " h, ";
            
        }
        if (minutes > 0)
        {
            output += minutes + " m, ";
            
            //return amount / 60 + " minutes, " + amount % 60 + "seconds";
        }
        output += seconds + " s";    

        return output;

    }

    public static string GetConvertedTimeStamp(DateTime time)
    {
        DateTime currentTime = DateTime.Now;

        if (time < currentTime.AddYears(-1))
        {
            string output = time.Day + "." + time.ToString("MMM", CultureInfo.InvariantCulture) + "." + time.Year;
            
            return output;
        }
        else if (time < currentTime.AddMonths(-1))
        {
            string output = time.Day + "." + time.ToString("MMM", CultureInfo.InvariantCulture);
            
            return output;
        }
        else if (time < currentTime.AddDays(-1))
        {
            string output = time.Day + "." + time.ToString("MMM", CultureInfo.InvariantCulture);
            
            return output;
            
        }
        else
        {
            string output = "" + (currentTime.Hour - time.Hour) + "H:" + (currentTime.Minute - time.Minute + "M ago");
            
            return output;
        }
    }


    public static string GetConvertedAmount(int amount)
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
                return "An unimaginable number of";

            if (amount >= 1000000)          // Millions
                return "A legion of";

            return "Zounds of";

        }
        if (amount >= 200)                  // Two Hundred
        {
            if (amount >= 10000)            // Ten Thousands
                return "A swarm of";

            if (amount >= 1000)             // Thousands
                return "A throng of";

            if (amount >= 500)              // Half Thousand
                return "A horde of";

            return "Lots of";
        }

        return "A few";                   // <Twenty
    }

    public static string GetConvertedResource(int amount)
    {
        
        if (amount >= 100000)               // Hundred Thousands
        {
            if (amount >= 100000000)        // > 100 Million
                return ( " > " + GetConvertedAmount(100000000) + " ");

            if (amount >= 100000000)          // Millions
                return ( " > " + GetConvertedAmount(100000000) + " ");

            return ( " > " + GetConvertedAmount(100000) + " ");

        }
        if (amount >= 200)                  // Two Hundred
        {
            if (amount >= 10000)            // Ten Thousands
                return ( " > " + GetConvertedAmount(10000) + " ");

            if (amount >= 1000)             // Thousands
                return ( " > " + GetConvertedAmount(1000) + " ");

            if (amount >= 500)              // Half Thousand
                return ( " > 500 ");

            return ( " > 200 ");
        }

        return ( " < 200 ");                  // <Twenty
    }
    
}
