using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models1;

namespace problems.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProblemsController : ControllerBase
    {
        private IProblemsRepository _problemsRepository;

        public ProblemsController(IProblemsRepository problemsRepository)
        {
            _problemsRepository = problemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ProblemEntity>> Get()
        {
            return await _problemsRepository.GetAllProblems();
        }

        [HttpGet("{id}")]
        public async Task<ProblemEntity> GetProblem([FromRoute] string id)
        {
            return await _problemsRepository.GetProblem(id);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] ProblemEntity problem)
        {
            try
            {
                await _problemsRepository.InsertNewProblem(problem);

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
                await _problemsRepository.DeleteProblem(id);

                return "S-a sters cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }

        }

        [HttpPut]
        public async Task<string> Edit([FromBody] ProblemEntity problem) {
            try
            {
                await _problemsRepository.EditProblem(problem);

                return "S-a modificat cu succes!";
            }
            catch (System.Exception e)
            {
                return "Eroare: " + e.Message;
            }

        }
    }
}

