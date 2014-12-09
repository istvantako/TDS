namespace TDSFormApp
{
    partial class SaveEntityForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.entityCombobox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.overwrite_CheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.Location = new System.Drawing.Point(56, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 339);
            this.panel1.TabIndex = 5;
            // 
            // entityCombobox
            // 
            this.entityCombobox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.entityCombobox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.entityCombobox.FormattingEnabled = true;
            this.entityCombobox.Location = new System.Drawing.Point(136, 33);
            this.entityCombobox.Name = "entityCombobox";
            this.entityCombobox.Size = new System.Drawing.Size(163, 21);
            this.entityCombobox.TabIndex = 4;
            this.entityCombobox.SelectedIndexChanged += new System.EventHandler(this.entityCombobox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(53, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Entity:";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(285, 426);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(383, 426);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 7;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // overwrite_CheckBox
            // 
            this.overwrite_CheckBox.AutoSize = true;
            this.overwrite_CheckBox.Location = new System.Drawing.Point(285, 403);
            this.overwrite_CheckBox.Name = "overwrite_CheckBox";
            this.overwrite_CheckBox.Size = new System.Drawing.Size(71, 17);
            this.overwrite_CheckBox.TabIndex = 8;
            this.overwrite_CheckBox.Text = "Overwrite";
            this.overwrite_CheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveEntityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 470);
            this.Controls.Add(this.overwrite_CheckBox);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.entityCombobox);
            this.Controls.Add(this.label1);
            this.Name = "SaveEntityForm";
            this.Text = "Save Entity Form";
            this.Load += new System.EventHandler(this.SaveEntityForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox entityCombobox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.CheckBox overwrite_CheckBox;
    }
}

