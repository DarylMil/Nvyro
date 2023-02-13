using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Nvyro.Models
{
    public class Request
    {
        private string _postalCode;
        private string _unitnumber;
        public int Id { get; set; }

        [Required, MaxLength(6)]
        [DisplayName("Postal Code")]
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                UpdatePostalCodeAndUnit();
            }
        }
        [Required, MaxLength(12)]
        [DisplayName("Block Number")]
        public string BlockNumber { get; set; } = string.Empty;
        [Required, MaxLength(6)]
        [DisplayName("Unit Number")]
        public string UnitNumber
        {
            get { return _unitnumber; }
            set
            {
                _unitnumber = value;
                UpdatePostalCodeAndUnit();
            }
        }

        [DisplayName("Road Name"), MaxLength(100), Required]
        public string RoadName { get; set; } = string.Empty;
        public string PostalCodeAndUnit { get; private set; }
      
        public ApplicationUser? Applicationuser { get; set; }
        public Event? Event { get; set; }

        private void UpdatePostalCodeAndUnit()
        {
            PostalCodeAndUnit = PostalCode + UnitNumber;
        }

        public List<Request_Images>? Request_Images { get; set; }
    }
}
