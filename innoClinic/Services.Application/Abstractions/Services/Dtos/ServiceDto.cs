namespace Services.Application.Abstractions.Services.Dtos {
    public class ServiceDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public string? SpecializationName { get; set; }
        public bool IsActive { get; set; }
    }
}
