namespace DataLayer.GeneratedEf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesLT.vGetAllCategories")]
    public partial class vGetAllCategory
    {
        [Key]
        [StringLength(50)]
        public string ParentProductCategoryName { get; set; }

        [StringLength(50)]
        public string ProductCategoryName { get; set; }

        public int? ProductCategoryID { get; set; }
    }
}
