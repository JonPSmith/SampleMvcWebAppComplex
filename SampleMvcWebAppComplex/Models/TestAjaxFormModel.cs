using System.ComponentModel.DataAnnotations;

namespace SampleMvcWebAppComplex.Models
{
    public class TestAjaxFormModel
    {
        [Range(0,10)]
        public int MyInt { get; set; }

        [StringLength(5)]
        [Required]
        public string MyString { get; set; }

        public bool ShouldFail { get; set; }
    }
}