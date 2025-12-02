namespace UstaTakipMvc.Web.Models.Customers
{
    public interface ICustomerForm
    {
        CustomerType Type { get; set; }

        string? FullName { get; set; }
        string? NationalId { get; set; }

        string? CompanyName { get; set; }
        string? TaxNumber { get; set; }

        string Phone { get; set; }
        string Address { get; set; }
    }
}
