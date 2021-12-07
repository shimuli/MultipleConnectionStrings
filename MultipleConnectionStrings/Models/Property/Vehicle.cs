using System;
using System.Collections.Generic;

#nullable disable

namespace MultipleConnectionStrings.Models.Property
{
    public partial class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NumberPlate { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
