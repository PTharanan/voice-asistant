using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace voice_asistant
{
    public partial class calculator : Form
    {
        SpeechRecognitionEngine recognizer;
        SpeechSynthesizer synthesizer;

        public calculator()
        {
            InitializeComponent();
            InitSpeech();
        }

        private void InitSpeech()
        {
            try
            {
                // Initialize objects here instead of in declaration
                recognizer = new SpeechRecognitionEngine();
                synthesizer = new SpeechSynthesizer();

                recognizer.SetInputToDefaultAudioDevice();

                // Create grammar for calculator
                Choices numbers = new Choices();
                for (int i = 0; i <= 100; i++) numbers.Add(i.ToString());

                Choices operations = new Choices(new string[] { "plus", "minus", "times", "divide", "close", "exit" });

                GrammarBuilder gb = new GrammarBuilder();
                gb.Append(numbers);
                gb.Append(operations);
                gb.Append(numbers);

                Grammar g = new Grammar(gb);
                recognizer.LoadGrammar(g);

                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing speech: " + ex.Message);
            }
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Check if synthesizer is disposed before using
            if (synthesizer == null || IsDisposed) return;

            string speech = e.Result.Text;
            
            // Use Invoke for thread safety
            if (label2.InvokeRequired)
            {
                label2.Invoke(new Action(() => label2.Text = "You said: " + speech));
            }
            else
            {
                label2.Text = "You said: " + speech;
            }

            try
            {
                string[] parts = speech.Split(' ');
                if (parts.Length < 3) return;

                int num1 = int.Parse(parts[0]);
                string op = parts[1];
                int num2 = int.Parse(parts[2]);

                // Handle close command separately
                if (op == "close" || op == "exit")
                {
                    CloseCalculator();
                    return;
                }

                int result = 0;
                switch (op)
                {
                    case "plus": result = num1 + num2; break;
                    case "minus": result = num1 - num2; break;
                    case "times": result = num1 * num2; break;
                    case "divide": 
                        if (num2 != 0) 
                            result = num1 / num2; 
                        else 
                            throw new DivideByZeroException();
                        break;
                    default: throw new Exception("Invalid operation");
                }

                string answer = $"Answer is {result}";
                
                // Use Invoke for thread safety
                if (label1.InvokeRequired)
                {
                    label1.Invoke(new Action(() => label1.Text = "PC said: " + answer));
                }
                else
                {
                    label1.Text = "PC said: " + answer;
                }

                // Check before speaking
                if (synthesizer != null && !IsDisposed)
                {
                    synthesizer.SpeakAsync(answer);
                }
            }
            catch (DivideByZeroException)
            {
                string errorMsg = "Cannot divide by zero";
                if (label1.InvokeRequired)
                {
                    label1.Invoke(new Action(() => label1.Text = "PC said: " + errorMsg));
                }
                else
                {
                    label1.Text = "PC said: " + errorMsg;
                }
                synthesizer?.SpeakAsync(errorMsg);
            }
            catch
            {
                string errorMsg = "Sorry, I didn't understand.";
                if (label1.InvokeRequired)
                {
                    label1.Invoke(new Action(() => label1.Text = "PC said: " + errorMsg));
                }
                else
                {
                    label1.Text = "PC said: " + errorMsg;
                }
                synthesizer?.SpeakAsync(errorMsg);
            }
        }

        private void CloseCalculator()
        {
            // Form1-ஐ find செய்து speech-ஐ மீண்டும் துவக்கு
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.ToggleSpeechRecognition(true);
                mainForm.Show();
            }
            this.Close();
        }

        private void calculator_Load(object sender, EventArgs e)
        {
            // Form1-ன் speech-ஐ நிறுத்து
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.ToggleSpeechRecognition(false);
            }

            if (synthesizer != null && !IsDisposed)
            {
                synthesizer.SpeakAsync("Voice calculator ready. Please say your calculation.");
            }
        }

        // Proper cleanup method
        private void CleanupSpeech()
        {
            if (recognizer != null)
            {
                recognizer.RecognizeAsyncStop();
                recognizer.SpeechRecognized -= Recognizer_SpeechRecognized;
                recognizer.Dispose();
                recognizer = null;
            }

            if (synthesizer != null)
            {
                synthesizer.Dispose();
                synthesizer = null;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CleanupSpeech();
            base.OnFormClosing(e);
            
            // Form1-ஐ find செய்து speech-ஐ மீண்டும் துவக்கு
            Form1 mainForm = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.ToggleSpeechRecognition(true);
                mainForm.Show();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CleanupSpeech();
            base.OnFormClosed(e);
        }
    }
}