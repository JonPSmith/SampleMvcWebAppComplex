using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using DataLayer.GeneratedEf;
using GenericLibsBase;
using GenericLibsBase.Core;
using GenericServices;

[assembly: InternalsVisibleTo("Tests")]

namespace ServiceLayer.OrdersServices.Support
{
    public static class DeleteLineItemHelper
    {
        /// <summary>
        /// This is used to update the SalesOrderHeader total when a line item is deleted
        /// </summary>
        /// <param name="db"></param>
        /// <param name="lineItemBeingDeleted"></param>
        /// <returns></returns>
        public static ISuccessOrErrors UpdateSalesOrderHeader(IGenericServicesDbContext db, SalesOrderDetail lineItemBeingDeleted)
        {
            var salesOrderHeader = db.Set<SalesOrderHeader>().Include(x => x.SalesOrderDetails).Single(x => x.SalesOrderID == lineItemBeingDeleted.SalesOrderID);

            salesOrderHeader.SubTotal =
                salesOrderHeader.SalesOrderDetails.Where(
                    x => x.SalesOrderDetailID != lineItemBeingDeleted.SalesOrderDetailID).Sum(x => x.LineTotal);

            return SuccessOrErrors.Success("Removed Ok");
        }

    }
}
