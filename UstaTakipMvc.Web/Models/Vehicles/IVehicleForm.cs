namespace UstaTakipMvc.Web.Models.Vehicles
{
    public interface IVehicleForm
    {
        string Plate { get; set; }
        string Brand { get; set; }
        string Model { get; set; }
        int Year { get; set; }
        Guid CustomerId { get; set; }
    }
}
