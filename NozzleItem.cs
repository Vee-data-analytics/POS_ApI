namespace POSApp
{
    public enum FuelTypeEnum
    {
        Diesel,
        Unleaded98,
        Unleaded95
    }

    public class FuelType
    {
        public FuelTypeEnum Type { get; set; }
        public string Name { get; set; }

        public FuelType(FuelTypeEnum type)
        {
            Type = type;
            Name = type.ToString();
        }
    }

    // Define Attendant class (corrected spelling)
    public class Attendant
    {
        public string? AttendantName { get; set; }
        public int AttendantID { get; set; }
    }
    
    // Define NozzleItem class
    public class NozzleItem
    {
        public string? AttendantName { get; set; }
        public string? NozzleName { get; set; }
        public FuelType? FuelType { get; set; }
        public double? Liters { get; set; }
        public string? CurrentTransaction { get; set; }
        public string? LastTransaction { get; set; }
        public string? TotalUnprocessed { get; set; }
    }
}