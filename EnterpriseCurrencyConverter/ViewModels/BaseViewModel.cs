using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseCurrencyConverter.ViewModels
{
    /// <summary>
    /// BASE CLASS for all ViewModels.
    /// 
    /// INotifyPropertyChanged is the CORE of WPF data binding.
    /// When a property changes, it raises the PropertyChanged event.
    /// WPF is listening — it then updates the UI automatically.
    /// 
    /// Without this: you change a property, UI shows old value.
    /// With this: you change a property, UI instantly shows new value.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        // The event WPF subscribes to
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Call this inside any property setter to notify WPF of a change.
        /// [CallerMemberName] automatically fills in the property name — 
        /// you don't need to pass it manually!
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Helper: sets a field and fires notification ONLY if the value changed.
        /// This prevents unnecessary UI refreshes.
        /// Usage in a property setter: SetProperty(ref _myField, value);
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
