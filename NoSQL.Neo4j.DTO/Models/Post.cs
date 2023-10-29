﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL.Neo4j.DTO.Models
{
    public class Post
    {
        [JsonProperty(PropertyName = "ownerid")]
        public string OwnerId { get; set; }

        [JsonProperty(PropertyName = "postid")]
        public string PostId { get; set; }
    }
}
