using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ConEmu.TasksEditor.Models;
using CloseReason = ConEmu.TasksEditor.Enums.CloseReason;

namespace ConEmu.TasksEditor.Views
{
    public partial class EditTaskWindow : Form
    {
        private readonly ConEmuTask task;

        public EditTaskWindow(ConEmuTask task)
        {
            InitializeComponent();
            this.task = task;
        }

        private void EditTaskWindow_Load(object sender, EventArgs e)
        {
            tbxName.Text = task.Name;
            foreach (Command command in task.Commands)
            {
                string cmdString = command.CommandString;
                string[] commands = Regex.Split(cmdString, "&&");
                foreach (string cmd in commands)
                {
                    Console.WriteLine(cmd);
                    rtbCommands.AppendText(cmd.Trim() + Environment.NewLine);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ConEmuTask clonedTask = task.Clone();
            clonedTask.Name = tbxName.Text;
            clonedTask.Commands.Clear();
            string[] lines = rtbCommands.Lines;
            string tmpCmd = String.Empty;
            foreach (string line in lines)
            {
                if (String.IsNullOrEmpty(line)) continue;
                tmpCmd += line.Trim() + " && ";
            }

            string cmd = tmpCmd.Remove(tmpCmd.Length - 4, 4);
            clonedTask.Commands.Add(new Command(cmd));

            ((MainWindow)Owner).OnChildClosed(this, new ConEmuArgs(CloseReason.Save, clonedTask));
            Close();
        }

        private void Canceled(object sender, EventArgs e)
        {
            ((MainWindow)Owner).OnChildClosed(this, new ConEmuArgs(CloseReason.Cancel, task));
            Close();
        }
    }
}
