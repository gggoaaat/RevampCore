using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Revamp.IO.Foundation
{
    public class ER_Mail
    {
        public static MailSendStatus SendEmail(MailMessage emailStruct, SmtpClient DeliveryStruct)
        { 
            var client = DeliveryStruct;
            
            try
            {
                client.Send(emailStruct);
            }
            catch(ArgumentNullException e)
            {
                return MailSendStatus.ErrorCannotSend;
            }
            catch(ObjectDisposedException e)
            {
                return MailSendStatus.ErrorCannotSend;
            }
            catch(SmtpFailedRecipientsException e)
            {
                return MailSendStatus.ErrorCannotSend;
            }
            catch(SmtpException e)
            {
                switch(e.StatusCode)
                {
                    case SmtpStatusCode.BadCommandSequence:
                    case SmtpStatusCode.MailboxNameNotAllowed:
                    case SmtpStatusCode.HelpMessage:
                    case SmtpStatusCode.SyntaxError:
                      return MailSendStatus.ErrorCannotSend;
                    case SmtpStatusCode.CannotVerifyUserWillAttemptDelivery:
                    case SmtpStatusCode.UserNotLocalWillForward:
                      return MailSendStatus.SentMaybe;
                    case SmtpStatusCode.ClientNotPermitted:
                    case SmtpStatusCode.CommandNotImplemented:
                    case SmtpStatusCode.CommandParameterNotImplemented:
                    case SmtpStatusCode.CommandUnrecognized:
                    case SmtpStatusCode.ExceededStorageAllocation:
                    case SmtpStatusCode.GeneralFailure:
                    case SmtpStatusCode.InsufficientStorage:
                    case SmtpStatusCode.LocalErrorInProcessing:
                    case SmtpStatusCode.MailboxBusy:
                    case SmtpStatusCode.MailboxUnavailable:
                    case SmtpStatusCode.MustIssueStartTlsFirst:
                    case SmtpStatusCode.ServiceClosingTransmissionChannel:
                    case SmtpStatusCode.ServiceNotAvailable:
                    case SmtpStatusCode.ServiceReady:
                    case SmtpStatusCode.StartMailInput:
                    case SmtpStatusCode.TransactionFailed:
                    case SmtpStatusCode.UserNotLocalTryAlternatePath:
                      return MailSendStatus.TryAgain;
                    case SmtpStatusCode.Ok:
                      break;
                }
            }
            catch(Exception e)
            {
                return MailSendStatus.SentMaybe;
            }
            
            return MailSendStatus.Sent;
        }
        
        [Serializable]
        public class EmailModelObject
        {
            public MailAddress toAddress { get; set; }
            public MailAddress fromAddress { get; set; }
            public MailAddress bccAddress { get; set; }
            public string subject { get; set; }
            public string messagebody { get; set; }
            
            public MailMessage _mailMessage { get; set; }
        }
        
        [Serializable]
        public enum MailSendStatus
        {
          None, Sent, ErrorCannotSend, TryAgain, SentMaybe
        }
    }
}
