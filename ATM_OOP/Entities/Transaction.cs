using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_OOP
{
    class Transaction
    {
        public int TransID { get; set; }
        public DateTime TransDate { get; set; }
        public TransactionType TransType { get; set; }
        public decimal TransAmount { get; set; }
        // SourceAcct = Account# of account which is currently accessed during transaction
        // Ex: Account from which a fund transfer is sent
        public string SourceAcct { get; set; }
        // TargetAcct = Account# of account which is the destination of the transaction
        // Ex: Account to which a fund transfer is sent
        public string TargetAcct { get; set; }

        private static int transCount = 0;

        public Transaction()
        {
            transCount++;
            TransID = transCount;
        }

    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
