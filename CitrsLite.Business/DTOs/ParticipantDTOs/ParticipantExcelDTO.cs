﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitrsLite.Business.DTOs.ParticipantDTOs
{
    public class ParticipantExcelDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Description { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public bool IsActive { get; set; }
    }
}
