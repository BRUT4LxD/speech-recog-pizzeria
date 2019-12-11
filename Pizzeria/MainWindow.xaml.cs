using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
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
        private readonly PizzaGrammarFactory _grammarFactory = new PizzaGrammarFactory();
        private readonly Pizza _pizza = new Pizza();
        private readonly SpeechSynthesizer _speechSynthesizer = new SpeechSynthesizer();
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private bool _speechOn = true;
        private SpeechRecognitionEngine _speechRecognitionEngine;
        private int _step = 2;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBot();
            _worker.DoWork += RunPizzeria;
            _worker.RunWorkerAsync();
        }

        private void InitializeBot()
        {
            var culture = new CultureInfo("pl-PL");
            _speechRecognitionEngine = new SpeechRecognitionEngine(culture);
            _speechSynthesizer.SetOutputToDefaultAudioDevice();
            _speechRecognitionEngine.SetInputToDefaultAudioDevice();
            _speechSynthesizer.SelectVoice("Microsoft Server Speech Text to Speech Voice (pl-PL, Paulina)");
        }

        private void PizzaManager(object sender, SpeechRecognizedEventArgs e)
        {
            _speechSynthesizer.SpeakAsync("Powieś cycki na choice");
            if (!_speechOn) return;
            var text = e.Result.Text;
            var textList = text.Split(' ').ToList();
            var confidence = e.Result.Confidence;
            Console.WriteLine($@"ROZPOZNANO (wiarygodność: {e.Result.Confidence:0.000}): '{text}'");

            if (confidence >= ConfidenceThreshold)
            {
                Console.WriteLine(text);

                if (text.IndexOf("Stop") >= 0)
                {
                    _speechOn = false;
                    _speechSynthesizer.SpeakAsync("Dziękuję za zamówienie!");
                }
                else if (text.IndexOf("Pomoc") >= 0)
                {
                    _speechSynthesizer.SpeakAsync("Zamów piccce!");
                }

                if (textList.Count == 1)
                {
                    switch (_step)
                    {
                        case 2:
                            _pizza.Number = text;
                            SetLabels();
                            _step++;
                            _speechSynthesizer.SpeakAsync("Jakie ciasto?");
                            break;
                        case 3:
                            _pizza.Cake = text;
                            SetLabels();
                            _step++;
                            _speechSynthesizer.SpeakAsync("Jaki sos?");
                            break;
                        case 4:
                            _pizza.Dip = text;
                            _pizza.Price = new Random().Next(40, 150).ToString();
                            SetLabels();
                            _step = 2;
                            _speechSynthesizer.SpeakAsync("Dziękuję za zamówienie!");
                            _speechOn = false;
                            break;
                        default:
                            _speechSynthesizer.SpeakAsync("Wystąpił błąd");
                            break;
                    }
                }
                else if (textList.Count == 2)
                {
                    if (textList.Select(el => el).Intersect(PizzaInfo.Cakes).ToList().Count == 0)
                    {
                    }
                    else if (textList.Select(el => el).Intersect(PizzaInfo.Dipps).ToList().Count == 0)
                    {
                    }
                    else if (textList.Select(el => el).Intersect(PizzaInfo.PizzaNumbers).ToList().Count == 0)
                    {
                    }
                }
                else
                {
                    SetLabels();
                    _speechSynthesizer.SpeakAsync("Dziękuję za zamówienie");
                }
            }
        }

        private void Reset()
        {
            _pizza.ResetPizze();
            SetLabels();
            Dispatcher?.Invoke(() => { ticket.Stroke = Brushes.White; });
            _speechSynthesizer.SpeakAsync("Spróbuj jeszcze raz");
        }

        private void RunPizzeria(object sender, DoWorkEventArgs e)
        {
            _speechSynthesizer.Speak("Witaj w pizzerii. Proszę podać nazwę piccy!");
            _grammarFactory.AddGrammars(_speechRecognitionEngine);

            _speechRecognitionEngine.SpeechRecognized += PizzaManager;
            _speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SetLabels()
        {
            Dispatcher?.Invoke(() =>
            {
                pizzaNumber.Content = _pizza.Number;
                pizzaCake.Content = _pizza.Cake;
                pizzaDip.Content = _pizza.Dip;
                pizzaPrice.Content = _pizza.Price;
            });
        }
    }
}