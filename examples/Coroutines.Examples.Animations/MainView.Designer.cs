namespace Coroutines.Examples.Animations
{
    partial class MainView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button = new System.Windows.Forms.Button();
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(10, 10);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(52, 42);
            this.button.TabIndex = 0;
            this.button.Text = "Click me!";
            this.button.UseVisualStyleBackColor = true;
            // 
            // MainView
            // 
            this.ClientSize = new System.Drawing.Size(174, 133);
            this.Controls.Add(this.button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View";

        }

        #endregion

        private System.Windows.Forms.Button button;
    }
}

