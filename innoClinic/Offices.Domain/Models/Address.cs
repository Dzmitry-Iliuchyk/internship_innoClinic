namespace Offices.Domain.Models {
    public class Address {
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string? OfficeNumber { get; set; }

        public override string ToString() {
            return $"{City}, {Street}, {HouseNumber}, office number: {OfficeNumber}";
        }
    }
}
