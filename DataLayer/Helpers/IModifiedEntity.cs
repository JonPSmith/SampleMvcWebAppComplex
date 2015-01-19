#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ITrackedEntity.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;

namespace DataLayer.Helpers
{
    //This interface is added dto all the database entities that have a modified date and rowGuid 
    //Save Changes uses this to find entities that need updating or a new rowguid added
    public interface IModifiedEntity 
    {
        DateTime ModifiedDate { get; set; }
        Guid rowguid { get; set; }
    }
}
