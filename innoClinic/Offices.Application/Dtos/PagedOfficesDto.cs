namespace Offices.Application.Dtos {
    public record PagedOfficesDto {
        public PagedOfficesDto() {
        }

        public PagedOfficesDto( List<OfficeDto> offices, int total ) {
            this.Offices = offices;
            this.Total = total;
        }

        public List<OfficeDto> Offices { get; set; }
        public int Total { get; set; }
    }
}
