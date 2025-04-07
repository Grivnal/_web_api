using System.ComponentModel.DataAnnotations;

namespace pv311_web_api.DAL.Entities
{
    public interface IBaseEntity<TId>
    {
        TId Id { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }

    public class BaseEntity<TId> : IBaseEntity<TId>
    {
        [Key]
        public virtual TId Id { get; set; } = default!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}