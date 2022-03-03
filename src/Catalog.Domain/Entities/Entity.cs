using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Domain.Entities
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }

        [Column(Order = 200)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 201)]
        public DateTime? ModifiedDate { get; set; }

        [Column(Order = 202)]
        public bool IsActive { get; set; }
        public void setIsActive(bool value)
        {
            IsActive = value;
        }
    }
}