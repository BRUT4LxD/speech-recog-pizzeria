namespace Pizzeria
{
    internal class Pizza
    {
        public string Choice { get; set; } = "";
        public string Dip { get; set; } = "";
        public string Cake { get; set; } = "";
        public string AdditionalIngredience { get; set; } = "";
        public string Price { get; set; }

        public void ResetPizze()
        {
            Choice = "";
            Dip = "";
            Cake = "";
            AdditionalIngredience = "";
            Price = "";
        }

        public bool OrderReady()
        {
            return !string.IsNullOrEmpty(Choice) && !string.IsNullOrEmpty(Dip) && !string.IsNullOrEmpty(Cake);
        }
    }
}
