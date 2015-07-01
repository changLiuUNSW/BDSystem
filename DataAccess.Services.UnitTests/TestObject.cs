using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Allocation;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.Services.UnitTests
{
    public struct ValuePair
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class TestObject
    {
        private const string Chars = "abcdefghijklmnopqrstuvwxyz";
        //business type
        private ValuePair _cleaning = new ValuePair {Id = 1, Value = "Cleaning"};
        private ValuePair _security = new ValuePair {Id = 2, Value = "Security"};
        private ValuePair _maintenance = new ValuePair {Id = 3, Value = "Maitenance"};
        private ValuePair _others = new ValuePair { Id = 4, Value = "Others" };

        public string[] SalesReps { get; set; }
        public string[] Sizes { get; set; }
        public SalesBox[] SalesBoxes { get; set; }
        public BusinessType[] BusinessTypes { get; set; }
 
        public TestObject()
        {
            Init();
            SalesReps = InitSeed(10, 3);
            Sizes = InitSeed(5, 3);
            SalesBoxes = InitSeedSalesbox(100, 4, 3, 2);
        }

        public TestObject(int salesReps, int sizes, int salesBoxes)
        {
            Init();
            SalesReps = InitSeed(salesReps, 3);
            Sizes = InitSeed(sizes, 3);
            SalesBoxes = InitSeedSalesbox(salesBoxes, 4, 3, 2);
        }

        public TestObject(string[] salesReps, string[] sizes, SalesBox[] salesBoxes)
        {
            Init();
            SalesReps = salesReps;
            Sizes = sizes;
            SalesBoxes = salesBoxes;
        }


        private void Init()
        {
            Random = new Random(Guid.NewGuid().GetHashCode());
            BusinessTypes = InitSeedBusinessType();
            InitSeedGmGroup();
        }

        private string[] InitSeed(int numToGenerate, int stringLength)
        {
            var seed = new string[numToGenerate];   
            for (var i = 0; i < numToGenerate; i++)
            {
                seed[i] = RandomString(stringLength);
            }

            return seed;
        }

        private void InitSeedGmGroup()
        {

        }

        private SalesBox[] InitSeedSalesbox(int numToGenerate, int postcodeLength, int stateLength, int zoneLength)
        {
            var seed = new SalesBox[numToGenerate];
            for (var i = 0; i < numToGenerate; i++)
            {
                var salesbox = new SalesBox
                {
                    Postcode = RandomString(postcodeLength),
                    State = RandomString(stateLength),
                    Zone = RandomString(zoneLength)
                };

                seed[i] = salesbox;
            }

            return seed;
        }

        private BusinessType[] InitSeedBusinessType()
        {
            var cleaning = new BusinessType { Id = _cleaning.Id, Type = _cleaning.Value };
            var security = new BusinessType { Id = _security.Id, Type = _security.Value };
            var maintenance = new BusinessType { Id = _maintenance.Id, Type = _maintenance.Value };
            var others = new BusinessType { Id = _others.Id, Type = _others.Value };
            return new [] { cleaning, security, maintenance, others };
        }

        public Random Random { get; set; }

        protected string RandomString(int length)
        {
            return new string(
                Enumerable.Repeat(Chars, length)
                    .Select(s => s[Random.Next(s.Length)])
                    .ToArray());
        }

        protected virtual bool RandomBool
        {
            get
            {
                const int ceil = 10;
                return Random.Next(ceil) > ceil/2;
            }
        }

        protected virtual DateTime RandomNextCall
        {
            get
            {
                switch (RandomBool)
                {
                    case true:
                        return DateTime.Today;
                    default:
                        return DateTime.Today.AddDays(-30);
                }
            }
        }

        public virtual IList<Contact> SetupContacts(int count)
        {
            var contacts = new List<Contact>();

            if (count <= 0)
                return contacts;

            while (count > 0)
            {
                var contact = new Contact
                {
                    Leads = new Collection<Lead>()
                };

                contact.Id = count;
                
                contacts.Add(contact);
                SetupContact(ref contact);
                count--;
            }

            return contacts;
        }

        public virtual void SetupContact(ref Contact contact)
        {
            SetupBusinessType(ref contact, _cleaning);
            SetupNextcall(ref contact);
            SetupSalesRep(ref contact);
            SetupDatToCheck(ref contact);
            SetupCannotGetName(ref contact);
            SetupRecallExtDetail(ref contact);
            SetupSite(ref contact);
        }

        public virtual void SetupBusinessType(ref Contact contact, ValuePair type)
        {
            contact.BusinessTypeId = BusinessTypes.Single(x => x.Type == type.Value).Id;
        }

        public virtual void SetupNextcall(ref Contact contact)
        {
            contact.NextCall = RandomNextCall;
        }

        public virtual void SetupSalesRep(ref Contact contact)
        {
            contact.Code = SalesReps[Random.Next(SalesReps.Length)];
        }

        public virtual void SetupDatToCheck(ref Contact contact)
        {
            contact.DaToCheck = RandomBool;
        }

        public virtual void SetupCannotGetName(ref Contact contact)
        {
            contact.ReceptionName = RandomBool;
        }

        public virtual void SetupRecallExtDetail(ref Contact contact)
        {
            contact.ExtManagement = RandomBool;
        }

        public virtual void SetupSite(ref Contact contact)
        {
            var site = new Site {Id = contact.Id, Contacts = new List<Contact>()};
            SetupAddress(ref site);
            SetupSize(ref site);
            site.InHouse = RandomBool;
            site.Contacts.Add(contact);
            contact.Site = site;
            contact.SiteId = site.Id;
        }

        public virtual void SetupAddress(ref Site site)
        {
            var location = SalesBoxes[Random.Next(SalesBoxes.Length)];
            site.Postcode = location.Postcode;
            site.State = location.State;
        }

        public virtual void SetupSize(ref Site site)
        {
            site.Size = Sizes[Random.Next(Sizes.Length)];
        }
    }
}