using Rolodex.Domain.Abstract;
using Rolodex.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data.Entity;


namespace Rolodex.Domain.Repository
{
    public class PeopleRepository : IPeopleRepository
    {
        public PeopleRepository(RolodexModelContext con)
        {
            context = con;
        }

        public IList<Person> ListContacts()
        {
            return context.People.ToList();
        }

        public IList<Person> SearchContacts(string lastName)
        {
            var query = from p in context.People
                        where p.LastName.ToUpper().Contains(lastName.ToUpper())
                        || p.Name.ToUpper().Contains(lastName.ToUpper())
                        select p;
            return query.ToList();
        }

        public void UpdatePerson(Person contact)
        {
            context.People.Attach(contact);
            context.Entry(contact).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void MassiveInsert(IList<Person> people)
        {
            foreach (var p in people)
            {
                context.People.Add(p);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// TODO: Complete the merge option in case of coincidence
        /// </summary>
        /// <param name="list"></param>
        private void MergeCoincidence(ref List<Person> list, out List<Person> sal)
        {
            sal = new List<Person>();
            foreach (var person in list)
            {
                var query = from p in context.People
                            where p.Name.Equals(person.Name) && p.LastName.Equals(person.LastName)
                            select p;
                sal.Add(query.First());
            }
        }

        public Person GetContact(int id)
        {
            return context.People.Find(id);
        }


        public RolodexModelContext context{get;set;}
    }
}
