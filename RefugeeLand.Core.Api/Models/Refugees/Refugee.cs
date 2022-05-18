// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using RefugeeLand.Core.Api.Models.Enums;
using RefugeeLand.Core.Api.Models.Languages;

namespace RefugeeLand.Core.Api.Models.Refugees
{
    public class Refugee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } // required
        public string MiddleName { get; set; }
        public string LastName { get; set; } // required
        public string CurrentLocation { get; set; }
        public Gender Gender { get; set; } // required
        public DateTimeOffset BirthDate { get; set; } // required
        //todo: Replace with Contact information class
        public string Phone { get; set; } 
        public string Email { get; set; } // required
        //todo: Add Family class or RefugeeGroup
        public DateTimeOffset CreatedDate { get; set; } // required
        public DateTimeOffset UpdatedDate { get; set; } // required
        public bool IsOpenToWork { get; set; }
        public string MedicalConditions { get; set; }
        public string SkillSets { get; set; }
        public string AdditionalDetails { get; set; }
        public IList<Language> Languages { get; set; } // required
    }
}