namespace BaseTest.Models.ReponseModels;

public class BasePaginationResponseModel
    <T> where T : class
{
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalPage { get; set; }
    public long TotalItems { get; set; }
    public List<T> Data { get; set; }

    public BasePaginationResponseModel(int pageNo, int pageSize, long totalItems, List<T> data)
    {
        PageNo = pageNo;
        PageSize = pageSize;
        TotalItems = totalItems;
        Data = data;
        TotalPage = (int)Math.Ceiling((decimal)totalItems / pageSize);
    }
}