# 💱 Enterprise Currency Converter

A desktop application built with **WPF (.NET 8.0)** demonstrating the **MVVM architectural pattern** with real-world enterprise practices including localization, data binding, and clean separation of concerns.

---

## 🖥️ Screenshots

> <img width="585" height="655" alt="image" src="https://github.com/user-attachments/assets/ae5bd144-e640-4e5b-b237-c6b518a4aafb" />
<img width="543" height="615" alt="image" src="https://github.com/user-attachments/assets/a21eb558-fc1e-4729-967c-e7bba14d29c0" />



---

## 🚀 Features

- Convert USD to 10 major world currencies (INR, EUR, GBP, JPY, CAD, AUD, CHF, CNY, SAR, AED)
- Real-time input validation with user-friendly error messages
- Runtime language switching between **English** and **Hindi** without restarting the app
- Live exchange rate display alongside conversion result
- Clean, minimal UI built entirely in XAML

---

## 🏗️ Architecture — MVVM Pattern

This project strictly follows the **Model-View-ViewModel (MVVM)** pattern.

```
┌─────────────────┐         ┌──────────────────┐         ┌─────────────┐
│      VIEW       │◄───────►│   VIEWMODEL      │────────►│    MODEL    │
│ MainWindow.xaml │ Binding │ MainViewModel.cs │  Uses   │CurrencyRate │
│   (XAML only)   │         │ (Logic & State)  │         │    .cs      │
└─────────────────┘         └──────────────────┘         └─────────────┘
```

| Layer | Responsibility | Files |
|---|---|---|
| **Model** | Plain data objects — no UI logic | `CurrencyRate.cs` |
| **View** | UI layout and controls — no business logic | `MainWindow.xaml` |
| **ViewModel** | Exposes data and commands to the View | `MainViewModel.cs`, `BaseViewModel.cs` |
| **Service** | Business logic — currency conversion math | `CurrencyService.cs` |
| **Command** | Connects Button clicks to ViewModel methods | `RelayCommand.cs` |
| **Converter** | Transforms data types for XAML binding | `BoolToVisibilityConverter.cs` |

---

## 📁 Project Structure

```
EnterpriseCurrencyConverter/
│
├── MainWindow.xaml               ← App shell (View)
├── MainWindow.xaml.cs            ← Code-behind (DataContext only)
├── App.xaml                      ← Application entry point
│
├── Models/
│   └── CurrencyRate.cs           ← Currency data object
│
├── ViewModels/
│   ├── BaseViewModel.cs          ← INotifyPropertyChanged base
│   └── MainViewModel.cs          ← Main screen logic
│
├── Commands/
│   └── RelayCommand.cs           ← ICommand implementation
│
├── Services/
│   └── CurrencyService.cs        ← Conversion logic
│
├── Converters/
│   └── BoolToVisibilityConverter.cs  ← bool → Visibility
│
├── Resources/
│   ├── Strings.resx              ← English strings
│   └── Strings.hi-IN.resx        ← Hindi strings
│
└── Views/                        ← Future content views go here
```

---

## 🔑 Key WPF & MVVM Concepts Demonstrated

### 1. Data Binding
UI controls are linked to ViewModel properties — no manual UI updates in code.
```xml
<TextBox Text="{Binding AmountInput, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>
```

### 2. INotifyPropertyChanged
`BaseViewModel` implements this interface so the View auto-refreshes when data changes.
```csharp
protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
```

### 3. ICommand / RelayCommand
Buttons bind to Commands instead of Click event handlers — keeping the ViewModel UI-agnostic.
```xml
<Button Content="{Binding ConvertButtonText}" Command="{Binding ConvertCommand}"/>
```

### 4. ObservableCollection
The currency list auto-updates the ComboBox when items are added or removed at runtime.
```csharp
Currencies = new ObservableCollection<CurrencyRate>(_currencyService.GetSupportedCurrencies());
```

### 5. Value Converter
`BoolToVisibilityConverter` transforms a `bool` into a WPF `Visibility` enum for show/hide logic.
```xml
Visibility="{Binding HasResult, Converter={StaticResource BoolToVisibilityConverter}}"
```

### 6. .resx Localization
Leveraged .NET resource files (`.resx`) to implement a localization pipeline, enabling seamless **runtime language switching** across all UI elements without restarting the application.

```csharp
Thread.CurrentThread.CurrentUICulture = new CultureInfo("hi-IN");
// .NET automatically loads Strings.hi-IN.resx
```

| Resource File | Language | Culture Code |
|---|---|---|
| `Strings.resx` | English (Default) | `en-US` |
| `Strings.hi-IN.resx` | Hindi | `hi-IN` |

---

## 🛠️ Tech Stack

| Technology | Version |
|---|---|
| .NET | 8.0 (LTS) |
| WPF (Windows Presentation Foundation) | .NET 8.0 |
| Language | C# 12 |
| IDE | Visual Studio 2022 |
| Pattern | MVVM |

---

## ▶️ How to Run

1. Clone or download this repository
2. Open `EnterpriseCurrencyConverter.sln` in **Visual Studio 2022**
3. Press **Ctrl + Shift + B** to build
4. Press **F5** to run

> **Requirements:** Windows OS, .NET 8.0 SDK, Visual Studio 2022

---

## 💡 How to Extend This Project

This project is designed to be easily extended:

- **Add a new language** → Create `Strings.fr-FR.resx` for French and add `"Français"` to the Languages list
- **Add a new currency** → Add one more entry in `CurrencyService.GetSupportedCurrencies()`
- **Connect a live API** → Replace the hardcoded rates in `CurrencyService.cs` with an `HttpClient` call to Open Exchange Rates or Fixer.io
- **Add a History view** → Create `Views/HistoryView.xaml` and a corresponding `HistoryViewModel.cs`

---

*Built as a demonstration of WPF MVVM architecture for enterprise-grade desktop application development.*
