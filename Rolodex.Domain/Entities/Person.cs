using System;
using System.Collections.Generic;

namespace Rolodex.Domain.Entities
{
    public class Person
    {
        public int PersonID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
    }
}
