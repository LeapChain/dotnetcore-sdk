﻿using Newtonsoft.Json;

namespace Thenewboston.Bank.Models
{
    public class UpgradeNotice
    {
        [JsonProperty(PropertyName ="message")]
        public UpgradeNoticeMessage Message { get; set; }

        [JsonProperty(PropertyName = "node_identifier")]
        public string NodeIdentifier { get; set; }

        [JsonProperty(PropertyName ="signature")]
        public string Signature { get; set; }
    }
}
