using System.Threading.Tasks;

namespace DBService.Interfaces.CommonInterfaces
{
    public interface IValidateUniqueText
    {
        Task<bool> CheckUniqueEntityText(string text);
    }
}
