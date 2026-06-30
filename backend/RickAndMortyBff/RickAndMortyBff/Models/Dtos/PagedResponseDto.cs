namespace RickAndMortyBff.Models.Dtos
{
    public class PagedResponseDto<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
    }
}
