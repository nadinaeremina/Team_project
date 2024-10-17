using Отдохни.Ru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.ModelsForDataLayer
{
    public class PhotoUser_mdl
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
        public PhotoUser_mdl(int id, string title, string description, int uid)
        {
            Id = id;
            Title = title;
            Description = description;
            UserID = uid;
        }
        public override string ToString()
        {
            return $"{Id,-5} {Title,-100} {Description,-100} {UserID,-5}";
        }
    }
}
