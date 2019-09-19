using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class People
    {
        public short Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PoliticalDivision { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string OnlineResource { get; set; }
        public string Affiliation { get; set; }
    }
}
