using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


namespace ApiFTP.Models
{
    public class ModeloFTP

    {
        public void UploadFileToFtp(string filePath)
        {
            string ftpUrl = "ftp://127.0.0.1/"; // Cambiar por la dirección IP de tu servidor
            string ftpUsername = "userftp";
            string ftpPassword = "passwordftp";

            string fileName = Path.GetFileName(filePath);
            string uploadUrl = $"{ftpUrl}/{fileName}";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            byte[] fileContents;
            using (StreamReader sourceStream = new StreamReader(filePath))
            {
                fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload status: {response.StatusDescription}");
            }
        }
    }
}