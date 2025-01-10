namespace Services.Application.Abstractions.Services {
    public class UpdateServiceDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public string? SpecializationName { get; set; }
        public bool IsActive { get; set; }
    }
}
