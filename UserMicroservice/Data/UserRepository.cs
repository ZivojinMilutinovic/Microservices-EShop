using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserMicroservice.Dtos;
using UserMicroservice.Helpers;
using UserMicroservice.Models;

namespace UserMicroservice.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        public UserRepository(AppDbContext context, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        public async Task<User> AddUser(CreateUserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Password))
                throw new AppException("Passwor is required");

            if (await _context.Users.AnyAsync(u => u.Username == userDTO.Username))
                throw new AppException("Username \"" + userDTO.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(userDTO.Password, out passwordHash, out passwordSalt);

            var _object = _mapper.Map<User>(userDTO);

            _object.PasswordHash = passwordHash;
            _object.PasswordSalt = passwordSalt;
            _object.InsertDate = DateTime.Now;

            var user = await _context.Users.AddAsync(_object);
            await SaveChanges();
            return user.Entity;
        }

        public async Task<User> Authenticate(string username, string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == userId);
                user.DeleteDate = DateTime.Now;
                _context.Update(user);
                return await SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllUsers()
        {
            var users= await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<GetUserDTO>>(users);
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public string getTokenStringForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUserDetailsById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserID == userId);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> UpdateUser(UpdateUserDTO userDTO)
        {
            try
            {
                var _object = _mapper.Map<User>(userDTO);
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == _object.UserID);
                user.FirstName = _object.FirstName;
                user.LastName = _object.LastName;
                user.Address = _object.Address;
                user.Username = _object.Username;
                user.Email = _object.Email;
                if (!string.IsNullOrWhiteSpace(userDTO.Password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(userDTO.Password, out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }
                _context.Update(user);
                return await SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}
