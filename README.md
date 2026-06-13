<<<<<<< HEAD
# 🎙️ Windows Voice Assistant (Desktop Console)

A sleek, lightweight, and offline-capable **Voice Assistant** built in C# using Windows Forms, leveraging the native `System.Speech` library for text-to-speech (TTS) and speech recognition. It allows users to control system volume, toggle system-wide dark/light themes via registry values, manage files, open/close common Windows apps, and use a dedicated hands-free **Voice Calculator** with vocal confirmations.

---

## 📸 Interface Preview

![Voice Assistant Console](assets/screenshot.png)

---

## ✨ Features

*   **🗣️ Speech Recognition & Synthesis:** Fully offline, local speech processing using `System.Speech.Recognition` and `System.Speech.Synthesis`.
*   **💻 System Commands:** Lock workstation, check time, current date, and perform volume controls (up/down).
*   **🎨 PC Theme Control:** Toggle system-wide Windows Immersive Dark Mode and Light Mode dynamically using registry hooks and DWM (Desktop Window Manager) updates.
*   **🧮 Voice Calculator Subsystem:** Activating with `"voicecal"` launches a standalone hands-free math utility, allowing verbal equations (e.g., `"10 plus 15"`, `"50 times 3"`) and vocalizing the answers.
*   **📁 File System Operations:** Voice commands to programmatically create or delete specific operational directories on the `D:` drive (`D:\Check\Folder`).
*   **🚀 App Launcher & Controller:** Launch and close native and third-party desktop applications (Notepad, Calculator, Paint, Word, Excel, Chrome, Firefox, Windows Camera, Settings panel).

---

## 📁 Project Folder Structure

```text
voice-assistant/
├── .git/                        # Git history and config
├── .gitignore                   # Standard Visual Studio gitignore
├── voice asistant.sln           # Visual Studio Solution file
│
├── voice asistant/              # Main Source Project
│   ├── App.config               # App configurations (.NET runtime settings)
│   ├── voice asistant.csproj    # MSBuild Project specification
│   ├── Program.cs               # App entrypoint
│   ├── Form1.cs                 # Main GUI & voice parsing loop
│   ├── Form1.Designer.cs        # Main designer code
│   ├── Form1.resx               # Main form resources
│   ├── calculator.cs            # Voice Calculator subsystem Form
│   ├── calculator.Designer.cs   # Calculator designer code
│   ├── calculator.resx          # Calculator form resources
│   ├── Properties/              # Assembly and resource properties
│   └── bin/                     # Output binaries (Debug/Release targets)
│
├── Voice-Assistant-App/         # Setup and Deployment Project folder
│   └── Voice-Assistant-App.vdproj
│
└── assets/                      # Shared media assets
    └── screenshot.png           # Console User Interface mockup
=======
# 🎙️ Voice Assistant for Windows (C# WinForms)

A **Windows Voice Assistant** built using **C# (.NET Framework)** and the **System.Speech API**, capable of performing desktop tasks through **voice commands** — such as opening/closing apps, controlling volume, switching dark/light themes, managing folders, and more.

---

## 🧠 Features

This assistant listens to your voice and performs Windows operations instantly.

| Command | Description |
|----------|--------------|
| `hello` | Greets you |
| `how are you` | Friendly response |
| `time` | Speaks the current time |
| `day` | Tells today’s date |
| `volume up` / `volume down` | Adjusts system volume |
| `pc dark` / `pc light` | Changes Windows theme |
| `lock` | Locks the computer |
| `create` / `delete` | Creates or deletes a folder at `D:\Check\Folder` |
| `open notepad` | Opens Notepad |
| `close notepad` | Closes Notepad |
| `voicecal` | Opens voice calculator |
| `exit` | Exits the program |

---

## 🪟 Supported Applications

Your assistant can **open/close** the following common apps:

📝 **Notepad**  
🧮 **Calculator**  
🎨 **Paint**  
🌐 **Chrome** / **Firefox**  
📷 **Camera**  
⚙️ **Settings**  
🧰 **Control Panel**  
🪟 **Microsoft Word / Excel**

---

## ⚙️ How It Works

### 🧭 System Flow

```text
🎤 Voice Input
      ↓
🧠 SpeechRecognitionEngine
      ↓
🪄 Command Parsing & Matching
      ↓
⚙️ Execute System Function
      ↓
🗣️ SpeechSynthesizer Response
      ↓
💬 Display in UI
````

---

## 🧩 System Architecture Diagram

![Architecture Diagram](docs/architecture-diagram.png)

> 💡 *Tip:* You can create this diagram in PowerPoint, Figma, or draw.io and save it as `architecture-diagram.png` in the `docs/` folder.

---

## 🧰 Requirements

| Component          | Version                                                                                                      |
| ------------------ | ------------------------------------------------------------------------------------------------------------ |
| **OS**             | Windows 10 or 11                                                                                             |
| **.NET Framework** | 4.7.2 or higher                                                                                              |
| **IDE**            | Visual Studio 2019 / 2022                                                                                    |
| **Libraries**      | `System.Speech`, `System.Drawing`, `Microsoft.Win32`, `System.Runtime.InteropServices`, `System.Diagnostics` |
| **Hardware**       | Microphone (for speech input)                                                                                |

---

## 🚀 Setup Instructions

### Step 1. Clone the Repository

```bash
git clone https://github.com/yourusername/voice-assistant-csharp.git
cd voice-assistant-csharp
```

### Step 2. Open the Project

* Open the `.sln` file in **Visual Studio**.
* Ensure target framework is **.NET Framework 4.7.2** or newer.

### Step 3. Build and Run

* Press **F5** or click **Start Debugging**.
* Grant microphone access if Windows prompts.

### Step 4. Try Voice Commands

Speak clearly into your microphone:

> “Hello”
> “Open Chrome”
> “PC Dark”
> “Volume Up”
> “Lock”

---

## 🧮 Voice Calculator (Secondary Form)

When you say `voicecal`:

* Main assistant pauses speech recognition.
* Opens a new **Calculator Form**.
* Calculator listens for arithmetic commands (e.g., “add 10 and 20”).
* When closed, the main assistant resumes recognition automatically.

---

## 🗂️ Folder Structure

```
voice_asistant/
├── Form1.cs               # Core logic of the Voice Assistant
├── calculator.cs          # Voice calculator window
├── Form1.Designer.cs      # UI layout
├── Program.cs             # Main entry point
└── Properties/
    ├── AssemblyInfo.cs
    ├── Resources.resx
    └── Settings.settings
>>>>>>> acf772f54841f8e17187c52fc30117578ae81fba
```

---

<<<<<<< HEAD
## 🚀 Installation & Setup

### For Users (Installing from Releases)

The voice assistant is packaged as a portable executable or a compressed installer:

1.  Navigate to the [Releases v1.0.0](https://github.com/PTharanan/voice-asistant/releases/tag/v1.0.0) page on the GitHub repository.
2.  Download the **[Voice.Assistant.zip](https://github.com/PTharanan/voice-asistant/releases/download/v1.0.0/Voice.Assistant.zip)** (containing the standalone `.exe` application bundle).
3.  Extract the `.zip` archive to any directory on your computer.
4.  Double-click **`voice asistant.exe`** to run the app instantly. No configuration is required!

> [!NOTE]
> Make sure your microphone is connected and configured as the default recording device in Windows Sound settings before launching.

---

### For Developers (Building from Source)

#### Prerequisites
*   Windows OS (Speech APIs are Windows native)
*   **Visual Studio 2019/2022** with the **.NET Desktop Development** workload installed.
*   **.NET Framework 4.7.2** or higher target developer pack.

#### Getting Started
1.  Clone this repository to your local machine:
    ```bash
    git clone https://github.com/PTharanan/voice-asistant.git
    ```
2.  Open the solution file `voice asistant.sln` inside Visual Studio.
3.  Verify references to the native assemblies:
    *   `System.Speech` (Speech recognition engine)
    *   `System.Windows.Forms` (User Interface)
4.  Set the solution configuration to `Release` or `Debug`.
5.  Press `F5` or click **Start** in Visual Studio to build and run the project.

---

## 🗣️ Voice Command Reference

Speak the following phrases clearly into your microphone to control the console:

### 1. General & System Controls
| Voice Command | Action Description | Assistant Response |
| :--- | :--- | :--- |
| `"hello"` | Greetings | *"Hello! How Can I help you?"* |
| `"time"` | Reads current system time | *"10:30 AM"* (Current time) |
| `"day"` | Reads current weekday and date | *"Today is Friday, June 12"* |
| `"lock"` | Locks your Windows workstation | *"Locking your computer"* |
| `"volume up"` | Simulates system volume-up keypresses | *"Volume increased"* |
| `"volume down"` | Simulates system volume-down keypresses | *"Volume decreased"* |
| `"exit"` | Closes the application | (Exits app) |

### 2. Desktop Application Management
| Voice Command | Action Description |
| :--- | :--- |
| `"open <app>"` | Launches: `notepad`, `calculator`, `paint`, `winword` (MS Word), `excel`, `chrome`, `firefox`, `camera`, `settings`, `control` (Control Panel) |
| `"close <app>"` | Terminates running process for the chosen app safely |

### 3. PC Theme Toggle
| Voice Command | Action Description |
| :--- | :--- |
| `"pc dark"` / `"dark"` | Switches Windows App/System settings to Dark Theme |
| `"pc light"` / `"light"` | Switches Windows App/System settings to Light Theme |

### 4. File Management
| Voice Command | Action Description | Target Path |
| :--- | :--- | :--- |
| `"create"` | Creates folder structure | `D:\Check\Folder` |
| `"delete"` | Safely deletes the empty folder | `D:\Check\Folder` |

### 5. Hands-Free Voice Calculator
Activate by saying `"voicecal"`. The main app window hides, and the custom Calculator form opens.
*   **Synthesized Cue:** *"Voice calculator ready. Please say your calculation."*
*   **Format:** `"[Number] [Operation] [Number]"` (Supports numbers `0` to `100`).
*   **Supported Operations:**
    *   `"plus"` (Addition)
    *   `"minus"` (Subtraction)
    *   `"times"` (Multiplication)
    *   `"divide"` (Division)
*   **Examples:**
    *   `"12 plus 45"` ➡️ *"Answer is 57"*
    *   `"10 times 8"` ➡️ *"Answer is 80"*
*   To exit calculator and return to main panel, say: `"close"` or `"exit"`.

---

## 🛠️ Built With

*   [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) - Language
*   [Windows Forms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/) - GUI Engine
*   [System.Speech](https://learn.microsoft.com/en-us/dotnet/api/system.speech.recognition) - Native Speech API

## 📝 License
This project is licensed under the MIT License.
=======
## 🧑‍💻 Key Methods Overview

| Method                                                | Description                                     |
| ----------------------------------------------------- | ----------------------------------------------- |
| `recognizer_SpeechRecognized()`                       | Handles all recognized voice commands           |
| `SetSystemDarkMode()`                                 | Enables or disables dark mode                   |
| `OpenApp()` / `CloseApp()`                            | Opens or closes applications                    |
| `CreateFolderInDDrive()` / `DeleteFolderFromDDrive()` | Folder management in D:                         |
| `ToggleSpeechRecognition()`                           | Enables or disables the speech engine           |
| `GetGreeting()`                                       | Time-based greetings (morning, afternoon, etc.) |

---

## 📸 Suggested Screenshots

Add these screenshots to your `docs/` folder and reference them in your README:

| Screenshot           | File Name             | Description                                      |
| -------------------- | --------------------- | ------------------------------------------------ |
| 🪟 App UI            | `app-ui.png`          | Main window showing recognized text and response |
| 🗣️ Speaking Command | `speech-detected.png` | Example of a spoken command and system response  |
| ⚫ Theme Change       | `dark-mode.png`       | Windows theme switched to dark mode              |
| 🪄 Folder Action     | `folder-created.png`  | Assistant confirming folder creation             |

Example:

```markdown
![Voice Assistant UI](docs/app-ui.png)
```

---

## ⚠️ Troubleshooting

| Issue                                  | Solution                                         |
| -------------------------------------- | ------------------------------------------------ |
| Speech not recognized                  | Check microphone input; set as default device    |
| Theme not changing                     | Run Visual Studio as **Administrator**           |
| App not opening                        | Ensure the app (e.g., Chrome, Word) is installed |
| “Access denied” during folder creation | Grant **write permission** to D: drive           |

---

## 🧩 Future Enhancements

* 🌐 Add internet-based commands (weather, news, search)
* 🗓️ Calendar and reminder integration
* 🪄 Custom user-defined commands
* 🎵 Music and media control
* 🧠 ChatGPT integration for conversational mode

---

## 🪪 License

This project is licensed under the **MIT License** — feel free to modify, distribute, and use it in your own projects.

---

## 🧑‍💻 Author

**Perinpamoorthy Tharanan**
💬 Developer • Voice UI Enthusiast • C# Automation Engineer

📧 *ptharanan@gmail.com*
🌐 [GitHub Profile](https://github.com/PTharanan)

>>>>>>> acf772f54841f8e17187c52fc30117578ae81fba
