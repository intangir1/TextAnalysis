using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
	public class UserAnaliticsManager : IUserAnaliticsRepository
	{
		private readonly IMongoCollection<UserForStatistics> usersAnalitics;
		private readonly IMongoCollection<User> users;

		public UserAnaliticsManager()
		{
			var client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
			usersAnalitics = database.GetCollection<UserForStatistics>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:UserAnaliticsCollectionName"));
			users = database.GetCollection<User>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:UsersCollectionName"));
		}

		public void AddUserToLoginAnalitics(UserForStatistics userModel)
		{
			usersAnalitics.InsertOne(userModel);
		}

		public List<User> GetAllUserAnalitics()
		{
			List<User> userForStatistics = (from userAnalitics in usersAnalitics.AsQueryable()
							   join user in users.AsQueryable() on userAnalitics.userID equals user.userID
							   select new User
							   {
								   userID = userAnalitics.userID,
								   userFirstName = user.userFirstName,
								   userLastName = user.userLastName,
								   userNickName = user.userNickName,
								   userEmail = user.userEmail,
								   userGender = user.userGender,
								   userBirthDate = user.userBirthDate,
								   userPicture = user.userPicture != null ? "/assets/images/users/" + user.userPicture : null,
								   userLevel = user.userLevel,
								   userRole = user.userRole,
								   userRegistrationDate = user.userRegistrationDate,
								   userLoginDate = userAnalitics.userLoginDate
							   }).ToList();
			return userForStatistics;
		}

		public List<User> GetUserAnaliticsByDates(DateTime startDate, DateTime endDate)
		{
			List<User> userForStatistics = (from userAnalitics in usersAnalitics.AsQueryable()
											join user in users.AsQueryable() on userAnalitics.userID equals user.userID
											where (userAnalitics.userLoginDate >= startDate && userAnalitics.userLoginDate <= endDate)
											select new User
											{
												userID = userAnalitics.userID,
												userFirstName = user.userFirstName,
												userLastName = user.userLastName,
												userNickName = user.userNickName,
												userEmail = user.userEmail,
												userGender = user.userGender,
												userBirthDate = user.userBirthDate,
												userPicture = user.userPicture != null ? "/assets/images/users/" + user.userPicture : null,
												userLevel = user.userLevel,
												userRole = user.userRole,
												userRegistrationDate = user.userRegistrationDate,
												userLoginDate = userAnalitics.userLoginDate
											}).ToList();
			return userForStatistics;
		}

		public List<User> GetUserAnaliticsByStart(DateTime startDate)
		{
			List<User> userForStatistics = (from userAnalitics in usersAnalitics.AsQueryable()
											join user in users.AsQueryable() on userAnalitics.userID equals user.userID
											where (userAnalitics.userLoginDate >= startDate)
											select new User
											{
												userID = userAnalitics.userID,
												userFirstName = user.userFirstName,
												userLastName = user.userLastName,
												userNickName = user.userNickName,
												userEmail = user.userEmail,
												userGender = user.userGender,
												userBirthDate = user.userBirthDate,
												userPicture = user.userPicture != null ? "/assets/images/users/" + user.userPicture : null,
												userLevel = user.userLevel,
												userRole = user.userRole,
												userRegistrationDate = user.userRegistrationDate,
												userLoginDate = userAnalitics.userLoginDate
											}).ToList();
			return userForStatistics;
		}

		public List<User> GetUserAnaliticsByEnd(DateTime endDate)
		{
			List<User> userForStatistics = (from userAnalitics in usersAnalitics.AsQueryable()
											join user in users.AsQueryable() on userAnalitics.userID equals user.userID
											where (userAnalitics.userLoginDate <= endDate)
											select new User
											{
												userID = userAnalitics.userID,
												userFirstName = user.userFirstName,
												userLastName = user.userLastName,
												userNickName = user.userNickName,
												userEmail = user.userEmail,
												userGender = user.userGender,
												userBirthDate = user.userBirthDate,
												userPicture = user.userPicture != null ? "/assets/images/users/" + user.userPicture : null,
												userLevel = user.userLevel,
												userRole = user.userRole,
												userRegistrationDate = user.userRegistrationDate,
												userLoginDate = userAnalitics.userLoginDate
											}).ToList();
			return userForStatistics;
		}

		public int DeleteUserAnalitics(string id)
		{
			int deleted = (int)usersAnalitics.DeleteOne(user => user.userID.Equals(id)).DeletedCount;
			return deleted;
		}

		public int DeleteUsersAnalitics()
		{
			int deleted = (int)usersAnalitics.DeleteMany(user => true).DeletedCount;
			return deleted;
		}
	}
}
