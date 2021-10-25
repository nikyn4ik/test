namespace CourseProjectAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Flat")]
    public partial class Flat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int flat_id { get; set; }

        public int flat_number { get; set; }

        public int quantity_rooms { get; set; }

        public int number_floor { get; set; }

        public virtual ObjectEstate ObjectEstate { get; set; }
    }
}
