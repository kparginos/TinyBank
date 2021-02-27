namespace TinyBank.Model.Types
{
    /// <summary>
    ///     Used to specify the type of a transaction on an account
    /// </summary>
    public enum TransactionType
    {
        Undefined = 0,
        Credit = -1,
        Debit = 1
    }
}
