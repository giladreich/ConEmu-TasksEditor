using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ConEmu.TasksEditor.Models;
using ConEmu.TasksEditor.Utils;
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
            tbxGuiArgs.Text = task.GuiArgs;

            foreach (Command command in task)
            {
                string[] commands = Regex.Split(command, "&&");
                commands.ForEach(c => rtbCommands.AppendText(c.Trim() + Environment.NewLine));
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ConEmuTask modifiedTask = task.Clone();
            modifiedTask.Name = tbxName.Text;
            modifiedTask.GuiArgs = tbxGuiArgs.Text;

            modifiedTask.Commands.Clear();
            string[] lines = rtbCommands.Lines;
            string tmpCmd = String.Empty;
            foreach (string line in lines)
            {
                if (String.IsNullOrEmpty(line)) continue;
                tmpCmd += line.Trim() + " && ";
            }

            string cmd = tmpCmd.Remove(tmpCmd.Length - 4, 4);
            modifiedTask.Commands.Add(new Command(cmd));

            ((MainWindow)Owner).OnChildClosed(this, new ConEmuArgs(CloseReason.Save, modifiedTask));
            Close();
        }

        private void Canceled(object sender, EventArgs e)
        {
            ((MainWindow)Owner).OnChildClosed(this, new ConEmuArgs(CloseReason.Cancel, task));
            Close();
        }
    }
}
