using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class User
	{
		private string _userID;
		private string _userFirstName;
		private string _userLastName;
		private string _userNickName;
		private string _userPassword;
		private string _userEmail;
		private string _userGender;
		private DateTime? _userBirthDate;
		private string _userPicture;
		private int _userLevel = 0;
		private string _userRole = "";
		private string _userImage;
		private string _userToken;
		private DateTime? _userRegistrationDate;
		private DateTime? _userLoginDate;

		[DataMember]
		[BsonId]
		[Required(ErrorMessage = "Missing user id.")]
		[PossibleID]
		public string userID
		{
			get
			{
				return _userID;
			}
			set
			{
				_userID = value;
			}
		}

		[DataMember]
		[Required(ErrorMessage = "Missing user first name.")]
		[StringLength(40, ErrorMessage = "User first name can't exceeds 40 chars.")]
		[MinLength(2, ErrorMessage = "User first name mast be minimum 2 chars.")]
		[RegularExpression("^[A-Z].*$", ErrorMessage = "User first name must start with a capital letter.")]
		public string userFirstName
		{
			get
			{
				return _userFirstName;
			}
			set
			{
				_userFirstName = value;
			}
		}

		[DataMember]
		[Required(ErrorMessage = "Missing user last name.")]
		[StringLength(40, ErrorMessage = "User last name can't exceeds 40 chars.")]
		[MinLength(2, ErrorMessage = "User first last mast be minimum 2 chars.")]
		[RegularExpression("^[A-Z].*$", ErrorMessage = "User last name must start with a capital letter.")]
		public string userLastName
		{
			get
			{
				return _userLastName;
			}
			set
			{
				_userLastName = value;
			}
		}

		[DataMember]
		[Required(ErrorMessage = "Missing user nick name.")]
		[StringLength(40, ErrorMessage = "User nick name can't exceeds 40 chars.")]
		[MinLength(2, ErrorMessage = "User first nick mast be minimum 2 chars.")]
		//[UniqueUserName]
		public string userNickName
		{
			get
			{
				return _userNickName;
			}
			set
			{
				_userNickName = value;
			}
		}

		[DataMember]
		[Required(ErrorMessage = "Missing user password.")]
		public string userPassword
		{
			get
			{
				return _userPassword;
			}
			set
			{
				_userPassword = value;
			}
		}

		[DataMember]
		const string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
		[Required(ErrorMessage = "Missing user email.")]
		//[RegularExpression(pattern, ErrorMessage = "User mail wrong.")]
		public string userEmail
		{
			get
			{
				return _userEmail;
			}
			set
			{
				_userEmail = value;
			}
		}

		[DataMember]
		[Required(ErrorMessage = "Missing user gender.")]
		[PossibleGender]
		public string userGender
		{
			get
			{
				return _userGender;
			}
			set
			{
				_userGender = value;
			}
		}

		[DataMember]
		public DateTime? userBirthDate
		{
			get { return _userBirthDate; }
			set { _userBirthDate = value; }
		}

		[DataMember]
		public string userPicture
		{
			get { return _userPicture; }
			set { _userPicture = value; }
		}

		[DataMember]
		public int userLevel
		{
			get { return _userLevel; }
			set { _userLevel = value;

				switch (_userLevel)
				{
					case 0:
						_userRole = "guest";
						break;
					case 1:
						_userRole = "registrated";
						break;
					case 2:
						_userRole = "vip";
						break;
					case 4:
						_userRole = "admin";
						break;
					default:
						break;
				}
			}
		}


		[DataMember]
		public string userRole
		{
			get { return _userRole; }
			set { _userRole = value;

				switch (_userRole)
				{
					case "guest":
						_userLevel = 0;
						break;
					case "registrated":
						_userLevel = 1;
						break;
					case "vip":
						_userLevel = 2;
						break;
					case "admin":
						_userLevel = 4;
						break;
					default:
						break;
				}
			}
		}

		[DataMember]
		public string Role {
			get { return _userRole; }
			set
			{
				_userRole = value;
				switch (_userRole)
				{
					case "guest":
						_userLevel = 0;
						break;
					case "registrated":
						_userLevel = 1;
						break;
					case "vip":
						_userLevel = 2;
						break;
					case "admin":
						_userLevel = 4;
						break;
					default:
						break;
				}
			}
		}

		[DataMember]
		public string userImage
		{
			get { return _userImage; }
			set { _userImage = value; }
		}

		[DataMember]
		public string userToken
		{
			get { return _userToken; }
			set { _userToken = value; }
		}

		[DataMember]
		public DateTime? userRegistrationDate
		{
			get { return _userRegistrationDate; }
			set { _userRegistrationDate = value; }
		}

		[DataMember]
		public DateTime? userLoginDate
		{
			get { return _userLoginDate; }
			set { _userLoginDate = value; }
		}

		[JsonIgnore]
		public List<RefreshToken> RefreshTokens { get; set; }

		public override string ToString()
		{
			return
				userID + " " +
				userFirstName + " " +
				userLastName + " " +
				userNickName + " " +
				userBirthDate + " " +
				userGender + " " +
				userEmail + " " +
				userLevel + " " +
				userRole + " " +
				userPicture + " " +
				userToken + " " +
				userRegistrationDate + " " +
				userLoginDate;
		}
	}
}
