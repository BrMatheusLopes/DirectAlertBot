using System.ComponentModel.DataAnnotations;

namespace DirectAlertBot.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
