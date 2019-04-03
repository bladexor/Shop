using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(50,ErrorMessage ="The field {0} only can contain {1} characters length.")]
        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString ="{0:C2}",ApplyFormatInEditMode =false)]
        public decimal Price { get; set; }

        [Display(Name="Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; } //Al colocarle el '?' Permite almacenar valor nulo

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }  //Al colocarle el '?' Permite almacenar valor nulo

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }
    }
}
