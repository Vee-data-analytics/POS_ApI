using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace POSApp
{
    public partial class MainWindow : Window
    {
        private Timer _timer;
        public ObservableCollection<NozzleItem> Nozzles{ get; set; }

 public MainWindow()
{
    InitializeComponent();
    DataContext = this;  // Set DataContext to the MainWindow itself
    UpdateDateTime();

      Nozzles = new ObservableCollection<NozzleItem>
    {
        new NozzleItem 
        { 
            AttendantName = "John Doe",
            NozzleName = "Nozzle 1", 
            FuelType = new FuelType(FuelTypeEnum.Diesel), 
            Liters = 10.5,
            CurrentTransaction = "R218.09",
            LastTransaction = "R438.78",
            TotalUnprocessed = "R656.87"
        },
        new NozzleItem 
        { 
            AttendantName = "Jane Smith",
            NozzleName = "Nozzle 2", 
            FuelType = new FuelType(FuelTypeEnum.Unleaded98), 
            Liters = 15.2,
            CurrentTransaction = "R463.09",
            LastTransaction = "R576.89",
            TotalUnprocessed = "R1039.98"
        },
        new NozzleItem 
        { 
            AttendantName = "Joe Mith",
            NozzleName = "Nozzle 2", 
            FuelType = new FuelType(FuelTypeEnum.Unleaded98), 
            Liters = 15.2,
            CurrentTransaction = "R 200.09",
            LastTransaction = "R69.99",
            TotalUnprocessed = "R270.80"
        },
    };


       
    
        // Set items from ItemSource of the NozzleGrid
        if (this.FindControl<DataGrid>("NozzleGrid") is DataGrid nozzleGrid)
        {
            nozzleGrid.ItemsSource = Nozzles;
        }
    
        // Set up a timer to update the date and time every second
        _timer = new Timer(1000);
        _timer.Elapsed += (sender, e) => Dispatcher.UIThread.InvokeAsync(UpdateDateTime);
        _timer.Start();
    }

        

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void UpdateDateTime()
        {
            if (this.FindControl<TextBlock>("DateTimeTextBlock") is TextBlock dateTimeTextBlock)
            {
                dateTimeTextBlock.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}
