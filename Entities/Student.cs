using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Entities
{
    public class Student
    {
        public string StudentId {get;set;}
        public string LastName {get;set;}
        public string FirstName {get;set;}
        public string Address {get;set;}
        public string Phone {get;set;}
        public string Email {get;set;}
        public List<Enrollment> Enrollments {get;set;}
        public List<AccountsReceivable> AccountsReceivables {get;set;}
    }
}