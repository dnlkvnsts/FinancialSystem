using FinancialSystem.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationDbContextInitializer> _logger;

        public ApplicationDbContextInitializer(ApplicationDbContext context, ILogger<ApplicationDbContextInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                // Применяем миграции автоматически, если база не создана
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }

                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при инициализации базы данных.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Наполняем банки
            if (!await _context.Banks.AnyAsync())
            {
                _context.Banks.AddRange(new List<Bank>
{
                  new Bank("Сбербанк", "044525225"),
                  new Bank("Тинькофф", "044525974"),
                  new Bank("Альфа-Банк", "044525593")
                });

                await _context.SaveChangesAsync();
                _logger.LogInformation("База данных успешно наполнена начальными банками.");
            }
        }
    }
}
