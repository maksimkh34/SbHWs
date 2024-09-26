using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12;

namespace HW12UI.ViewModel
{
    internal class JournalViewModel
    {
        public ObservableCollection<JournalEntryArgs> Journal => HW12.Journal.Entries;
    }
}
