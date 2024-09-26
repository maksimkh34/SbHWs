using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12
{
    public static class Journal
    {
        public static ObservableCollection<JournalEntryArgs> Entries { get; } = [];

        public static void Register(JournalEntryArgs args)
        {
            Entries.Add(args);
        }
    }

    public enum JournalEntryType
    {
        AccountOpened,
        AccountClosed,
        Deposited,
        Transferred,
        ClientDataChanged
    }

    public abstract record JournalEntryArgs(JournalEntryType Type)
    {
        public JournalEntryType Type = Type;
    }

    public record AccountOpenedArgs(bool IsDeposit, string Username) : JournalEntryArgs(JournalEntryType.AccountOpened)
    {
        public bool IsDeposit = IsDeposit;
        public string Username = Username;

        public override string ToString()
        {
            return $"{(IsDeposit ? "Д" : "Нед")}епозитный аккаунт для пользователя {Username} открыт. ";
        }
    }

    public record AccountClosedArgs(bool IsDeposit, string Username) : JournalEntryArgs(JournalEntryType.AccountClosed)
    {
        public bool IsDeposit = IsDeposit;
        public string Username = Username;

        public override string ToString()
        {
            return $"{(IsDeposit ? "Д" : "Нед")}епозитный аккаунт для пользователя {Username} закрыт. ";
        }
    }

    public record DepositedArgs(uint Sum, string Username) : JournalEntryArgs(JournalEntryType.Deposited)
    {
        public uint Sum = Sum;
        public string Username = Username;

        public override string ToString()
        {
            return $"На аккаунт {Username} зачислено {Sum}. ";
        }
    }

    public record TransferredArgs(uint Sum, string PayerUsername, string ReceiverUsername) : JournalEntryArgs(JournalEntryType.Transferred)
    {
        public uint Sum = Sum;
        public string PayerUsername = PayerUsername;
        public string ReceiverUsername = ReceiverUsername;

        public override string ToString()
        {
            return $"С аккаунта {PayerUsername} переведено {Sum} на аккаунт {ReceiverUsername}. ";
        }
    }

    public record DataChangedArgs(string Username, string ChangedFieldName, (string oldValue, string newValue) Changes)
        : JournalEntryArgs(JournalEntryType.ClientDataChanged)
    {
        public string Username = Username;
        public string ChangedFieldName = ChangedFieldName;

        public override string ToString()
        {
            return $"{ChangedFieldName} для пользователя {Username} изменено ({Changes.oldValue} => {Changes.newValue})";
        }
    }
}
