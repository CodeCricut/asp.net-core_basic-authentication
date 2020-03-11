using basic_authentication_api.Domain.Entities;
using basic_authentication_api.Domain.HelperModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace basic_authentication_api.Domain.Repositories
{
	public interface IUserRepository
	{
		User RegisterUser(RegisterModel registerModel);
		User Login(LoginModel loginModel);
		User GetById(int id);
		IEnumerable<User> GetUsers();
		IEnumerable<User> GetUsers(Func<User, bool> condition);
	}
}
