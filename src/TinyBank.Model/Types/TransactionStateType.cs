namespace TinyBank.Model.Types
{
    /// <summary>
    ///     Used to specify whether a transaction has been applied in the balances of an account
    ///     during an EOD process.
    /// </summary>
    public enum TransactionStateType
    {
        Pending = 0,
        Committed = 1
    }
}
