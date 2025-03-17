using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class PartialUpdateModel
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string NewValue { get; set; }
    }
}
