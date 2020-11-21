using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodersAcademy.API.ViewModel.Request
{
    public class AlbumRequest : IValidatableObject
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String Band { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public String Backdrop { get; set; }

        [Required]
        public List<MusicRequest> Musics { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (!Musics.Any())
                result.Add(new ValidationResult("Album must contain at least one music"));

            //Valida Todas as propriedades do objeto
            foreach (var item in Musics)
                Validator.TryValidateObject(item, new ValidationContext(item), result);

            return result;
        }
    }
}
