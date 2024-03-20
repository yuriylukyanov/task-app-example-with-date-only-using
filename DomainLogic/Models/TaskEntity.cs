using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLogic.Models
{
    public record TaskEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }
        public string Name { get; set; }
    }
}
