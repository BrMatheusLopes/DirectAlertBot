using System.ComponentModel.DataAnnotations;

namespace DirectAlertBot.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
