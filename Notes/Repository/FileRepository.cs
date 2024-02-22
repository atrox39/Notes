using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Notes.Data.DTOs.Note;
using Notes.Data.Models;
using System.ComponentModel;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Notes.Repository
{

    public interface IFileRepository
    {
        public Task<NoteFile> Create(NoteFile file,IFormFile imageFile);
    }
    public class FileRepository(NotesContext db):IFileRepository
    {

        //BlobServiceCliente
        WebApplication builder;
        IConfiguration config;
        
            //.GetSection("AppSettings");
        private readonly BlobServiceClient blobServiceClient;        

        public async Task<NoteFile> Create (NoteFile file,IFormFile imageFile)
        {
            try
            {
                
                string azureKey = config.GetValue<string>("blob:connectionString");
                string account = config.GetValue<string>("blob:StorageAccount");
                var credential = new StorageSharedKeyCredential(account,azureKey);
                string blobUri = $"https://{account}.blob.core.windows.net/";
                var blobServiceClient= new BlobServiceClient(new Uri(blobUri),credential);
                //Container
                var containerInstance = blobServiceClient.GetBlobContainerClient("stuff-files");
                //Upload file
                var blobInstace = containerInstance.GetBlobClient(imageFile.FileName);
                await blobInstace.UploadAsync(imageFile.OpenReadStream());


                var result = await db.NoteFileModel.AddAsync(file);
                return result.Entity;
            }
            catch
            {
                return null;
            }
        }


    }
}
