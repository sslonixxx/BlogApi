public abstract class UserMapper
{
    public static User MapFromRegisterModelToEntity(UserRegisterModel userRegisterModel, string hashedPassword)
    {
        return new User
        {
            Name = userRegisterModel.Name,
            BirthDate = userRegisterModel.BirthDate.ToUniversalTime(),
            CreateTime = DateTime.Now.ToUniversalTime(),
            Email = userRegisterModel.Email,
            Gender = userRegisterModel.Gender,
            Id = Guid.NewGuid(),
            Password = hashedPassword,
            PhoneNumber = userRegisterModel.PhoneNumber,
        };
    }
}