export class UserDto{
  constructor(
    public id: number,
    public name: string,
    public email: string,
    public mobilePhone: string,
    public role: string
  ){}
  }