using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace NvrOrganizer.Model
{
    public class Meeting
    {
        public Meeting()
        {
            Nvrs = new Collection<Nvr>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public ICollection<Nvr> Nvrs { get; set; }
    }
}
