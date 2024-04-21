using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuickKartServices.Models
{
    public class Categories
    {
       
        public byte CategoryId { get; set; }
        
        public string CategoryName { get; set; }
    }

}
