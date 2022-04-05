using Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Interfaces.CommonInterfaces
{
    public interface IModifyOperations<T> where T : class
    {
        Task<BasicResponse> Update(T obj);
        Task<BasicResponse> Delete(int Id);
    }
}
