using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.DTOs.Action
{
    public class ActionResponseDTO
    {
        public Guid Id { get; set; }
        public string Remark { get; set; }
        public string Amount { get; set; }
        public bool IsTransfer { get; set; }

    }
}
