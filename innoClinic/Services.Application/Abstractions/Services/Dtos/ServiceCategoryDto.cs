namespace Services.Application.Abstractions.Services.Dtos {
    public class ServiceCategoryDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan TimeSlotSize { get; set; }
        public bool IsActive { set; get; }
    }
}
