using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.Models
{
    [Table(name: "Bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateOfEntry { get; set; }
        [Required]
        public DateTime DateOfDeparture { get; set; }
        public Property PropertyID { get; set; }
        public Landlord LandlordID { get; set; }
    }
}
