using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_26_04.Models
{
    public class Country : Entity
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
