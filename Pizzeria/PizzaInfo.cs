using System.Collections.Generic;
namespace Pizzeria
{
    public static class PizzaInfo
    {
        public static List<string> DefaultCommands = new List<string>
        {
            "Pomoc",
            "Stop"
        };



        //public static List<string> AdditionalIngredience = new List<string>
        //{
        //    "Papryka",
        //    "Ser",
        //    "Oliwki",
        //    "Kukurydza",
        //    "Kurczak",
        //    "Bez dodatków",
        //    "Bez"
        //};

        public static List<string> PizzaNumbers = new List<string>
        {
            "peperoni",
            "hawajska",
        };
        public static List<string> Dipps = new List<string>
        {
            "Czosnkowy",
            "Meksykański",
            "Arabski",
            "Salmon",
            "Bez sosu",
            "Bez"
        };

        public static List<string> Cakes = new List<string>
        {
            "Grube",
            "Cienke",
            "Średnie"
        };
    }
}
