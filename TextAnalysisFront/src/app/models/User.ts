export class User {
    public constructor(
        public userID?: string,
        public userFirstName?: string,
        public userLastName?: string,
        public userNickName?: string,

        public userPassword?: string,

        public userEmail?: string,
        public userGender?: string,
        public userBirthDate?: Date,
        public userPicture?: string,

        public userLevel?:number,
        public userRole?: string,

        public userImage?:string,
        
        public userRegistrationDate?: Date,
        public userLoginDate?: Date
    ) {}
}