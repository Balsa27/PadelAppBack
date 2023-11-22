using System;
using System.Collections.Generic;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects
{
    public class ContactInfo : ValueObject
    {
        public string PhoneNumber { get; private set; }
        public string? WebsiteUrl { get; private set; }

        public ContactInfo( string phoneNumber, string? websiteUrl = null)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty or null");

            PhoneNumber = phoneNumber;
            WebsiteUrl = websiteUrl;
        }
            
        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return PhoneNumber;
            yield return WebsiteUrl;
        }
    }
}