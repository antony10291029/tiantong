using System;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Tiantong.Wms.Api
{
  public class Mail : TaskQueue
  {
    private string _stmpHost;

    public int _stmpPort;

    public string _stmpAddress;

    public string _stmpPassword;

    public Mail(Config config)
    {
      _stmpHost = config.STMP_HOST;
      _stmpPort = config.STMP_PORT;
      _stmpAddress = config.STMP_ADDRESS;
      _stmpPassword = config.STMP_PASSWORD;
    }

    public void Send(MailMessage msg)
    {
      Enqueue(async CancellationToken => {
        var client = new SmtpClient(_stmpHost, _stmpPort);
        client.Credentials = new NetworkCredential(_stmpAddress, _stmpPassword);
        await client.SendMailAsync(msg);
      });
    }

    public void ResetPassword(string email, string verifyCode)
    {
      var msg = new MailMessage();
      msg.From = new MailAddress(_stmpAddress, "天瞳WMS");
      msg.To.Add(new MailAddress(email));
      msg.Subject = "密码找回";
      string html = $"您的邮箱验证码为: {verifyCode}";
      msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
    }
  }

  public class MailHostedService : QueueService
  {
    public MailHostedService(Mail mailQueue): base(mailQueue)
    {

    }

  }
}
