using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using Tao.FreeGlut;

namespace SdlDotNetExamples
{
    public partial class SdlDotNetExamplesBrowser : Form
    {
        public SdlDotNetExamplesBrowser()
        {
            Glut.glutInit();
            SdlDotNet.Graphics.Video.Initialize();
            InitializeComponent();
        }

        Dictionary<string, string> demoList = new Dictionary<string, string>();

        private void frmExamples_Load(object sender, EventArgs e)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                MemberInfo[] runMethods = type.GetMember("Run");

                if (runMethods.Length > 0)
                {
                    if (!treeView1.Nodes.ContainsKey(type.Namespace))
                    {
                        treeView1.Nodes.Add(type.Namespace, type.Namespace.Substring(type.Namespace.IndexOf('.') + 1));
                    }

                    object result = type.InvokeMember("Title",
                            BindingFlags.GetProperty, null, type, null);
                    treeView1.Nodes[type.Namespace].Nodes.Add(type.FullName, (string)result);
                }
            }

            treeView1.Sort();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            RunExample();
        }

        private void RunExample()
        {
            SdlDotNet.Core.Events.QuitApplication();
            try
            {
                Type example = Assembly.GetExecutingAssembly().GetType(treeView1.SelectedNode.Name, true, true);
                example.InvokeMember("Run", BindingFlags.InvokeMethod, null, null, null);
                Application.Restart();
            }
            catch (TypeLoadException e)
            {
                e.ToString();
            }
            catch (System.Reflection.TargetInvocationException)
            {
                // User changed demo - do nothing
            }
            catch (System.ArgumentOutOfRangeException)
            {
            }
            catch (System.MissingMethodException)
            {
                // missing method - do nothing
            }
        }

        void treeView1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            RunExample();
        }
    }
}
