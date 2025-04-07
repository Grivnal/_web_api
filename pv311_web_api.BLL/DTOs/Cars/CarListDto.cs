namespace pv311_web_api.BLL.DTOs.Cars
{
    public class CarListDto
    {
        public List<CarDto> Cars { get; set; } = [];
        public int TotalCount { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int PageCount { get; set; } = 3;
    }
}