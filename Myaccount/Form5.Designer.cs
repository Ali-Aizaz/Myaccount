namespace Myaccount
{
    partial class Form5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.StateProperties();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.StateProperties();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Newpass = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.skt = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.npt = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.cnpt = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Raleway", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(114, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome New User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(137, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Create New Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(88, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Security Key:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(71, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "New Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Confirm New Password:";
            // 
            // Newpass
            // 
            this.Newpass.AllowToggling = false;
            this.Newpass.AnimationSpeed = 200;
            this.Newpass.AutoGenerateColors = false;
            this.Newpass.BackColor = System.Drawing.Color.Transparent;
            this.Newpass.BackColor1 = System.Drawing.Color.DodgerBlue;
            this.Newpass.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Newpass.BackgroundImage")));
            this.Newpass.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.Newpass.ButtonText = "Create New Password";
            this.Newpass.ButtonTextMarginLeft = 0;
            this.Newpass.ColorContrastOnClick = 45;
            this.Newpass.ColorContrastOnHover = 45;
            this.Newpass.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.Newpass.CustomizableEdges = borderEdges2;
            this.Newpass.DialogResult = System.Windows.Forms.DialogResult.None;
            this.Newpass.DisabledBorderColor = System.Drawing.Color.Empty;
            this.Newpass.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.Newpass.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.Newpass.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.Newpass.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.Newpass.ForeColor = System.Drawing.Color.White;
            this.Newpass.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.Newpass.IconMarginLeft = 11;
            this.Newpass.IconPadding = 10;
            this.Newpass.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.Newpass.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this.Newpass.IdleBorderRadius = 3;
            this.Newpass.IdleBorderThickness = 1;
            this.Newpass.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this.Newpass.IdleIconLeftImage = null;
            this.Newpass.IdleIconRightImage = null;
            this.Newpass.IndicateFocus = false;
            this.Newpass.Location = new System.Drawing.Point(314, 212);
            this.Newpass.Name = "Newpass";
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.BorderRadius = 3;
            stateProperties3.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            stateProperties3.BorderThickness = 1;
            stateProperties3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.ForeColor = System.Drawing.Color.White;
            stateProperties3.IconLeftImage = null;
            stateProperties3.IconRightImage = null;
            this.Newpass.onHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            stateProperties4.BorderRadius = 3;
            stateProperties4.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            stateProperties4.BorderThickness = 1;
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            stateProperties4.ForeColor = System.Drawing.Color.White;
            stateProperties4.IconLeftImage = null;
            stateProperties4.IconRightImage = null;
            this.Newpass.OnPressedState = stateProperties4;
            this.Newpass.Size = new System.Drawing.Size(146, 36);
            this.Newpass.TabIndex = 5;
            this.Newpass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Newpass.TextMarginLeft = 0;
            this.Newpass.UseDefaultRadiusAndThickness = true;
            this.Newpass.Click += new System.EventHandler(this.Newpass_Click);
            // 
            // skt
            // 
            this.skt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.skt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.skt.characterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.skt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.skt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.skt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.skt.HintForeColor = System.Drawing.Color.Empty;
            this.skt.HintText = "";
            this.skt.isPassword = false;
            this.skt.LineFocusedColor = System.Drawing.Color.Blue;
            this.skt.LineIdleColor = System.Drawing.Color.Gray;
            this.skt.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.skt.LineThickness = 3;
            this.skt.Location = new System.Drawing.Point(195, 89);
            this.skt.Margin = new System.Windows.Forms.Padding(4);
            this.skt.MaxLength = 32767;
            this.skt.Name = "skt";
            this.skt.Size = new System.Drawing.Size(265, 33);
            this.skt.TabIndex = 6;
            this.skt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // npt
            // 
            this.npt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.npt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.npt.characterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.npt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.npt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.npt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.npt.HintForeColor = System.Drawing.Color.Empty;
            this.npt.HintText = "";
            this.npt.isPassword = false;
            this.npt.LineFocusedColor = System.Drawing.Color.Blue;
            this.npt.LineIdleColor = System.Drawing.Color.Gray;
            this.npt.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.npt.LineThickness = 3;
            this.npt.Location = new System.Drawing.Point(195, 124);
            this.npt.Margin = new System.Windows.Forms.Padding(4);
            this.npt.MaxLength = 32767;
            this.npt.Name = "npt";
            this.npt.Size = new System.Drawing.Size(265, 33);
            this.npt.TabIndex = 7;
            this.npt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // cnpt
            // 
            this.cnpt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cnpt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cnpt.characterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cnpt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cnpt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cnpt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cnpt.HintForeColor = System.Drawing.Color.Empty;
            this.cnpt.HintText = "";
            this.cnpt.isPassword = false;
            this.cnpt.LineFocusedColor = System.Drawing.Color.Blue;
            this.cnpt.LineIdleColor = System.Drawing.Color.Gray;
            this.cnpt.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.cnpt.LineThickness = 3;
            this.cnpt.Location = new System.Drawing.Point(195, 159);
            this.cnpt.Margin = new System.Windows.Forms.Padding(4);
            this.cnpt.MaxLength = 32767;
            this.cnpt.Name = "cnpt";
            this.cnpt.Size = new System.Drawing.Size(265, 33);
            this.cnpt.TabIndex = 8;
            this.cnpt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 260);
            this.Controls.Add(this.cnpt);
            this.Controls.Add(this.npt);
            this.Controls.Add(this.skt);
            this.Controls.Add(this.Newpass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ceate User Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton Newpass;
        private Bunifu.Framework.UI.BunifuMaterialTextbox skt;
        private Bunifu.Framework.UI.BunifuMaterialTextbox npt;
        private Bunifu.Framework.UI.BunifuMaterialTextbox cnpt;

    }
}