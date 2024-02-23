using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDto.Dtos.Invoices
{
    public class PossibleTemplateFieldsDto
    {
        public string Field { get; set; }
        public string Description { get; set; }
        public PossibleTemplateFieldsDto(string field, string description)
        {
            Field = field;
            Description = description;
        }
    }
}
