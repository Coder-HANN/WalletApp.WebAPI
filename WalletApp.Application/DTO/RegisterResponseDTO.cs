﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.DTO
{
    public class RegisterResponseDTO
    {
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Message { get; set; } = "Registered Successfully";
    }
}
