using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using ISC_ELIB_SERVER.DTOs.Responses;

public class GhnService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api";

    public GhnService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(string ProvinceName, string DistrictName, string WardName)> GetLocationName(int provinceId, int districtId, string wardCode)
    {
        try
        {
            var provinceJson = await _httpClient.GetStringAsync($"{_baseUrl}/master-data/province");
            var provinces = JsonSerializer.Deserialize<GhnResponse<Province[]>>(provinceJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var province = provinces?.Data?.FirstOrDefault(p => p.ProvinceId == provinceId)?.ProvinceName ?? "Không tìm thấy tỉnh";

            var districtJson = await _httpClient.GetStringAsync($"{_baseUrl}/master-data/district?province_id={provinceId}");
            var districts = JsonSerializer.Deserialize<GhnResponse<District[]>>(districtJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var district = districts?.Data?.FirstOrDefault(d => d.DistrictId == districtId)?.DistrictName ?? "Không tìm thấy huyện";

            var wardJson = await _httpClient.GetStringAsync($"{_baseUrl}/master-data/ward?district_id={districtId}");
            var wards = JsonSerializer.Deserialize<GhnResponse<Ward[]>>(wardJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var ward = wards?.Data?.FirstOrDefault(w => w.WardCode == wardCode)?.WardName ?? "Không tìm thấy xã";

            return (province, district, ward);
        }
        catch (Exception ex)
        {
            return ($"Lỗi: {ex.Message}", "", "");
        }
    }

    public async Task<ApiResponse<Province[]>> GetProvinces()
    {
        try
        {
            var provinceJson = await _httpClient.GetStringAsync($"{_baseUrl}/master-data/province");
            var provinces = JsonSerializer.Deserialize<GhnResponse<Province[]>>(provinceJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            // province id = 286 Không dùng - filter bỏ đi
            return ApiResponse<Province[]>.Success(provinces?.Data.Where(p => p.ProvinceId != 286).ToArray() ?? Array.Empty<Province>());
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Lỗi: {ex.Message}");
            return ApiResponse<Province[]>.Fail("Lỗi khi lấy danh sách tỉnh/thành phố");
        }
    }

    public async Task<ApiResponse<District[]>> GetDistricts(int provinceId)
    {
        try
        {
            // Validate if provinceId exists
            var provinceResponse = await GetProvinces();
            if (provinceResponse.Data?.All(p => p.ProvinceId != provinceId) == true)
            {
                return ApiResponse<District[]>.Fail("Mã tỉnh/thành phố không tồn tại");
            }

            var districtJson = await _httpClient.GetStringAsync($"{_baseUrl}/master-data/district?province_id={provinceId}");
            var districts = JsonSerializer.Deserialize<GhnResponse<District[]>>(districtJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return ApiResponse<District[]>.Success(districts?.Data);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Lỗi: {ex.Message}");
            return ApiResponse<District[]>.Fail("Lỗi khi lấy danh sách quận huyện");
        }
    }

    public async Task<ApiResponse<Ward[]>> GetWards(int districtId)
    {
        try
        {
            // Validate if districtId exists
            var districtJson = await _httpClient.GetStringAsync($"{_baseUrl}/master-data/district");
            var districts = JsonSerializer.Deserialize<GhnResponse<District[]>>(districtJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (districts?.Data?.All(d => d.DistrictId != districtId) == true)
            {
                return ApiResponse<Ward[]>.Fail("Mã quận/huyện không tồn tại");
            }

            var wardJson = await _httpClient.GetStringAsync($"{_baseUrl}/master-data/ward?district_id={districtId}");
            var wards = JsonSerializer.Deserialize<GhnResponse<Ward[]>>(wardJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return ApiResponse<Ward[]>.Success(wards?.Data);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Lỗi: {ex.Message}");
            return ApiResponse<Ward[]>.Fail("Lỗi khi lấy danh sách xã phường");
        }
    }

}


public class GhnResponse<T>
{
    public T Data { get; set; }
}

public class Province
{
    public int ProvinceId { get; set; }
    public string ProvinceName { get; set; }
}

public class District
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; }
}

public class Ward
{
    public string WardCode { get; set; }
    public string WardName { get; set; }
}
