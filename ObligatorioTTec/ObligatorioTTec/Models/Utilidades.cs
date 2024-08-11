using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTTec.Models
{
    internal class Utilidades
    {
        public async static void TomarFoto(int ID)
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, ID + Path.GetExtension(photo.FileName));
                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);
                await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }
}
