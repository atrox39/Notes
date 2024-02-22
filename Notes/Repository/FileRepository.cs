using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Notes.Data.DTOs.Note;
using Notes.Data.Models;
using System.ComponentModel;
using System.IO;

namespace Notes.Repository
{
    public class FileRepository(NotesContext db)
    {

        //BlobServiceCliente
        private readonly BlobServiceClient blobServiceClient;
        public interface IFileRepository
        {
            Task<NoteFile> Create(NoteFile file);
        }       

        
        public async Task<NoteFile> Create (NoteFile file,IFormFile imageFile)
        {

            //Container
            var containerInstance = blobServiceClient.GetBlobContainerClient("stuff-files");
            //Upload file
            var blobInstace = containerInstance.GetBlobClient(imageFile.FileName);            
            await blobInstace.UploadAsync(imageFile.OpenReadStream());


            var result = await db.NoteFileModel.AddAsync(file);
            return result.Entity;
        }


    }
}
