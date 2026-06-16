using RevenueSystem.Application.DTOs;
using RevenueSystem.Application.Repositories;
using RevenueSystem.Domain.Entities;

namespace RevenueSystem.Application.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly ISoftwareRepository _softwareRepository;
    private readonly IDiscountRepository _discountRepository;

    public ContractService(
        IContractRepository contractRepository,
        ISoftwareRepository softwareRepository,
        IDiscountRepository discountRepository)
    {
        _contractRepository = contractRepository;
        _softwareRepository = softwareRepository;
        _discountRepository = discountRepository;
    }

    public async Task<int> CreateContractAsync(CreateContractDto dto)
    {
        var durationDays = (dto.EndDate - dto.StartDate).TotalDays;
        if (durationDays is < 3 or > 30)
            throw new ArgumentException("Przedział czasowy kontraktu musi wynosić od 3 do 30 dni.");
        
        var hasActive = await _contractRepository.HasActiveContractForSoftwareAsync(dto.ClientId, dto.SoftwareId);
        if (hasActive)
            throw new ArgumentException("Klient posiada już aktywną umowę na to oprogramowanie.");
        
        var software = await _softwareRepository.GetSoftwareByIdAsync(dto.SoftwareId) 
            ?? throw new ArgumentException("Podane oprogramowanie nie istnieje.");
        
        var discountPercentage = await _discountRepository.GetHighestActiveDiscountAsync(DateTime.UtcNow);
        
        var isReturningClient = await _contractRepository.IsReturningClientAsync(dto.ClientId);
        if (isReturningClient)
        {
            discountPercentage += 5m;
        }
        
        var basePrice = software.YearlyCost;
        var discountValue = basePrice * (discountPercentage / 100m);
        var priceAfterDiscount = basePrice - discountValue;
        
        var finalPrice = priceAfterDiscount + (dto.ExtraSupportYears * 1000m);
        
        var contract = new Contract
        {
            ClientId = dto.ClientId,
            SoftwareId = dto.SoftwareId,
            SoftwareVersion = dto.SoftwareVersion,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ExtraSupportYears = dto.ExtraSupportYears,
            TotalPrice = finalPrice,
            IsSigned = false
        };

        return await _contractRepository.AddContractAsync(contract);
    }
    
    public async Task ProcessPaymentAsync(int contractId, CreatePaymentDto dto)
    {
        var contract = await _contractRepository.GetContractWithPaymentsAsync(contractId) 
                       ?? throw new ArgumentException("Wskazany kontrakt nie istnieje.");

        if (contract.ClientId != dto.ClientId)
            throw new ArgumentException("Ten kontrakt należy do innego klienta.");

        if (contract.IsSigned)
            throw new ArgumentException("Ten kontrakt został już w pełni opłacony i podpisany.");
        
        if (DateTime.UtcNow > contract.EndDate)
        {
            foreach (var p in contract.Payments)
            {
                p.IsRefunded = true;
            }
            await _contractRepository.UpdateContractAsync(contract);
        
            throw new ArgumentException("Termin ważności oferty minął. Dotychczasowe wpłaty zostały zwrócone. Należy wygenerować nowy kontrakt.");
        }
        
        var alreadyPaid = contract.Payments
            .Where(p => !p.IsRefunded)
            .Sum(p => p.Amount);

        if (alreadyPaid + dto.Amount > contract.TotalPrice)
            throw new ArgumentException($"Wpłacana kwota jest za duża. Pozostało do zapłaty: {contract.TotalPrice - alreadyPaid} PLN.");

        var payment = new Payment
        {
            ContractId = contract.Id,
            Amount = dto.Amount,
            PaymentDate = DateTime.UtcNow
        };
    
        contract.Payments.Add(payment);

        if (alreadyPaid + dto.Amount == contract.TotalPrice)
        {
            contract.IsSigned = true;
        }
        
        await _contractRepository.UpdateContractAsync(contract);
    }
}