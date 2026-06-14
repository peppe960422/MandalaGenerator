# 🎵 Audio Reactive Mandala Generator

Ein interaktiver, musikgesteuerter Mandala-Generator in C# WinForms.

Das Projekt kombiniert rekursive Fraktal-/Mandala-Generierung mit Echtzeit-Audioanalyse über NAudio. Die erzeugten Mandalas reagieren dynamisch auf Bass-Frequenzen und Beat-Erkennung (Kick Detection) und erzeugen visuelle Effekte synchron zur Musik.

---

## ✨ Features

### 🎶 Audio Analyse
- Echtzeit-System-Audioaufnahme mit `WasapiLoopbackCapture`
- FFT (Fast Fourier Transform) Analyse
- Bass-Erkennung
- Kick-Detection durch Vergleich mit gleitendem Durchschnitt
- Musikabhängige Animationen

### 🌸 Mandala Generator
- Rekursive Mandala-Strukturen
- Frei konfigurierbare:
  - Generationstiefe
  - Anzahl der Kinderknoten
  - Bewegungsmuster
  - Wachstumsalgorithmen
  - Animationseffekte

### 🎨 Visuelle Effekte
- Dynamische HSV-Farbverläufe
- Audio-abhängige Helligkeit
- Explosions-Effekte bei Bass-Kicks
- Glow-Effekte
- Anti-Aliasing Rendering

---

## 📦 Verwendete Technologien

- .NET WinForms
- C#
- NAudio
- FFT (Fast Fourier Transform)
- GDI+ Rendering

---

## 🚀 Installation

### Voraussetzungen

- .NET 6.0 oder höher
- Visual Studio 2022
- NAudio Package

### NAudio installieren

```powershell
Install-Package NAudio
```

oder

```bash
dotnet add package NAudio
```

---

## ▶️ Anwendung starten

Projekt klonen:

```bash
git clone https://github.com/deinname/audio-reactive-mandala.git
```

Projekt öffnen:

```bash
AudioReactiveMandala.sln
```

Starten:

```bash
F5
```

Musik auf dem System abspielen und beobachten, wie das Mandala darauf reagiert.

---

## 🎛️ Steuerung

### Tastatur

| Taste | Funktion |
|---------|-----------|
| ↑ | Mehr Muster |
| ↓ | Weniger Muster |
| ← | Weniger Kinder |
| → | Mehr Kinder |

---

## 🧩 Konfigurierbare Parameter

### Bewegungsmuster

```csharp
TypeOfMandalaMovement
```

- Const
- Random

---

### Musik Animationen

```csharp
TypeOfMusicAnimation
```

- Normal
- Explode

---

### Generationsalgorithmen

```csharp
TypeOfMandalaGeneretionalProgress
```

Verfügbare Strategien:

- Const
- Progressive
- Regressive
- Random
- Div2
- GenerationDependent
- SinWave
- Explosion
- Organic
- SpiralGrowth
- Fibonacci
- HeartBeat

---

## 🏗️ Projektstruktur

```text
AudioAnalyzer
│
├── Audio Capture
├── FFT Analyse
├── Bass Detection
└── Kick Detection

MandalaPoint
│
├── Rekursive Struktur
├── Animation
├── Position Update
└── Rendering

MandalaFuncs
│
├── Bewegungsstrategien
├── Wachstumsstrategien
└── Zeichnungsstrategien

FormMandala
│
├── UI
├── Timer Loop
├── Rendering
└── Benutzerinteraktion
```

---

## 🎵 Audio-Reaktive Effekte

Der Basspegel wird global gespeichert:

```csharp
MandalaPoint.GlobalBass
```

Kick-Ereignisse aktivieren:

```csharp
MandalaPoint.KickFlash
```

Dadurch werden:

- Linien dicker
- Farben heller
- Glow-Effekte aktiviert
- Partikel größer dargestellt

---

## ⚡ Performance-Schutz

Zur Vermeidung von Abstürzen existiert ein Knoten-Limit:

```csharp
const int NODE_LIMIT = 10000;
```

Vor dem Erzeugen neuer Mandalas wird die maximale Knotenzahl geprüft.

---

## 📸 Beispiel

```text
Musik läuft
      ↓
FFT Analyse
      ↓
Bass Detection
      ↓
Kick Detection
      ↓
Mandala Animation
      ↓
Visualisierung
```

---

## 🔮 Erweiterungsmöglichkeiten

- Shader Rendering (OpenGL / DirectX)
- Spektrum-Visualisierung
- Partikelsysteme
- Export als Video
- MIDI-Unterstützung
- Mehr Audioeffekte
- Preset-System
- Screenshot Export

---

## 📄 Lizenz

Dieses Projekt steht unter der MIT-Lizenz.

---

## 👨‍💻 Autor

Entwickelt als Experiment für:

- Rekursion
- Fraktale Geometrie
- Audioanalyse
- Echtzeitgrafik
- Kreatives Coding
