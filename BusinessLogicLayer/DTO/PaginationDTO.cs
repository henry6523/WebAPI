using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer.DTO
{
    public class PaginationDTO
    {
        [StringLength(50, ErrorMessage = "FilterValue must be at most 50 characters long.")]
        public string? FilterValue { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageNum must be greater than 0.")]
        public int PageNum { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
    }
}
