using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Notes.Data.DTOs.Note;
using Notes.Data.Models;
using Notes.Services;
using System.ComponentModel;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Notes.Repository
{

    public interface IFileRepository
    {
        public Task<NoteFile> Create(IFormFile imageFile);
    }
    public class FileRepository(NotesContext db):IFileRepository
    {
        //BlobServiceCliente
        IConfiguration config;       

        //public async Task<BlobDto> 
        public async Task<NoteFile> Create (IFormFile imageFile)
        {
            try
            {
                //FileService
                var azureBlob = new FileService(config);
                var response = await azureBlob.UploadFile(imageFile);
                var pathFile = response.Blob.Uri;
                

                if(response?.Error ?? false)
                {

                    var result = db.NoteFileModel.AddAsync(new() { Date=new DateTime(),PathFile=response.Blob.Uri});
                    return result.Entity;

                }

                
                
            }
            catch
            {
                return null;
            }
        }


    }
}
