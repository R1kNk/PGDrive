﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using PGoogleDrive.Internal.Models;
using PGoogleDrive.Internal.Models.Scopes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace PGoogleDrive.Internal.Services
{
    class Auth
    {


        public string ApplicationName { get; set; }
        public static FileDataStore FileDataStore { get; private set; }

        public DriveService GetDriveService(string pGDriveConfigElementName, PGScope scopes = null, bool recreateOAuthToken = false)
        {
            OAuthGDriveElement oAuth = PGDriveConfig.GetOAuthElement(pGDriveConfigElementName);
            if (oAuth != null)
            {
                List<string> scopesList = new List<string>();
                if (scopes != null) scopesList = scopes.Scopes;
                ApplicationName = oAuth.ApplicationName;
                return GetOAuthDriveService(oAuth.ApplicationName, oAuth.ClientSecretPath, scopesList.ToArray(), recreateOAuthToken);
            }
            ApiKeyGDriveElement apiKey = PGDriveConfig.GetApiKeyElement(pGDriveConfigElementName);
            if (apiKey != null)
            {
                ApplicationName = apiKey.ApplicationName;
                return GetApiKeyDriveService(apiKey.ApplicationName, apiKey.ApiKey);
            }
            throw new NullReferenceException("There is no drive with such name in PGDrive Config Section");

        }

         public DriveService GetApiKeyDriveService(string ApplicationName, string ApiKey)
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                ApiKey = ApiKey,
                ApplicationName = ApplicationName,
            }
            );
        }
        public DriveService GetOAuthDriveService(string ApplicationName, string ClientSecretPath, string[] scopes, bool recreateOAuthToken = false)
        {
            UserCredential credential = GetUserCredential(ApplicationName, ClientSecretPath, scopes, recreateOAuthToken);
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            }
            );
        }

         static UserCredential GetUserCredential(string ApplicationName, string ClientSecretPath, string[] scopes, bool recreateOAuthToken = false)
        {
                string exactPath = Path.GetFullPath(ClientSecretPath);
                string CredentialPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                CredentialPath = Path.Combine(CredentialPath, "driveApiCredentials", ApplicationName + "_drive-credentials.json");
                FileDataStore filedataStore = new FileDataStore(CredentialPath, true);
            if (recreateOAuthToken)
            {
                filedataStore.DeleteAsync<TokenResponse>(Environment.UserName);
            }

            using (var stream = new FileStream(exactPath, FileMode.Open, FileAccess.Read))
                {
                   
                    var authToken =  GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        scopes,
                        Environment.UserName,
                      CancellationToken.None, filedataStore).Result;
                //string lul = FileDataStore.GenerateStoredKey("auction", typeof(UserCredential));
               // FileDataStore.GetAsync<>
                //TokenResponse tokenResponse = new TokenResponse();
                //tokenResponse.
                //FileDataStore.DeleteAsync<FileDataStore>(lul);
                return authToken;
                }
           

        }
    }
}