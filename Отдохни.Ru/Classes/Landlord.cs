using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.Classes
{
    public class Landlord
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string surName;

        public string SurName
        {
            get { return surName; }
            set { surName = value; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        private int quantityOfProperty;

        public int QuantityOfProperty
        {
            get { return quantityOfProperty; }
            set { quantityOfProperty = value; }
        }

        private int photoUserId;

        public int PhotoUserId
        {
            get { return photoUserId; }
            set { photoUserId = value; }
        }

        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
    }
}
