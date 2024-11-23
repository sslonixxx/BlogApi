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

    public static UserDto MapFromEntityToDto(User user)
    {
        return new UserDto()
        {
            Name = user.Name,
            BirthDate = user.BirthDate,
            CreateTime = user.CreateTime,
            Email = user.Email,
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber,
            Id = user.Id,
        };
    }

    public static User MapFromEditModelToEntity(UserEditModel userEditModel, User user)
    {
        {
            user.Name = userEditModel.Name;
            user.BirthDate = userEditModel.BirthDate;
            user.Email = userEditModel.Email;
            user.Gender = userEditModel.Gender;
            user.PhoneNumber = userEditModel.PhoneNumber;
        };
        return user;
    }
}