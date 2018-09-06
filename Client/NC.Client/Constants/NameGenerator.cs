using System;

namespace NC.Client.Constants
{
    /// <summary>
    /// Player name generator.
    /// </summary>
    public static class NameGenerator
    {
        private static readonly string[] Names = {
            "Con Mammoth",
            "D-Hog-Day",
            "Pinball Esq",
            "CommandX",
            "Houston Rocket",
            "Third Moon",
            "Stallion Patton",
            "Hyper Kong",
            "Elder Pogue",
            "Rando Tank",
            "JesterZilla",
            "Lord Theus",
            "Omega Sub",
            "Uncle Psycho",
            "Station WMD",
            "FrankenGrin",
            "Sir Shove",
            "Goatee Shield",
            "The Final Judgement",
            "Trash Master",
            "Bug Blitz",
            "Landfill Max",
            "Knight Light",
            "Bit Sentinel",
            "K-Tin Man",
            "Father Abbot",
            "Blistered Outlaw",
            "Scare Stone",
            "Admiral Tot",
            "RedFeet",
            "Chew Chew",
            "Scrapple",
            "Renegade Slugger",
            "Solo Kill",
            "Prof.Screw",
            "ManMaker",
            "RedFisher",
            "Trick Baron",
            "General Broomdog",
            "Automatic Slicer",
            "Shadow Bishop",
            "Raid Bucker",
            "Fist Wizard",
            "Centurion Sherman",
            "Atomic Blastoid",
            "Doz Killer",
            "Don Stab",
            "Bootleg Taximan",
            "Liquid Death",
            "Earl of Arms",
            "FlyGuardX",
            "Baby Brown",
            "Gov Skull",
            "Guncap Slingbad",
            "General Finish",
            "Sky Herald",
            "Pistol Hydro",
            "Mt.Indiana",
            "Lope Lope",
            "Greek Rifle",
            "Rocky Highway",
            "Seal Snake",
            "Poptart AK47",
            "Mad Robin",
            "Snow Pharaoh",
            "The Shield Toronto",
            "Jack Cassidy",
            "Saint La-Z-Boy",
            "Twix Bond",
            "Mad Kid",
            "Sultan of Speed",
            "Bazooka Har-de-har",
            "DanimalDaze",
            "Wolf Tribune",
            "Demand Chopper"
        };

        /// <summary>
        /// Get player name.
        /// </summary>
        /// <returns>Player name.</returns>
        public static string GetNext()
        {
            return Names[new Random().Next(Names.Length - 1)];
        }
    }
}