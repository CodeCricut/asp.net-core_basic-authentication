using basic_authentication_api.Domain.Entities;
using basic_authentication_api.Domain.HelperModels;
using basic_authentication_api.Domain.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using basic_authentication_api.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace basic_authentication_api.Domain.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;
		private readonly AppSettings _appSettings;

		public UserRepository(AppDbContext context, IOptions<AppSettings> appSettings)
		{
			_context = context;
			_appSettings = appSettings.Value;
		}

		public User GetById(int id)
		{
			return _context.Users.Select(SelectPublicFields()).FirstOrDefault(u => u.Id == id);
		}

		public IEnumerable<User> GetUsers()
		{
			return _context.Users.Select(
				SelectPublicFields())
				.AsEnumerable();
		}

		public IEnumerable<User> GetUsers(Func<User, bool> condition)
		{
			return GetUsers().Where(condition);
		}

		public User Login(LoginModel loginModel)
		{
			var user = _context.Users.FirstOrDefault(u =>
				u.Username == loginModel.Username &&
				u.Password == loginModel.Password.Sha256()
				);
			return AuthenticateUser(user);
		}

		public User RegisterUser(RegisterModel registerModel)
		{
			var user = new User
			{
				FirstName = registerModel.FirstName,
				LastName = registerModel.LastName,
				Password = registerModel.Password.Sha256(),
				Username = registerModel.Username
			};

			_context.Users.Add(user);
			_context.SaveChanges();

			return AuthenticateUser(user);
		}

		private User AuthenticateUser(User user)
		{
			// authentication successful so generate jwt token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.FirstName),
					new Claim("LastName", user.LastName)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);

			// remove password before returning
			user.Password = null;

			return user;
		}

		private Func<User, User> SelectPublicFields()
		{
			return u => new User() { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, Username = u.Username };
		}
			 
	}
	
}
