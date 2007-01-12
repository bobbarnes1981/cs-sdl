#region LICENSE
/*
 * Copyright (C) 2004 - 2006 David Hudson (jendave@yahoo.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.Resources;

using Tao.FreeGlut;

namespace SdlDotNetExamples
{
    public partial class SdlDotNetExamplesBrowser : Form
    {
        static ResourceManager stringManager;

        public static ResourceManager StringManager
        {
            get { return SdlDotNetExamplesBrowser.stringManager; }
            set { SdlDotNetExamplesBrowser.stringManager = value; }
        }

        public SdlDotNetExamplesBrowser()
        {
            Glut.glutInit();
            SdlDotNet.Graphics.Video.Initialize();
            stringManager =
                new ResourceManager("SdlDotNetExamples.Properties.Resources", Assembly.GetExecutingAssembly());
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
                            BindingFlags.GetProperty, null, type, null, CultureInfo.CurrentCulture);
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
                example.InvokeMember("Run", BindingFlags.InvokeMethod, null, null, null, CultureInfo.CurrentCulture);
                Application.Restart();
            }
            catch (TypeLoadException e)
            {
                e.ToString();
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                e.ToString();
            }
            catch (System.ArgumentOutOfRangeException e )
            {
                e.ToString();
            }
            catch (System.MissingMethodException e)
            {
                e.ToString();
            }
        }

        void treeView1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            RunExample();
        }
    }
}
