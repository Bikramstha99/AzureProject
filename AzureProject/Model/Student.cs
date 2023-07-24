using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureProject.Model
{
    public class Student
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Place { get; set; }
    }
}
