using System;
using System.Diagnostics;
using System.Windows.Forms;
using ConEmu.TasksEditor.Models;
using ConEmu.TasksEditor.Views.Interfaces;
using ConEmu.TasksEditor.Views.Presenters;

namespace ConEmu.TasksEditor.Views
{
    public partial class MainWindow : Form, IMainView
    {
        public string ConEmuXmlPath
        {
            get => tbxConXML.Text;
            set => tbxConXML.Text = value;
        }
        public ListBox.ObjectCollection TasksListBox => lbxTasks.Items;


        public event Action<object, ConEmuArgs> ChildClosed;

        private readonly MainPresenter presenter;

        public MainWindow()
        {
            InitializeComponent();
            presenter = new MainPresenter(this);
        }

        public void OnChildClosed(object sender, ConEmuArgs e)
        {
            ChildClosed?.Invoke(sender, e);
            foreach (ConEmuTask task in lbxTasks.Items)
            {
                if (task.Guid == e.Task.Guid)
                    lbxTasks.SelectedItem = task;
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            presenter.UpdateConEmuConfPath();
            if (tbxConXML.Text != null)
                Refresh();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "ConEmu XML File (*.xml) | *.xml" };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            presenter.LoadConEmuXmlFile(ofd.FileName);
        }

        private void LbxTasks_DoubleClick(object sender, EventArgs e)
        {
            ConEmuTask task = lbxTasks.SelectedItem as ConEmuTask;
            Debug.Assert(task != null, "ConEmuTask != null");
            new EditTaskWindow(task).Show(this);
        }

        private void BtnRefreshTasks_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbxConXML.Text)) return;

            presenter.LoadConEmuXmlFile(tbxConXML.Text);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        private void BtnUp_Click(object sender, EventArgs e)
        {

        }

        private void BtnDown_Click(object sender, EventArgs e)
        {

        }

        public new void Refresh()
        {
            btnRefreshTasks.PerformClick();
        }

        public new void BringToFront()
        {
            Activate();
        }
    }
}
