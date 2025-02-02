using System.Collections.Generic;
using System.Threading.Tasks;
using Models1;

public interface IProblemsRepository
{
    Task<List<ProblemEntity>> GetAllProblems();

    Task<ProblemEntity> GetProblem(string id);

    Task InsertNewProblem(ProblemEntity problem);

    Task EditProblem(ProblemEntity problem);

    Task DeleteProblem(string id);
}