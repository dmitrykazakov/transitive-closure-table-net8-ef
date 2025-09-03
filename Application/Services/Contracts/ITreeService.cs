using TransitiveClosureTable.Application.Dto;

namespace TransitiveClosureTable.Application.Services.Contracts;

public interface ITreeService
{
    Task<TreeDto> GetOrCreateAsync(string name);
}