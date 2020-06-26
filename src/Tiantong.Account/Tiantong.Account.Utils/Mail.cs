using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Tiantong.Account.Utils
{
  public class Mail
  {
    private string _stmpHost;

    private int _stmpPort;

    private string _stmpAddress;

    private string _stmpPassword;

    public Mail()
    {
      _stmpHost = "smtpdm.aliyun.com";
      _stmpPort = 80;
      _stmpAddress = "admin@post.als-yuchuan.com";
      _stmpPassword = "TiantongAeoikj9217";
    }

    private async Task SendAsync(MailMessage msg)
    {
      var client = new SmtpClient(_stmpHost, _stmpPort);
      client.Credentials = new NetworkCredential(_stmpAddress, _stmpPassword);
      await client.SendMailAsync(msg);
    }

    public async Task SendAsync(string email, string subject, string html, MailAddress from = null)
    {
      var msg = new MailMessage();
      msg.From ??= new MailAddress(_stmpAddress, "天瞳账户");
      msg.To.Add(new MailAddress(email));
      msg.Subject = subject;
      msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
      await SendAsync(msg);
    }
  }
}
