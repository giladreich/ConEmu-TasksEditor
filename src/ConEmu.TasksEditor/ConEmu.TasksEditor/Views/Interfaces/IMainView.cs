using System;
using System.Windows.Forms;
using ConEmu.TasksEditor.Models;

namespace ConEmu.TasksEditor.Views.Interfaces
{
    public interface IMainView
    {
        event Action<object, ConEmuArgs> ChildClosed;

        string ConEmuXmlPath { get; set; }

        ListBox.ObjectCollection TasksListBox { get; }

        void BringToFront();
        void Refresh();
    }
}
