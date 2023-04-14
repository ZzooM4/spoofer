using System.Net.Mail;
using System.Windows.Forms;

namespace SpooferApp
{
    public static class GlobalSettings
    {
        public static bool DebugOutput { get; set; }
    }

    public partial class Spoofer : Form
    {
        public Spoofer()
        {
            InitializeComponent();
        }

        private void AMIDEWINB_Click(object sender, EventArgs e)
        {
            AMIDEWIN.SpoofAMIDEWIN();
            MessageBox.Show("AMIDEWIN spoofed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RegistryB_Click(object sender, EventArgs e)
        {
            RegistryChange.SpoofRegistry();
            MessageBox.Show("Registry GUID addresses spoofed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void VolumeIDB_Click(object sender, EventArgs e)
        {
            VolumeID.SpoofVolumeID();
            MessageBox.Show("Volume ID spoofed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MacAddressB_Click(object sender, EventArgs e)
        {
            MacAddress.SpoofMacAddress();
            MessageBox.Show("MAC address spoofed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DebugOutputB_Click(object sender, EventArgs e)
        {
            GlobalSettings.DebugOutput = !GlobalSettings.DebugOutput;
            Program.ToggleConsole();
            string message = GlobalSettings.DebugOutput ? "Debug output has been enabled!" : "Debug output has been disabled!";
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PeripheralsB_Click(object sender, EventArgs e)
        {
            Peripherals.BlockPeripherals();
            MessageBox.Show("Peripherals blocked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AllB_Click(object sender, EventArgs e)
        {
            AMIDEWIN.SpoofAMIDEWIN();
            RegistryChange.SpoofRegistry();
            VolumeID.SpoofVolumeID();
            MacAddress.SpoofMacAddress();
            Peripherals.BlockPeripherals();
            MessageBox.Show("All settings spoofed and peripherals blocked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
