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
    public partial class SdlDotNetExamples : Form
    {
        public SdlDotNetExamples()
        {
            Glut.glutInit();
            InitializeComponent();
        }

        Dictionary<string, string> demoList = new Dictionary<string, string>();

        private void frmExamples_Load(object sender, EventArgs e)
        {
            List<string> namespaces = new List<string>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                MemberInfo[] runMethods = type.GetMember("Run");
                foreach (MemberInfo run in runMethods)
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
            SelectExample();
        }

        private void SelectExample()
        {
            Type example = Assembly.GetExecutingAssembly().GetType(treeView1.SelectedNode.Name, true, true);
            example.InvokeMember("Run", BindingFlags.InvokeMethod, null, null, null);
        }

        private void lstExamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectExample();
        }
    }
}