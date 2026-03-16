using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{
    public class AuditLogDto
    {
        public int Id { get; set; }           // ID записи лога
        public int UserId { get; set; }        // ID пользователя для системных ссылок
        public string UserEmail { get; set; } // Email — чтобы админ понимал, кто это
        public string Action { get; set; }    // Переименуй в Action, чтобы совпадало с Entity
        public string Details { get; set; }   // JSON с деталями операции
        public DateTime CreatedAt { get; set; } 
    }
}
