using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spa.model
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
