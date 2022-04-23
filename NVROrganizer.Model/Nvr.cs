﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace NvrOrganizer.Model
{
    public class Nvr
    {
         public Nvr()
            {
            PhoneNumbers = new Collection<NvrPhoneNumber>();
            }
        
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [StringLength(50)]
        public string LastName { get; set; }
       
        [StringLength(50)]
        public string NvrInfo { get; set; }

        public int? FavoriteLanguageId { get; set; }
        public ProgrammingLanguage FavoriteLanguage { get; set; }

        public ICollection<NvrPhoneNumber> PhoneNumbers { get; set; }
    }
}
