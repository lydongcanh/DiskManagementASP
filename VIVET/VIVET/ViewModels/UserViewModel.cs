using Ehr.Common.Constraint;
using Ehr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ehr.ViewModels
{
    public class UserViewModel
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
        public string Roles { get; set; }

        /// <summary>
        /// Người dùng có thuộc cục thú y hay không
        /// </summary>
        public bool IsCentral
        {
            get; set;
        }
        /// <summary>
        /// người dùng thuộc chi cục nào
        /// </summary>
        public int Province
        {
            get; set;
        }
        /// <summary>
        /// Người dùng loại nào
        /// </summary>
        public string UserType
        {
            get; set;
        }
    }
}