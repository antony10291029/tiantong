using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Tiantong.Iot.Api
{
  public class Mail
  {
    private string _stmpHost;

    private int _stmpPort;

    private string _stmpAddress;

    private string _stmpPassword;

    public Mail(Config config)
    {
      _stmpHost = config.STMP_HOST;
      _stmpPort = config.STMP_PORT;
      _stmpAddress = config.STMP_ADDRESS;
      _stmpPassword = config.STMP_PASSWORD;
    }

    private async Task SendAsync(MailMessage msg)
    {
      var client = new SmtpClient(_stmpHost, _stmpPort);
      client.Credentials = new NetworkCredential(_stmpAddress, _stmpPassword);
      await client.SendMailAsync(msg);
    }

    public async Task SendVerifyCodeAsync(string email, string verifyCode, string subject = "邮箱验证")
    {
      var msg = new MailMessage();
      msg.From = new MailAddress(_stmpAddress, "天瞳IOT");
      msg.To.Add(new MailAddress(email));
      msg.Subject = subject;
      string html = $"您的邮箱验证码为：{verifyCode}";
      msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
      await SendAsync(msg);
    }

  }

}
