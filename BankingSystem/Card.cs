using System;

namespace BankingSystem
{
    internal class Card
    {
        public required string AccountId { get; set; }
        public required string User { get; set; }
        public decimal Balance { get; set; }
        public required string Pin { get; set; }


        public Card()
        {

        }

        public Card
            (
            string accountId,
            string user,
            decimal balance,
            string pin
            )
        {
            this.AccountId = accountId;
            this.User = user;
            this.Balance = balance;
            this.Pin = pin;

        }
    }
}

