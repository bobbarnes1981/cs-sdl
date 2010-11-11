#region Header

/*
 * Copyright (C) 2010 Pikablu
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

#endregion Header

namespace SdlDotNet.Widgets
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    public class MessageBox : SdlDotNet.Widgets.Window
    {
        #region Fields

        Button btnOptionOne;
        Button btnOptionThree;
        Button btnOptionTwo;
        Label lblText;

        #endregion Fields

        #region Constructors

        MessageBox()
            : base("MessageBox") {
        }

        #endregion Constructors

        #region Methods

        public static DialogResult Show(string message, string caption) {
            return Show(message, caption, MessageBoxButtons.OK);
        }

        public static DialogResult Show(string message, string caption, MessageBoxButtons messageBoxButtons) {
            switch (messageBoxButtons) {
                case MessageBoxButtons.OKCancel: {
                        return Show(message, caption, "OK", "Cancel");
                    }
                case MessageBoxButtons.AbortRetryIgnore: {
                        return Show(message, caption, "Abort", "Retry", "Ignore");
                    }
                case MessageBoxButtons.YesNoCancel: {
                        return Show(message, caption, "Yes", "No", "Cancel");
                    }
                case MessageBoxButtons.YesNo: {
                        return Show(message, caption, "Yes", "No");
                    }
                case MessageBoxButtons.RetryCancel: {
                        return Show(message, caption, "Retry", "Cancel");
                    }
                default:
                case MessageBoxButtons.OK: {
                        return Show(message, caption, "OK");
                    }
            }
        }

        public static DialogResult Show(string message, string caption, string buttonOneText) {
            return Show(message, caption, buttonOneText, null, null);
        }

        public static DialogResult Show(string message, string caption, string buttonOneText, string buttonTwoText) {
            return Show(message, caption, buttonOneText, buttonTwoText, null);
        }

        public static DialogResult Show(string message, string caption, string buttonOneText, string buttonTwoText, string buttonThreeText) {
            MessageBox msgBox = new MessageBox();
            msgBox.TitleBar.CloseButton.Hide();
            msgBox.Text = caption;
            msgBox.lblText = new Label("lblText");
            msgBox.lblText.Size = msgBox.DetermineTextSize(message);
            msgBox.lblText.Location = new Point(5, 5);
            msgBox.lblText.AutoSize = false;
            msgBox.lblText.Text = message;
            msgBox.AddWidget(msgBox.lblText);

            msgBox.Size = new Size(msgBox.lblText.Width + 10, msgBox.lblText.Height + 75);
            msgBox.Location = DrawingSupport.GetCenter(WindowManager.ScreenSize, msgBox.Size);

            string[] buttonText = null;

            if (!string.IsNullOrEmpty(buttonOneText)) {
                if (!string.IsNullOrEmpty(buttonTwoText)) {
                    if (!string.IsNullOrEmpty(buttonThreeText)) {
                        // All three buttons have text
                        buttonText = new string[] { buttonOneText, buttonTwoText, buttonThreeText };
                    } else {
                        // Only buttons one and two have text
                        buttonText = new string[] { buttonOneText, buttonTwoText };
                    }
                } else {
                    // Only button one has text
                    buttonText = new string[] { buttonOneText };
                }
            }

            if (buttonText == null) {
                throw new ArgumentNullException("buttonOneText");
            }

            msgBox.InitializeButtons(buttonText.Length);
            msgBox.SetButtonText(buttonText);

            return msgBox.ShowDialog();
        }

        void btnConfirm_Click(object sender, MouseButtonEventArgs e) {
            this.Close();
        }

        void btnOptionOne_Click(object sender, MouseButtonEventArgs e) {
            switch (btnOptionOne.Text) {
                case "OK": {
                        DialogResult = DialogResult.OK;
                    }
                    break;
                case "Abort": {
                        DialogResult = DialogResult.Abort;
                    }
                    break;
                case "Yes": {
                        DialogResult = DialogResult.Yes;
                    }
                    break;
                case "Retry": {
                        DialogResult = DialogResult.Retry;
                    }
                    break;
            }
            this.Close();
        }

        void btnOptionThree_Click(object sender, MouseButtonEventArgs e) {
            switch (btnOptionThree.Text) {
                case "Cancel": {
                        DialogResult = DialogResult.Cancel;
                    }
                    break;
                case "Ignore": {
                        DialogResult = DialogResult.Ignore;
                    }
                    break;
                default: {
                        DialogResult = DialogResult.CustomThree;
                    }
                    break;
            }
            this.Close();
        }

        void btnOptionTwo_Click(object sender, MouseButtonEventArgs e) {
            switch (btnOptionTwo.Text) {
                case "Cancel": {
                        DialogResult = DialogResult.Cancel;
                    }
                    break;
                case "Retry": {
                        DialogResult = DialogResult.Retry;
                    }
                    break;
                case "No": {
                        DialogResult = DialogResult.No;
                    }
                    break;
                default: {
                        DialogResult = DialogResult.CustomTwo;
                    }
                    break;
            }
            this.Close();
        }

        private Size DetermineTextSize(string text) {
            Size size = TextRenderer.SizeText(lblText.Font, text, false, 300);
            return size;
        }

        private void InitializeButtons(int buttonCount) {
            switch (buttonCount) {
                case 1: {
                        btnOptionOne = new Button("btnOptionOne");
                        btnOptionOne.Size = new Size(100, 15);
                        btnOptionOne.Location = new Point(DrawingSupport.GetCenter(this.Width, btnOptionOne.Width), this.Height - btnOptionOne.Height - 5);
                        btnOptionOne.Click += new EventHandler<MouseButtonEventArgs>(btnOptionOne_Click);

                        AddWidget(btnOptionOne);
                    }
                    break;
                case 2: {
                        btnOptionOne = new Button("btnOptionOne");
                        btnOptionOne.Size = new Size(100, 15);
                        btnOptionOne.Location = new Point(DrawingSupport.GetCenter(this.Width, btnOptionOne.Width) - (btnOptionOne.Width / 2), this.Height - btnOptionOne.Height - 5);
                        btnOptionOne.Click += new EventHandler<MouseButtonEventArgs>(btnOptionOne_Click);

                        btnOptionTwo = new Button("btnOptionTwo");
                        btnOptionTwo.Size = new Size(100, 15);
                        btnOptionTwo.Location = new Point(DrawingSupport.GetCenter(this.Width, btnOptionTwo.Width) + (btnOptionTwo.Width / 2), this.Height - btnOptionTwo.Height - 5);
                        btnOptionTwo.Click += new EventHandler<MouseButtonEventArgs>(btnOptionTwo_Click);

                        AddWidget(btnOptionOne);
                        AddWidget(btnOptionTwo);
                    }
                    break;
                case 3: {
                        btnOptionOne = new Button("btnOptionOne");
                        btnOptionOne.Size = new Size(100, 15);
                        btnOptionOne.Location = new Point(DrawingSupport.GetCenter(this.Width, btnOptionOne.Width) - ((btnOptionOne.Width / 3) * 3) - 1, this.Height - btnOptionOne.Height - 5);
                        btnOptionOne.Click += new EventHandler<MouseButtonEventArgs>(btnOptionOne_Click);

                        btnOptionTwo = new Button("btnOptionTwo");
                        btnOptionTwo.Size = new Size(100, 15);
                        btnOptionTwo.Location = new Point(DrawingSupport.GetCenter(this.Width, btnOptionTwo.Width), this.Height - btnOptionTwo.Height - 5);
                        btnOptionTwo.Click += new EventHandler<MouseButtonEventArgs>(btnOptionTwo_Click);

                        btnOptionThree = new Button("btnOptionThree");
                        btnOptionThree.Size = new Size(100, 15);
                        btnOptionThree.Location = new Point(DrawingSupport.GetCenter(this.Width, btnOptionThree.Width) + ((btnOptionThree.Width / 3) * 3) + 1, this.Height - btnOptionThree.Height - 5);
                        btnOptionThree.Click += new EventHandler<MouseButtonEventArgs>(btnOptionThree_Click);

                        AddWidget(btnOptionOne);
                        AddWidget(btnOptionTwo);
                        AddWidget(btnOptionThree);
                    }
                    break;
            }
        }

        private void SetButtonText(string[] buttonText) {
            switch (buttonText.Length) {
                case 1: {
                        btnOptionOne.Text = buttonText[0];
                    }
                    break;
                case 2: {
                        btnOptionOne.Text = buttonText[0];
                        btnOptionTwo.Text = buttonText[1];
                    }
                    break;
                case 3: {
                        btnOptionOne.Text = buttonText[0];
                        btnOptionTwo.Text = buttonText[1];
                        btnOptionThree.Text = buttonText[2];
                    }
                    break;
            }
        }

        #endregion Methods
    }
}