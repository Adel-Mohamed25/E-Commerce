using Domain.Entities.Comman;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Reply : BaseEntity
    {
        public string Text { get; set; }

        [ForeignKey("Review")]
        public string ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
