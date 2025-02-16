export class AuthUser {
    constructor(
        public id: number,
        public email: string,
        public role: string,
        public accessToken: string,
    ) {}
}