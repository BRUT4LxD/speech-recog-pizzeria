using Microsoft.Speech.Recognition;
namespace Pizzeria
{
    public class GrammarFactory
    {
        private Grammar CreateGrammarFromList(string[] listOfWords)
        {
            Choices pizzaChoice = new Choices(listOfWords);

            GrammarBuilder grammarProgram = new GrammarBuilder(pizzaChoice);

            return new Grammar(grammarProgram);
        }
    }
}
