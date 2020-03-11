using System;
using System.Collections.Generic;
using System.Text;

namespace basic_authentication_api.Domain.Helpers
{
	public static class Encryption
	{
		public static string Sha256(this string plainStr)
		{
			var crypt = new System.Security.Cryptography.SHA256Managed();
			var hash = new StringBuilder();
			byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(plainStr));
			foreach (byte theByte in crypto)
			{
				hash.Append(theByte.ToString("x2"));
			}
			return hash.ToString();
		}
	}
}
