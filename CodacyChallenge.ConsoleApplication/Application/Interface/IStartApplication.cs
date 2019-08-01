using System.Threading.Tasks;

namespace CodacyChallenge.Application
{
    public interface IStartApplication
    {
        Task Execute(string url);
    }
}
