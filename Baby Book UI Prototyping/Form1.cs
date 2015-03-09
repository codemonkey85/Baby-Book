using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Drive.v2;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Drive.v2.Data;
namespace Baby_Book_UI_Prototyping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DriveService service;
        bool loggedin = false;
        private void button1_Click(object sender, EventArgs e)
        {
            Login();
            if (loggedin)
            {
                File body = new File();
                body.Title = "My document.txt";
                body.Description = "A test document";
                body.MimeType = "text/plain";
                byte[] byteArray = Encoding.ASCII.GetBytes(txtBody.Text.ToCharArray());
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, "text/plain");
                request.Upload();
                File file = request.ResponseBody;
            }
        }
        private void Login()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,
                                 DriveService.Scope.DriveFile};
            UserCredential credential =
                        GoogleWebAuthorizationBroker
                                      .AuthorizeAsync(new ClientSecrets
                                      {
                                          ClientId = Properties.Settings.Default.CLIENT_ID
                                      ,
                                          ClientSecret = Properties.Settings.Default.CLIENT_SECRET
                                      }
                                                      , scopes
                                                      , Environment.UserName
                                                      , CancellationToken.None
                                                      , new FileDataStore("Journaling.GoogleDrive.Auth.Store")
                                                      ).Result;
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Journaling",
            });
            loggedin = true;
        }
    }
}
