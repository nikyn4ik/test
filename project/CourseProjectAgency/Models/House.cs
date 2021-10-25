namespace CourseProjectAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("House")]
    public partial class House
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int house_id { get; set; }

        public int plot_size { get; set; }

        public int quantity_floors { get; set; }

        public virtual ObjectEstate ObjectEstate { get; set; }
    }
}
