using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HW12;
using HW12UI.Core;

namespace HW12UI.ViewModel
{
    internal class CreateNewUserViewModel
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public ICommand AddCommand { get; }
        public Action<string> MessageAction;

        /// <summary>
        /// <param name="messageAction"></param> Будет также использован как messageAction для созданного пользователя
        /// </summary>
        public CreateNewUserViewModel(Action<string> messageAction)
        {
            AddCommand = new DelegateCommand(AddUser);
            MessageAction = messageAction;
        }

        public CreateNewUserViewModel() : this(_ => {}) {}

        public void AddUser(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
            {
                MessageAction.Invoke("Неверно введены данные! ");
                return;
            }
            Program.Users.Add(new User(Name, Surname, MessageAction));
            MessageAction.Invoke($"{Surname} {Name} зарегистрирован! ");
        }
    }
}
