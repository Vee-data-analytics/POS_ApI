using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Timers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;


namespace POSApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
    private Timer _timer;
    private readonly HttpClient _httpClient = new HttpClient();
    private ObservableCollection<NozzleItem> _nozzles;
    private string _currentDateTime;
    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<NozzleItem> Nozzles
    {
        get => _nozzles;
        set
        {
            _nozzles = value;
            OnPropertyChanged(nameof(Nozzles));
        }
    }

    public string CurrentDateTime
    {
        get => _currentDateTime;
        set
        {
            _currentDateTime = value;
            OnPropertyChanged(nameof(CurrentDateTime));
        }
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        Nozzles = new ObservableCollection<NozzleItem>();
        
        // Initialize timers
        InitializeTimers();
        
        // Load initial data
        Task.Run(async () => await LoadNozzlesFromApi());
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void UpdateDateTime()
    {
        CurrentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private void InitializeTimers()
    {
        // Timer for updating date/time
        _timer = new Timer(1000);
        _timer.Elapsed += (sender, e) => Dispatcher.UIThread.InvokeAsync(UpdateDateTime);
        _timer.Start();

        // Timer for refreshing transaction data every 5 seconds
        var transactionTimer = new Timer(5000);
        transactionTimer.Elapsed += async (sender, e) => await LoadNozzlesFromApi();
        transactionTimer.Start();
    }
    private async Task LoadNozzlesFromApi()
    {
        try
        {
            var transactions = await FetchUnprocessedTransactions();
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var groupedData = transactions
                    .GroupBy(t => new { t.NozzleName, t.FuelTypeName, t.AttendantName })
                    .Select(g => new NozzleItem
                    {
                        NozzleName = g.Key.NozzleName,
                        FuelType = g.Key.FuelTypeName,
                        AttendantName = g.Key.AttendantName,
                        Liters = g.Sum(t => t.Volume),
                        LastTransaction = g.Max(t => t.Timestamp).ToString("yyyy-MM-dd HH:mm:ss"),
                        TotalUnprocessed = $"R{g.Sum(t => t.TotalCost):F2}"
                    })
                    .ToList();

                UpdateNozzlesList(groupedData);
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in LoadNozzlesFromApi: {ex.Message}");
            await Dispatcher.UIThread.InvokeAsync(() => 
                ShowError("Failed to load transaction data. Please check your connection."));
        }
    }

    private void UpdateNozzlesList(List<NozzleItem> newData)
    {
        // Update existing items and add new ones
        foreach (var newItem in newData)
        {
            var existingItem = Nozzles.FirstOrDefault(n => 
                n.NozzleName == newItem.NozzleName && 
                n.AttendantName == newItem.AttendantName);

            if (existingItem != null)
            {
                existingItem.Liters = newItem.Liters;
                existingItem.LastTransaction = newItem.LastTransaction;
                existingItem.TotalUnprocessed = newItem.TotalUnprocessed;
            }
            else
            {
                Nozzles.Add(newItem);
            }
        }

        // Remove items that are no longer in the new data
        var itemsToRemove = Nozzles
            .Where(existingItem => !newData.Any(newItem => 
                newItem.NozzleName == existingItem.NozzleName && 
                newItem.AttendantName == existingItem.AttendantName))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            Nozzles.Remove(item);
        }
    }

    private async Task<List<TransactionData>> FetchUnprocessedTransactions()
    {
        try
        {
            string apiUrl = "http://localhost:8000/backoffice/api/unprocessed-transactions/";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TransactionData>>(jsonResponse);
            }
            
            Debug.WriteLine($"Error fetching data: {response.StatusCode}");
            return new List<TransactionData>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception when fetching data: {ex.Message}");
            return new List<TransactionData>();
        }
    }

    private async void ShowError(string message)
    {
        // Implement error display logic here
        Debug.WriteLine($"Error: {message}");
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _timer?.Stop();
        _timer?.Dispose();
    }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "CashDrawButton":
                        HandleCashDraw();
                        break;
                    case "PrintLastBillButton":
                        HandlePrintLastBill();
                        break;
                    case "ClosedBillsButton":
                        HandleClosedBills();
                        break;
                    case "PaymentButton":
                        HandlePayment();
                        break;
                    case "CashPayoutButton":
                        HandleCashPayout();
                        break;
                    case "CashierMenuButton":
                        HandleCashierMenu();
                        break;
                    case "ManagerMenuButton":
                        HandleManagerMenu();
                        break;
                    case "CloseShiftButton":
                        HandleCloseShift();
                        break;
                    case "TankLevelsButton":
                        HandleTankLevels();
                        break;
                    case "AdminButton":
                        HandleAdmin();
                        break;
                    default:
                        // Handle keypad buttons
                        if (button.Classes.Contains("keypad"))
                        {
                            HandleKeypadInput(button.Content.ToString());
                        }
                        break;
                }
            }
        }

        private void HandleCashDraw()
        {
            Console.WriteLine("Cash Draw button clicked");
        }

        private void HandlePrintLastBill()
        {
            Console.WriteLine("Print Last Bill button clicked");
        }

        private void HandleClosedBills()
        {
            Console.WriteLine("Closed Bills button clicked");
        }

        private void HandlePayment()
        {
            Console.WriteLine("Payment button clicked");
        }

        private void HandleCashPayout()
        {
            Console.WriteLine("Cash Payout button clicked");
        }

        private void HandleCashierMenu()
        {
            Console.WriteLine("Cashier Menu button clicked");
        }

        private void HandleManagerMenu()
        {
            Console.WriteLine("Manager Menu button clicked");
        }

        private void HandleCloseShift()
        {
            Console.WriteLine("Close Shift button clicked");
        }

        private void HandleTankLevels()
        {
            Console.WriteLine("Tank Levels button clicked");
        }

        private void HandleAdmin()
        {
            Console.WriteLine("Admin button clicked");
        }

        private void HandleKeypadInput(string input)
        {
            Console.WriteLine($"Keypad input: {input}");
        }

      
    }

    

    public class TransactionData
    {
        [JsonProperty("attendant_name")]
        public string AttendantName { get; set; }
    
        [JsonProperty("nozzle_name")]
        public string NozzleName { get; set; }
    
        [JsonProperty("fuel_type_name")]
        public string FuelTypeName { get; set; }
    
        public double Volume { get; set; }
        public double TotalCost { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class NozzleItem : INotifyPropertyChanged
    {
    public event PropertyChangedEventHandler PropertyChanged;

    private string _attendantName;
    private string _nozzleName;
    private string _fuelType;
    private double _liters;
    private string _lastTransaction;
    private string _totalUnprocessed;

    public string AttendantName
    {
        get => _attendantName;
        set
        {
            _attendantName = value;
            OnPropertyChanged(nameof(AttendantName));
        }
    }

    public string NozzleName
    {
        get => _nozzleName;
        set
        {
            _nozzleName = value;
            OnPropertyChanged(nameof(NozzleName));
        }
    }

    public string FuelType
    {
        get => _fuelType;
        set
        {
            _fuelType = value;
            OnPropertyChanged(nameof(FuelType));
        }
    }

    public double Liters
    {
        get => _liters;
        set
        {
            _liters = value;
            OnPropertyChanged(nameof(Liters));
        }
    }

    public string LastTransaction
    {
        get => _lastTransaction;
        set
        {
            _lastTransaction = value;
            OnPropertyChanged(nameof(LastTransaction));
        }
    }

    public string TotalUnprocessed
    {
        get => _totalUnprocessed;
        set
        {
            _totalUnprocessed = value;
            OnPropertyChanged(nameof(TotalUnprocessed));
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    }

}


        
