using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class Login
	{
		private string _userNickName;
		private string _userPassword;
		private int? _userLevel;
		private string _userRole;
		private string _userPicture;

		[DataMember]
		[Required(ErrorMessage = "Missing user nick name.")]
		[StringLength(40, ErrorMessage = "User nick name can't exceeds 40 chars.")]
		[MinLength(2, ErrorMessage = "User first nick mast be minimum 2 chars.")]
		public string userNickName
		{
			get { return _userNickName; }
			set { _userNickName = value; }
		}

		[DataMember]
		[Required(ErrorMessage = "Missing user password.")]
		public string userPassword
		{
			get { return _userPassword; }
			set { _userPassword = value; }
		}

		[DataMember]
		public int? userLevel
		{
			get
			{
				return _userLevel;
			}
			set
			{
				_userLevel = value;
				switch (value)
				{
					case 0:
						_userRole = "Guest";
						break;
					case 1:
						_userRole = "User";
						break;
					case 2:
						_userRole = "Manager";
						break;
					default:
						_userRole = "Admin";
						break;
				}
			}
		}

		public string Role
		{
			get
			{
				return _userRole;
			}
			set
			{
				_userRole = value;
				switch (value)
				{
					case "Guest":
						_userLevel = 0;
						break;
					case "User":
						_userLevel = 1;
						break;
					case "Manager":
						_userLevel = 2;
						break;
					default:
						_userLevel = 3;
						break;
				}

			}
		}

		[DataMember]
		public string userPicture
		{
			get { return _userPicture; }
			set { _userPicture = value; }
		}

		public override string ToString()
		{
			return
				userNickName + " " +
				userPassword + " " +
				userLevel + " " +
				userNickName + " " +
				userPicture;
		}
	}
}
