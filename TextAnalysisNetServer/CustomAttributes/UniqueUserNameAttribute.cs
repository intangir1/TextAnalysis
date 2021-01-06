using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TextAnalysis
{
	public class UniqueUserNameAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null || value.ToString() == "")
			{
				Debug.WriteLine("User name is empty!");
				return ValidationResult.Success;
			}

			string name = value.ToString();

			IUserRepository usersManager = new UserManager();

			if (usersManager.IsNameTaken(name))
			{
				Debug.WriteLine("User name " + name + " already taken!");
				return new ValidationResult("User name " + name + " already taken!");
			}

			Debug.WriteLine("User name " + name + " is ok!");
			return ValidationResult.Success;
		}
	}
}
