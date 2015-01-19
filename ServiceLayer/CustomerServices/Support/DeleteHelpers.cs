using DataLayer.GeneratedEf;
using GenericLibsBase;
using GenericLibsBase.Core;
using GenericServices;

namespace ServiceLayer.CustomerServices.Support
{
    public static class DeleteHelpers
    {

        public static ISuccessOrErrors DeleteAssociatedAddress(IGenericServicesDbContext db, CustomerAddress customerAddress)
        {
            var address = db.Set<Address>().Find(customerAddress.AddressID);

            if (address == null)
                return
                    new SuccessOrErrors().AddSingleError(
                        "Could not delete the associated entry as it was not in the database. Could it have been deleted by someone else?");

            db.Set<Address>().Remove(address);
            return SuccessOrErrors.Success("Removed Ok");
        }
    }
}
