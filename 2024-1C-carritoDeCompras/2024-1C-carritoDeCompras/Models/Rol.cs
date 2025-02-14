﻿using _2024_1C_carritoDeCompras.Helper;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace _2024_1C_carritoDeCompras.Models
{
    public class Rol : IdentityRole<int>
    {

        //public int Id { get; set; }

        #region Constructores
        public Rol() : base()
        {

        }

        public Rol(string rolName) : base(rolName)
        {

        }
        #endregion

        #region Propiedades

        [Display(Name = Alias.RoleName)]
        public string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        public override string NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }
        #endregion

      
    }
}