using System;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using BlogEngine.Core.Providers;
using BlogEngine.Core.Web.Controls;

namespace BlogEngine.NET.Custom.Extensions
{

    [Extension(
        "EmailListExtractor - Extract email addresses from post comments and use them for your mailing list database",
        "1.0.0", "Adrian Cini")]
    public class EmailListExtractor
    {
        public EmailListExtractor()
        {
            Post.CommentAdded += new EventHandler<EventArgs>(Comment_Serving);
        }

        public void Comment_Serving(object sender, EventArgs e)
        {
            //Save email . 
            string email = ((BlogEngine.Core.Comment) (sender)).Email;
            string source = "postcomment_email";
            string blogId = BlogEngine.Core.Blog.CurrentInstance.Id.ToString();
            var dbMail = new MailListProvider();
            dbMail.AddToMailList(blogId, email, source);
        }
    }
}