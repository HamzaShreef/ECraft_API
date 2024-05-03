﻿using ECraft.Contracts.Request;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECraft.Models
{
	public class AppUser : IdentityUser<int>
	{
        public AppUser(string firstName,string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        public DateOnly? Dob { get; private set; }

        public bool isMaleGender { get; set; } = true;

		public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? ProfileImg { get; set; }

		public int? CityId { get; set; }

		public LocationCity? City { get; set; }

		[MaxLength(250)]
		public string? ExtraLocationDetails { get; set; }

		[NotMapped]
		public int? Age
		{
			get
			{
				if (Dob.HasValue)
				{
					int age = DateTime.UtcNow.Year - Dob.Value.Year;

					if (Dob.Value.AddYears(age) > DateOnly.FromDateTime(DateTime.UtcNow))
						age--;

					return age;
				}
				return null;
			}
		}

		public bool SetDob(DateOnly dob)
        {
			Dob = dob;
			var age = this.Age;
			if (age.HasValue && age >= 16)
			{
				return true;
			}
			this.Dob = null;

            return false;
        }
    }
}