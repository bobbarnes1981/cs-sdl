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
using System.Collections;
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
        public static ResourceManager StringManager
        {
            get { return SdlDotNetExamplesBrowser.stringManager; }
            set { SdlDotNetExamplesBrowser.stringManager = value; }
        }

        static ResourceManager stringManager;

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
            demoList = new Dictionary<string, Dictionary<string, string>>();
            stringManager =
                new ResourceManager("SdlDotNetExamples.Properties.Resources", Assembly.GetExecutingAssembly());
            LoadDemos();
            InitializeComponent();
            this.Text = SdlDotNetExamples.SdlDotNetExamplesBrowser.StringManager.GetString(
                        "Title", CultureInfo.CurrentUICulture);
            this.demoCategory.Text = SdlDotNetExamples.SdlDotNetExamplesBrowser.StringManager.GetString(
                        "DemoCategory", CultureInfo.CurrentUICulture);
            this.btnRun.Text = SdlDotNetExamples.SdlDotNetExamplesBrowser.StringManager.GetString(
                        "Run", CultureInfo.CurrentUICulture);
            LoadComboBox();
            LoadListBox();
        }

        Dictionary<string, Dictionary<string, string>> demoList;

        private void LoadDemos()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                MemberInfo[] runMethods = type.GetMember("Run");

                if (runMethods.Length > 0)
                {
                    string result = (string)type.InvokeMember("Title",
                             BindingFlags.GetProperty, null, type, null, CultureInfo.CurrentCulture);
                    if (!this.demoList.ContainsKey(type.Namespace.Substring(type.Namespace.IndexOf('.') + 1)))
                    {
                        Dictionary<string, string> list = new Dictionary<string, string>();
                        list.Add(result, type.Name);
                        this.demoList.Add(type.Namespace.Substring(type.Namespace.IndexOf('.') + 1), list);
                    }
                    else
                    {
                        this.demoList[type.Namespace.Substring(type.Namespace.IndexOf('.') + 1)].Add(result, type.Name);
                    }
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            RunExample();
        }

        private void RunExample()
        {
            try
            {
                this.Enabled = false;
                SdlDotNet.Core.Events.QuitApplication();
                string typeString = "SdlDotNetExamples." + this.comboBoxNamespaces.SelectedItem.ToString() + "." + this.demoList[this.comboBoxNamespaces.SelectedItem.ToString()][this.listBoxDemos.SelectedItem.ToString()].ToString();
                Type example = Assembly.GetExecutingAssembly().GetType(typeString, true, true);
                example.InvokeMember("Run", BindingFlags.InvokeMethod, null, null, null, CultureInfo.CurrentCulture);
                this.Enabled = true;
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
            catch (NullReferenceException e)
            {
                e.ToString();
            }
            finally
            {
                SdlDotNet.Core.Events.QuitApplication();
                this.Enabled = true;
            }
        }

        void listBoxDemos_DoubleClick(object sender, EventArgs e)
        {
            RunExample();
        }

        private void comboBoxNamespaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadListBox();
        }

        private void LoadListBox()
        {
            this.listBoxDemos.Items.Clear();

            foreach (string s in this.demoList[this.comboBoxNamespaces.SelectedItem.ToString()].Keys)
            {
                this.listBoxDemos.Items.Add(s);
            }

            this.listBoxDemos.SelectedIndex = 0;
        }

        private void LoadComboBox()
        {
            foreach (string s in this.demoList.Keys)
            {
                this.comboBoxNamespaces.Items.Add(s);
            }
            this.comboBoxNamespaces.SelectedIndex = 0;
        }
    }
}
