using System.Linq;
using Microsoft.Speech.Recognition;

namespace Pizzeria
{
    internal class PizzaGrammarFactory
    {
        public void AddGrammars(SpeechRecognitionEngine engine)
        {
            Choices systemChoices = new Choices(PizzaInfo.DefaultCommands.ToArray());
            Choices pizzaChoice = new Choices(PizzaInfo.PizzaChoices.ToArray());
            Choices pizzaCakes = new Choices(PizzaInfo.Cakes.ToArray());
            Choices pizzaDips = new Choices(PizzaInfo.Dipps.ToArray());

            GrammarBuilder systemGrammarBuilder = new GrammarBuilder();
            systemGrammarBuilder.Append(systemChoices);
            Grammar systemGrammar = new Grammar(systemGrammarBuilder);

            GrammarBuilder choiceBuilder = new GrammarBuilder();
            GrammarBuilder cakeBuilder = new GrammarBuilder();
            GrammarBuilder dipBuilder = new GrammarBuilder();
            choiceBuilder.Append(pizzaChoice);
            cakeBuilder.Append(pizzaCakes);
            dipBuilder.Append(pizzaDips);

            Grammar choiceGrammar = new Grammar(choiceBuilder);
            Grammar cakeGrammar = new Grammar(cakeBuilder);
            Grammar dipGrammar = new Grammar(dipBuilder);
            engine.LoadGrammarAsync(choiceGrammar);
            engine.LoadGrammarAsync(cakeGrammar);
            engine.LoadGrammarAsync(dipGrammar);
            engine.LoadGrammarAsync(systemGrammar);

            GrammarBuilder allBuilder = new GrammarBuilder();
            allBuilder.Append(choiceBuilder);
            allBuilder.Append(cakeBuilder);
            allBuilder.Append(dipBuilder);
            Grammar allGrammar = new Grammar(allBuilder);

            engine.LoadGrammarAsync(allGrammar);
        }

    }
}
