using DAL.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public interface IRequestMemberService : IDisposable
    {
        void Register(RequestMember value);
        void SetMemberStatus(RequestMember value, string password);
        List<RequestMember> Get();
        List<Hospital> GetHospital();
        string Get(string email);
        List<RequestMember> GetMember();
        void Delete(string email);
    }
}
