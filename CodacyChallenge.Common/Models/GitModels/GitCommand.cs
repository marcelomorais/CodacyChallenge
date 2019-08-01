namespace CodacyChallenge.Common.Models
{
    /* Just to remember
     * a = author
     * c = committer
    * '%H': commit hash
    * '%s': subject
    * '%N': commit notes
    * '%aN': author name 
    * '%aD': author date
    * '%cN': committer name 
    * '%cD': committer date
    */

    public static class GitCommand
    {

        //I need to put the  so that I can replace it to double quotes after the PowerShell return my object because the it's scaping the double quotes... 
        private static string _format { get { return "{\"^^Sha^^\":\"^^%H^^\", \"^^Commit^^\":{\"^^Subject^^\":\"^^%s^^\", \"^^Author^^\":{\"^^Name^^\":\"^^%an^^\",\"^^Email^^\":\"^^%ae^^\",\"^^Date^^\":\"^^%aD^^\"}, \"^^Committer^^\":{\"^^Name^^\":\"^^%cn^^\",\"^^Email^^\":\"^^%ce^^\",\"^^Date^^\":\"^^%cD^^\"}}}"; } }

        public static string Clone = "git clone";
        public static string Log = "git log";
        public static string PrettyFormat { get { return $"--pretty=format:'{_format}'"; } }
    }
}
