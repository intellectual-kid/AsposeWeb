using AsposeWeb.Api.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsposeWeb.DesktopClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Please type file name. ");
                return;
            }

            try
            {
                var client = AsposeApiHttpClient.GetClient();
                ProtectionData uploadResult = null;
                bool _responseReceived = false;                

              Protection protect =  JsonConvert.DeserializeObject<Protection>(richTextBox1.Text);
               var serializedItemToCreate = JsonConvert.SerializeObject(protect);

                Task taskUpload = client.PostAsync("words/"+textBox4.Text.ToString() +"/protection", new StringContent(serializedItemToCreate,
                        System.Text.Encoding.Unicode,
                        "application/json")).ContinueWith(task =>                    
                    {
                        if (task.Status == TaskStatus.RanToCompletion)
                        {
                            var response = task.Result;

                            if (response.IsSuccessStatusCode)
                            {
                                uploadResult = response.Content.ReadAsAsync<ProtectionData>().Result;
                                if (uploadResult != null)
                                    _responseReceived = true;

                                // Read other header values if you want..
                                foreach (var header in response.Content.Headers)
                                {
                                    Debug.WriteLine("{0}: {1}", header.Key, string.Join(",", header.Value));
                                }
                            }
                            else
                            {
                                Debug.WriteLine("Status Code: {0} - {1}", response.StatusCode, response.ReasonPhrase);
                                Debug.WriteLine("Response Body: {0}", response.Content.ReadAsStringAsync().Result);
                            }
                        }
                
                    });

                    taskUpload.Wait();
                    if (_responseReceived)

                        textBox5.Text = uploadResult.ProtectionType + " has been applied on given file name .";

                client.Dispose();
            }
            catch (Exception ex)
            {
                textBox5.Text = ex.Message.ToString();
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ( string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Please type file name. ");
                return;
            }

            var client = AsposeApiHttpClient.GetClient();
            ProtectionData uploadResult = null;
            bool _responseReceived = false;

            Task taskUpload = client.GetAsync("words/"+textBox3.Text.ToString() +"/protection").ContinueWith(task =>
                        {
                            if (task.Status == TaskStatus.RanToCompletion)
                            {
                                var response = task.Result;

                                if (response.IsSuccessStatusCode)
                                {
                                    uploadResult = response.Content.ReadAsAsync<ProtectionData>().Result;
                                    if (uploadResult != null)
                                        _responseReceived = true;                                    

                                    // Read other header values if you want..
                                    foreach (var header in response.Content.Headers)
                                    {
                                        Debug.WriteLine("{0}: {1}", header.Key, string.Join(",", header.Value));
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("Status Code: {0} - {1}", response.StatusCode, response.ReasonPhrase);
                                    Debug.WriteLine("Response Body: {0}", response.Content.ReadAsStringAsync().Result);
                                }
                            }


                        });

              taskUpload.Wait();
            client.Dispose();
            if (_responseReceived)

                textBox2.Text = uploadResult.ProtectionType + " type is on given file name .";



        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var client = AsposeApiHttpClient.GetClient();

                    // Read the files
                    foreach (String file in openFileDialog1.FileNames)
                    {

                        var fileStream = File.Open(file, FileMode.Open);
                        var fileInfo = new FileInfo(file);
                        FileUploadResult uploadResult = null;
                        bool _fileUploaded = false;

                        var content = new MultipartFormDataContent();
                        content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", fileInfo.Name)
                        );

                        Task taskUpload = client.PutAsync("words/convert", content).ContinueWith(task =>
                        //Task taskUpload = httpClient.PostAsync(uploadServiceBaseAddress, content).ContinueWith(task =>
                        {
                            if (task.Status == TaskStatus.RanToCompletion)
                            {
                                var response = task.Result;

                                if (response.IsSuccessStatusCode)
                                {
                                    uploadResult = response.Content.ReadAsAsync<FileUploadResult>().Result;
                                    if (uploadResult != null)
                                        _fileUploaded = true;

                                    // Read other header values if you want..
                                    foreach (var header in response.Content.Headers)
                                    {
                                        Debug.WriteLine("{0}: {1}", header.Key, string.Join(",", header.Value));
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("Status Code: {0} - {1}", response.StatusCode, response.ReasonPhrase);
                                    Debug.WriteLine("Response Body: {0}", response.Content.ReadAsStringAsync().Result);
                                }
                            }

                            fileStream.Dispose();
                        });

                        taskUpload.Wait();
                        if (_fileUploaded)

                            textBox1.Text = uploadResult.FileName + " with length " + uploadResult.FileLength
                                            + " has been uploaded at " + uploadResult.LocalFilePath;
                            
                    }

                    client.Dispose();
                }
                catch (Exception ex)
                {
                    textBox1.Text = ex.Message.ToString();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox6.Text = "hi.doc , test.doc ,B anothertest.doc";
        }
    }
}
