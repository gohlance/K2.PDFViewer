using SourceCode.Forms.Controls.Web.SDK.Attributes;
using System;
using System.Configuration;
using System.Web;
using SCHC = SourceCode.Hosting.Client.BaseAPI;
using SCSMOC = SourceCode.SmartObjects.Client;

namespace K2Field.PDFViewer
{   [ClientAjaxHandler("GetFileFromIsolatedStorage.handler")]
    public class GetFile : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

           public void ProcessRequest(HttpContext context)
        {
            //write your handler implementation here.
            string pdfid = context.Request.Form["control"];

            SCSMOC.SmartObjectClientServer smoSvr = new SCSMOC.SmartObjectClientServer();
            try
            {
                smoSvr.CreateConnection();
                smoSvr.Connection.Open(GetSMOConnStr());

                SCSMOC.SmartObject smoObj = smoSvr.GetSmartObject("PDFFile");
                smoObj.Properties["ID"].Value = pdfid;
                smoObj.MethodToExecute = "Load";
                smoObj = smoSvr.ExecuteScalar(smoObj);

                
             string temp =    smoObj.Properties["PDF"].Value.ToString();
             string output = temp.Substring(temp.IndexOf("<content>") + "<content>".Length, temp.Length - temp.IndexOf("<content>") - "<content>".Length - "</content></file>".Length);
                string newFileName = string.Empty;
             try
             {
                 string filePath = AppDomain.CurrentDomain.BaseDirectory + "Files\\"; //file storage.
                 System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                 file.Directory.Create();
                 newFileName = Guid.NewGuid().ToString() + ".pdf";
                 string newFilePath = filePath + newFileName.ToLower();
                 byte[] fileBinary = Convert.FromBase64String(output);
                 System.IO.File.WriteAllBytes(newFilePath, fileBinary);
               
             }
             catch (Exception ex)
             {
                newFileName= "2|" + ex.Message + '|' + ex.StackTrace;
             }
             string path = "https://" + context.Request.Url.Host + "/Runtime/Files/" + newFileName;
                context.Response.Write(path);
            }
            finally
            {
                if (smoSvr.Connection != null && smoSvr.Connection.IsConnected)
                    smoSvr.Connection.Dispose();
            }
            
        }

           private static void SavetoTempfolder() {
              
           }
        private static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        #endregion

        private string GetSMOConnStr()
        {
            SCHC.SCConnectionStringBuilder connStr = new SCHC.SCConnectionStringBuilder();
            connStr.Host = ConfigurationManager.AppSettings["HostName"];
            connStr.Port = Convert.ToUInt32(ConfigurationManager.AppSettings["HostPort"]);
            connStr.Integrated = true;
            connStr.IsPrimaryLogin = true;
            return connStr.ConnectionString;
        }

      
    }
}
