using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace SdlDotNetExamples
{
    public partial class SdlDotNetExamples : Form
    {
        public SdlDotNetExamples()
        {
            InitializeComponent();
        }

        private void frmExamples_Load(object sender, EventArgs e)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                MemberInfo[] runMethods = type.GetMember("Run");
                foreach (MemberInfo run in runMethods)
                {
                    lstExamples.Items.Add(type.FullName);
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            SelectExample();
        }

        private void SelectExample()
        {
            Type example = Assembly.GetExecutingAssembly().GetType(lstExamples.SelectedItem.ToString(), true, true);
            example.InvokeMember("Run", BindingFlags.InvokeMethod, null, null, null);
        }

        private void lstExamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectExample();
        }
    }
}