using System;
namespace StuApiWithDatabaseDeeDee.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public float StudentGpa { get; set; }


        public override string ToString() 
        {
            return StudentId + "," + StudentName + "," + StudentGpa + ",";
        }

    }
}
