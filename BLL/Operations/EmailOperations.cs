using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Operations
{
    public class EmailOperations
    {
        public EmailOperations()
        {

        }

        public async Task Close(TicketDTO ticket, string email) // отправка сообщения об отмене билета
        {
            MailMessage emailMessage = new MailMessage();
            emailMessage.IsBodyHtml = true;
            emailMessage.From = new MailAddress("ivan.siziakov@mail.ru",
                "WBSTO"); //отправитель сообщения и его подпись, срабатывает корректно при указании
            // реальной почты
            emailMessage.To.Add(email); //адресат сообщения
            emailMessage.Subject = "Отмена билета"; //тема сообщения
            emailMessage.Body = "<div><h3>Билет номер " + ticket.TicketId // сообщение
                 + "</h3><label>Номер маршрута - " + ticket.RouteId
             + "</label><br><label>Номер рейса - " + ticket.CruiseId
                 + "</label><br><label>Стоимость - " + ticket.Cost
                 + "</label><br><label>Место - " + ticket.Place
                 + "</label><br><label>Время отправления - " + ticket.StartDate
                 + "</label><br><label>Остановка отправления - " +
                 ticket.StartHalt.Halt.locality_model.locality_name
                 + ", " + ticket.StartHalt.Halt.adress
                 + "</label><br><label>Остановка прибытия - " +
                 ticket.EndHalt.Halt.locality_model.locality_name
                 + ", " + ticket.EndHalt.Halt.adress
                 + "</label><br/><h4>Билет был отменен</h4></div>";


            using (SmtpClient client = new SmtpClient("smtp.mail.ru", 25)) // отправка письма
            {
                client.EnableSsl = true; // требуется mail.ru
                #region Указание данных аккаунта отправителя
                client.Credentials = new NetworkCredential("ivan.siziakov",
                "aDu3KEkVN0kwqamGnpfD");
                #endregion
                client.Send(emailMessage); // отправка
            }
        }

        public async Task Ticket(TicketDTO ticket, string email) // отправка билета на почту
        {
            MailMessage emailMessage = new MailMessage();
            emailMessage.IsBodyHtml = true;
            emailMessage.From = new MailAddress("ivan.siziakov@mail.ru", 
                "WBSTO"); //отправитель сообщения и его подпись, срабатывает корректно при указании
            // реальной почты
            emailMessage.To.Add(email); //адресат сообщения
            emailMessage.Subject = "Билет - WBSTO"; //тема сообщения
            emailMessage.Body = "<div><h3>Билет номер " + ticket.TicketId // сообщение
                + "</h3><label>Номер маршрута - " + ticket.RouteId
            + "</label><br><label>Номер рейса - " + ticket.CruiseId 
                + "</label><br><label>Стоимость - " + ticket.Cost 
                + "</label><br><label>Место - " + ticket.Place
                + "</label><br><label>Время отправления - " + ticket.StartDate
                + "</label><br><label>Остановка отправления - " + 
                ticket.StartHalt.Halt.locality_model.locality_name 
                + ", " + ticket.StartHalt.Halt.adress
                + "</label><br><label>Остановка прибытия - " + 
                ticket.EndHalt.Halt.locality_model.locality_name
                + ", " + ticket.EndHalt.Halt.adress
                + "</label></div>";
            if (ticket.Returned)
            {
                emailMessage.Body += "<br/><div>Возвращен<div>";
            }

            using (SmtpClient client = new SmtpClient("smtp.mail.ru", 25)) // отправка письма
            {
                client.EnableSsl = true; // требуется mail.ru
                #region Указание данных аккаунта отправителя
                client.Credentials = new NetworkCredential("ivan.siziakov",
                "aDu3KEkVN0kwqamGnpfD");
                #endregion
                client.Send(emailMessage); // отправка
            }
        }
    }
}
