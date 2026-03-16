using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.Features.Accounts.Commands.OpenAccount;
using FinancialSystem.Application.Features.Accounts.Commands.TransferFunds;
using FinancialSystem.Application.Features.Accounts.Commands.UpdateAccountLockStatus;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Admin.Commands.UndoAction
{
    public class UndoActionCommandHandler : IRequestHandler<UndoActionCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public UndoActionCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UndoActionCommand request, CancellationToken cancellationToken)
        {
            // 1. Фикс ошибки CS8600 (добавлен знак ?)
            var log = await _context.AuditLogs
                .FirstOrDefaultAsync(x => x.Id == request.LogId, cancellationToken);

            if (log == null) throw new Exception("Лог не найден");

            // 2. Фикс ошибки регистра (log вместо Log)
            if (log.IsReverted) throw new Exception("Действие уже отменено");

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            switch (log.Action)
            {
                // ПЕРЕВОД (Transfer)
                case nameof(TransferFundsCommand):
                    var trf = JsonSerializer.Deserialize<TransferFundsCommand>(log.Details, jsonOptions);
                    if (trf != null)
                    {
                        // Отмена: отправляем деньги назад (меняем To и From местами)
                        // Если будет ошибка CS7036, замени { } на (trf.ToAccountId, trf.FromAccountId, trf.Amount)
                        await _mediator.Send(new TransferFundsCommand
                        {
                            FromAccountId = trf.ToAccountId,
                            ToAccountId = trf.FromAccountId,
                            Amount = trf.Amount
                        });
                    }
                    break;

                // ОТКРЫТИЕ СЧЕТА (OpenAccount) — ВОТ ТУТ ТЕПЕРЬ РАБОЧИЙ КОД
                case nameof(OpenAccountCommand):
                    var open = JsonSerializer.Deserialize<OpenAccountCommand>(log.Details, jsonOptions);
                    if (open != null)
                    {
                        // Ищем в базе счет, который был создан этой командой
                        // Мы ищем по OwnerId и BankId самый последний созданный счет
                        var accountToDelete = await _context.Accounts
                            .Where(a => a.OwnerId == open.OwnerId && a.BankId == open.BankId)
                            .OrderByDescending(a => a.Id)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (accountToDelete != null)
                        {
                            _context.Accounts.Remove(accountToDelete);
                            // После этого счет исчезнет из таблицы Accounts
                        }
                    }
                    break;

                // БЛОКИРОВКА (UpdateAccountLockStatus)
                case nameof(UpdateAccountLockStatusCommand):
                    var lockCmd = JsonSerializer.Deserialize<UpdateAccountLockStatusCommand>(log.Details, jsonOptions);
                    if (lockCmd != null)
                    {
                        // Инвертируем статус: если блокировали — разблокируем, и наоборот
                        await _mediator.Send(new UpdateAccountLockStatusCommand(
                            lockCmd.AccountId,
                            !lockCmd.ShouldBlock
                        ));
                    }
                    break;

                default:
                    throw new Exception($"Нет логики отмены для действия: {log.Action}");
            }

            log.IsReverted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
