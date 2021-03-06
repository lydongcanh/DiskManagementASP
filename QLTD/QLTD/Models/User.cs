﻿using Ehr.Common.Constraint;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ehr.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Experience { get; set; }
        public string ResetPasswordCode { get; set; }
        public string ConfirmPasswordCode { get; set; }
        [NotMapped]
        public bool IsRemember { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}