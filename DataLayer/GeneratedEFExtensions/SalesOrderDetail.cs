#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: SalesOrderDetail.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Helpers;

namespace DataLayer.GeneratedEf
{
    [MetadataType(typeof(SalesOrderDetailMetaData))]
    public partial class SalesOrderDetail : IModifiedEntity
    {

    }

    public class SalesOrderDetailMetaData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderDetailID { get; set; }
    }
}
