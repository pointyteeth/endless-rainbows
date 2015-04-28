using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class LogEmailer : MonoBehaviour {
    
    public string gmail;
    public string appPassword;
    SmtpClient smtpServer;
    
    void Start() {
        smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(gmail, appPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            return true;
        };
    }
    
    void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type) {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(gmail);
        mail.To.Add(gmail);
        mail.Subject = "[endless rainbows] " + type.ToString() + ": " + logString;
        mail.Body = logString + "\n\n" + stackTrace;
        smtpServer.Send(mail);
    }
}