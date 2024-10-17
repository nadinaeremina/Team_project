using Отдохни.Ru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.ModelsForDataLayer
{
    public class Booking_mdl
    {
        public int Id { get; set; }
        public DateTime DateOfEntry { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public int PropertyID { get; set; }
        public int LandlordID { get; set; }
        public Booking_mdl(int id, DateTime DoE, DateTime DoD, int pid, int llid)
        {
            Id = id;
            DateOfEntry = DoE;
            DateOfDeparture = DoD;
            PropertyID = pid;
            LandlordID = llid;
        }
        public override string ToString()
        {
            return $"{Id,-5} {DateOfEntry,-15} {DateOfDeparture,-15} {PropertyID,-5} {LandlordID,-5}";
        }
    }
}
