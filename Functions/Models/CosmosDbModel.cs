using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Functions.Models
{
    public class CosmosDbModel : TwinModel
    {
        [Key]
        [JsonProperty(propertyName: "id")]
        public string id => deviceId;
    }
}
