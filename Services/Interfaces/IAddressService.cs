namespace blog_api.Services.Interfaces;

public interface IAddressService
{
    public Task<List<SearchAddressModel>> SearchAddress(long parentObjectId, string? query);
    public Task<List<SearchAddressModel>> SearchChain(Guid objectGuid);
    public Task<bool> IsAddressAvailable(Guid id);




}