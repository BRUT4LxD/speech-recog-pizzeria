using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Management;
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
        private const double ConfidenceThreshold = 0.01;
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
            #region
            var mos = new ManagementObjectSearcher("select * from Win32_SoundDevice");
            foreach (ManagementObject soundDevice in mos.Get())
            {
                var sDeviceId = soundDevice.GetPropertyValue("DeviceId").ToString();
                var sDeviceName = soundDevice.GetPropertyValue("Name").ToString();
                var sDeviceStatus = soundDevice.GetPropertyValue("Status").ToString();
                Console.WriteLine(@"Device Name = {0} - Device Id = {1} - Status = {2}", sDeviceName, sDeviceId, sDeviceStatus);
            }
            var mos2 = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject videoDevice in mos2.Get())
            {
                var sDeviceId = videoDevice.GetPropertyValue("DeviceId").ToString();
                var sDeviceName = videoDevice.GetPropertyValue("Name").ToString();
                var sDeviceStatus = videoDevice.GetPropertyValue("Status").ToString();
                Console.WriteLine(@"Device Name = {0} - Device Id = {1} - Status = {2}", sDeviceName, sDeviceId, sDeviceStatus);
            }
            #endregion


            var culture = new CultureInfo("pl-PL");
            _speechRecognitionEngine = new SpeechRecognitionEngine(culture);

            _speechSynthesizer.SetOutputToDefaultAudioDevice();
            _speechRecognitionEngine.SetInputToDefaultAudioDevice();
            _speechSynthesizer.SelectVoice("Microsoft Server Speech Text to Speech Voice (pl-PL, Paulina)");
        }

        private void RunPizzeria(object sender, DoWorkEventArgs e)
        {
            InitializeBot();
            _speechSynthesizer.Speak("Witaj w pizzerii. Proszę podać nazwę piccy!");
            _grammarFactory.AddGrammars(_speechRecognitionEngine);
            _speechRecognitionEngine.SpeechRecognized += PizzaManager;
            _speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

        }

        private void PizzaManager(object sender, SpeechRecognizedEventArgs e)
        {
            _speechSynthesizer.SpeakAsync("Powieś cycki na choince");
            if (!_speechOn) return;
            var text = e.Result.Text;
            var textList = text.Split(' ').ToList();
            var confidence = e.Result.Confidence;
            Console.WriteLine($@"ROZPOZNANO (wiarygodność: {e.Result.Confidence:0.000}): '{text}'");

            if (confidence >= ConfidenceThreshold)
            {
                Console.WriteLine(text);

                ProcessHelpMessages(textList);

                ProcessOrder(textList);

                FillKnownProperties(textList);

                if (_pizza.OrderReady())
                {
                    _speechSynthesizer.SpeakAsync("Dziękuję za zamówienie");
                    _speechOn = false;
                }
            }
        }

        private void ProcessOrder(IReadOnlyCollection<string> textList)
        {
            switch (textList.Count)
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
                _speechSynthesizer.SpeakAsync("Proszę podać rodzaj ciasta");
            }
            else if (textList.Select(el => el).Intersect(PizzaInfo.Dipps).ToList().Count == 0)
            {
                _speechSynthesizer.SpeakAsync("Proszę podać sos");
            }
            else if (textList.Select(el => el).Intersect(PizzaInfo.PizzaChoices).ToList().Count == 0)
            {
                _speechSynthesizer.SpeakAsync("Jaka będzie picca?");
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
                _speechSynthesizer.SpeakAsync("Dziękuję za zamówienie!");
                return;
            }

            if (textList.Contains("Pomoc"))
            {
                _speechSynthesizer.SpeakAsync("Zamów piccce!");
                return;
            }

            if (textList.Contains("Anuluj"))
            {
                _speechSynthesizer.SpeakAsync("Anulowano zamówienie!");
                _pizza.ResetPizza();
                SetLabels();
            }
        }

        private void ProcessStepByStep()
        {
            if (string.IsNullOrEmpty(_pizza.Cake))
            {
                _speechSynthesizer.SpeakAsync("Jakie ciasto?");
                return;
            }

            if (string.IsNullOrEmpty(_pizza.Dip))
            {
                _speechSynthesizer.SpeakAsync("Jaki sos?");
                return;
            }

            if (string.IsNullOrEmpty(_pizza.Choice))
            {
                _speechSynthesizer.SpeakAsync("Jaka picca?");
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