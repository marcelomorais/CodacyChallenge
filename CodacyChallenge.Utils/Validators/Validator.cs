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

            if (url.Split('/').Length != 5)
            {
                return false;
            }

            return true;
        }
    }
}
