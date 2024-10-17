using Отдохни.Ru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.ModelsForDataLayer
{
    public class Landlord_mdl
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int QuantityOfProperty { get; set; }
        public int PhotoUserID { get; set; }
        public int UserID { get; set; }
        public Landlord_mdl(int id, string LN, string FN, string SN, string em, string pn, int QoP, int pid, int uid)
        {
            Id = id;
            LastName = LN;
            FirstName = FN;
            Surname = FN;
            Email = SN;
            PhoneNumber = FN;
            QuantityOfProperty = QoP;
            PhotoUserID = pid;
            UserID = uid;
        }
        public override string ToString()
        {
            return $"{Id,-5} {LastName,-25} {FirstName,-25} {Surname,-25} {Email,-50} {PhoneNumber,-15} {QuantityOfProperty,-5} {PhotoUserID,-5} {UserID,-5}";
        }
    }
}
