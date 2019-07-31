namespace CodacyChallenge.Common.Models
{
    /* Just to remember
    * '%H': commit hash
    * '%s': subject
    * '%N': commit notes
    * '%aN': author name 
    * '%aD': author date
    */

    public static class GitCommand
    {
        public static string Clone = "git clone";
        public static string Log = "git log";
        public static string PrettyFormat(string format) { return $"--pretty=format:'{format}'"; }
    }
}
