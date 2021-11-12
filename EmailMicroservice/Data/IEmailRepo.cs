using EmailMicroservice.Models;
using System.Threading.Tasks;

namespace EmailMicroservice.Data
{
    public interface IEmailRepo
    {
        Task<EmailUser> AddEmailUser(EmailUser emailUser);
        Task<bool> SaveChanges();
    }
}