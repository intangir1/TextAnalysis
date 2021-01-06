﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TextAnalysis
{
	public class PossibleGenderAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			bool possibleGender = false;
			string[] genders = { "Male", "Female", "Other" };


			if (value == null || value.ToString() == "")
				return ValidationResult.Success;

			string gender = value.ToString().ToLower();

			for (int i = 0; i < genders.Length; i++)
			{
				if (gender.Equals(genders[i].ToLower()))
				{
					possibleGender = true;
					Debug.WriteLine("The gender " + gender + " is possible");
				}
			}
			if (possibleGender == false)
			{
				Debug.WriteLine("The gender " + gender + " is impossible");
				return new ValidationResult("The gender " + gender + " is impossible"); // Will cause a validation error.
			}

			return ValidationResult.Success;
		}
	}
}
