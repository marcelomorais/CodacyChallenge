using System;
using System.Collections.Generic;

namespace CodacyChallenge.Common.Models
{
    public class GitResponse
    {
        public string Sha { get; set; }
        public string Node_id { get; set; }
        public Commit Commit { get; set; }
        public string Url { get; set; }
        public string Html_url { get; set; }
        public string Comments_url { get; set; }
        public List<Parent> Parents { get; set; }
    }
}
