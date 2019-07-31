using System;

namespace CodacyChallenge.Common.Models
{
    public class GitCommit
    {
        public string Commit { get; set; }
        public string Subject { get; set; }
        public string CommitNotes { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
    }

}
