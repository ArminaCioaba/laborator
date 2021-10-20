using System;
using System.Net;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using System.IO;

namespace L03
{
    class Program
    {
        private static DriveService _service;
        private static string _token;
        static void Main(string[] args)
        {
            Initialize();
        }
     static void Initialize()
    {
        string[] scopes = new string[]{
           DriveService.Scope.Drive, DriveService.Scope.DriveFile
        };

        var clientID="807106198545-lm6totkeouvmkn76sesi77pjgur4ua7o.apps.googleusercontent.com";
        var clientSecret="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";

        var credential=GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId=clientID,
                ClientSecret=clientSecret
            },
            scopes, 
            Environment.UserName,
            CancellationToken.None,


            new FileDataStore("Dainto.GoogleDrive.Auth.Store1")
        ).Result;

        _service=new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
        {
            HttpClientInitializer=credential
        });

        _token=credential.Token.AccessToken;

        Console.Write("Token: "+ credential.Token.AccessToken);

        GetMyFiles();
    }
    static void GetMyFiles()
    {
        var request=(HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
        request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);

        using(var response=request.GetResponse())
        {
            using (Stream data=response.GetResponseStream())
            using (var reader=new StreamReader(data))
            {
                string text=reader.ReadToEnd();
                var pers_data=JObject.Parse(text);
                foreach(var file in pers_data["files"])
                {
                    if(file["mimeType"].ToString()!="application/vnd.google-apps.folder")
                    {
                        Console.WriteLine("File name: "+ file["name"]);
                    }
                }
            }
        }
    }
 
    }

    
}
