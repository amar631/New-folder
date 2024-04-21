﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace QuickKartDataAccessLayer.Models
{
    public partial class Products
    {
        public Products()
        {
            PurchaseDetails = new HashSet<PurchaseDetails>();
        }
        
        public string ProductId { get; set; }
       
        public string ProductName { get; set; }
        
        public byte? CategoryId { get; set; }
        
        public decimal Price { get; set; }
       
        public int QuantityAvailable { get; set; }

        [JsonIgnore]
        public Categories Category { get; set; }

        [JsonIgnore]
        public ICollection<PurchaseDetails> PurchaseDetails { get; set; }
    }

}
