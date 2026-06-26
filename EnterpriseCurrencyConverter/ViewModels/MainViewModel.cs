using EnterpriseCurrencyConverter.Commands;
using EnterpriseCurrencyConverter.Models;
using EnterpriseCurrencyConverter.Resources;
using EnterpriseCurrencyConverter.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseCurrencyConverter.ViewModels
{
    /// <summary>
    /// MAIN VIEWMODEL: The brain of the application.
    /// 
    /// Responsibilities:
    /// 1. Expose data for the View to display (Properties)
    /// 2. Expose actions for the View to trigger (Commands)  
    /// 3. Coordinate between Service (logic) and View (display)
    /// 4. Handle validation and error messaging
    /// 5. Manage localization (language switching)
    /// 
    /// The View knows about the ViewModel.
    /// The ViewModel does NOT know about the View. ← KEY MVVM RULE
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        // ── PRIVATE FIELDS ───────────────────────────────────────────────────────
        private readonly CurrencyService _currencyService;

        private string _amountInput = string.Empty;
        private CurrencyRate? _selectedCurrency;
        private string _resultText = string.Empty;
        private string _statusMessage = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _hasError;
        private bool _hasResult;
        private bool _isConverting;
        private string _selectedLanguage = "English";

        // ── CONSTRUCTOR ──────────────────────────────────────────────────────────
        public MainViewModel()
        {
            // Dependency: Service is created here (in enterprise apps, it would be injected)
            _currencyService = new CurrencyService();

            // Initialize the currency list from the service
            Currencies = new ObservableCollection<CurrencyRate>(_currencyService.GetSupportedCurrencies());

            // Available languages for the switcher ComboBox
            Languages = new ObservableCollection<string> { "English", "हिंदी (Hindi)" };

            // Wire up commands to their handler methods
            ConvertCommand = new RelayCommand(ExecuteConvert, CanExecuteConvert);
            ClearCommand = new RelayCommand(ExecuteClear);

            // Set initial status
            StatusMessage = Strings.StatusReady;
        }

        // ── BINDABLE PROPERTIES ──────────────────────────────────────────────────
        // Each property uses SetProperty() from BaseViewModel.
        // SetProperty sets the field AND fires PropertyChanged if value changed.

        /// <summary>
        /// What the user types in the TextBox.
        /// Two-way binding: View → ViewModel when user types.
        /// </summary>
        public string AmountInput
        {
            get => _amountInput;
            set
            {
                SetProperty(ref _amountInput, value);
                // Clear previous errors when user starts typing
                HasError = false;
                ErrorMessage = string.Empty;
            }
        }

        /// <summary>
        /// The currency the user picks from the ComboBox.
        /// WPF's SelectedItem binding points to this property.
        /// </summary>
        public CurrencyRate? SelectedCurrency
        {
            get => _selectedCurrency;
            set => SetProperty(ref _selectedCurrency, value);
        }

        /// <summary>
        /// The formatted conversion result shown to the user.
        /// One-way binding: ViewModel → View only.
        /// </summary>
        public string ResultText
        {
            get => _resultText;
            set => SetProperty(ref _resultText, value);
        }

        /// <summary>
        /// Status bar message at the bottom of the window.
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        /// <summary>
        /// Validation error message shown in red below the inputs.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Controls visibility of the error panel via BoolToVisibilityConverter.
        /// </summary>
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        /// <summary>
        /// Controls visibility of the result panel.
        /// </summary>
        public bool HasResult
        {
            get => _hasResult;
            set => SetProperty(ref _hasResult, value);
        }

        /// <summary>
        /// True while conversion is happening — disables the button.
        /// Simulates async operation even though our math is instant.
        /// </summary>
        public bool IsConverting
        {
            get => _isConverting;
            set => SetProperty(ref _isConverting, value);
        }

        /// <summary>
        /// Currently selected display language.
        /// When set, triggers the language switch logic.
        /// </summary>
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    ApplyLanguage(value);
                }
            }
        }

        // ── COLLECTIONS (for ComboBox ItemsSource) ───────────────────────────────

        /// <summary>
        /// ObservableCollection: Like a List, but WPF watches it for Add/Remove.
        /// When you add an item, the ComboBox updates automatically.
        /// </summary>
        public ObservableCollection<CurrencyRate> Currencies { get; }

        public ObservableCollection<string> Languages { get; }

        // ── COMMANDS ─────────────────────────────────────────────────────────────
        // Commands are bound to Buttons in XAML. This replaces Click event handlers.

        public ICommand ConvertCommand { get; }
        public ICommand ClearCommand { get; }

        // ── COMMAND HANDLERS ─────────────────────────────────────────────────────

        /// <summary>
        /// CanExecute: Returns true only when the Convert button should be active.
        /// WPF checks this automatically and enables/disables the button.
        /// </summary>
        private bool CanExecuteConvert(object? parameter)
        {
            return !IsConverting;
        }

        /// <summary>
        /// Execute: Runs when Convert button is clicked.
        /// Validates input → calls service → displays result.
        /// </summary>
        private void ExecuteConvert(object? parameter)
        {
            // Step 1: Validate amount input
            if (!decimal.TryParse(AmountInput, out decimal amount) || amount <= 0)
            {
                ShowError(Strings.ErrorInvalidAmount);
                return;
            }

            // Step 2: Validate currency selection
            if (SelectedCurrency == null)
            {
                ShowError(Strings.ErrorSelectCurrency);
                return;
            }

            // Step 3: Clear previous errors and show converting state
            HasError = false;
            IsConverting = true;
            StatusMessage = Strings.StatusConverting;

            // Step 4: Perform the conversion using the service
            decimal result = _currencyService.Convert(amount, SelectedCurrency);

            // Step 5: Format and display the result
            // C2 format: currency with 2 decimal places
            string formattedAmount = amount.ToString("N2");
            string formattedResult = result.ToString("N2");
            string rateInfo = $"{Strings.RateInfo} 1 USD = {SelectedCurrency.Rate} {SelectedCurrency.CurrencyCode}";

            ResultText = $"$ {formattedAmount} USD  =  {SelectedCurrency.Flag} {formattedResult} {SelectedCurrency.CurrencyCode}\n\n{rateInfo}";
            HasResult = true;
            IsConverting = false;
            StatusMessage = Strings.StatusReady;
        }

        /// <summary>
        /// Clears all inputs and results — resets the form.
        /// </summary>
        private void ExecuteClear(object? parameter)
        {
            AmountInput = string.Empty;
            SelectedCurrency = null;
            ResultText = string.Empty;
            ErrorMessage = string.Empty;
            HasError = false;
            HasResult = false;
            StatusMessage = Strings.StatusReady;
        }

        // ── LOCALIZATION (Language Switching) ────────────────────────────────────

        /// <summary>
        /// Switches the application language at runtime.
        /// 
        /// .NET's ResourceManager automatically loads the correct .resx file
        /// based on Thread.CurrentThread.CurrentUICulture.
        /// 
        /// After switching culture, we fire PropertyChanged for all string
        /// properties so the View re-reads them from the new resource file.
        /// </summary>
        private void ApplyLanguage(string language)
        {
            string cultureCode = language switch
            {
                "हिंदी (Hindi)" => "hi-IN",
                _ => "en-US",
            };

            // This is the KEY line — tells .NET which resource file to load
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);

            // Notify the View that all label properties have changed
            // The View re-reads from Strings.ResourceManager which now uses the new culture
            OnPropertyChanged(nameof(AppTitle));
            OnPropertyChanged(nameof(AmountLabelText));
            OnPropertyChanged(nameof(SelectCurrencyLabelText));
            OnPropertyChanged(nameof(ConvertButtonText));
            OnPropertyChanged(nameof(ClearButtonText));
            OnPropertyChanged(nameof(ResultLabelText));
            OnPropertyChanged(nameof(LanguageLabelText));
            OnPropertyChanged(nameof(FooterText));

            // Update status message in the new language
            StatusMessage = Strings.StatusReady;
        }

        // ── LOCALIZED STRING PROPERTIES ──────────────────────────────────────────
        // These read from the .resx file at runtime.
        // When language switches, OnPropertyChanged fires for these, View re-reads them.

        public string AppTitle => Strings.AppTitle;
        public string AmountLabelText => Strings.AmountLabel;
        public string SelectCurrencyLabelText => Strings.SelectCurrencyLabel;
        public string ConvertButtonText => Strings.ConvertButton;
        public string ClearButtonText => Strings.ClearButton;
        public string ResultLabelText => Strings.ResultLabel;
        public string LanguageLabelText => Strings.LanguageLabel;
        public string FooterText => Strings.FooterText;

        // ── HELPERS ──────────────────────────────────────────────────────────────

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;
            HasResult = false;
        }
    }

}
