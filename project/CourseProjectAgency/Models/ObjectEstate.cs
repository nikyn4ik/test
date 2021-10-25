namespace CourseProjectAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ObjectEstate")]
    public partial class ObjectEstate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ObjectEstate()
        {
            Trades = new HashSet<Trade>();
        }

        [Key]
        public int object_id { get; set; }

        [Required]
        [StringLength(100)]
        public string address { get; set; }

        public int square { get; set; }

        public int price { get; set; }

        [Required]
        [StringLength(17)]
        public string cadastral_number { get; set; }

        public int owner_id { get; set; }

        public int status_id { get; set; }

        public virtual Client Client { get; set; }

        public virtual Flat Flat { get; set; }

        public virtual House House { get; set; }

        public virtual Status Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
