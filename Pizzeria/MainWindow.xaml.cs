using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;

namespace Pizzeria
{
    /// <summary>
    ///     Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ConfidenceThreshold = 0.4;
        private bool _initiative = true;
        private readonly PizzaGrammarFactory _grammarFactory = new PizzaGrammarFactory();
        private readonly Pizza _pizza = new Pizza();
        private readonly SpeechSynthesizer _speechSynthesizer = new SpeechSynthesizer();
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private bool _speechOn = true;
        private SpeechRecognitionEngine _speechRecognitionEngine;

        public MainWindow()
        {
            InitializeComponent();
            _worker.DoWork += RunPizzeria;
            _worker.RunWorkerAsync();
        }

        private void InitializeBot()
        {

            var culture = new CultureInfo("en-US");
            _speechRecognitionEngine = new SpeechRecognitionEngine(culture);

            _speechRecognitionEngine.BabbleTimeout += TimeSpan.FromSeconds(2);
            _speechRecognitionEngine.InitialSilenceTimeout += TimeSpan.FromSeconds(10);
            _speechSynthesizer.SetOutputToDefaultAudioDevice();
            _speechRecognitionEngine.SetInputToDefaultAudioDevice();
            // _speechSynthesizer.SelectVoice("Microsoft Server Speech Text to Speech Voice (pl-PL, Paulina)");
        }

        private void RunPizzeria(object sender, DoWorkEventArgs e)
        {
            InitializeBot();
            _speechSynthesizer.Speak("Welcome in intergalactic Pizzeria. Which pizza do you prefer?");
            //_speechSynthesizer.Speak("Witaj w pizzerii. Proszę podać nazwę piccy!");
            _grammarFactory.AddGrammars(_speechRecognitionEngine);
            _speechRecognitionEngine.SpeechRecognized += PizzaManager;
            _speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

        }

        private void PizzaManager(object sender, SpeechRecognizedEventArgs e)
        {
            if (!_speechOn) return;
            var text = e.Result.Text;
            var textList = text.Split(' ').ToList();
            var confidence = e.Result.Confidence;
            Console.WriteLine($@"ROZPOZNANO (wiarygodność: {e.Result.Confidence:0.000}): '{text}'");

            if (confidence >= ConfidenceThreshold)
            {
                Console.WriteLine(text);
                if (_initiative)
                {
                    ProcessHelpMessages(textList);
                    ProcessOrder(textList);
                    FillKnownProperties(textList);
                    _initiative = false;
                }
                else
                {
                    FillKnownProperties(textList);
                    ProcessHelpMessages(textList);
                    ProcessOrder(textList);

                }

                if (_pizza.OrderReady())
                {
                    CalculateThePrice();
                    SetLabels();
                    _speechSynthesizer.SpeakAsync($"Thank you for the order. It will be {_pizza.Price} dollars. Have a nice day!");
                    //_speechSynthesizer.SpeakAsync("Dziękuję za zamówienie");
                    _speechOn = false;
                }

            }
            else
            {
                _speechSynthesizer.SpeakAsync($"Sorry I didn't get that.");
            }
        }

        private void CalculateThePrice()
        {
            _pizza.Price = new Random().Next(20, 45).ToString();
        }

        private void ProcessOrder(IReadOnlyCollection<string> textList)
        {
            int noice = 0;
            if (textList.Contains("sauce"))
            {
                noice++;
            }

            if (textList.Contains("pizza"))
            {
                noice++;
            }
            switch (textList.Count - noice)
            {
                case 1:
                    ProcessStepByStep();
                    break;
                case 2:
                    ProcessPartialOrder(textList);
                    break;
            }
        }

        private void ProcessPartialOrder(IReadOnlyCollection<string> textList)
        {
            if (textList.Select(el => el).Intersect(PizzaInfo.Cakes).ToList().Count == 0)
            {
                _speechSynthesizer.SpeakAsync("And what kind of cake do you prefer?");
                //_speechSynthesizer.SpeakAsync("Proszę podać rodzaj ciasta");
            }
            else if (textList.Select(el => el).Intersect(PizzaInfo.Dipps).ToList().Count == 0)
            {
                _speechSynthesizer.SpeakAsync("How about the sauce?");
                //_speechSynthesizer.SpeakAsync("Proszę podać sos");
            }
            else if (textList.Select(el => el).Intersect(PizzaInfo.PizzaChoices).ToList().Count == 0)
            {
                _speechSynthesizer.SpeakAsync("Which pizza? For now we have only hawaiian or peperoni");
                //_speechSynthesizer.SpeakAsync("Jaka będzie picca?");
            }
        }

        private void FillKnownProperties(IReadOnlyCollection<string> textList)
        {
            var cakes = textList.Select(el => el).Intersect(PizzaInfo.Cakes).FirstOrDefault();
            var dips = textList.Select(el => el).Intersect(PizzaInfo.Dipps).FirstOrDefault();
            var choices = textList.Select(el => el).Intersect(PizzaInfo.PizzaChoices).FirstOrDefault();

            if (!string.IsNullOrEmpty(cakes))
            {
                _pizza.Cake = cakes;
            }

            if (!string.IsNullOrEmpty(dips))
            {
                _pizza.Dip = dips;
            }

            if (!string.IsNullOrEmpty(choices))
            {
                _pizza.Choice = choices;
            }

            SetLabels();
        }

        private void ProcessHelpMessages(IReadOnlyCollection<string> textList)
        {
            if (textList.Contains("Stop"))
            {
                _speechOn = false;
                //_speechSynthesizer.SpeakAsync("Dziękuję za zamówienie!");
                CalculateThePrice();
                SetLabels();
                _speechSynthesizer.SpeakAsync("It was a pleasure to serve you. Have a nice day!");
                return;
            }

            if (textList.Contains("Help"))
            {
                _speechSynthesizer.SpeakAsync("Please order a pizza!");
                //_speechSynthesizer.SpeakAsync("Zamów piccce!");
                return;
            }

            if (textList.Contains("Reset"))
            {
                _speechSynthesizer.SpeakAsync("Resetting the order! You can order new one now.");
                //_speechSynthesizer.SpeakAsync("Anulowano zamówienie!");
                _pizza.ResetPizza();
                SetLabels();
            }
        }

        private void ProcessStepByStep()
        {
            if (string.IsNullOrEmpty(_pizza.Cake))
            {
                _speechSynthesizer.SpeakAsync("What kind of cake?");
                //_speechSynthesizer.SpeakAsync("Jakie ciasto?");
                return;
            }

            if (string.IsNullOrEmpty(_pizza.Dip))
            {
                //_speechSynthesizer.SpeakAsync("Jaki sos?");
                _speechSynthesizer.SpeakAsync("Which sauce?");
                return;
            }

            if (string.IsNullOrEmpty(_pizza.Choice))
            {
                //_speechSynthesizer.SpeakAsync("Jaka pizza?");
                _speechSynthesizer.SpeakAsync("Which pizza?");
            }

        }

        private void SetLabels()
        {
            Dispatcher?.Invoke(() =>
            {
                pizzaNumber.Text = _pizza.Choice;
                pizzaCake.Text = _pizza.Cake;
                pizzaDip.Text = _pizza.Dip;
                pizzaPrice.Text = _pizza.Price;
            });
        }
    }
}