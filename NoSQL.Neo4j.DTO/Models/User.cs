using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL.Neo4j.DTO.Models
{
    public class User
    {

        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }
    }
}
