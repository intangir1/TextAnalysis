using System.Collections.Generic;

namespace TextAnalysis
{
	public interface IUserRepository
	{
		User Authenticate(string username, string password);
		List<User> GetAllUsers();
		User GetOneUserById(string id);
		User GetOneUserByName(string name);
		User GetOneUserByLogin(string name, string password);
		User AddUser(User userModel);
		User UpdateUser(User userModel);
		int DeleteUser(string id, string rootPath);
		int DeleteUsers(string rootPath);
		Login ReturnUserByNamePassword(Login checkUser);
		Login ReturnUserByNameLevel(string username, int userLevel = 0);
		bool IsNameTaken(string name);
		User UploadUserImage(string id, string img);
		User GetOneUserForMessageById(string id);
	}
}
