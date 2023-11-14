using System;
using System.Collections.Generic;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects
{
    public class ContactInfo : ValueObject
    {
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? WebsiteUrl { get; private set; }

        public ContactInfo(string email, string phoneNumber, string? websiteUrl = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty or null");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty or null");

            Email = email;
            PhoneNumber = phoneNumber;
            WebsiteUrl = websiteUrl;
        }
            
        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Email;
            yield return PhoneNumber;
            yield return WebsiteUrl;
        }
    }
}