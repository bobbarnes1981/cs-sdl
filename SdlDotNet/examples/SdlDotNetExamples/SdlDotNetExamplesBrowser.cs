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
using SdlDotNet.Core;

namespace SdlDotNetExamples
{
    public partial class SdlDotNetExamplesBrowser : Form
    {
        static bool isInitialized = Initialize();

        public static bool IsInitialized
        {
            get { return SdlDotNetExamplesBrowser.isInitialized; }
            set { SdlDotNetExamplesBrowser.isInitialized = value; }
        }

        static bool Initialize()
        {
            Tao.Sdl.Sdl.SDL_Quit();
            Glut.glutInit();
            return true;
        }

        public SdlDotNetExamplesBrowser()
        {
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
                   // foreach (TreeNode node in treeView1.Nodes)
                   // {
                        if (!treeView1.Nodes.ContainsKey(type.Namespace))
                        {
                            treeView1.Nodes.Add(type.Namespace, type.Namespace.Substring(type.Namespace.IndexOf('.') + 1));
                        }

                        object result = type.InvokeMember("Title",
                                BindingFlags.GetProperty, null, type, null, CultureInfo.CurrentCulture);
                        treeView1.Nodes[type.Namespace].Nodes.Add(type.FullName, (string)result);
                    //}
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
            try
            {
                SdlDotNet.Core.Events.QuitApplication();
                Type example = Assembly.GetExecutingAssembly().GetType(treeView1.SelectedNode.Name, true, true);
                example.InvokeMember("Run", BindingFlags.InvokeMethod, null, null, null, CultureInfo.CurrentCulture);
                Application.Exit();
            }
            catch (TypeLoadException e)
            {
                e.ToString();
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                e.ToString();
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                e.ToString();
            }
            catch (System.MissingMethodException e)
            {
                e.ToString();
            }
            finally
            {
                SdlDotNet.Core.Events.QuitApplication();
            }
        }

        void treeView1_DoubleClick(object sender, EventArgs e)
        {
            RunExample();
        }
    }
}
