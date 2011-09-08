﻿
using BusinessObjects;
using System.Collections.Generic;
using TagCloudLib;
using Common.Entities.Pagination;
using BusinessObjects.Artificial;
namespace Modules.Tags.Services
{
    public interface IDataService
    {
        bool SaveTag(Tag tag);
        IList<ITag> GetTagsWithAssociatedEntitiesCount();
        IPagedList<TaggedObject> GetTaggedObjects(ILoadOptions loadOptions);
    }
}
