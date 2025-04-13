using System;
using System.ComponentModel.DataAnnotations;

namespace OneWholesale.Model.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string CompanyLogo { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string DeletedBy { get; set; }
        public bool IsActive { get; set; }
    }

}
