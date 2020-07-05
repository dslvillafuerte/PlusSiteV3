namespace SistemaPedidos.Models.DTO
{
    public class ProductoDto
    {
        public string itemId { get; set; }
        public string vendorId { get; set; }
        public string category { get; set; }
        public string sku { get; set; }
        public string vendorSku { get; set; }
        public float price { get; set; }
        public float taxPercent { get; set; }
        public string itemDescription { get; set; }
        public string commentsForAgent { get; set; }
        public bool isActive { get; set; }
    }
}
