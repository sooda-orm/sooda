using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sooda
{
    public static class TransactionStrategyMenager
    {
        private static IDefaultSoodaTransactionStrategy _transactionStrategy;
        public static void SetTransactionStrategy(IDefaultSoodaTransactionStrategy transactionStrategy)
        {
            _transactionStrategy = transactionStrategy;
        }
        public static IDefaultSoodaTransactionStrategy GetTransactionStrategy()
        {
            return _transactionStrategy;
        }
    }
}
