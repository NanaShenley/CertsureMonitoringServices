using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Timers;

namespace CertsureMonitoringServices
{
   public class LoginService
    {
        private System.Diagnostics.EventLog eventLog1 = new System.Diagnostics.EventLog();
        private Timer timer = new Timer();
        public void Start()
        {
            timer = new Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Enabled = true;
            timer.Start();

            eventLog1.Source = "NOCS Events source";
            eventLog1.Log = "";
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            Login();
        }

        public void Stop()
        {
            timer.Enabled = false;
            timer.Elapsed -= new ElapsedEventHandler(this.OnTimer);
        }

        public void Login()
        {
            //Kill all chromedriver processes
            KillAllChromeDriverProcesses();
            System.Threading.Thread.Sleep(5000);

            timer.Enabled = false;
            try
            {
                string userName = "PaulCollins1";
                string password = "warwick";

                var options = new ChromeOptions();
                options.AddArgument("headless");
                options.AddArgument("no-sandbox");
                options.AddArgument("proxy-server='direct://'");
                options.AddArgument("proxy-bypass-list=*");

                IWebDriver driver = new ChromeDriver(options);
                eventLog1.WriteEntry("chrome browser created successfully");

                driver.Navigate().GoToUrl("https://www.niceiconline.com/");
                eventLog1.WriteEntry("Successfully navigate to www.niceiconline.com");
                System.Threading.Thread.Sleep(5000);

                driver.FindElement(By.PartialLinkText("Register/ Login")).Click();
                eventLog1.WriteEntry("Successfully click 0n Register/Login link.");
                System.Threading.Thread.Sleep(2000);

                driver.FindElement(By.Id("username")).SendKeys(userName);
                eventLog1.WriteEntry(string.Format("Successfully enter username {0}", userName));

                driver.FindElement(By.Id("password")).SendKeys(password);
                eventLog1.WriteEntry(string.Format("Successfully enter password {0}", password)); 

                driver.FindElement(By.CssSelector("input[type ='submit']")).Click();
                eventLog1.WriteEntry("Successfully click on login button.");
                System.Threading.Thread.Sleep(3000);

                try
                {
                    if (driver.FindElement(By.PartialLinkText("Log out")).Text == "Log out")
                    {
                        eventLog1.WriteEntry("Successfully logged in. No alert sent");
                        //SendEmail();
                        //LocalSending();
                        
                    }
                    else
                    {
                        eventLog1.WriteEntry("SEND EMAIL ALERT!! LOGIN NOT SUCCESSFUL");
                        //SendEmail();
                        LocalSending();
                    }
                }
                catch (Exception)
                {
                    //SendEmail();
                    eventLog1.WriteEntry("SEND EMAIL ALERT!! LOGIN NOT SUCCESSFUL, EXCEPTION, LOGOUT ELEMENT NOT FOUND");
                    LocalSending();
                }
                

                if (driver != null)
                    driver.Quit();
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                timer.Enabled = true;
            }
            
        }

        private void KillAllChromeDriverProcesses()
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                process.Kill();
            }
        }

        private void SendEmail()
        {
            try
            {
                var emailHost = "outlook.office365.com";
                var noReplyAddress = "OptionsAdm01@gmail.com";

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(noReplyAddress);
                mail.To.Add("divine.agbor@certsure.com");
                mail.To.Add("Sam.Antonini@certsure.com");
                mail.To.Add("Simon.Lascelles@certsure.com");
                mail.To.Add("Brian.Parris@certsure.com");
                mail.To.Add("Tracy.Rinaldi@certsure.com");
                mail.To.Add("tom.barnes@certsure.com");
                mail.To.Add("paul.oshea@eca.co.uk");

                mail.IsBodyHtml = true;
                string starttime = DateTime.Now.ToString();
                mail.Subject = "P1 INCIDENT: LOGIN FAILURE IN NOCS APPLICATION " + starttime;
                mail.Body = "P1 INCIDENT: LOGIN FAILURE IN NOCS APPLICATION: " + starttime;

                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Host = emailHost;
                    client.EnableSsl = false;
                    client.Port = 25;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(mail);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void LocalSending() 
        {
            try
            {
                var emailHost = "smtp.gmail.com";
                var noReplyAddress = "OptionsAdm01@gmail.com";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(noReplyAddress);
                mail.To.Add("divine_agbor@yahoo.com");
                mail.To.Add("divine.agbor@certsure.com");
                mail.To.Add("Sam.Antonini@certsure.com");
                mail.To.Add("Simon.Lascelles@certsure.com");
                mail.To.Add("Brian.Parris@certsure.com");
                mail.To.Add("Tracy.Rinaldi@certsure.com");
                mail.To.Add("tom.barnes@certsure.com");
                mail.To.Add("paul.oshea@eca.co.uk");
                mail.IsBodyHtml = true;
                string starttime = DateTime.Now.ToString();
                mail.Subject = "P1 Incident: LOGIN FAILURE IN NOCS APPLICATION: " + starttime;
                mail.Body = "P1 INCIDENT LOGIN FAILURE IN NOCS APPLICATION: " + starttime;

                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("divineagbor@gmail.com", "Stanley15000");
                    client.Host = emailHost;
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(mail);
                }


            }
            catch (Exception ex)
            {

            }
        }

    }
}
