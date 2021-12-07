using System;
using System.Collections.Generic;

#nullable disable

namespace MultipleConnectionStrings.Models.Property
{
    public partial class House
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlotNumber { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
