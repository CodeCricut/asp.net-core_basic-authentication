using System;
using System.Collections.Generic;
using System.Text;

namespace basic_authentication_api.Domain.HelperModels
{
	public class RegisterModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
