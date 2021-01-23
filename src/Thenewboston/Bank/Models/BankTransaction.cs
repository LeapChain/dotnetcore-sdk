﻿using System.Numerics;
using Newtonsoft.Json;

namespace Thenewboston.Bank.Models
{
    public class BankTransaction
    {
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "block")]
        public BankBlock Block { get; set; }

        public string Sender { get; set; }

        public string Recipient { get; set; }

        public BigDecimal Amount { get; set; }
        
    }
}
