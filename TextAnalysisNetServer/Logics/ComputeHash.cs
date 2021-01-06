using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace TextAnalysis
{
	public static class ComputeHash
	{
		public static string ComputeNewHash(string password)
		{
			var dataAsBytes = Encoding.UTF8.GetBytes(password);
			using (var hasher = new HMACSHA256(dataAsBytes))
			{
				Debug.WriteLine("ComputeNewHash from: " + password);
				string hashedPassword = Convert.ToBase64String(hasher.ComputeHash(dataAsBytes));
				Debug.WriteLine("HashedPassword: " + hashedPassword);
				return hashedPassword;
			}
		}
	}
}
