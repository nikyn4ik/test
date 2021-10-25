namespace CourseProjectAgency
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Agent")]
    public partial class Agent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agent()
        {
            Trades = new HashSet<Trade>();
        }

        [Key]
        public int agent_id { get; set; }

        [Required]
        [StringLength(50)]
        public string full_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime birthday { get; set; }

        [Required]
        [StringLength(100)]
        public string address { get; set; }

        [Required]
        [StringLength(10)]
        public string passport_series_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime passport_date { get; set; }

        [Required]
        [StringLength(100)]
        public string passport_issued_by { get; set; }

        [Required]
        [StringLength(11)]
        public string phone_number { get; set; }

        [Required]
        [StringLength(10)]
        public string INN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trade> Trades { get; set; }
    }
}
