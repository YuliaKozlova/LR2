using ConcerteService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcerteService.Data
{
    public class DbInitializer
    {
        public static void Initialize(StudentContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var Students = new Student[]
            {
                new Student { StudentName = "Avdeeva Ekaterina Aleksandrovna",  NumberOfRecord = 104343},
                new Student { StudentName = "Kozlova Yulia Alekseevna", NumberOfRecord = 72324},
                new Student { StudentName = "Glukhovskaya Elena Aleksandrovna", NumberOfRecord = 4565656},
                new Student { StudentName = "Glukhovskoy Sergey Viktorovuch", NumberOfRecord = 123434},
                new Student { StudentName = "Avsaragov Pavel Ivanovich", NumberOfRecord = 1443434},
                new Student { StudentName = "Kuznetsova Ekaterina Aleksandrovna", NumberOfRecord = 2545454},
                new Student { StudentName = "Kozlova Olga Vladimirovna", NumberOfRecord = 1554545},
                new Student { StudentName = "Panasenko Maria Alekseevna", NumberOfRecord = 35454545},
                new Student { StudentName = "Volgov Andrey Viktorovich", NumberOfRecord = 4423424},
                new Student { StudentName = "Grishin Viktor Ivanovich", NumberOfRecord = 8432423},
                new Student { StudentName = "Golubev Victor Aleksandrovich", NumberOfRecord = 434349},
                new Student { StudentName = "Popov Aleksey Viktorovich", NumberOfRecord = 434659},
                new Student { StudentName = "Popova Olga Viktorovna", NumberOfRecord = 898965},
                new Student { StudentName = "Ivanov Ivan Ivanovich", NumberOfRecord = 2312312},
                new Student { StudentName = "Pavlova Olga Vladimirovna", NumberOfRecord = 6657657},
                new Student { StudentName = "Soboleva Yulia Dmitrievna", NumberOfRecord = 6456565},
            };

            foreach (Student s in Students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();
        }
    }
}
