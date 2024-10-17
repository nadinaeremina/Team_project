using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.Models
{
    [Table(name: "Feedbacks")]
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int QuanytityOfStars { get; set; }
        public Property PropertyID { get; set; }
        public Landlord LandlordID { get; set; }
    }
}
