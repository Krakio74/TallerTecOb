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

        public static List<string> GetfullFileName(string? Id)
        {
            var filePath = FileSystem.CacheDirectory;
            var files = Directory.GetFiles(filePath);
            List<string> FileName = new List<string>();
            if (Id != null)
            {
                foreach (var file in files)
                {
                    string extension = Path.GetExtension(file);
                    string name = Path.GetFileNameWithoutExtension(file);
                    FileName.Add(name + extension);
                }
            }
            else
            {
                foreach (var file in files)
                {
                    if (Path.GetFileNameWithoutExtension(file) == Id)
                    {
                        string extension = Path.GetExtension(file);
                        string name = Path.GetFileNameWithoutExtension(file);
                        FileName.Add(name + extension);
                    }
                }
            }
            return FileName;
        }
    }
}
