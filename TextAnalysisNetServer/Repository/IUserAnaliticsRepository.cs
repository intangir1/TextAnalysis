using System;
using System.Collections.Generic;

namespace TextAnalysis
{
	public interface IUserAnaliticsRepository
	{
		public void AddUserToLoginAnalitics(UserForStatistics userModel);
		public List<User> GetAllUserAnalitics();
		public List<User> GetUserAnaliticsByDates(DateTime startDate, DateTime endDate);
		public List<User> GetUserAnaliticsByStart(DateTime startDate);
		public List<User> GetUserAnaliticsByEnd(DateTime endDate);
	}
}
