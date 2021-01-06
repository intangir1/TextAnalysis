using System;
using System.Runtime.Serialization;

namespace TextAnalysis
{
	public class UserForStatistics
	{
		private string _userID;
		private DateTime? _userRegistrationDate;
		private DateTime? _userLoginDate;

		public UserForStatistics()
		{

		}

		public UserForStatistics(string tmpUserID, DateTime? tmpUserRegistrationDate, DateTime? tmpUserLoginDate)
		{
			userID = tmpUserID;
			userRegistrationDate = tmpUserRegistrationDate;
			userLoginDate = tmpUserLoginDate;
		}

		[DataMember]
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

		public override string ToString()
		{
			return
				userID + " " +
				userRegistrationDate + " " +
				userLoginDate;
		}
	}
}
