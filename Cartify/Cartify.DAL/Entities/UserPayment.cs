﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.DAL.Entities
{
    public class UserPayment
    {

        [Key]
        public int Id { get; private set; }

        [Required]
        public int UserId { get; private set; }

        [Required]
        public int PaymentId { get; private set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; private set; }

        [ForeignKey(nameof(PaymentId))]
        public Payment Payment { get; private set; }
        [Required]
        public string CreatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string? ModifiedBy { get; private set; }
        public DateTime? ModifiedOn { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string? DeletedBy { get; private set; }

    }
    
}
