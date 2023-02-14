using System.ComponentModel.DataAnnotations;

namespace Nvyro.Models
{
    public class Request_Images
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string? ImageURL { get; set; }

        public Request? Request { get; set; }
    }
}
