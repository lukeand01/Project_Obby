using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;


public class ReportUI : MonoBehaviour
{
    //can send a bug or anything.

    GameObject holder;
    [SerializeField] TMP_InputField inputField;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void StartReport()
    {
        inputField.text = string.Empty;
    }

    void CloseReport()
    {

    }

    public void SendReport()
    {
        if(inputField.text.Length <= 10)
        {
            return;
        }

        if(inputField.text.Length > 100)
        {
            return;
        }


        //send the report.
        //close it
        //maybe a give cooldown to limit spannig.

        SendEmail();

        CloseReport();
    }

    const string EMAIL = "";

    [ContextMenu("DEBUG SEND EMAIL")]
    public void SendEmail()
    {
        MailMessage mail = new MailMessage();
        SmtpClient smtp = new SmtpClient("smtp.gmail.com"); //this is the email.
        smtp.Timeout = 1000;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.UseDefaultCredentials = true;
        smtp.Port = 586;

        mail.From = new MailAddress("obbyd62@gmail.com");
        mail.To.Add(new MailAddress(EMAIL));
        mail.Subject = "teste";
        mail.Body = "this is subject";


        smtp.Credentials = new System.Net.NetworkCredential("obbyd62@gmail.com", "Y3eqPKgA") as ICredentialsByHost; smtp.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyError)
        {
            return true;
        };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        smtp.Send(mail);


    }

}
