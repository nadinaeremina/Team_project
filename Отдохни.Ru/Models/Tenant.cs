using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.Models
{
    [Table(name: "Tenants")]
    public class Tenant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DayOfBirth { get; set; }
        public PhotoUser PhotoUserID { get; set; }
        public User UserID { get; set; }
    }
}
