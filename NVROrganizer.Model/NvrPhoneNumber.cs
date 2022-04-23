using System.ComponentModel.DataAnnotations;

namespace NvrOrganizer.Model
{
    public class NvrPhoneNumber
    {
        public int Id { get; set; }

        [Phone]
        [Required]
        public string Number { get; set; }
        public int NvrId { get; set; }

        public Nvr Nvr { get; set; }
    }
}
