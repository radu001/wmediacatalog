using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagCloudLib;
using BusinessObjects;

namespace Modules.Tags.Model
{
    public class TagAdapter : ITag
    {
        #region ITag Members

        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Rank { get; private set; }
        public string Color { get; private set; }
        public string TextColor { get; private set; }

        #endregion

        public TagAdapter(Tag tag)
        {
            ID = tag.ID;
            Name = tag.Name;
            Rank = tag.AssociatedEntitiesCount;
            Color = tag.Color;
            TextColor = tag.TextColor;
        }
    }
}
