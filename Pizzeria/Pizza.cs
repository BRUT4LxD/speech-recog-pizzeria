namespace Pizzeria
{
    internal class Pizza
    {
        public string Number { get; set; } = "";
        public string Dip { get; set; } = "";
        public string Cake { get; set; } = "";
        public string AdditionalIngredience { get; set; } = "";
        public string Price{ get; set; }

        public void ResetPizze()
        {
            Number = "";
            Dip = "";
            Cake = "";
            AdditionalIngredience = "";
            Price = "";
        }
    }
}
