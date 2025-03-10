﻿using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class User : IdentityUser<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public int? Age { get; set; }
        public string? Image { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string? Code { get; set; }

        public string? CartId { get; set; }
        public Cart? Cart { get; set; }
        public string? FavouriteProductId { get; set; }
        public FavouriteProduct? FavouriteProduct { get; set; }
        public List<Order> OrderList { get; set; } = new List<Order>();
        public List<Payment> PaymentList { get; set; } = new List<Payment>();
        public List<Review> ReviewsList { get; set; } = new List<Review>();

        [InverseProperty("User")]
        public ICollection<JwtToken> JwtTokens { get; set; } = new List<JwtToken>();
    }
}
