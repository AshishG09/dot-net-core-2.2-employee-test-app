using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Name = "John",
                    Department = DeptEnum.Staff,
                    Email = "john@abc.com",
                    Id = 4
                },
                new Employee
                {
                    Name = "Mary",
                    Department = DeptEnum.Staff,
                    Email = "john@abc.com",
                    Id = 5
                }
                );
        }
    }
}
