using System;
using System.ComponentModel.DataAnnotations;

namespace DirectAlertBot.Entities
{
    public class Alert : BaseEntity
    {
        [Required]
        public long ChatId { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }

        [Required, MaxLength(4096)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public bool IsTriggered { get; set; }
    }
}
