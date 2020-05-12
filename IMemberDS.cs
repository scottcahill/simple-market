
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmWeb.Models;

namespace SmWeb.Services
{
    public interface IMemberDS
    {
        Task<IEnumerable<Member>> GetAllMembers();
        Task<Member> GetMemberById(string memberId);
        Task<bool> AddMember(Member Member);
        Task UpdateMember(Member Member);
        Task DeleteMember(string memberId);
    }
}
