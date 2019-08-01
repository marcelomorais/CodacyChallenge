using System;
using System.Collections.Generic;

namespace CodacyChallenge.Common.Models
{
    public class Verification
    {
        public bool Verified { get; set; }
        public string Reason { get; set; }
        public object Signature { get; set; }
        public object Payload { get; set; }
    }
}
