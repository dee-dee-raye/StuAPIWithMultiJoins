using System;
using StuApiWithDatabaseDeeDee.Models;
using System.Collections.Generic;
namespace StuApiWithDatabaseDeeDee.Repositories
{
    public interface IStuRepository
    {
        Student GetStudent(string id);

        List<Student> GetAllStudents();

        void CreateStudent(Student stu);

        void UpdateStudent(Student updatedStudent);

        void DeleteStudent(Student deleteStudent);

        float[] GetRange();

        List<Invoice> GetInventory();

        List<Customer> GetCustomerInventory();

    }
}
