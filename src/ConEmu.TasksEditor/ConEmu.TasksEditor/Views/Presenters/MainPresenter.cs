using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using ConEmu.TasksEditor.Enums;
using ConEmu.TasksEditor.Models;
using ConEmu.TasksEditor.Utils;
using ConEmu.TasksEditor.Views.Interfaces;

namespace ConEmu.TasksEditor.Views.Presenters
{
    public sealed class MainPresenter
    {
        private readonly IMainView view;
        private List<ConEmuTask> tasks;


        public MainPresenter(IMainView view)
        {
            this.view = view;
            tasks = new List<ConEmuTask>();

            view.ChildClosed += ChildWindowClosed;
        }

        private void ChildWindowClosed(object sender, ConEmuArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.Save:
                    foreach (ConEmuTask task in tasks)
                    {
                        if (task.Guid != e.Task.Guid) continue;

                        Console.WriteLine(task.Commands.First());
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(view.ConEmuXmlPath);
                        UpdateTask(task, xmlDoc, e.Task);
                        break;
                    }

                    break;

                case CloseReason.Cancel:
                case CloseReason.Unknown:
                default:
                    break;
            }

            view.BringToFront();
        }

        public void UpdateConEmuConfPath()
        {
            string cmderPath = GetProcPath("cmder.exe");
            if (cmderPath != null)
            {
                string dir = Path.GetDirectoryName(cmderPath);
                Debug.Assert(dir != null, nameof(dir) + " != null");
                string path = Path.Combine(dir, "vendor\\conemu-maximus5", "ConEmu.xml");
                view.ConEmuXmlPath = path;
            }
            else
            {
                string conEmuPath = GetProcPath("conemu.exe");
                if (conEmuPath == null) return;

                string dir = Path.GetDirectoryName(conEmuPath);
                Debug.Assert(dir != null, nameof(dir) + " != null");
                string path = Path.Combine(dir, "ConEmu.xml");
                view.ConEmuXmlPath = path;
            }
        }

        public string GetProcPath(string program)
        {
            ProcessStartInfo si = new ProcessStartInfo
            {
                FileName = "where.exe",
                Arguments = program,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process proc = new Process { StartInfo = si };

            proc.Start();
            string result = proc.StandardOutput.ReadLine();
            proc.Close();

            return result;
        }

        private void UpdateTask(ConEmuTask task, XmlDocument doc, ConEmuTask newTask)
        {
            XmlNodeList tasksNode = doc.SelectNodes("//key[@name='Tasks']/key");
            XmlNode tasksRootNode = doc.SelectSingleNode("//key[@name='Tasks']");
            Debug.Assert(tasksNode != null, nameof(tasksNode) + " != null");
            Debug.Assert(tasksRootNode != null, nameof(tasksRootNode) + " != null");

            string xpath =
                $"//value[@name='Name' and @type='string' and @data='{task.Name}']";

            foreach (XmlNode n in tasksNode)
            {
                XmlNode targetNode = n.SelectSingleNode(xpath)?.ParentNode;
                if (targetNode == null) continue;

                XmlNode keyElem = CreateTaskNode(doc, newTask, targetNode);
                tasksRootNode.InsertAfter(keyElem, targetNode.PreviousSibling);
                tasksRootNode.RemoveChild(targetNode);

                doc.Save(view.ConEmuXmlPath);
                view.Refresh();
                break;
            }
        }

        private static XmlElement CreateTaskNode(XmlDocument doc, ConEmuTask newTask, XmlNode targetNode)
        {
            Debug.Assert(targetNode.Attributes != null, "targetNode.Attributes != null");

            XmlElement keyElem = doc.CreateElement("key");
            keyElem.SetAttribute("name", targetNode.Attributes["name"].Value);
            keyElem.SetAttribute("modified", targetNode.Attributes["modified"].Value);
            keyElem.SetAttribute("build", targetNode.Attributes["build"].Value);

            // Child elements
            XmlElement elem1 = doc.CreateElement("value");
            XmlElement elem2 = doc.CreateElement("value");
            XmlElement elem3 = doc.CreateElement("value");
            XmlElement elem4 = doc.CreateElement("value");
            XmlElement elem5 = doc.CreateElement("value");
            XmlElement elem6 = doc.CreateElement("value");
            XmlElement elem7 = doc.CreateElement("value");

            elem1.SetAttribute("name", "Name");
            elem1.SetAttribute("type", "string");
            elem1.SetAttribute("data", newTask.Name);

            elem2.SetAttribute("name", "Flags");
            elem2.SetAttribute("type", "dword");
            elem2.SetAttribute("data", newTask.Flags);

            elem3.SetAttribute("name", "Hotkey");
            elem3.SetAttribute("type", "dword");
            elem3.SetAttribute("data", newTask.Hotkey);

            elem4.SetAttribute("name", "GuiArgs");
            elem4.SetAttribute("type", "string");
            elem4.SetAttribute("data", newTask.GuiArgs);

            elem5.SetAttribute("name", "Cmd1");
            elem5.SetAttribute("type", "string");
            elem5.SetAttribute("data", newTask.Commands.First());

            elem6.SetAttribute("name", "Active");
            elem6.SetAttribute("type", "long");
            elem6.SetAttribute("data", newTask.Active ? "1" : "0"); // > mark for active tabs.

            elem7.SetAttribute("name", "Count");
            elem7.SetAttribute("type", "long");
            elem7.SetAttribute("data", newTask.Count.ToString());

            keyElem.AppendChild(elem1);
            keyElem.AppendChild(elem2);
            keyElem.AppendChild(elem3);
            keyElem.AppendChild(elem4);
            keyElem.AppendChild(elem5);
            keyElem.AppendChild(elem6);
            keyElem.AppendChild(elem7);

            return keyElem;
        }

        public void LoadConEmuXmlFile(string path)
        {
            view.TasksListBox.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList tasksNode = xmlDoc.SelectNodes("//key[@name='Tasks']/key");
            Debug.Assert(tasksNode != null, "ARGGGGGGHHH NULL tasksNode");

            foreach (XmlNode taskNode in tasksNode)
            {
                List<XmlNode> cmdNodes = taskNode.Where(c => c.Attributes["name"].Value.StartsWith("Cmd")).ToList();
                List<XmlNode> otherNodes = taskNode.Where(c => !c.Attributes["name"].Value.StartsWith("Cmd")).ToList();

                ConEmuTask task = new ConEmuTask();
                otherNodes.ForEach(n => InitializeTaskFields(n, task));
                cmdNodes.ForEach(c => task.AddCommand(new Command(c.Attributes["data"].Value)));

                tasks.Add(task);
                view.TasksListBox.Add(task);
            }
        }

        public void InitializeTaskFields(XmlNode other, ConEmuTask task)
        {
            Debug.Assert(other != null, "InitializeTaskFields giving XmlNode was null.");

            Debug.Assert(other.Attributes != null, "other.Attributes != null");
            string option = other.Attributes["name"].Value;
            string dataOption = other.Attributes["data"].Value;
            switch (option)
            {
                case "Name":
                    task.Name = dataOption;
                    break;
                case "GuiArgs":
                    task.GuiArgs = dataOption;
                    break;
                case "Active":
                    task.Active = dataOption == "1";
                    break;
                case "Count":
                    task.Count = dataOption.ToInt32();
                    break;
                case "Hotkey":
                    task.Hotkey = dataOption;
                    break;
                case "Flags":
                    task.Flags = dataOption;
                    break;

                default:
                    Console.WriteLine($"### Option not implemented -> {dataOption} ###");
                    break;
            }
        }
    }
}
