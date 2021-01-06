using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace TextAnalysis
{
	public class UserManager : IUserRepository
	{
		private readonly IMongoCollection<User> users;

		public UserManager()
		{
			var client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
			users = database.GetCollection<User>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:UsersCollectionName"));
		}

		public User Authenticate(string userNickName, string userPassword)
		{
			var loggedUser = GetOneUserByLogin(userNickName, userPassword);

			if (loggedUser == null || loggedUser.userID == null || loggedUser.userID.Equals(""))
				return null;

			var jwtToken = GenerateJwtToken(loggedUser);
			//var refreshToken = GenerateRefreshToken(ipAddress);

			User userModel = new User();
			userModel.userToken = jwtToken;
			userModel.userID = loggedUser.userID;
			userModel.userNickName = loggedUser.userNickName;
			userModel.userPicture = loggedUser.userPicture != null ? loggedUser.userPicture : null;
			userModel.userLevel = loggedUser.userLevel;
			return userModel;
		}

		public List<User> GetAllUsers()
		{
			return users.Find(_user => true).Project(u => new User
			{
				userID = u.userID,
				userFirstName = u.userFirstName,
				userLastName = u.userLastName,
				userNickName = u.userNickName,
				userPassword = u.userPassword,
				userEmail = u.userEmail,
				userGender = u.userGender,
				userBirthDate = u.userBirthDate,
				userPicture = u.userPicture != null ? "/assets/images/users/" + u.userPicture : null,
				userLevel = u.userLevel,
				userRegistrationDate = u.userRegistrationDate
			}).ToList();
		}

		public User GetOneUserById(string id)
		{
			return users.Find(Builders<User>.Filter.Eq(_user => _user.userID, id)).Project(u => new User
			{
				userID = u.userID,
				userFirstName = u.userFirstName,
				userLastName = u.userLastName,
				userNickName = u.userNickName,
				userPassword = u.userPassword,
				userEmail = u.userEmail,
				userGender = u.userGender,
				userBirthDate = u.userBirthDate,
				userPicture = u.userPicture != null ? "/assets/images/users/" + u.userPicture : null,
				userLevel = u.userLevel,
				userRegistrationDate = u.userRegistrationDate
			}).SingleOrDefault();
		}

		public User GetOneUserForMessageById(string id)
		{
			return users.Find(Builders<User>.Filter.Eq(_user => _user.userID, id)).Project(u => new User
			{
				userFirstName = u.userFirstName,
				userLastName = u.userLastName,
				userEmail = u.userEmail
			}).SingleOrDefault();
		}

		public User GetOneUserByName(string name)
		{
			return users.Find(Builders<User>.Filter.Eq(_user => _user.userNickName, name)).Project(u => new User
			{
				userID = u.userID,
				userFirstName = u.userFirstName,
				userLastName = u.userLastName,
				userNickName = u.userNickName,
				userPassword = u.userPassword,
				userEmail = u.userEmail,
				userGender = u.userGender,
				userBirthDate = u.userBirthDate,
				userPicture = u.userPicture != null ? "/assets/images/users/" + u.userPicture : null,
				userLevel = u.userLevel,
				userRegistrationDate = u.userRegistrationDate
			}).SingleOrDefault();
		}

		public User GetOneUserByLogin(string userNickName, string userPassword)
		{
			if (!CheckStringFormat.IsBase64String(userPassword))
			{
				userPassword = ComputeHash.ComputeNewHash(userPassword);
			}

			User userToReturn = null;
			var exists = users.AsQueryable().Any(user => user.userNickName.Equals(userNickName) && user.userPassword.Equals(userPassword));
			if (exists)
			{
				userToReturn = (from user in users.AsQueryable()
						where user.userNickName.Equals(userNickName)
						where user.userPassword.Equals(userPassword)
						select new User
						{
							userID = user.userID,
							userFirstName = user.userFirstName,
							userLastName = user.userLastName,
							userNickName = user.userNickName,
							userPassword = user.userPassword,
							userEmail = user.userEmail,
							userGender = user.userGender,
							userBirthDate = user.userBirthDate,
							userPicture = user.userPicture != null ? "/assets/images/users/" + user.userPicture : null,
							userLevel = user.userLevel,
							userRegistrationDate = user.userRegistrationDate
						}).SingleOrDefault();
			}

			return userToReturn;
		}

		public User AddUser(User userModel)
		{
			if (userModel.userLevel < 1)
			{
				userModel.userLevel = 1;
			}

			string userPassword = userModel.userPassword;
			userModel.userPassword = ComputeHash.ComputeNewHash(userModel.userPassword);
			userModel.userRegistrationDate = DateTime.Now;
			users.InsertOne(userModel);
			User user = GetOneUserByLogin(userModel.userNickName, userModel.userPassword);

			if (ComputeHash.ComputeNewHash(userPassword).Equals(user.userPassword))
			{
				user.userPassword = userPassword;
			}

			return user;
		}

		public User UpdateUser(User userModel)
		{
			users.ReplaceOne(user => user.userID.Equals(userModel.userID), userModel);
			User user = GetOneUserByLogin(userModel.userNickName, userModel.userPassword);

			if (ComputeHash.ComputeNewHash(userModel.userPassword).Equals(user.userPassword))
			{
				user.userPassword = userModel.userPassword;
			}
			return user;
		}

		public int DeleteUser(string id, string rootPath)
		{
			User user = GetOneUserById(id);
			if (user!=null)
			{
				string userPicture = user.userPicture;
				if(userPicture!=null && !userPicture.Equals(String.Empty))
				{
					try
					{
						// Check if file exists with its full path    
						if (File.Exists(Path.Combine(rootPath, userPicture)))
						{
							// If file found, delete it    
							File.Delete(Path.Combine(rootPath, userPicture));
							Console.WriteLine("File deleted.");
						}
						else Console.WriteLine("File not found");
					}
					catch (IOException ioExp)
					{
						Console.WriteLine(ioExp.Message);
					}
				}
			}
			int deleted = (int)users.DeleteOne(user => user.userID.Equals(id)).DeletedCount;
			new UserAnaliticsManager().DeleteUserAnalitics(id);
			return deleted;
		}

		public int DeleteUsers(string rootPath)
		{
			int deleted = 0;
			try
			{
				string[] files = Directory.GetFiles(rootPath);
				foreach (string file in files)
				{
					File.Delete(file);
					Console.WriteLine($"{file} is deleted.");
				}
				deleted = (int)users.DeleteMany(user => true).DeletedCount;
				new UserAnaliticsManager().DeleteUsersAnalitics();
			}
			catch (IOException ioExp)
			{
				Console.WriteLine(ioExp.Message);
			}
			return deleted;
		}

		public User UploadUserImage(string id, string img)
		{
			User tmpUserModel = GetOneUserById(id);
			tmpUserModel.userImage = img;

			users.ReplaceOne(user => user.userID.Equals(id), tmpUserModel);
			tmpUserModel = GetOneUserById(id);

			return tmpUserModel;
		}

		public Login ReturnUserByNameLevel(string username, int userLevel = 0)
		{
			return (from user in users.AsQueryable()
					where user.userNickName.Equals(username)
					where user.userLevel.Equals(userLevel)
					select new Login
					{
						userNickName = user.userNickName,
						userLevel = user.userLevel
					}).SingleOrDefault();
		}

		public bool IsNameTaken(string name)
		{
			if (name.Equals(string.Empty) || name.Equals(""))
				throw new ArgumentOutOfRangeException();

			return users.Find<User>(user => user.userNickName.Equals(name)).Any();
		}

		public Login ReturnUserByNamePassword(Login checkUser)
		{
			checkUser.userPassword = ComputeHash.ComputeNewHash(checkUser.userPassword);

			return (from user in users.AsQueryable()
					where user.userNickName.Equals(checkUser.userNickName)
					where user.userPassword.Equals(checkUser.userPassword)
					select new Login
					{
						userNickName = user.userNickName,
						userLevel = user.userLevel,
						userPicture = user.userPicture != null ? "/assets/images/users/" + user.userPicture : null
					}).SingleOrDefault();
		}

		private string GenerateJwtToken(User loggedUser)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(Startup.staticConfiguration.GetValue<string>("AppSettings:Secret"));
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, loggedUser.userID),
					new Claim(ClaimTypes.Name, loggedUser.userNickName),
					new Claim(ClaimTypes.Role, loggedUser.userRole)
				}),
				Expires = DateTime.UtcNow.AddMinutes(30),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}













		

		//private RefreshToken GenerateRefreshToken(string ipAddress)
		//{
		//	using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
		//	{
		//		var randomBytes = new byte[64];
		//		rngCryptoServiceProvider.GetBytes(randomBytes);
		//		return new RefreshToken
		//		{
		//			Token = Convert.ToBase64String(randomBytes),
		//			Expires = DateTime.UtcNow.AddDays(7),
		//			Created = DateTime.UtcNow,
		//			CreatedByIp = ipAddress
		//		};
		//	}
		//}



		//public User RefreshToken(string token, string ipAddress)
		//{
		//	var user = users.Find<User>(user => user.RefreshTokens.Any(t => t.Token == token)).SingleOrDefault();

		//	// return null if no user found with token
		//	if (user == null) return null;

		//	var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

		//	// return null if token is no longer active
		//	if (!refreshToken.IsActive) return null;

		//	// replace old refresh token with a new one and save
		//	var newRefreshToken = GenerateRefreshToken(ipAddress);
		//	refreshToken.Revoked = DateTime.UtcNow;
		//	refreshToken.RevokedByIp = ipAddress;
		//	refreshToken.ReplacedByToken = newRefreshToken.Token;
		//	user.RefreshTokens.Add(newRefreshToken);

		//	// generate new jwt
		//	user.userToken = GenerateJwtToken(user);

		//	return new AuthenticateResponse(user, newRefreshToken.Token);
		//}

		//public bool RevokeToken(string token, string ipAddress)
		//{
		//	var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

		//	// return false if no user found with token
		//	if (user == null) return false;

		//	var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

		//	// return false if token is not active
		//	if (!refreshToken.IsActive) return false;

		//	// revoke token and save
		//	refreshToken.Revoked = DateTime.UtcNow;
		//	refreshToken.RevokedByIp = ipAddress;
		//	_context.Update(user);
		//	_context.SaveChanges();

		//	return true;
		//}
	}
}