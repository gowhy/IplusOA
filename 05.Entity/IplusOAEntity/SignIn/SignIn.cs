﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class SignIn
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SignInTime { get; set; }
        public int Score { get; set; }
        public string Address { get; set; }

    }
}
