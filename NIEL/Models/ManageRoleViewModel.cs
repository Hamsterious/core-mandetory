using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NIEL.Models
{
    public class ManageRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public SelectList Roles { get; set; }
        [Display(Name = "Role")]
        public string SelectedRoleId { get; set; }
    }
}
