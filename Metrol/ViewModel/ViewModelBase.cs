using System.ComponentModel;

namespace Metrol.ViewModel
{
    /// <summary>
    /// Базовая реализация ViewModel.
    /// </summary>
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
