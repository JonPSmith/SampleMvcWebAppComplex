using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Core;
using ServiceLayer.CustomerServices;

namespace ServiceLayer.ProductServices
{
    public class ListProductDto : EfGenericDto<Product,ListProductDto>
    {

        private DateTime _today;

        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string ProductModelName { get; set; }

        [Required]
        [StringLength(25)]
        public string ProductNumber { get; set; }

        [DataType(DataType.Currency)]
        public decimal StandardCost { get; set; }

        [DataType(DataType.Currency)]
        public decimal ListPrice { get; set; }

        public int ProductCategoryIDNonNull { get; set; }

        public int? ProductModelID { get; set; }

        //-----------------------------------------
        //things to go in detail template

        [StringLength(15)]
        public string Color { get; set; }

        public string Size { get; set; }

        [DataType(DataType.Date)]
        public DateTime SellStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SellEndDate { get; set; }


        //---------------------------------------
        //filter item

        /// <summary>
        /// This returns true if the product is on sale at the moment
        /// </summary>
        public bool IsOnSale { get; set; }

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.List; }
        }

        protected override Action<IMappingExpression<Product, ListProductDto>> AddedDatabaseToDtoMapping
        {
            get
            {
                return m => m.ForMember(d => d.IsOnSale,
                    opt => opt.MapFrom(c => c.SellStartDate < _today && (_today <= (c.SellEndDate ?? _today))));
            }
        }

        protected override IQueryable<ListProductDto> ListQueryUntracked(IGenericServicesDbContext context)
        {
            _today = DateTime.Today;
            return base.ListQueryUntracked(context);
        }
    }
}
