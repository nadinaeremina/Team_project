using Отдохни.Ru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.ModelsForDataLayer
{
    public class Feedback_mdl
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int QuanytityOfStars { get; set; }
        public int PropertyID { get; set; }
        public int LandlordID { get; set; }
        public Feedback_mdl(int id, string title, int quanytityOfStars, int propertyID, int landlordID)
        {
            Id = id;
            Title = title;
            QuanytityOfStars = quanytityOfStars;
            PropertyID = propertyID;
            LandlordID = landlordID;
        }
        public override string ToString()
        {
            return $"{Id,-5} {Title,-300}, {QuanytityOfStars,-5} {PropertyID,-5} {LandlordID,-5}";
        }
    }
}
