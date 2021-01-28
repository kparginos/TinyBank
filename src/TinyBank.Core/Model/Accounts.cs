﻿using System;

namespace TinyBank.Core.Model
{
    public class Accounts
    {
        public int AccountsId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountDescription { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public DateTime Created { get; private set; }
        public bool Active { get; set; }
        //public List<Transactions> Transactions { get; set; }
        public Accounts()
        {
            Created = DateTime.Now;
        }
    }
}
