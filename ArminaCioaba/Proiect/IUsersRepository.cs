using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

public interface IUsersRepository
{
    Task<List<UserEntity>> GetAllUsers();

    Task<UserEntity> GetUser(string id);

    Task InsertNewUser(UserEntity user);

    Task EditUser(UserEntity user);

    Task DeleteUser(string id);
}