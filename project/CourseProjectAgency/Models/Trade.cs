namespace CourseProjectAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trade")]
    public partial class Trade
    {
        [Key]
        public int trade_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        public int object_id { get; set; }

        public int agent_id { get; set; }

        public int buyer_id { get; set; }

        public virtual Agent Agent { get; set; }

        public virtual Client Client { get; set; }

        public virtual ObjectEstate ObjectEstate { get; set; }

        public virtual Rent Rent { get; set; }

        public virtual Sale Sale { get; set; }
    }
}
