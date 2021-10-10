using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Repo;

namespace Tema2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        public StudentsController()
        {
            
        }

        [HttpGet]
        public IEnumerable<Student> Get()
        {
          return StudentsRepo.Students;
        }

        [HttpGet("{id}")]
        public Student Get(int id)
        {
          return StudentsRepo.Students.FirstOrDefault(s=>s.ID==id);
        }

        [HttpPost]
        public void Post([FromBody]Student student) 
        {
           StudentsRepo.Students.Add(student);       
            
        }

        [HttpDelete]
        public void Delete(int id)
        {
          StudentsRepo.Students.Remove(StudentsRepo.Students.FirstOrDefault(s=>s.ID==id));
        }
        
        [HttpPut]
        public void Put(int id, [FromBody]Student student)
        {
          var here=StudentsRepo.Students.FirstOrDefault(s=>s.ID==id);
          here.Nume=student.Nume;
          here.Prenume=student.Prenume;
          here.Facultate=student.Facultate;
          here.Sectie=student.Sectie;
          here.An_studiu=student.An_studiu;
          here.Bursa=student.Bursa;
          here.Taxa=student.Taxa;
        }
    }
}
