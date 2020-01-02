using System.Collections.Generic;
namespace Pizzeria
{
    public static class PizzaInfo
    {
        public static readonly IEnumerable<string> DefaultCommands = new List<string>(3)
        {
            "Help",
            "Stop",
            "Cancel"
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

        public static readonly IEnumerable<string> PizzaChoices = new List<string>(2)
        {
            "peperoni",
            "hawai",
            "hawaiian",
        };
        public static readonly IEnumerable<string> Dipps = new List<string>(6)
        {
            "garlic",
            "maxican",
            "arabian",
            "no sauce",
            "no"
        };

        public static readonly IEnumerable<string> Cakes = new List<string>(3)
        {
            "thick",
            "thin",
            "medium"
        };
    }
}
