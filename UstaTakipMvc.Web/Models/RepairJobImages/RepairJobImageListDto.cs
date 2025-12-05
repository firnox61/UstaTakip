namespace UstaTakipMvc.Web.Models.RepairJobImages
{
    public class RepairJobImageListDto
    {
        public Guid Id { get; set; }
        public Guid RepairJobId { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; }
    }
}
