using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.ObjectModel;

namespace POSApp.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<NozzleItem> Nozzles { get; set; }

        public MainViewModel()
        {
            Nozzles = new ObservableCollection<NozzleItem>
            {
                new NozzleItem
                {
                    NozzleName = "Nozzle1",
                    CurrentTransaction = "Transaction123",
                    LastTransaction = "Transaction122",
                    TotalUnprocessed = "50L",
                    FuelType = new FuelType(FuelTypeEnum.Diesel),
                    Liters = 150.0
                },
                // Add more nozzles here if needed
            };
        }

        // If you need a method to add nozzles, use this:
        public void AddNozzle(NozzleItem nozzle)
        {
            Nozzles.Add(nozzle);
        }
    }
}