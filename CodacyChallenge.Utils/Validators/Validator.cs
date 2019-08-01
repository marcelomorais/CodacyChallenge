using CodacyChallenge.Common.Enumerators;

namespace CodacyChallenge.Utils.Validators
{
    public static class Validator
    {
        public static bool ValidateUrl(RequestType requestType, string url)
        {
            if (!url.Contains("GitHub", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (requestType == RequestType.Shell && !url.Contains(".git", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
