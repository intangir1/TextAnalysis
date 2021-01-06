export class Login {
    public constructor(
        public userNickName: string="",
        public userPassword: string="",
        public userLevel: number = 0,
        public userPicture: string="/assets/images/no-image.png"
    ) {}
}