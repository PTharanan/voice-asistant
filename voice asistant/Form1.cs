using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;
using Microsoft.Win32;

namespace voice_asistant
{
    public partial class Form1 : Form
    {
        private readonly SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        private readonly SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        [DllImport("user32.dll")]
        static extern bool LockWorkStation();

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        // PC System Theme Control
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const uint KEYEVENTF_EXTENDEDKEY = 0x1;
        private const uint KEYEVENTF_KEYUP = 0x2;
        private const byte VK_VOLUME_UP = 0xAF;
        private const byte VK_VOLUME_DOWN = 0xAE;

        // Constants for system theme control
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;

        // Registry keys for system theme
        private const string PERSONALIZE_KEY = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string APPS_USE_LIGHT_THEME = "AppsUseLightTheme";
        private const string SYSTEM_USES_LIGHT_THEME = "SystemUsesLightTheme";

        // Additional Windows API for system theme change notification
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam,
                                                      string lParam, uint fuFlags, uint uTimeout, out IntPtr result);

        private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xFFFF);
        private const uint WM_SETTINGCHANGE = 0x001A;
        private const uint SMTO_ABORTIFHUNG = 0x0002;

        public Form1()
        {
            InitializeComponent();

            // Main commands with theme commands
            Choices commands = new Choices("hello", "exit", "how are you", "time", "volume up", "volume down", "voicecal", 
                                            "pc dark", "pc light", "dark", "light", "lock", "day", "create", "delete");

            // "open <app>" command
            Choices apps = new Choices("notepad", "calculator", "paint", "winword", "excel", "chrome", "firefox", "camera", 
                                        "settings", "Control");

            GrammarBuilder openCommand = new GrammarBuilder("open");
            openCommand.Append(apps);
            GrammarBuilder closeCommand = new GrammarBuilder("close");
            closeCommand.Append(apps);

            // Combine into one grammar
            Grammar grammar = new Grammar(new GrammarBuilder(new Choices(commands, openCommand, closeCommand)));

            recognizer.LoadGrammar(grammar);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.SpeechRecognized += recognizer_SpeechRecognized;
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            synthesizer.SelectVoiceByHints(VoiceGender.Female);
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string spoken = e.Result.Text.ToLower();

            // Use Invoke to update UI from different thread
            if (User.InvokeRequired)
            {
                User.Invoke(new Action(() => User.Text = spoken));
            }
            else
            {
                User.Text = spoken;
            }

            string response;

            switch (spoken)
            {
                case "hello":
                    response = "Hello!, How Can I help you";
                    break;

                case "exit":
                    Application.Exit();
                    return;  // முழு function-உம் stop பண்ண

                case "how are you":
                    response = "I'm fine thanks!";
                    break;

                case "time":
                    response = DateTime.Now.ToString("h:mm tt");
                    break;

                case "volume up":
                    for (int i = 0; i < 2; i++)
                    {
                        keybd_event(VK_VOLUME_UP, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                        keybd_event(VK_VOLUME_UP, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                    }
                    response = "Volume increased";
                    break;

                case "volume down":
                    for (int i = 0; i < 2; i++)
                    {
                        keybd_event(VK_VOLUME_DOWN, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                        keybd_event(VK_VOLUME_DOWN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                    }
                    response = "Volume decreased";
                    break;

                case "voicecal":
                    this.Invoke(new Action(() =>
                    {
                        // Form1 speech stop to start Form2 speech
                        ToggleSpeechRecognition(false);

                        calculator calcForm = new calculator();
                        calcForm.FormClosed += (s, args) =>
                        {
                            this.Show();
                            // Form2 close to start Form1 speech again
                            ToggleSpeechRecognition(true);
                        };
                        calcForm.Show();
                        this.Hide();
                    }));
                    response = "Opening calculator";
                    break;

                // System Theme control commands
                case "pc dark":
                case "dark":
                    SetSystemDarkMode(true);
                    response = "PC dark mode activated";
                    break;

                case "pc light":
                case "light":
                    SetSystemDarkMode(false);
                    response = "PC light mode activated";
                    break;

                case "lock":
                    response = "Locking your computer";
                    LockWorkStation();
                    break;
                case "create":
                    CreateFolderInDDrive();
                    response = "Creating folder in D drive";
                    break;
                case "delete":
                    DeleteFolderFromDDrive();
                    response = "Deleting folder from D drive";
                    break;
                case "day":
                    response = $"Today is {DateTime.Now.ToString("dddd, MMMM dd")}";
                    break;

                default:
                    if (spoken.StartsWith("open "))
                    {
                        string appName = spoken.Substring(5).Trim();
                        OpenApp(appName);
                        return;
                    }
                    else if (spoken.StartsWith("close "))
                    {
                        string appName = spoken.Substring(6).Trim();
                        CloseApp(appName);
                        return;
                    }
                    response = "I didn't understand that command.";
                    break;
            }

            synthesizer.SpeakAsync(response);

            // Use Invoke to update UI from different thread
            if (PC.InvokeRequired)
            {
                PC.Invoke(new Action(() => PC.Text = response));
            }
            else
            {
                PC.Text = response;
            }
        }

        private void SetSystemDarkMode(bool darkMode)
        {
            try
            {
                // Method 1: Registry setting for system theme
                SetSystemThemeViaRegistry(darkMode);

                // Method 2: DWMAPI for current window (optional)
                SetWindowDarkMode(this.Handle, darkMode);

                // Refresh system to apply changes
                SystemEvents.UserPreferenceChanged += (s, e) => { };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting system theme: {ex.Message}");
                synthesizer.SpeakAsync("Sorry, I couldn't change the system theme");
            }
        }

        private void SetSystemThemeViaRegistry(bool darkMode)
        {
            try
            {
                // Set Apps theme (File Explorer, Settings, etc.)
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(PERSONALIZE_KEY, true))
                {
                    if (key != null)
                    {
                        key.SetValue(APPS_USE_LIGHT_THEME, darkMode ? 0 : 1, RegistryValueKind.DWord);
                        key.SetValue(SYSTEM_USES_LIGHT_THEME, darkMode ? 0 : 1, RegistryValueKind.DWord);
                    }
                    else
                    {
                        // Create the key if it doesn't exist
                        using (RegistryKey newKey = Registry.CurrentUser.CreateSubKey(PERSONALIZE_KEY))
                        {
                            newKey.SetValue(APPS_USE_LIGHT_THEME, darkMode ? 0 : 1, RegistryValueKind.DWord);
                            newKey.SetValue(SYSTEM_USES_LIGHT_THEME, darkMode ? 0 : 1, RegistryValueKind.DWord);
                        }
                    }
                }

                // Broadcast theme change message to all windows
                SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, IntPtr.Zero,
                                  "ImmersiveColorSet", SMTO_ABORTIFHUNG, 1000, out IntPtr _);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting registry theme: {ex.Message}");
                throw;
            }
        }

        public void ToggleSpeechRecognition(bool enable)
        {
            try
            {
                if (recognizer != null)
                {
                    if (enable)
                    {
                        // Speech recognition-ஐ துவக்கு
                        recognizer.RecognizeAsync(RecognizeMode.Multiple);
                    }
                    else
                    {
                        // Speech recognition-ஐ நிறுத்து
                        recognizer.RecognizeAsyncStop();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling speech: {ex.Message}");
            }
        }

        private void SetWindowDarkMode(IntPtr handle, bool darkMode)
        {
            try
            {
                int darkModeValue = darkMode ? 1 : 0;

                // Set dark mode for the window
                int result = DwmSetWindowAttribute(handle,
                    DWMWA_USE_IMMERSIVE_DARK_MODE,
                    ref darkModeValue,
                    sizeof(int));

                if (result != 0)
                {
                    // For older Windows versions
                    DwmSetWindowAttribute(handle,
                        DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1,
                        ref darkModeValue,
                        sizeof(int));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting window dark mode: {ex.Message}");
            }
        }

        private void OpenApp(string appName)
        {
            try
            {
               
                string processName;
                switch (appName.ToLower())
                {
                    case "winword":
                        processName = "winword";
                        break;
                    case "excel":
                        processName = "excel";
                        break;
                    case "chrome":
                        processName = "chrome";
                        break;
                    case "firefox":
                        processName = "firefox";
                        break;
                    case "notepad":
                        processName = "notepad";
                        break;
                    case "calculator":
                        processName = "calc";
                        break;
                    case "paint":
                        processName = "mspaint";
                        break;
                    case "camera":
                        processName = "microsoft.windows.camera:";
                        break;
                    case "settings":
                        processName = "ms-settings:";
                        break;
                    case "Control":
                        processName = "control";
                        break;
                    default:
                        processName = appName;
                        break;
                }
                ;

                Process.Start(new ProcessStartInfo
                {
                    FileName = processName,
                    UseShellExecute = true
                });
                synthesizer.SpeakAsync($"Opening {appName}");
            }
            catch (Win32Exception)
            {
                synthesizer.SpeakAsync($"{appName} is not available on your computer");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening app: {ex.Message}");
                synthesizer.SpeakAsync($"Could not open {appName}");
            }
        }

        private void CloseApp(string appName)
        {
            try
            {
                string processName = appName.ToLower();

                // UWP ஆப்ஸ்களை handle செய்வதற்கான special cases
                if (processName == "camera")
                {
                    CloseUWPApp("Microsoft.WindowsCamera_8wekyb3d8bbwe!App");
                    synthesizer.SpeakAsync("Closing camera");
                    return;
                }
                else if (processName == "settings")
                {
                    CloseUWPApp("windows.immersivecontrolpanel_cw5n1h2txyewy!microsoft.windows.immersivecontrolpanel");
                    synthesizer.SpeakAsync("Closing settings");
                    return;
                }
                else if (processName == "control")
                {
                    // Control panel is a system component, not recommended to force close
                    synthesizer.SpeakAsync("Control panel should be closed manually");
                    return;
                }

                // Map other app names to actual process names
                switch (processName)
                {
                    case "winword":
                        processName = "winword";
                        break;
                    case "excel":
                        processName = "excel";
                        break;
                    case "chrome":
                        processName = "chrome";
                        break;
                    case "firefox":
                        processName = "firefox";
                        break;
                    case "notepad":
                        processName = "notepad";
                        break;
                    case "calculator":
                        processName = "calc";
                        break;
                    case "paint":
                        processName = "mspaint";
                        break;
                    default:
                        processName = appName;
                        break;
                }

                // Find all processes with matching name
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        try
                        {
                            // Try to close gracefully first
                            if (!process.CloseMainWindow())
                            {
                                // If graceful close fails, kill the process
                                process.Kill();
                            }
                            process.WaitForExit(1000); // Wait up to 1 second
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error closing process {processName}: {ex.Message}");
                        }
                    }

                    synthesizer.SpeakAsync($"Closing {appName}");
                }
                else
                {
                    synthesizer.SpeakAsync($"{appName} is not currently running");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CloseApp: {ex.Message}");
                synthesizer.SpeakAsync($"Could not close {appName}");
            }
        }

        // UWP ஆப்ஸ்களை மூடுவதற்கான special method
        private void CloseUWPApp(string appId)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "taskkill",
                    Arguments = $"/f /im ApplicationFrameHost.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing UWP app: {ex.Message}");
            }
        }

        private string GetGreeting()
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 5 && hour < 12) return "Good Morning";
            if (hour >= 12 && hour < 17) return "Good Afternoon";
            if (hour >= 17 && hour < 21) return "Good Evening";
            return "Good Night";
        }

        // இந்த method-ஐ உங்கள் Form1 class-ல் சேர்க்கவும்
        private void CreateFolderInDDrive()
        {
            try
            {
                string folderPath = @"D:\Check\Folder";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    synthesizer.SpeakAsync("Folder successfully created in D drive");

                }
                else
                {
                    synthesizer.SpeakAsync("Folder already exists in D drive");
                }
            }
            catch (UnauthorizedAccessException)
            {
                synthesizer.SpeakAsync("I don't have permission to create folder in D drive");
            }
            catch (Exception ex)
            {
                synthesizer.SpeakAsync("Error creating folder: " + ex.Message);
                Console.WriteLine("Error creating folder: " + ex.Message);
            }
        }

        // இந்த method-ஐ உங்கள் Form1 class-ல் சேர்க்கவும்
        private void DeleteFolderFromDDrive()
        {
            try
            {
                string folderPath = @"D:\Check\Folder";

                if (Directory.Exists(folderPath))
                {
                    // Folder-ல் எதுவும் இல்லையென்றால் மட்டுமே நீக்கு
                    if (Directory.GetFiles(folderPath).Length == 0 &&
                        Directory.GetDirectories(folderPath).Length == 0)
                    {
                        Directory.Delete(folderPath);
                        synthesizer.SpeakAsync("Folder successfully deleted from D drive");

                        // UI-ஐ update செய்ய optional
                        if (PC.InvokeRequired)
                        {
                            PC.Invoke(new Action(() => PC.Text = "Folder deleted from D:"));
                        }
                        else
                        {
                            PC.Text = "Folder deleted from D:";
                        }
                    }
                    else
                    {
                        synthesizer.SpeakAsync("Folder is not empty. Cannot delete.");
                    }
                }
                else
                {
                    synthesizer.SpeakAsync("Folder does not exist in D drive");
                }
            }
            catch (UnauthorizedAccessException)
            {
                synthesizer.SpeakAsync("I don't have permission to delete folder from D drive");
            }
            catch (Exception ex)
            {
                synthesizer.SpeakAsync("Error deleting folder: " + ex.Message);
                Console.WriteLine("Error deleting folder: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            synthesizer.Speak($"{GetGreeting()}, how can I help you?");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Clean up resources
            if (recognizer != null)
            {
                recognizer.RecognizeAsyncStop();
                recognizer.Dispose();
            }

            if (synthesizer != null)
            {
                synthesizer.Dispose();
            }
        }

        private void PC_Click(object sender, EventArgs e)
        {
                
        }
    }
}