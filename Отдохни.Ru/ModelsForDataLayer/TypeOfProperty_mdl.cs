using Отдохни.Ru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.ModelsForDataLayer
{
    public class TypeOfProperty_mdl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TypeOfProperty_mdl(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public override string ToString()
        {
            return $"{Id,-5} {Name,-45}";
        }
    }
}
