using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;
using StuApiWithDatabaseDeeDee.Models;
using System.Collections.Generic;
using System.Linq;

namespace StuApiWithDatabaseDeeDee.Repositories
{
    public class StuRepository : IStuRepository
    {
        const string connectionString = "Server=localhost; Database=TestDB; UID=deedee; Password=1@Abcdefg";

        public Student GetStudent(string id)
        {
            string sql = "SELECT * FROM Student WHERE StudentId = @StudentID;";
            Student stu;

            using (var connection = new MySqlConnection(connectionString))
            {
                stu = connection.QuerySingleOrDefault<Student>(sql, new { StudentID = id });
            }

            return stu;
        }

        public List<Student> GetAllStudents()
        {
            string sql = "SELECT * FROM Student;";
            List<Student> stuList;

            using (var connection = new MySqlConnection(connectionString))
            {
                stuList = connection.Query<Student>(sql).ToList();
            }

            return stuList;
        }

        public void CreateStudent(Student stu)
        {
            string sql = "INSERT INTO Student (StudentName, StudentId, StudentGpa) Values (@StudentName, @StudentId, @StudentGpa);";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(sql, new {StudentName = stu.StudentName, StudentID = stu.StudentId, StudentGpa = stu.StudentGpa } );
            }
        }

        public void UpdateStudent(Student updatedStudent)
        {
            string sql = "UPDATE Student SET StudentName = @StudentName, StudentId = @StudentId, StudentGpa = @StudentGpa WHERE StudentId = @StudentId;";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(sql, new { StudentName = updatedStudent.StudentName, StudentID = updatedStudent.StudentId, StudentGpa = updatedStudent.StudentGpa });
            }
        }

        public void DeleteStudent(Student deleteStudent)
        {
            string sql = "DELETE FROM Student WHERE StudentId = @StudentID";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(sql, new { StudentID = deleteStudent.StudentId });
            }
        }

        public float[] GetRange()
        {
            string sql = "SELECT min(StudentGpa) as min, max(StudentGpa) as max from Student;";
            float[] range = new float[2];

            using (var connection = new MySqlConnection(connectionString))
            {
                var result = connection.Query(sql).Single();
                range[0] = result.min;
                range[1] = result.max; ;
            }

            return range;
        }

        public List<Invoice> GetInventory()
        {
            string sql = "SELECT * FROM INVOICE AS A INNER JOIN LINE AS B ON A.INV_NUMBER = B.INV_NUMBER";

            List<Invoice> list;

            using (var connection = new MySqlConnection(connectionString))
            {
                var invoiceDictionary = new Dictionary<int, Invoice>();

                list = connection.Query<Invoice, LineItem, Invoice>(
                   sql,
                   (invoice, lineItem) =>
                   {
                       Invoice invoiceEntry;

                    if (!invoiceDictionary.TryGetValue(invoice.INV_NUMBER, out invoiceEntry))
                       {
                           invoiceEntry = invoice;
                           invoiceEntry.InvoiceLineItems = new List<LineItem>();
                           invoiceDictionary.Add(invoiceEntry.INV_NUMBER, invoiceEntry);
                       }

                    invoiceEntry.InvoiceLineItems.Add(lineItem);
                    return invoiceEntry;
                   },
                   splitOn: "LINE_NUMBER")
               .Distinct()
               .ToList();
            }

            return list;
        }

        public List<Customer> GetCustomerInventory()
        {
            string sql = "SELECT * FROM CUSTOMER AS A INNER JOIN INVOICE AS B ON A.CUS_CODE = B.CUS_CODE INNER JOIN LINE AS C on B.INV_NUMBER = C.INV_NUMBER";

            List<Customer> list;

            using (var connection = new MySqlConnection(connectionString))
            {
                var customerDictionary = new Dictionary<int, Customer>();
                var invoiceDictionary = new Dictionary<int, Invoice>();

                list = connection.Query<Customer, Invoice, LineItem, Customer>(
                   sql,
                   (customer, invoice, lineItem) =>
                   {
                       Customer customerEntry;
                       Invoice invoiceEntry;

                       if (!customerDictionary.TryGetValue(customer.Cus_Code, out customerEntry))
                       {
                           customerEntry = customer;
                           customerEntry.Cus_Invoices = new List<Invoice>();
                           customerDictionary.Add(customerEntry.Cus_Code, customerEntry);
                       }

                       customerEntry.Cus_Invoices.Add(invoice);

                       if (!invoiceDictionary.TryGetValue(invoice.INV_NUMBER, out invoiceEntry))
                       {
                           invoiceEntry = invoice;
                           invoiceEntry.InvoiceLineItems = new List<LineItem>();
                           invoiceDictionary.Add(invoiceEntry.INV_NUMBER, invoiceEntry);
                       }
                       invoiceEntry.InvoiceLineItems.Add(lineItem);
                       customerEntry.Cus_Invoices.Add(invoiceEntry);
                       return customerEntry;
                   },
                    splitOn: "INV_NUMBER, LINE_NUMBER")
               .Distinct()
               .ToList();
            }

            return list;
        }
    }
   
}
