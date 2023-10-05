using Azure.Storage.Blobs;

namespace E_Learning.API.Controllers.blob
{
    public interface IBlobService
    {

          Task<Uri> UploadFileBlobAsync(string blobContainerName, Stream content, string contentType, string fileName);


    }

}
