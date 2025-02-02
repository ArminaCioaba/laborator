using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace users.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<UserEntity>> Get()
        {
            return await _usersRepository.GetAllUsers();
        }

        [HttpGet("{id}")]
        public async Task<UserEntity> GetUser([FromRoute] string id)
        {
            return await _usersRepository.GetUser(id);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] UserEntity user)
        {
            try
            {
                await _usersRepository.InsertNewUser(user);

                return "S-a adaugat cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }
        }

        [HttpDelete("{id}")]
        public async Task<string> Delete([FromRoute] string id)
        {
            try
            {
                await _usersRepository.DeleteUser(id);

                return "S-a sters cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }

        }

        [HttpPut]
        public async Task<string> Edit([FromBody] UserEntity user) {
            try
            {
                await _usersRepository.EditUser(user);

                return "S-a modificat cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }

        }
    }
}

