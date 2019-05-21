using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RecoverPasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }
}

