using System;
using System.Collections.Generic;
using TinyBank.Model;
using TinyBank.Model.Types;

namespace TinyBank.Model
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public bool Active { get; set; }
        public CardType Type { get; set; }
        public decimal? AvailableBalance { get; set; }
        //public List<CardLimit> Limits { get; set; }
        public List<Accounts> Accounts { get; set; }

        public Card()
        {
            Expiration = DateTimeOffset.Now.AddYears(6);
            Accounts = new List<Accounts>();
        }
    }
}
