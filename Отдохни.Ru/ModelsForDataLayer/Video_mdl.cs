using Отдохни.Ru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.ModelsForDataLayer
{
    public class Video_mdl
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PropertyID { get; set; }
        public Video_mdl(int id, string title, string description, int pid)
        {
            Id = id;
            Title = title;
            Description = description;
            PropertyID = pid;
        }
        public override string ToString()
        {
            return $"{Id,-5} {Title,-25} {Description,-50} {PropertyID,-5}";
        }
    }
}
