using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pv311_web_api.DAL.Entities
{
    public class Car : BaseEntity<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(100)]
        public required string Model { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Brand { get; set; }
        [Range(1800, int.MaxValue)]
        public int Year { get; set; }
        [Range(0, double.MaxValue)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [MaxLength(50)]
        public string? Gearbox { get; set; }
        [MaxLength(50)]
        public string? Color { get; set; }

        [ForeignKey("Manufacture")]
        public string? ManufactureId { get; set; }
        public Manufacture? Manufacture { get; set; }
        public virtual ICollection<CarImage> Images { get; set; } = [];
    }
}