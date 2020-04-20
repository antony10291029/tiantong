using System;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Tiantong.Wms.Api
{
  public class Mail : TaskQueue
  {
    private MailAddress _fromAddress;

    public Mail(IConfiguration config)
    {
      _fromAddress = new MailAddress("postman@wms.als-yuchuan.com", "天瞳WMS");
    }

    public void Send(string email, string code)
    {
      Enqueue(async CancellationToken => {
        var msg = new MailMessage();
        var toAddress = new MailAddress(email);
        Console.WriteLine(email, code);

        msg.To.Add(toAddress);
        msg.From = _fromAddress;
        msg.ReplyToList.Add(_fromAddress);

        msg.Subject = "密码找回";
        string html = $@"您的邮箱验证码为: {code}";
        msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

        var client = new SmtpClient("smtpdm.aliyun.com", 25);
        var credentials = new NetworkCredential("postman@wms.als-yuchuan.com", "TiantongWms9217");
        client.Credentials = credentials;
        try {
          await client.SendMailAsync(msg);
          Console.WriteLine("邮件已发送");
        } catch (Exception e) {
          Console.WriteLine(e.Message);
        }
      });
    }
  }

  public class MailHostedService : QueueService
  {
    public MailHostedService(Mail mailQueue): base(mailQueue)
    {

    }

  }
}
