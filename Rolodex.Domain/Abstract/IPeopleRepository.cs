using Rolodex.Domain.Entities;
using Rolodex.Domain.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Rolodex.Domain.Abstract
{
    public interface IPeopleRepository
    {
        IList<Person> ListContacts();
        Person GetContact(int id);
        void UpdatePerson(Person contact);
        void MassiveInsert(IList<Person> people);
        IList<Person> SearchContacts(string lastName);
        RolodexModelContext context { get; set; }

    }
}
