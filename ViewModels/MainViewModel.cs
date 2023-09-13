using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Online_Resume_Viewer.Commands;
using Online_Resume_Viewer.Models;
using MongoDB.Driver;

namespace Online_Resume_Viewer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string Title { get; set; } = "Resume Viewer App";
        public ObservableCollection<DocumentModel> Documents { get; set; } = new ObservableCollection<DocumentModel>();

        // Add commands for uploading and downloading documents
        public ICommand UploadDocumentCommand { get; set; }
        public ICommand DownloadDocumentCommand { get; set; }

        // Implement constructor and methods here

        public MainViewModel()
        {
            // Initialize the commands
            UploadDocumentCommand = new RelayCommand(UploadDocument);
            DownloadDocumentCommand = new RelayCommand(DownloadDocument);
        }

        public async void UploadDocument()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file path
                string filePath = openFileDialog.FileName;
                string title = Path.GetFileName(filePath);

                // Read the PDF content as bytes
                byte[] pdfContent = File.ReadAllBytes(filePath);

                // Create a DocumentModel and add it to the ObservableCollection
                var document = new DocumentModel
                {
                    Title = title,
                    FilePath = filePath
                };

                var check = Documents.Where(X => X.Title == title).FirstOrDefault();
                if (check != null)
                {
                    //new screen textbox that says pdf already exists
                    //or delete and insert this new one to rewrite
                }
                else
                {
                    string connectionString = "mongodb+srv://wesleyhuhuhu:wesleyhuhuhu123@cluster0.mwrmduh.mongodb.net/";
                    string databaseName = "Resume";
                    string collectionName = "document";
                    var client = new MongoClient(connectionString);
                    var db = client.GetDatabase(databaseName);
                    var collection = db.GetCollection<DocumentModel>(collectionName);

                    var newDoc = new DocumentModel { Title = Path.GetFileName(filePath), FilePath = filePath, content = pdfContent };

                    await collection.InsertOneAsync(newDoc);

                    Documents.Add(document);
                }

                // Add code to store pdfContent in MongoDB
                // You can call your MongoDB integration code here
                
            }
        }

        public async void DownloadDocument()
        {
            /*OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file path
                string filePath = openFileDialog.FileName;

                // Read the PDF content as bytes
                byte[] pdfContent = File.ReadAllBytes(filePath);

                // Create a DocumentModel and add it to the ObservableCollection
                var document = new DocumentModel
                {
                    Title = Path.GetFileName(filePath),
                    FilePath = filePath
                };

                Documents.Add(document);

                // Add code to store pdfContent in MongoDB
                // You can call your MongoDB integration code here
            }*/
            string connectionString = "mongodb+srv://wesleyhuhuhu:wesleyhuhuhu123@cluster0.mwrmduh.mongodb.net/";
            string databaseName = "Resume";
            string collectionName = "document";
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<DocumentModel>(collectionName);

            var results = await collection.FindAsync(_ => true);

            foreach (var result in results.ToList())
            {
                string title = result.Title;
                var check = Documents.Where(X => X.Title == title).FirstOrDefault();
                if (check != null)
                {
     
                }
                else
                {
                    Documents.Add(result);
                }
            }
        }


    }
}
