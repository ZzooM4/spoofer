using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;

namespace SpooferApp
{
    partial class Spoofer
    {
        // Required designer variable.
        private System.ComponentModel.IContainer components = null;

        // Clean up any resources being used.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Required method for Designer support - do not modify the contents of this method with the code editor.
        private void InitializeComponent()
        {
            AMIDEWINB = new Button();
            RegistryB = new Button();
            VolumeIDB = new Button();
            MacAddressB = new Button();
            DebugOutputB = new Button();
            PeripheralsB = new Button();
            AllB = new Button();
            SuspendLayout();
            // 
            // AMIDEWINB
            // 
            AMIDEWINB.BackColor = SystemColors.ControlDarkDark;
            AMIDEWINB.FlatAppearance.BorderSize = 0;
            AMIDEWINB.FlatStyle = FlatStyle.Flat;
            AMIDEWINB.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            AMIDEWINB.ForeColor = Color.White;
            AMIDEWINB.Location = new Point(25, 35);
            AMIDEWINB.Name = "AMIDEWINB";
            AMIDEWINB.Size = new Size(125, 50);
            AMIDEWINB.TabIndex = 0;
            AMIDEWINB.Text = "AMIDEWIN Spoof";
            AMIDEWINB.UseVisualStyleBackColor = false;
            AMIDEWINB.Click += AMIDEWINB_Click;
            // 
            // RegistryB
            // 
            RegistryB.BackColor = SystemColors.ControlDarkDark;
            RegistryB.FlatAppearance.BorderSize = 0;
            RegistryB.FlatStyle = FlatStyle.Flat;
            RegistryB.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            RegistryB.ForeColor = Color.White;
            RegistryB.Location = new Point(175, 35);
            RegistryB.Name = "RegistryB";
            RegistryB.Size = new Size(125, 50);
            RegistryB.TabIndex = 1;
            RegistryB.Text = "Registry Spoof";
            RegistryB.UseVisualStyleBackColor = false;
            RegistryB.Click += RegistryB_Click;
            // 
            // VolumeIDB
            // 
            VolumeIDB.BackColor = SystemColors.ControlDarkDark;
            VolumeIDB.FlatAppearance.BorderSize = 0;
            VolumeIDB.FlatStyle = FlatStyle.Flat;
            VolumeIDB.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            VolumeIDB.ForeColor = Color.White;
            VolumeIDB.Location = new Point(325, 35);
            VolumeIDB.Name = "VolumeIDB";
            VolumeIDB.Size = new Size(125, 50);
            VolumeIDB.TabIndex = 2;
            VolumeIDB.Text = "VolumeID Spoof";
            VolumeIDB.UseVisualStyleBackColor = false;
            VolumeIDB.Click += VolumeIDB_Click;
            // MacAddressB
            MacAddressB.BackColor = SystemColors.ControlDarkDark;
            MacAddressB.FlatAppearance.BorderSize = 0;
            MacAddressB.FlatStyle = FlatStyle.Flat;
            MacAddressB.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            MacAddressB.ForeColor = Color.White;
            MacAddressB.Location = new Point(25, 105);
            MacAddressB.Name = "MacAddressB";
            MacAddressB.Size = new Size(125, 50);
            MacAddressB.TabIndex = 3;
            MacAddressB.Text = "Mac Address Spoof";
            MacAddressB.UseVisualStyleBackColor = false;
            MacAddressB.Click += MacAddressB_Click;
            // 
            // DebugOutputB
            // 
            DebugOutputB.BackColor = SystemColors.ControlDarkDark;
            DebugOutputB.FlatAppearance.BorderSize = 0;
            DebugOutputB.FlatStyle = FlatStyle.Flat;
            DebugOutputB.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            DebugOutputB.ForeColor = Color.White;
            DebugOutputB.Location = new Point(175, 105);
            DebugOutputB.Name = "DebugOutputB";
            DebugOutputB.Size = new Size(125, 50);
            DebugOutputB.TabIndex = 4;
            DebugOutputB.Text = "Enable Debug Output";
            DebugOutputB.UseVisualStyleBackColor = false;
            DebugOutputB.Click += DebugOutputB_Click;
            // 
            // PeripheralsB
            // 
            PeripheralsB.BackColor = SystemColors.ControlDarkDark;
            PeripheralsB.FlatAppearance.BorderSize = 0;
            PeripheralsB.FlatStyle = FlatStyle.Flat;
            PeripheralsB.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            PeripheralsB.ForeColor = Color.White;
            PeripheralsB.Location = new Point(325, 105);
            PeripheralsB.Name = "PeripheralsB";
            PeripheralsB.Size = new Size(125, 50);
            PeripheralsB.TabIndex = 5;
            PeripheralsB.Text = "Block Peripherals Access";
            PeripheralsB.UseVisualStyleBackColor = false;
            PeripheralsB.Click += PeripheralsB_Click;
            // 
            // AllB
            // 
            AllB.BackColor = SystemColors.ControlDarkDark;
            AllB.FlatAppearance.BorderSize = 0;
            AllB.FlatStyle = FlatStyle.Flat;
            AllB.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            AllB.ForeColor = Color.White;
            AllB.Location = new Point(175, 175);
            AllB.Name = "AllB";
            AllB.Size = new Size(125, 50);
            AllB.TabIndex = 6;
            AllB.Text = "All of it";
            AllB.UseVisualStyleBackColor = false;
            AllB.Click += AllB_Click;
            // 
            // Spoofer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(475, 250);
            Controls.Add(AMIDEWINB);
            Controls.Add(RegistryB);
            Controls.Add(VolumeIDB);
            Controls.Add(MacAddressB);
            Controls.Add(DebugOutputB);
            Controls.Add(PeripheralsB);
            Controls.Add(AllB);
            Name = "Spoofer";
            Text = "Spoofer";
            ResumeLayout(false);
        }
        #endregion

        private Button AMIDEWINB;
        private Button RegistryB;
        private Button VolumeIDB;
        private Button MacAddressB;
        private Button DebugOutputB;
        private Button PeripheralsB;
        private Button AllB;
    }
}

