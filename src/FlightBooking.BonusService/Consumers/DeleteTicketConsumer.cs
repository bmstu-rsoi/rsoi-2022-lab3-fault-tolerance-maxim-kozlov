using AutoMapper;
using FlightBooking.BonusService.Database;
using FlightBooking.BonusService.Database.Entities;
using FlightBooking.BonusService.Dto;
using FlightBooking.BonusService.Dto.Contracts;
using FlightBooking.BonusService.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.BonusService.Consumers;

public class DeleteTicketConsumer: IConsumer<DeleteTicket>
{
    private readonly BonusContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteTicketConsumer> _logger;

    public DeleteTicketConsumer(BonusContext context, IMapper mapper, ILogger<DeleteTicketConsumer> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteTicket> context)
    {
        var ticketUid = context.Message.TicketUid;
        var username = context.Message.Username;
        
        var privilege = await _context.Privileges
            .FirstOrDefaultAsync(x => x.Username == username);
            
        if (privilege == null)
            return;
            
        var lastOperation = await _context.PrivilegeHistories
            .OrderByDescending(x => x.Datetime)
            .LastOrDefaultAsync(x => x.TicketUid == ticketUid);

        if (lastOperation == null)
            return;

        var newOperation = new PrivilegeHistory();
        _mapper.Map(lastOperation, newOperation);
            
        var balanceDiff = -lastOperation.BalanceDiff;
        if (balanceDiff > 0)
            newOperation.OperationType = OperationTypeDto.FillInBalance.GetValue();
        else
        {
            if (balanceDiff + privilege.Balance < 0)
                balanceDiff = -privilege.Balance;
            newOperation.OperationType = OperationTypeDto.DebitAccount.GetValue();
        }

        newOperation.Datetime = DateTime.Now;
        newOperation.BalanceDiff = balanceDiff;

        await _context.PrivilegeHistories.AddAsync(newOperation);
            
        privilege.Balance += balanceDiff;
        await _context.SaveChangesAsync();
        
    }
}