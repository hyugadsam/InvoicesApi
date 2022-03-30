using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface IValidateUniqueText
    {
        Task<bool> CheckUniqueEntityText(string text);
    }
}
