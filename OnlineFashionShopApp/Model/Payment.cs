﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;

namespace OnlineFashionShopApp.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string PaymentCardNumber { get; set; }

    public string PaymentCardSecurityCode { get; set; }

    public DateOnly PaymentCardExpiryDate { get; set; }

    public string PaymentCardName { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; }
}