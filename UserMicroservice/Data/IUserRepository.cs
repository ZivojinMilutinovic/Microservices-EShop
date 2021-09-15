using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Dtos;
using UserMicroservice.Models;

namespace UserMicroservice.Data
{
    public interface IUserRepository
    {
         Task<IEnumerable<GetUserDTO>> GetAllUsers();

         Task<User> GetUserDetailsById(int userId);

         Task<User> AddUser(CreateUserDTO userDTO);

         Task<bool> DeleteUser(int userId);

         Task<bool> UpdateUser(UpdateUserDTO userDTO);

         public Task<User> Authenticate(string username, string password);

         User GetById(int id);

         string getTokenStringForUser(User user);

         Task<bool> SaveChanges();
        
    }
}
