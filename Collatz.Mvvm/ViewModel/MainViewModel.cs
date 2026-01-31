using Collatz.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Collatz.Mvvm.ViewModel
{
    public sealed class MainViewModel : INotifyPropertyChanged,IDataErrorInfo
    {
        private string _startText = "7";
        private string _oddOps = "-";
        private string _evenOps = "-";
        private string _maxValue = "-";
        private bool _isRunning;
        private string _status = "bereit";

        private CancellationTokenSource? _cts;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string StartText
        {
            get => _startText;
            set
            {
                if (Set(ref _startText, value))
                {
                    // Button-Status aktualisieren, wenn sich die UI-Validierung ändert
                    ComputeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string OddOps { get => _oddOps; private set => Set(ref _oddOps, value); }
        public string EvenOps { get => _evenOps; private set => Set(ref _evenOps, value); }
        public string MaxValue { get => _maxValue; private set => Set(ref _maxValue, value); }
        public string Status { get => _status; private set => Set(ref _status, value); }

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (Set(ref _isRunning, value))
                {
                    ComputeCommand.RaiseCanExecuteChanged();
                    CancelCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public AsyncRelayCommand ComputeCommand { get; }
        public RelayCommand CancelCommand { get; }

        public MainViewModel()
        {
            ComputeCommand = new AsyncRelayCommand(ComputeAsync, CanCompute);
            CancelCommand = new RelayCommand(() => _cts?.Cancel(), () => IsRunning);
        }

        // IDataErrorInfo: WPF-Datenbindungsvalidierung (UI-seitige Validierung)
        public string Error => "";

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(StartText))
                {
                    bool ok = NaturalNumberValidator.TryParse(StartText, out _, out string error);
                    return ok ? "" : error;
                }
                return "";
            }
        }

        private bool CanCompute()
        {
            if (IsRunning) return false;
            return NaturalNumberValidator.TryParse(StartText, out _, out _);
        }

        private void ClearResult()
        {
            OddOps = "-";
            EvenOps = "-";
            MaxValue = "-";
        }

        private async Task ComputeAsync()
        {
            // Erneut eine Validierung in der Logikschicht durchführen (Anforderung: Validierung auch in der Logikschicht)
            if (!NaturalNumberValidator.TryParse(StartText, out BigInteger n, out string error))
            {
                Status = $"Eingabefehler：{error}";
                return;
            }

            ClearResult();
            IsRunning = true;
            Status = "Recnnen";

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            var sw = Stopwatch.StartNew();
          

            try
            {
                CollatzResult result = await Task.Run(
                    () => new CollatzCalculator().Calculate(n),
                    ct);

                sw.Stop();

                OddOps = result.OddOps.ToString();
                EvenOps = result.EvenOps.ToString();
                MaxValue = result.MaxValue.ToString();

                Status = $"Abgeschlossen (benötigte Zeit: {sw.Elapsed.TotalSeconds:F2} Sekunden)";
            }
            catch (OperationCanceledException)
            {
                sw.Stop();
                Status = "Abgebrochen";
            }
            catch (Exception ex)
            {
                sw.Stop();
                Status = $"Fehler: {ex.Message}";
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
                IsRunning = false;
            }
        }

        private bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            return true;
        }

    }
}
