using CinemaAppWPF.DTO;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAppWPF.ViewModels
{
    public class SeatSelectionViewModel : INotifyPropertyChanged
    {
        public sessionDto Session { get; }

        private string? _selectedRow;
        public string? SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (_selectedRow != value)
                {
                    _selectedRow = value;
                    OnPropertyChanged();
                    ProceedToPurchaseCommand.NotifyCanExecuteChanged();
                }
            }
        }

        private int? _selectedNumber;
        public int? SelectedNumber
        {
            get => _selectedNumber;
            set
            {
                if (_selectedNumber != value)
                {
                    _selectedNumber = value;
                    OnPropertyChanged();
                    ProceedToPurchaseCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<string> Rows { get; } = new ObservableCollection<string>();
        public ObservableCollection<int> Numbers { get; } = new ObservableCollection<int>();

        public IRelayCommand ProceedToPurchaseCommand { get; }

        public string SelectedSeatString { get; private set; } = string.Empty;

        public bool IsSelectionConfirmed { get; private set; } = false;

        public event Action? RequestClose;

        public SeatSelectionViewModel(sessionDto session)
        {
            Session = session;

            // Populate Rows (A-H)
            for (char c = 'A'; c <= 'H'; c++)
            {
                Rows.Add(c.ToString());
            }

            // Populate Numbers (1-20)
            for (int i = 1; i <= 20; i++)
            {
                Numbers.Add(i);
            }

            ProceedToPurchaseCommand = new AsyncRelayCommand(ExecuteProceedToPurchase, CanExecuteProceedToPurchase);
        }

        private bool CanExecuteProceedToPurchase()
        {
            return (SelectedRow != null && SelectedNumber.HasValue);
        }

        private async Task ExecuteProceedToPurchase()
        {
            SelectedSeatString = $"{SelectedRow}{SelectedNumber}";
            IsSelectionConfirmed = true;
            RequestClose?.Invoke(); // Signal the window to close
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
