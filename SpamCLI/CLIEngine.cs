using System;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace SpamCLI
{
    public class CLIEngine
    {

        private SmtpClient m_Client;
        private MailMessage m_Message;
        string body = string.Format(@"שלום,

לא יודע למה החלטתם שאתם רוצים אותי כלקוח, אני לא רוצה אתכם כספקים.
מעולם לא ביקשתי להתחבר לשירות שלכם, מעולם לא חתמתי על כלום.
יום אחד התקשר נציג שלכם וביקש לתת לי מתנה הצטרפות לשירות שלכם בחינם  - אמרתי לו שאני לא מעוניין.
הוא חזר ואמר שזה בחינם ולא יעלה לי דבר - ושוב אני אמרתי לו שאני לא מעוניין.
אמרתי לו שאני לא מסכים להצטרף ולא מוכן שיצרף אותי.
אמר שאין בעיה ולא אצורף לשירות.

עוברים החודשים והחיובים התחילו להגיע :
ב 17/9/20 חיוב של 14.81 שח.
היום (15/11/202)הגיעה חשבונית של 51 שח.
לשירות שלא אני ביקשתי וחזרתי ואמרתי שוב ושוב שאני לא מעוניין!!!!

אתם כמובן לא זמינים לשירות (מעניין למה? ) ולכן זכיתם לקבל אותי במיילים שלכם.

לא אפסיק את השליחה עד שתחזרו אלי, תבטלו ותחזירו את הכסף.
כסף שחייבתם עבור שירות שלא ביקשתי ולא צרכתי.
כל יום אגיע לעוד ועוד כתובות אצליכם ואשלח מכתובות שונות, עד שתיצרו איתי קשר לביצוע ההחזר
עד שתחזרו אלי, תבטלו ותחזירו את הכסף.


לא אהסס להעלות את הסיפור למערכות חדשות הצרכנות על מנת לחשוף את פרצופכם ושיטותיכם, שאני בטוח שאתם מפעילים על אנשים נוספים.

אשמח לשמוע מכם,

איתי כהן
058-7600202

"); 
        string body1 = string.Format(@"שלום,

התקשר אלי הנציג גמיל.כנראה שלא הובהרתי נכון ולכן אחדד.
הנציגה שחיברה אותי לHOT נתנה לי מחיר עבור שירותי אינטרנט בלבד.
לא דובר אף לא לרגע על NEXTTVץ כמו שהבהרתי לנציג שלכם שהתקשר כדי לחבר אותי
לNEXTTV שאני לא מעוניין, כך גם הבהרתי את עצמי לגמיל. אני עכשיו
בהמתנה שיענו לי אצליכם.

נא טפלו בעיניין !!! אני לא מעוניין בשינוי החבילה או המחיר.לא רוצה לשמוע
ולראות חיובים שקשורים לNEXTTV.

חיזרו אלי במהרה!!!

איתי כהן
058-7600202");
        private string[] m_Senders;

        private string[] m_Recivers;

        private string[] m_Headers;

        private string[] m_BodyMultiLine;

        private string m_BodyOneString;
        public string SendersFilePath { get; set; }
        public string ReciversFilePath { get; set; }
        public string HeadersFilePath { get; set; }
        public string LogFilePath { get; set; }
        public string BodyFilePath { get; set; }
        public string Root { get; set; }
        public int NumberOfMails { get; set; }

        public CLIEngine(string i_Root, string i_LogPath, string i_SendersPath, string i_ReciversPath,string i_HeadersPath, int i_numberOfMails)
        {
            this.LogFilePath = Path.Combine(i_Root, i_LogPath);
            this.SendersFilePath = Path.Combine(i_Root, i_SendersPath);
            this.ReciversFilePath = Path.Combine(i_Root, i_ReciversPath);
            this.HeadersFilePath = Path.Combine(i_Root, i_HeadersPath);
            this.BodyFilePath = Path.Combine(i_Root, "Body.txt");
            this.NumberOfMails = i_numberOfMails;
            this.Root = i_Root;
        }
        public void Run()
        {
            initLoger();
            loadSenders();
            loadRecivers();
            loadHeaders();
            loadBody();
            startSending(this.NumberOfMails);
        }

        private void loadBody()
        {
            this.m_BodyMultiLine = loadTextLinesFromFile(BodyFilePath);
            this.m_BodyOneString = joinToOneString(this.m_BodyMultiLine);
        }


        /// <summary>
        /// take array of string and concat it to one long string
        /// </summary>
        /// <param name="i_BodyMultiLine"></param>
        /// <returns></returns>
        private string joinToOneString(string[] i_BodyMultiLine)
        {
            if (i_BodyMultiLine.Length < 1)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(i_BodyMultiLine[0]);
            for (int i = 1; i < i_BodyMultiLine.Length; i++)
            {
                sb.Append(Environment.NewLine);
                sb.Append(i_BodyMultiLine[i]);
            }
            return sb.ToString();
        }

        private void loadHeaders()
        {
            this.m_Headers = loadTextLinesFromFile(HeadersFilePath);
        }
        /// <summary>
        /// define one source for sending message
        /// </summary>
        /// <param name="i_Source"></param>
        /// <param name="i_Token"></param>
        /// <returns></returns>
        private SmtpClient defineOneSource(string i_Source, string i_Token)
        {
            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = i_Source,
                    Password = i_Token
                }
            };
            return client;
        }
        /// <summary>
        /// define one message 
        /// </summary>
        /// <param name="i_SourceMail"></param>
        /// <param name="i_DestenationMail"></param>
        /// <param name="i_MessegeHeader"></param>
        /// <param name="i_MessegeBody"></param>
        /// <returns></returns>
        private MailMessage defineOneMessage(string i_SourceMail, string i_DestenationMail, string i_MessegeHeader, string i_MessegeBody)
        {
            MailAddress FromEmail = new MailAddress("momo@gmail.com", i_MessegeHeader);
            MailAddress ToEmail = new MailAddress(i_DestenationMail, "check in the toemail");
            MailMessage messege = new MailMessage()
            {
                From = FromEmail,
                Subject = i_MessegeHeader,
                Body = i_MessegeBody
            };
            messege.To.Add(ToEmail);
            return messege;
        }
        /// <summary>
        /// start the sending process :
        /// it randomeize subjects from the headers array, randomize a sender from the senders array.
        /// for each recivers in the recivers array for each iteration of the loop
        /// </summary>
        /// <param name="i_NumberOfMails"></param>
        private void startSending(int i_NumberOfMails)
        {
            SmtpClient client;
            MailMessage message;
            string[] currentSenderDetails;
            int totalNumberOfMails = i_NumberOfMails * m_Recivers.Length, sendsCouter = 0;
            foreach (string reciver in m_Recivers)
            {
                for (int i = 0; i < i_NumberOfMails; i++)
                {
                    currentSenderDetails = randomizeTwoLines(m_Senders);
                    client = defineOneSource(currentSenderDetails[0], currentSenderDetails[1]);
                    message = defineOneMessage(currentSenderDetails[0], reciver, randomizeLine(m_Headers), this.m_BodyOneString);
                    client.Send(message);
                    sendsCouter++;
                    logSuccessSend(currentSenderDetails[0],reciver, sendsCouter, totalNumberOfMails);
                }
            }
            File.AppendAllText(LogFilePath, $"End of Log file at {DateTime.Now} {Environment.NewLine}");
        }

        private void initLoger()
        {
            File.AppendAllText(LogFilePath, $"Start of Log file at {DateTime.Now} {Environment.NewLine}");
        }

        void logSuccessSend(string i_Sender, string i_MailAddress, int i_numberOfMail, int i_ToatalNumberOfMails)
        {
            string text = $"Mail {i_numberOfMail}/{i_ToatalNumberOfMails} from {i_Sender}, send to : {i_MailAddress} at {DateTime.Now}.";
            File.AppendAllText(LogFilePath, $"{ text} {Environment.NewLine} " );
            Console.WriteLine(text);
        }
        private void loadRecivers()
        {
            this.m_Recivers = loadTextLinesFromFile(ReciversFilePath);
            
        }

        private void loadSenders()
        {
            this.m_Senders = loadTextLinesFromFile(SendersFilePath);
        }

        private string[] loadTextLinesFromFile(string i_FilePath)
        {
            string[] lines = File.ReadAllLines(i_FilePath);
            if (lines.Length > 0 )
            {
                return lines;
            }
            return null;
        }

        private string randomizeLine(string [] i_StringsPool)
        {
            Random rnd = new Random();
            return i_StringsPool[rnd.Next(i_StringsPool.Length - 1)];
        }

        private string[] randomizeTwoLines(string[] i_StringsPool)
        {
            Random rnd = new Random();
            string[] result = new string[2];
            if (i_StringsPool.Length < 2)
            {
                return null;
            }
            int rndPlace = rnd.Next(0, (i_StringsPool.Length / 2));
            result[0] = i_StringsPool[rndPlace * 2];
            result[1] = i_StringsPool[rndPlace * 2 + 1];
            return result;
        }
    }


}

