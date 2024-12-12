using blog_api.Exeptions;
using blog_api.Mappers;
using blog_api.Models.Fias;
using blog_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace blog_api.Services.Impls;

public class AddressService(AddressContext addressContext) : IAddressService
{
    public async Task<List<SearchAddressModel>> SearchAddress(long parentObjectId, string? query)
    {
        var addressParents = addressContext.AsAdmHierarchies.Where(a => a.Parentobjid == parentObjectId).ToList();
        List<SearchAddressModel> result = new List<SearchAddressModel>();
        foreach (var address in addressParents)
        {
            var searchAddress = await GetAddressById(address.Objectid);
            if (searchAddress == null) continue;
            if (searchAddress.Text.ToLower().Contains(query == null ? "" : query.ToLower())) result.Add(searchAddress);
            if (result.Count == 10) break;
        }

        return result;
    }

    private async Task<SearchAddressModel?> GetAddressById(long? id)
    {
        var addressObj = await addressContext.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectid == id);
        if (addressObj == null)
        {
            var addressHouse = await addressContext.AsHouses.FirstOrDefaultAsync(a => a.Objectid == id);
            if (addressHouse == null) return null;
            var text = $"{addressHouse.Housenum}";
            if (addressHouse.Addnum1 != null) text += $" {AddressMapper.ObjectLevelToString(AddressMapper.ToGarAddressLevel("10"))} {addressHouse.Addnum1}";
            if (addressHouse.Addnum2 != null) text += $" {AddressMapper.ObjectLevelToString(AddressMapper.ToGarAddressLevel("10"))} {addressHouse.Addnum2}";
            return new SearchAddressModel()
            {
                ObjectGuid = addressHouse.Objectguid,
                ObjectId = addressHouse.Objectid,
                Text = text,
                Level = AddressMapper.ToGarAddressLevel("10").ToString(),
                ObjectLevelText = AddressMapper.ObjectLevelToString(AddressMapper.ToGarAddressLevel("10"))
            };
        }
        return new SearchAddressModel()
        {
            ObjectGuid = addressObj.Objectguid,
            ObjectId = addressObj.Objectid,
            Text = $"{addressObj.Typename} {addressObj.Name}",
            Level = addressObj.Level,
            ObjectLevelText = AddressMapper.ObjectLevelToString(AddressMapper.ToGarAddressLevel(addressObj.Level))
        };
    }
    
    public async Task<List<SearchAddressModel>> SearchChain(Guid objectGuid)
    {
        var objectId = await GetIdByGuid(objectGuid);
        var path = (await addressContext.AsAdmHierarchies.FirstOrDefaultAsync(a => a.Objectid == objectId))?.Path!.Split(".");
        List<SearchAddressModel> addresses = new List<SearchAddressModel>();
        foreach (var id in path)
        {   
            addresses.Add(await GetAddressById(long.Parse(id)));
        }

        return addresses;
    }
    
    private async Task<long?> GetIdByGuid(Guid guid)
    {
        var id = (await addressContext.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectguid == guid))?.Objectid;
        if (id == null) id = (await addressContext.AsHouses.FirstOrDefaultAsync(a => a.Objectguid == guid))?.Objectid;
        if (id == null) throw new CustomException("Can't find address with this id",400);
        return id;
    }
    public async Task<bool> IsAddressAvailable(Guid id)
    {
        var isAddressObj = await addressContext.AsAddrObjs.AnyAsync(a => a.Objectguid == id);
        if (!isAddressObj)
        {
            var isAddressHouse = await addressContext.AsHouses.AnyAsync(a => a.Objectguid == id);
            return isAddressHouse;
        }
        return isAddressObj;
    }
}