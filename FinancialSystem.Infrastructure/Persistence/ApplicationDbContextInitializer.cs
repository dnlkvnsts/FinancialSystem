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
            // 1. Наполняем банки
            if (!await _context.Banks.AnyAsync())
            {
                _context.Banks.AddRange(new List<Bank>
        {
            new Bank("Сбербанк", "044525225"),
            new Bank("Тинькофф", "044525974"),
            new Bank("Альфа-Банк", "044525593")
        });

                await _context.SaveChangesAsync();
                _logger.LogInformation("База данных наполнена банками.");
            }

            // 2. Наполняем предприятия (Enterprises)
            if (!await _context.Enterprises.AnyAsync())
            {
                _context.Enterprises.AddRange(new List<Enterprise>
        {
            // Название и Юридический адрес (согласно вашему EnterpriseDto)
            new Enterprise("ООО «Яндекс»", "г. Москва, ул. Льва Толстого, д. 16"),
            new Enterprise("ПАО «Газпром»", "г. Санкт-Петербург, Лахтинский пр-кт, д. 2"),
            new Enterprise("АО «Тандер» (Магнит)", "г. Краснодар, ул. им. Леваневского, д. 185"),
            new Enterprise("ООО «Вайлдберриз»", "Московская обл., г. Подольск, деревня Коледино")
        });

                await _context.SaveChangesAsync();
                _logger.LogInformation("База данных наполнена начальными предприятиями.");
            }
        }
    }
}
