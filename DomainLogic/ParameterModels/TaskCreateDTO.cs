using DomainLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLogic.ParameterModels
{
    public record TaskCreateDTO {
        [Required]
        public string Name { get; init; }
        [Required]
        public DateOnly StartDate { get; init; }
        [Required]
        public DateOnly FinishDate { get; init; }
    }
}
