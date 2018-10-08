using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using StuApiWithDatabaseDeeDee.Models;
using StuApiWithDatabaseDeeDee.Repositories;
using System.Linq;
namespace StuApiWithDatabaseDeeDee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StuController : ControllerBase
    {
        private readonly IStuRepository _stuRepository;

        public StuController(IStuRepository stuRepository)
        {
            _stuRepository = stuRepository;
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAll()
        {
            return _stuRepository.GetAllStudents();
        }

        [HttpGet("{id}", Name = "GetStudent")]
        public ActionResult<Student> GetById(string id)
        {
            var student = _stuRepository.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpGet]
        [Route("range")]
        public ActionResult<float[]> GetRange() 
        {
            return _stuRepository.GetRange();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            _stuRepository.CreateStudent(student);

            return CreatedAtRoute("GetStudent", new { id = student.StudentId }, student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Student student)
        {
            var stu = _stuRepository.GetStudent(id);
            if (stu == null)
            {
                return NotFound();
            }

            _stuRepository.UpdateStudent(student);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var stu = _stuRepository.GetStudent(id);
            if (stu == null)
            {
                return NotFound();
            }

            _stuRepository.DeleteStudent(stu);
            return NoContent();
        }

        [HttpGet]
        [Route("inv")]
        public ActionResult<List<Invoice>> GetInventory()
        {
            return _stuRepository.GetInventory();
        }

        [HttpGet]
        [Route("Cusinv")]
        public ActionResult<List<Customer>> GetCustomerInventory()
        {
            return _stuRepository.GetCustomerInventory();
        }
    }
}
